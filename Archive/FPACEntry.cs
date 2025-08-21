using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalcomPACTool.Archive
{
    internal class FPACEntry : IDisposable
    {
        private readonly Stream _stream;

        public ulong Field00 { get; private set; } // some CRC?
        public ulong NameOffset { get; private set; } // absolute
        public ulong DataSize { get; private set; }
        public ulong DataOffset { get; private set; } // absolute

        public string FilePath => GetFilename();
        public FPACEntry(Stream stream)
        {
            _stream = stream;
            if (_stream is not null)
            {
                Field00 = _stream.ReadU64();
                NameOffset = _stream.ReadU64();
                DataSize = _stream.ReadU64();
                DataOffset = _stream.ReadU64();
            }
        }
        public byte[] GetBytes()
        {
            _stream.Position = (long)DataOffset;
            return _stream.ReadBytes((uint)DataSize);
        }

        private string GetFilename()
        {
            string result = string.Empty;
            if (_stream is not null)
            {
                var savepos = _stream.Position;
                _stream.Position = (long)NameOffset;
                result = _stream.ReadCString(Encoding.UTF8);
                _stream.Position = savepos;
            }
            return result;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
