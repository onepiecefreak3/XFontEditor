﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L5XFEditor.Interface;
using System.Drawing;
using L5XFEditor.IO;
using System.IO;

namespace L5XFEditor.Image.Format
{
    public class RGBA : IImageFormat
    {
        public int BitDepth { get; set; }

        public string FormatName { get; set; }

        int rDepth;
        int gDepth;
        int bDepth;
        int aDepth;

        ByteOrder byteOrder;
        bool swapChannels;

        public RGBA(int r, int g, int b, int a = 0, ByteOrder byteOrder = ByteOrder.LittleEndian, bool swapChannels = false, bool standard = false)
        {
            BitDepth = r + g + b + a;
            if (BitDepth < 8) throw new Exception($"Overall bitDepth can't be smaller than 8. Given bitDepth: {BitDepth}");
            if (BitDepth > 32) throw new Exception($"Overall bitDepth can't be bigger than 32. Given bitDepth: {BitDepth}");

            this.byteOrder = byteOrder;
            this.swapChannels = swapChannels;

            rDepth = r;
            gDepth = g;
            bDepth = b;
            aDepth = a;

            FormatName = ((standard) ? "s" : "") + ((swapChannels) ?
                ((a != 0) ? "A" : "") + "BGR" + ((a != 0) ? a.ToString() : "") + b.ToString() + g.ToString() + r.ToString() :   //ABGR
                "RGB" + ((a != 0) ? "A" : "") + r.ToString() + g.ToString() + b.ToString() + ((a != 0) ? a.ToString() : ""));   //RGBA
        }

        public IEnumerable<Color> Load(byte[] tex)
        {
            using (var br = new BinaryReaderX(new MemoryStream(tex), byteOrder))
            {
                int bShift, gShift, rShift, aShift;
                if (swapChannels)
                {
                    rShift = 0;
                    gShift = rDepth;
                    bShift = gShift + gDepth;
                    aShift = bShift + bDepth;
                }
                else
                {
                    aShift = 0;
                    bShift = aDepth;
                    gShift = bShift + bDepth;
                    rShift = gShift + gDepth;
                }

                var aBitMask = (1 << aDepth) - 1;
                var bBitMask = (1 << bDepth) - 1;
                var gBitMask = (1 << gDepth) - 1;
                var rBitMask = (1 << rDepth) - 1;

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    long value = 0;

                    if (BitDepth <= 8)
                        value = br.ReadByte();
                    else if (BitDepth <= 16)
                        value = br.ReadUInt16();
                    else if (BitDepth <= 24)
                    {
                        var tmp = br.ReadBytes(3);
                        value = (byteOrder == ByteOrder.LittleEndian) ? tmp[2] << 16 | tmp[1] << 8 | tmp[0] : tmp[0] << 16 | tmp[1] << 8 | tmp[0];
                    }
                    else if (BitDepth <= 32)
                        value = br.ReadUInt32();
                    else
                        throw new Exception($"BitDepth {BitDepth} not supported!");

                    yield return Color.FromArgb(
                        (aDepth == 0) ? 255 : Support.Support.ChangeBitDepth((int)(value >> aShift & aBitMask), aDepth, 8),
                        Support.Support.ChangeBitDepth((int)(value >> rShift & rBitMask), rDepth, 8),
                        Support.Support.ChangeBitDepth((int)(value >> gShift & gBitMask), gDepth, 8),
                        Support.Support.ChangeBitDepth((int)(value >> bShift & bBitMask), bDepth, 8));
                }
            }
        }

        public byte[] Save(IEnumerable<Color> colors)
        {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriterX(ms, true, byteOrder))
                foreach (var color in colors)
                {
                    var a = (aDepth == 0) ? 0 : Support.Support.ChangeBitDepth(color.A, 8, aDepth);
                    var r = Support.Support.ChangeBitDepth(color.R, 8, rDepth);
                    var g = Support.Support.ChangeBitDepth(color.G, 8, gDepth);
                    var b = Support.Support.ChangeBitDepth(color.B, 8, bDepth);

                    int rShift, bShift, gShift, aShift;
                    if (swapChannels)
                    {
                        rShift = 0;
                        gShift = rDepth;
                        bShift = gShift + gDepth;
                        aShift = bShift + bDepth;
                    }
                    else
                    {
                        aShift = 0;
                        bShift = aDepth;
                        gShift = bShift + bDepth;
                        rShift = gShift + gDepth;
                    }

                    long value = 0;
                    value |= (uint)(a << aShift);
                    value |= (uint)(b << bShift);
                    value |= (uint)(g << gShift);
                    value |= (uint)(r << rShift);

                    if (BitDepth <= 8)
                        bw.Write((byte)value);
                    else if (BitDepth <= 16)
                        bw.Write((ushort)value);
                    else if (BitDepth <= 24)
                    {
                        var tmp = (byteOrder == ByteOrder.LittleEndian) ?
                                new byte[] { (byte)(value & 0xff), (byte)(value >> 8 & 0xff), (byte)(value >> 16 & 0xff) } :
                                new byte[] { (byte)(value >> 16 & 0xff), (byte)(value >> 8 & 0xff), (byte)(value & 0xff) };
                        bw.Write(tmp);
                    }
                    else if (BitDepth <= 32)
                        bw.Write((uint)value);
                    else
                        throw new Exception($"BitDepth {BitDepth} not supported!");
                }

            return ms.ToArray();
        }
    }
}
