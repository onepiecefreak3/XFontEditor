using System;
using System.IO;
using System.Text;

namespace L5XFEditor.Format
{
    public static class FontHelper
    {
        public static FontType PeekFontType(Stream input)
        {
            var buffer = new byte[6];
            _ = input.Read(buffer, 0, 6);
            input.Position -= 6;

            var value = ((long)buffer[0] << 40) | ((long)buffer[1] << 32) | ((long)buffer[2] << 24) | ((long)buffer[3] << 16)| ((long)buffer[4] << 8)| buffer[5];
            switch (value)
            {
                case 0x464E54433030:
                    return FontType.Xf00;

                case 0x464E54433031:
                    return FontType.Xf01;

                default:
                    throw new InvalidOperationException($"Unsupported font format {Encoding.ASCII.GetString(buffer)}.");
            }
        }
    }

    public enum FontType
    {
        Xf00,
        Xf01
    }
}
