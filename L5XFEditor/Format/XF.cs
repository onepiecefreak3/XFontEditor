using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using L5XFEditor.IO;
using L5XFEditor.Compression;

namespace L5XFEditor.Format
{
    public class XF
    {
        Bitmap bmp;
        public Bitmap image_0;
        public Bitmap image_1;
        public Bitmap image_2;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        [DebuggerDisplay("[{offset_x}, {offset_y}, {glyph_width}, {glyph_height}]")]
        public class CharSizeInfo
        {
            public sbyte offset_x;
            public sbyte offset_y;
            public byte glyph_width;
            public byte glyph_height;

            public override bool Equals(object obj)
            {
                var csi = (CharSizeInfo)obj;
                return offset_x == csi.offset_x && offset_y == csi.offset_y && glyph_width == csi.glyph_width && glyph_height == csi.glyph_height;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        [DebuggerDisplay("[{code_point}] {ColorChannel}:{ImageOffsetX}:{ImageOffsetY}")]
        public class CharacterMap
        {
            public char code_point;
            public ushort char_size;
            public int image_offset;

            public int CharSizeInfoIndex => char_size % 1024;
            public int CharWidth => char_size / 1024;
            public int ColorChannel => image_offset % 16;
            public int ImageOffsetX => image_offset / 16 % 16384;
            public int ImageOffsetY => image_offset / 16 / 16384;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class XFHeader
        {
            public Magic8 magic;
            public int unk1;
            public short unk2;
            public short unk3;
            public short unk4;
            public short unk5;
            public long zero0;

            public short table0Offset;
            public short table0EntryCount;
            public short table1Offset;
            public short table1EntryCount;
            public short table2Offset;
            public short table2EntryCount;
        }

        public Dictionary<char, CharSizeInfo> lstCharSizeInfoLarge;
        public Dictionary<char, CharSizeInfo> lstCharSizeInfoSmall;
        public Dictionary<char, CharacterMap> dicGlyphLarge;
        public Dictionary<char, CharacterMap> dicGlyphSmall;

        XPCK xpck;
        IMGC xi;
        XFHeader xfheader;

        Level5.Method t0Comp;
        Level5.Method t1Comp;
        Level5.Method t2Comp;

        public XF(Stream input)
        {
            using (var br = new BinaryReaderX(input))
            {
                xpck = new XPCK(input);

                //get xi image to bmp
                xi = new IMGC(xpck.Files[0].FileData);
                bmp = xi.Image;

                //decompress fnt.bin
                var tempCharSizeInfo = new List<CharSizeInfo>();
                using (var fntR = new BinaryReaderX(xpck.Files[1].FileData, true))
                {
                    xfheader = fntR.ReadStruct<XFHeader>();

                    fntR.BaseStream.Position = xfheader.table0Offset << 2;
                    t0Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                    fntR.BaseStream.Position -= 4;
                    tempCharSizeInfo = new BinaryReaderX(new MemoryStream(Level5.Decompress(fntR.BaseStream))).ReadMultiple<CharSizeInfo>(xfheader.table0EntryCount);

                    fntR.BaseStream.Position = xfheader.table1Offset << 2;
                    t1Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                    fntR.BaseStream.Position -= 4;
                    dicGlyphLarge = new BinaryReaderX(new MemoryStream(Level5.Decompress(fntR.BaseStream))).ReadMultiple<CharacterMap>(xfheader.table1EntryCount).ToDictionary(x => x.code_point);

                    fntR.BaseStream.Position = xfheader.table2Offset << 2;
                    t2Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                    fntR.BaseStream.Position -= 4;
                    dicGlyphSmall = new BinaryReaderX(new MemoryStream(Level5.Decompress(fntR.BaseStream))).ReadMultiple<CharacterMap>(xfheader.table2EntryCount).ToDictionary(x => x.code_point);
                }

                #region Expand charsizeinfo
                lstCharSizeInfoLarge = new Dictionary<char, CharSizeInfo>();
                lstCharSizeInfoSmall = new Dictionary<char, CharSizeInfo>();
                foreach (var dic in dicGlyphLarge)
                {
                    lstCharSizeInfoLarge.Add(dic.Value.code_point, new CharSizeInfo
                    {
                        offset_x = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].offset_x,
                        offset_y = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].offset_y,
                        glyph_width = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].glyph_width,
                        glyph_height = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].glyph_height
                    });
                }
                foreach (var dic in dicGlyphSmall)
                {
                    lstCharSizeInfoSmall.Add(dic.Value.code_point, new CharSizeInfo
                    {
                        offset_x = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].offset_x,
                        offset_y = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].offset_y,
                        glyph_width = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].glyph_width,
                        glyph_height = tempCharSizeInfo[dic.Value.CharSizeInfoIndex].glyph_height
                    });
                }
                #endregion

                var bmpInfo = new BitmapInfo(bmp);

                image_0 = bmpInfo.CreateChannelBitmap(BitmapInfo.Channel.Red);
                image_1 = bmpInfo.CreateChannelBitmap(BitmapInfo.Channel.Green);
                image_2 = bmpInfo.CreateChannelBitmap(BitmapInfo.Channel.Blue);
            }
        }

        public void Save(Stream output)
        {
            //Update image
            #region  Compiling and saving new image
            var img = new MemoryStream();
            var i0a = new BitmapInfo(image_0).pixelMap(BitmapInfo.Channel.Alpha);
            var i1a = new BitmapInfo(image_1).pixelMap(BitmapInfo.Channel.Alpha);
            var i2a = new BitmapInfo(image_2).pixelMap(BitmapInfo.Channel.Alpha);

            bmp = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                    bmp.SetPixel(x, y, Color.FromArgb(255, i0a[x, y], i1a[x, y], i2a[x, y]));

            xi.Image = bmp;
            xi.Save(img);
            xpck.Files[0].FileData = img;
            #endregion

            //Compact charSizeInfo
            var compactCharSizeInfo = new List<CharSizeInfo>();
            #region Compacting and updating dictionaries
            foreach (var info in lstCharSizeInfoLarge)
                if (compactCharSizeInfo.Contains(info.Value))
                    dicGlyphLarge[info.Key].char_size = (ushort)(compactCharSizeInfo.FindIndex(c => c.Equals(info.Value)) % 1024 + dicGlyphLarge[info.Key].CharWidth * 1024);
                else
                {
                    dicGlyphLarge[info.Key].char_size = (ushort)(compactCharSizeInfo.Count % 1024 + dicGlyphLarge[info.Key].CharWidth * 1024);
                    compactCharSizeInfo.Add(info.Value);
                }
            foreach (var info in lstCharSizeInfoSmall)
                if (compactCharSizeInfo.Contains(info.Value))
                    dicGlyphSmall[info.Key].char_size = (ushort)(compactCharSizeInfo.FindIndex(c => c.Equals(info.Value)) % 1024 + dicGlyphSmall[info.Key].CharWidth * 1024);
                else
                {
                    dicGlyphSmall[info.Key].char_size = (ushort)(compactCharSizeInfo.Count % 1024 + dicGlyphSmall[info.Key].CharWidth * 1024);
                    compactCharSizeInfo.Add(info.Value);
                }
            #endregion

            //Writing
            var ms = new MemoryStream();
            using (var bw = new BinaryWriterX(ms, true))
            {
                //Table0
                xfheader.table0EntryCount = (short)compactCharSizeInfo.Count;
                bw.BaseStream.Position = 0x28;
                bw.WriteMultipleCompressed(compactCharSizeInfo, t0Comp);
                bw.WriteAlignment(4);

                //Table1
                xfheader.table1Offset = (short)(bw.BaseStream.Position >> 2);
                xfheader.table1EntryCount = (short)dicGlyphLarge.Count;
                bw.WriteMultipleCompressed(dicGlyphLarge.Select(d=>d.Value), t1Comp);
                bw.WriteAlignment(4);

                //Table2
                xfheader.table2Offset = (short)(bw.BaseStream.Position >> 2);
                xfheader.table2EntryCount = (short)dicGlyphSmall.Count;
                bw.WriteMultipleCompressed(dicGlyphSmall.Select(d => d.Value), t2Comp);
                bw.WriteAlignment(4);

                //Header
                bw.BaseStream.Position = 0;
                bw.WriteStruct(xfheader);
            }
            xpck.Files[1].FileData = ms;

            xpck.Save(output);
        }

        public CharacterMap GetCharacterMap(char c, bool small)
        {
            CharacterMap result;
            if (small == false)
            {
                var success = dicGlyphLarge.TryGetValue(c, out result) || dicGlyphLarge.TryGetValue('?', out result);
            }
            else
            {
                var success = dicGlyphSmall.TryGetValue(c, out result) || dicGlyphSmall.TryGetValue('?', out result);
            }
            return result;
        }
        public CharSizeInfo GetCharacterInfo(char code, bool small) => (small) ? lstCharSizeInfoSmall[code] : lstCharSizeInfoLarge[code];

        public float DrawCharacter(char c, Color color, Graphics g, float x, float y, bool small)
        {
            CharacterMap charMap = GetCharacterMap(c, small);
            CharSizeInfo charInfo = GetCharacterInfo(charMap.code_point, small);

            var attr = new ImageAttributes();
            var matrix = Enumerable.Repeat(new float[5], 5).ToArray();
            matrix[charMap.ColorChannel] = new[] { 0, 0, 0, 1f, 0 };
            matrix[4] = new[] { color.R / 255f, color.G / 255f, color.B / 255f, 0, 0 };
            attr.SetColorMatrix(new ColorMatrix(matrix));

            g.DrawImage(bmp,
                new[] { new PointF(x + charInfo.offset_x, y + charInfo.offset_y),
                    new PointF(x + charInfo.offset_x + charInfo.glyph_width, y + charInfo.offset_y),
                    new PointF(x + charInfo.offset_x, y + charInfo.offset_y + charInfo.glyph_height) },
                new RectangleF(charMap.ImageOffsetX, charMap.ImageOffsetY, charInfo.glyph_width, charInfo.glyph_height),
                GraphicsUnit.Pixel,
                attr);

            return x + charMap.CharWidth;
        }

    }
}