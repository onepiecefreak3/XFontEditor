using L5XFEditor.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace L5XFEditor.Format.FNT
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class XF00Header
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
        public short table3Offset;
        public short table3EntryCount;
        public short table2Offset;
        public short table2EntryCount;
        public short table4Offset;
        public short table4EntryCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("[{offset_x}, {offset_y}, {char_width}, {char_height}]")]
    public class XF00CharSizeInfo
    {
        public uint image_info;
        public sbyte offset_x;
        public sbyte offset_y;
        public byte char_width;
        public byte char_height;

        public int CharWidth => (int)(image_info & 0xFF);
        public int ColorChannel => (int)((image_info >> 8) & 0xF);
        public int ImageOffsetX => (int)((image_info >> 12) & 0x3FF);
        public int ImageOffsetY => (int)(image_info >> 22);

        public override bool Equals(object obj)
        {
            if (obj is XF00CharSizeInfo)
            {
                var csi = (XF00CharSizeInfo)obj;
                return image_info == csi.image_info && offset_x == csi.offset_x && offset_y == csi.offset_y && char_width == csi.char_width && char_height == csi.char_height;
            }

            return base.Equals(obj);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    [DebuggerDisplay("[{code_point}]")]
    public class XF00CharacterMap
    {
        public ushort code_point;
        public ushort char_index;
    }
}
