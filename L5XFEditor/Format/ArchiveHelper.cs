using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5XFEditor.Format
{
    public static class ArchiveHelper
    {
        public static ArchiveType PeekArchiveType(Stream input)
        {
            var buffer = new byte[4];
            _ = input.Read(buffer, 0, 4);
            input.Position -= 4;

            var value = (buffer[0] << 24) | (buffer[1] << 16)| (buffer[2] << 8)| buffer[3];
            switch (value)
            {
                case 0x58465350:
                    return ArchiveType.Xfsp;

                case 0x5850434B:
                    return ArchiveType.Xpck;

                default:
                    throw new InvalidOperationException($"Unsupported archive format {Encoding.ASCII.GetString(buffer)}.");
            }
        }
    }

    public enum ArchiveType
    {
        Xfsp,
        Xpck
    }
}
