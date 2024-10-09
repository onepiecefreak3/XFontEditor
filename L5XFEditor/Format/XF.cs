using System.Drawing;
using System.IO;
using L5XFEditor.IO;
using System;
using L5XFEditor.Format.FNT;
using System.Collections.Generic;

namespace L5XFEditor.Format
{
    public class XF : IDisposable
    {
        ArchiveType archiveType;
        XPCK xpck;
        XFSP xfsp;

        FontType fontType;
        FNT00 fnt00;
        FNT01 fnt01;

        IMGC xi;

        public Dictionary<ushort, object> CharSizeInfosLarge => fnt00?.lstCharSizeInfoLarge ?? fnt01.lstCharSizeInfoLarge;
        public Dictionary<ushort, object> CharSizeInfosSmall => fnt00?.lstCharSizeInfoSmall ?? fnt01.lstCharSizeInfoSmall;
        public Dictionary<ushort, object> GlyphsLarge => fnt00?.dicGlyphLarge ?? fnt01.dicGlyphLarge;
        public Dictionary<ushort, object> GlyphsSmall => fnt00?.dicGlyphSmall ?? fnt01.dicGlyphSmall;
        public Bitmap[] Images => fnt00?.Images ?? fnt01.Images;

        public XF(Stream input)
        {
            // Read archive
            archiveType = ArchiveHelper.PeekArchiveType(input);

            Stream fntFile;
            switch (archiveType)
            {
                case ArchiveType.Xpck:
                    xpck = new XPCK(input);
                    xi = new IMGC(xpck.Files[0].FileData);
                    fntFile = xpck.Files[1].FileData;
                    break;

                case ArchiveType.Xfsp:
                    xfsp = new XFSP(input);
                    xi = new IMGC(xfsp.Files[0].FileData);
                    fntFile = xfsp.Files[1].FileData;
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported archive format {archiveType}.");
            }

            // Read FNT
            fontType = FontHelper.PeekFontType(fntFile);

            switch (fontType)
            {
                case FontType.Xf00:
                    fnt00 = new FNT00(fntFile, xi.Image);
                    break;

                case FontType.Xf01:
                    fnt01 = new FNT01(fntFile, xi.Image);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported font format {fontType}.");
            }
        }

        public void Save(Stream output)
        {
            byte[][,] alphaMaps;
            switch (fontType)
            {
                case FontType.Xf00:
                    alphaMaps = new byte[][,]
                    {
                        new BitmapInfo(fnt00.Images[0]).pixelMap(BitmapInfo.Channel.Alpha),
                        new BitmapInfo(fnt00.Images[1]).pixelMap(BitmapInfo.Channel.Alpha),
                        new BitmapInfo(fnt00.Images[2]).pixelMap(BitmapInfo.Channel.Alpha)
                    };
                    break;

                case FontType.Xf01:
                    alphaMaps = new byte[][,]
                    {
                        new BitmapInfo(fnt01.Images[0]).pixelMap(BitmapInfo.Channel.Alpha),
                        new BitmapInfo(fnt01.Images[1]).pixelMap(BitmapInfo.Channel.Alpha),
                        new BitmapInfo(fnt01.Images[2]).pixelMap(BitmapInfo.Channel.Alpha)
                    };
                    break;

                default:
                    // Should never occur
                    return;
            }

            // Update image
            var img = new MemoryStream();

            xi.Image = new Bitmap(xi.Image.Width, xi.Image.Height);
            for (var y = 0; y < xi.Image.Height; y++)
                for (var x = 0; x < xi.Image.Width; x++)
                    xi.Image.SetPixel(x, y, Color.FromArgb(255, alphaMaps[0][x, y], alphaMaps[1][x, y], alphaMaps[2][x, y]));

            xi.Save(img);

            // Update font
            var font = new MemoryStream();

            switch (fontType)
            {
                case FontType.Xf00:
                    fnt00.Save(font);
                    break;

                case FontType.Xf01:
                    fnt01.Save(font);
                    break;

                default:
                    // Should never occur
                    return;
            }

            // Update archive
            switch (archiveType)
            {
                case ArchiveType.Xpck:
                    xpck.Files[0].FileData = img;
                    xpck.Files[1].FileData = font;
                    xpck.Save(output);
                    break;

                case ArchiveType.Xfsp:
                    xfsp.Files[0].FileData = img;
                    xfsp.Files[1].FileData = font;
                    xfsp.Save(output);
                    break;
            }
        }

        public void Dispose()
        {
            xpck?.Close();
            xfsp?.Close();
            xi = null;
        }
    }
}