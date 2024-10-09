using L5XFEditor.Compression;
using L5XFEditor.IO;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace L5XFEditor.Format.FNT
{
    public sealed class FNT00
    {
        Encoding sjis;

        XF00Header xf0header;

        Level5.Method t0Comp;
        Level5.Method t1Comp;
        Level5.Method t2Comp;
        Level5.Method t3Comp;
        Level5.Method t4Comp;

        public Dictionary<ushort, object> lstCharSizeInfoLarge;
        public Dictionary<ushort, object> lstCharSizeInfoSmall;
        public Dictionary<ushort, object> dicGlyphLarge;
        public Dictionary<ushort, object> dicGlyphSmall;

        Dictionary<ushort, XF00CharacterMap> dicUnicodeGlyphLarge;
        Dictionary<ushort, XF00CharacterMap> dicUnicodeGlyphSmall;

        public Bitmap[] Images { get; set; }

        public FNT00(Stream fntFile, Bitmap bmp)
        {
            sjis = Encoding.GetEncoding("Shift-JIS");

            List<XF00CharSizeInfo> tempCharSizeInfo;
            using (var fntR = new BinaryReaderX(fntFile, true))
            {
                xf0header = fntR.ReadStruct<XF00Header>();

                fntR.BaseStream.Position = xf0header.table0Offset << 2;
                t0Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                var decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                tempCharSizeInfo = new BinaryReaderX(decompressed).ReadMultiple<XF00CharSizeInfo>(xf0header.table0EntryCount);

                fntR.BaseStream.Position = xf0header.table1Offset << 2;
                t3Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                var sjisLargeChars = new BinaryReaderX(decompressed).ReadMultiple<XF00CharacterMap>(xf0header.table1EntryCount);
                dicGlyphLarge = new Dictionary<ushort, object>();
                foreach (XF00CharacterMap largeChar in sjisLargeChars)
                {
                    largeChar.code_point = ConvertToUnicode(largeChar.code_point);
                    if (dicGlyphLarge.ContainsKey(largeChar.code_point))
                        continue;

                    dicGlyphLarge[largeChar.code_point] = largeChar;
                }

                fntR.BaseStream.Position = xf0header.table3Offset << 2;
                t4Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                var sjisSmallChars = new BinaryReaderX(decompressed).ReadMultiple<XF00CharacterMap>(xf0header.table3EntryCount);
                dicGlyphSmall = new Dictionary<ushort, object>();
                foreach (XF00CharacterMap smallChar in sjisSmallChars)
                {
                    smallChar.code_point = ConvertToUnicode(smallChar.code_point);
                    if (dicGlyphSmall.ContainsKey(smallChar.code_point))
                        continue;

                    dicGlyphSmall[smallChar.code_point] = smallChar;
                }

                fntR.BaseStream.Position = xf0header.table2Offset << 2;
                t1Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                var unicodeLargeChars = new BinaryReaderX(decompressed).ReadMultiple<XF00CharacterMap>(xf0header.table2EntryCount);
                dicUnicodeGlyphLarge = new Dictionary<ushort, XF00CharacterMap>();
                foreach (XF00CharacterMap largeChar in unicodeLargeChars)
                {
                    largeChar.code_point = ConvertToUnicode(largeChar.code_point);
                    if (dicUnicodeGlyphLarge.ContainsKey(largeChar.code_point))
                        continue;

                    dicUnicodeGlyphLarge[largeChar.code_point] = largeChar;
                }

                fntR.BaseStream.Position = xf0header.table4Offset << 2;
                t2Comp = (Level5.Method)(fntR.ReadInt32() & 0x7);
                fntR.BaseStream.Position -= 4;
                decompressed = new MemoryStream(Level5.Decompress(fntR.BaseStream));
                var unicodeSmallChars = new BinaryReaderX(decompressed).ReadMultiple<XF00CharacterMap>(xf0header.table4EntryCount);
                dicUnicodeGlyphSmall = new Dictionary<ushort, XF00CharacterMap>();
                foreach (XF00CharacterMap smallChar in unicodeSmallChars)
                {
                    smallChar.code_point = ConvertToUnicode(smallChar.code_point);
                    if (dicUnicodeGlyphSmall.ContainsKey(smallChar.code_point))
                        continue;

                    dicUnicodeGlyphSmall[smallChar.code_point] = smallChar;
                }
            }

            lstCharSizeInfoLarge = new Dictionary<ushort, object>();
            lstCharSizeInfoSmall = new Dictionary<ushort, object>();
            foreach (var dic in dicGlyphLarge)
            {
                var charIndex = ((XF00CharacterMap)dic.Value).char_index;

                lstCharSizeInfoLarge.Add(dic.Key, new XF00CharSizeInfo
                {
                    offset_x = tempCharSizeInfo[charIndex].offset_x,
                    offset_y = tempCharSizeInfo[charIndex].offset_y,
                    char_width = tempCharSizeInfo[charIndex].char_width,
                    char_height = tempCharSizeInfo[charIndex].char_height,
                    image_info = tempCharSizeInfo[charIndex].image_info
                });
            }
            foreach (var dic in dicGlyphSmall)
            {
                var charIndex = ((XF00CharacterMap)dic.Value).char_index;

                lstCharSizeInfoSmall.Add(dic.Key, new XF00CharSizeInfo
                {
                    offset_x = tempCharSizeInfo[charIndex].offset_x,
                    offset_y = tempCharSizeInfo[charIndex].offset_y,
                    char_width = tempCharSizeInfo[charIndex].char_width,
                    char_height = tempCharSizeInfo[charIndex].char_height,
                    image_info = tempCharSizeInfo[charIndex].image_info
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
            var compactCharSizeInfo = new List<XF00CharSizeInfo>();
            foreach (var info in lstCharSizeInfoLarge)
            {
                var glyph = (XF00CharacterMap)dicGlyphLarge[info.Key];

                if (compactCharSizeInfo.Contains(info.Value))
                {
                    var index = (ushort)compactCharSizeInfo.FindIndex(c => c.Equals(info.Value));

                    if (dicUnicodeGlyphLarge.ContainsKey(info.Key))
                        dicUnicodeGlyphLarge[info.Key].char_index = index;

                    glyph.char_index = index;
                }
                else
                {
                    if (dicUnicodeGlyphLarge.ContainsKey(info.Key))
                        dicUnicodeGlyphLarge[info.Key].char_index = (ushort)compactCharSizeInfo.Count;

                    glyph.char_index = (ushort)compactCharSizeInfo.Count;

                    compactCharSizeInfo.Add((XF00CharSizeInfo)info.Value);
                }
            }

            foreach (var info in lstCharSizeInfoSmall)
            {
                var glyph = (XF00CharacterMap)dicGlyphSmall[info.Key];

                if (compactCharSizeInfo.Contains(info.Value))
                {
                    var index = (ushort)compactCharSizeInfo.FindIndex(c => c.Equals(info.Value));

                    if (dicUnicodeGlyphSmall.ContainsKey(info.Key))
                        dicUnicodeGlyphSmall[info.Key].char_index = index;

                    glyph.char_index = index;
                }
                else
                {
                    if (dicUnicodeGlyphSmall.ContainsKey(info.Key))
                        dicUnicodeGlyphSmall[info.Key].char_index = (ushort)compactCharSizeInfo.Count;

                    glyph.char_index = (ushort)compactCharSizeInfo.Count;

                    compactCharSizeInfo.Add((XF00CharSizeInfo)info.Value);
                }
            }

            // Write data
            using (var bw = new BinaryWriterX(output, true))
            {
                // Table 0
                xf0header.table0EntryCount = (short)compactCharSizeInfo.Count;
                bw.BaseStream.Position = 0x30;
                bw.WriteMultipleCompressed(compactCharSizeInfo, t0Comp);
                bw.WriteAlignment(4);

                // Table 1
                xf0header.table1Offset = (short)(bw.BaseStream.Position >> 2);
                xf0header.table1EntryCount = (short)dicGlyphLarge.Count;
                bw.WriteMultipleCompressed(dicGlyphLarge.Select(d =>
                {
                    ((XF00CharacterMap)d.Value).code_point = ConvertToSjis(((XF00CharacterMap)d.Value).code_point);
                    return (XF00CharacterMap)d.Value;
                }), t3Comp);
                bw.WriteAlignment(4);

                // Table 3
                xf0header.table3Offset = (short)(bw.BaseStream.Position >> 2);
                xf0header.table3EntryCount = (short)dicGlyphSmall.Count;
                bw.WriteMultipleCompressed(dicGlyphSmall.Select(d =>
                {
                    ((XF00CharacterMap)d.Value).code_point = ConvertToSjis(((XF00CharacterMap)d.Value).code_point);
                    return (XF00CharacterMap)d.Value;
                }), t4Comp);
                bw.WriteAlignment(4);

                // Table 2
                xf0header.table2Offset = (short)(bw.BaseStream.Position >> 2);
                xf0header.table2EntryCount = (short)dicUnicodeGlyphLarge.Count;
                bw.WriteMultipleCompressed(dicUnicodeGlyphLarge.Select(d => d.Value), t1Comp);
                bw.WriteAlignment(4);

                // Table 4
                xf0header.table4Offset = (short)(bw.BaseStream.Position >> 2);
                xf0header.table4EntryCount = (short)dicUnicodeGlyphSmall.Count;
                bw.WriteMultipleCompressed(dicUnicodeGlyphSmall.Select(d => d.Value), t2Comp);
                bw.WriteAlignment(4);

                // Header
                bw.BaseStream.Position = 0;
                bw.WriteStruct(xf0header);
            }
        }

        private ushort ConvertToUnicode(ushort codePoint)
        {
            var buffer = (codePoint & 0xFF00) != 0 ?
                new[] { (byte)(codePoint >> 8), (byte)codePoint } :
                new[] { (byte)codePoint };
            return sjis.GetString(buffer)[0];
        }

        private ushort ConvertToSjis(ushort codePoint)
        {
            var buffer = sjis.GetBytes(new[] { (char)codePoint });
            if (buffer.Length == 1)
                return buffer[0];

            return (ushort)((buffer[0] << 8) | buffer[1]);
        }
    }
}
