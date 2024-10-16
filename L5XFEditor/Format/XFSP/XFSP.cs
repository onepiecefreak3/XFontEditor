﻿using L5XFEditor.Compression;
using L5XFEditor.Interface;
using L5XFEditor.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5XFEditor.Format
{
    public sealed class XFSP
    {
        public List<XFSPFileInfo> Files = new List<XFSPFileInfo>();
        Stream _stream = null;

        XFSPHeader header;
        List<XfspFileInfoEntry> entries = new List<XfspFileInfoEntry>();
        byte[] compNameTable;

        public XFSP(Stream input)
        {
            _stream = input;
            using (var br = new BinaryReaderX(input, true))
            {
                //Header
                header = br.ReadStruct<XFSPHeader>();

                //Entries
                br.BaseStream.Position = header.fileInfoOffset;
                entries.AddRange(br.ReadMultiple<XfspFileInfoEntry>(header.fileCount));

                //Filenames
                br.BaseStream.Position = header.filenameTableOffset;
                compNameTable = br.ReadBytes(header.filenameTableSize);
                var decNames = new MemoryStream(Level5.Decompress(new MemoryStream(compNameTable)));

                //Files
                using (var nameList = new BinaryReaderX(decNames))
                    for (int i = 0; i < entries.Count; i++)
                    {
                        nameList.BaseStream.Position = entries[i].nameOffset;
                        Files.Add(new XFSPFileInfo
                        {
                            State = ArchiveFileState.Archived,
                            FileName = nameList.ReadCStringA(),
                            FileData = new SubStream(br.BaseStream, header.dataOffset + entries[i].fileOffset, entries[i].fileSize),
                            Entry = entries[i]
                        });
                    }
            }
        }

        public void Save(Stream xpck)
        {
            using (BinaryWriterX bw = new BinaryWriterX(xpck))
            {
                int absDataOffset = header.dataOffset;

                //Files
                var files = Files.OrderBy(x => x.Entry.fileOffset).ToList();
                var dataOffset = absDataOffset;
                foreach (var file in files)
                {
                    dataOffset = file.Write(bw.BaseStream, dataOffset, absDataOffset);
                }

                //Entries
                bw.BaseStream.Position = 0x14;
                foreach (var file in Files)
                    bw.WriteStruct(file.Entry);

                //Namelist
                bw.Write(compNameTable);

                //Header
                header.tmp6 = (uint)(bw.BaseStream.Length - absDataOffset) >> 2;
                bw.BaseStream.Position = 0;
                bw.WriteStruct(header);
            }
        }

        public void Close()
        {
            _stream?.Close();
            _stream = null;
        }
    }
}
