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
    public class XF01Header
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("[{offset_x}, {offset_y}, {char_width}, {char_height}]")]
    public class XF01CharSizeInfo
    {
        public sbyte offset_x;
        public sbyte offset_y;
        public byte char_width;
        public byte char_height;

        public override bool Equals(object obj)
        {
            if (obj is XF01CharSizeInfo)
            {
                var csi = (XF01CharSizeInfo)obj;
                return offset_x == csi.offset_x && offset_y == csi.offset_y && char_width == csi.char_width && char_height == csi.char_height;
            }

            return base.Equals(obj);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    [DebuggerDisplay("[{code_point}] {ColorChannel}:{ImageOffsetX}:{ImageOffsetY}")]
    public class XF01CharacterMap
    {
        public ushort code_point;
        public ushort char_size;
        public int image_offset;

        public int CharSizeInfoIndex => char_size & 0x3FF;
        public int CharWidth => char_size >> 10;
        public int ColorChannel => image_offset & 0xF;
        public int ImageOffsetX => (image_offset >> 4) & 0x3FFF;
        public int ImageOffsetY => image_offset >> 18;
    }
}
