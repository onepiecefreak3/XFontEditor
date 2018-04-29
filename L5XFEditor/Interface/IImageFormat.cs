using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace L5XFEditor.Interface
{
    public interface IImageFormat
    {
        int BitDepth { get; }

        string FormatName { get; }

        IEnumerable<Color> Load(byte[] input);
        byte[] Save(IEnumerable<Color> colors);
    }
}
