using L5XFEditor.Compression;
using L5XFEditor.IO;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace L5XFEditor.Format.FNT
{
    public sealed class FNT01
    {
        XF01Header xf1header;

        Level5.Method t0Comp;
        Level5.Method t1Comp;
        Level5.Method t2Comp;

        public Dictionary<ushort, object> lstCharSizeInfoLarge;
        public Dictionary<ushort, object> lstCharSizeInfoSmall;
        public Dictionary<ushort, object> dicGlyphLarge;
        public Dictionary<ushort, object> dicGlyphSmall;

        public Bitmap[] Images { get; set; }

        public FNT01(Stream fntFile, Bitmap bmp)
        {
            var tempCharSizeInfo = new List<XF01CharSizeInfo>();
            using (var fntR = new BinaryReaderX(fntFile, true))
            {
                xf1header = fntR.ReadStruct<XF01Header>();

                fntR.BaseStream.Position = xf1header.table0Offset << 2;
                t0Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                var decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                tempCharSizeInfo = new BinaryReaderX(decompressed).ReadMultiple<XF01CharSizeInfo>(xf1header.table0EntryCount);

                fntR.BaseStream.Position = xf1header.table1Offset << 2;
                t1Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                var largeChars = new BinaryReaderX(decompressed).ReadMultiple<XF01CharacterMap>(xf1header.table1EntryCount);
                dicGlyphLarge = new Dictionary<ushort, object>();
                foreach (XF01CharacterMap largeChar in largeChars)
                {
                    if (dicGlyphLarge.ContainsKey(largeChar.code_point))
                        continue;

                    dicGlyphLarge[largeChar.code_point] = largeChar;
                }

                fntR.BaseStream.Position = xf1header.table2Offset << 2;
                t2Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                var smallChars = new BinaryReaderX(decompressed).ReadMultiple<XF01CharacterMap>(xf1header.table2EntryCount);
                dicGlyphSmall = new Dictionary<ushort, object>();
                foreach (XF01CharacterMap smallChar in smallChars)
                {
                    if (dicGlyphSmall.ContainsKey(smallChar.code_point))
                        continue;

                    dicGlyphSmall[smallChar.code_point] = smallChar;
                }
            }
            
            lstCharSizeInfoLarge = new Dictionary<ushort, object>();
            lstCharSizeInfoSmall = new Dictionary<ushort, object>();
            foreach (var dic in dicGlyphLarge)
            {
                var charIndex = ((XF01CharacterMap)dic.Value).CharSizeInfoIndex;

                lstCharSizeInfoLarge.Add(dic.Key, new XF01CharSizeInfo
                {
                    offset_x = tempCharSizeInfo[charIndex].offset_x,
                    offset_y = tempCharSizeInfo[charIndex].offset_y,
                    char_width = tempCharSizeInfo[charIndex].char_width,
                    char_height = tempCharSizeInfo[charIndex].char_height
                });
            }
            foreach (var dic in dicGlyphSmall)
            {
                var charIndex = ((XF01CharacterMap)dic.Value).CharSizeInfoIndex;

                lstCharSizeInfoSmall.Add(dic.Key, new XF01CharSizeInfo
                {
                    offset_x = tempCharSizeInfo[charIndex].offset_x,
                    offset_y = tempCharSizeInfo[charIndex].offset_y,
                    char_width = tempCharSizeInfo[charIndex].char_width,
                    char_height = tempCharSizeInfo[charIndex].char_height
                });
            }

            var bmpInfo = new BitmapInfo(bmp);

            Images = new[]
            {
                bmpInfo.CreateChannelBitmap(BitmapInfo.Channel.Red),
                bmpInfo.CreateChannelBitmap(BitmapInfo.Channel.Green),
                bmpInfo.CreateChannelBitmap(BitmapInfo.Channel.Blue)
            };
        }

        public void Save(Stream output)
        {
            var compactCharSizeInfo = new List<object>();
            foreach (var info in lstCharSizeInfoLarge)
            {
                var glyph = (XF01CharacterMap)dicGlyphLarge[info.Key];

                if (compactCharSizeInfo.Contains(info.Value))
                {
                    glyph.char_size =
                        (ushort)(compactCharSizeInfo.FindIndex(c => c.Equals(info.Value)) % 1024 + glyph.CharWidth * 1024);
                }
                else
                {
                    glyph.char_size =
                        (ushort)(compactCharSizeInfo.Count % 1024 + glyph.CharWidth * 1024);

                    compactCharSizeInfo.Add(info.Value);
                }
            }

            foreach (var info in lstCharSizeInfoSmall)
            {
                var glyph = (XF01CharacterMap)dicGlyphSmall[info.Key];

                if (compactCharSizeInfo.Contains(info.Value))
                {
                    glyph.char_size =
                        (ushort)(compactCharSizeInfo.FindIndex(c => c.Equals(info.Value)) % 1024 + glyph.CharWidth * 1024);
                }
                else
                {
                    glyph.char_size =
                        (ushort)(compactCharSizeInfo.Count % 1024 + glyph.CharWidth * 1024);

                    compactCharSizeInfo.Add(info.Value);
                }
            }

            // Write data
            using (var bw = new BinaryWriterX(output, true))
            {
                // Table 0
                xf1header.table0EntryCount = (short)compactCharSizeInfo.Count;
                bw.BaseStream.Position = 0x28;
                bw.WriteMultipleCompressed(compactCharSizeInfo.Cast<XF01CharSizeInfo>(), t0Comp);
                bw.WriteAlignment(4);

                // Table 1
                xf1header.table1Offset = (short)(bw.BaseStream.Position >> 2);
                xf1header.table1EntryCount = (short)dicGlyphLarge.Count;
                bw.WriteMultipleCompressed(dicGlyphLarge.Select(d => (XF01CharacterMap)d.Value), t1Comp);
                bw.WriteAlignment(4);

                // Table 2
                xf1header.table2Offset = (short)(bw.BaseStream.Position >> 2);
                xf1header.table2EntryCount = (short)dicGlyphSmall.Count;
                bw.WriteMultipleCompressed(dicGlyphSmall.Select(d => (XF01CharacterMap)d.Value), t2Comp);
                bw.WriteAlignment(4);

                // Header
                bw.BaseStream.Position = 0;
                bw.WriteStruct(xf1header);
            }
        }
    }
}
