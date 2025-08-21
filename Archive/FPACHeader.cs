using System.Text;

namespace FalcomPACTool.Archive
{
    internal class FPACHeader : IDisposable
    {
        private Stream _stream;
        public uint MagicNumber { get; private set; } // FPAC
        public uint FileCount { get; private set; }
        public uint DataStartOffset { get; private set; }
        private int Field0C { get; set; } // 1

        public string MagicWord => Encoding.ASCII.GetString(BitConverter.GetBytes(MagicNumber));

        public FPACHeader(Stream stream)
        {
            _stream = stream;
            if (_stream is not null)
            {
                MagicNumber = _stream.ReadU32();
                if (MagicNumber != 0x43415046) 
                    throw new Exception("Unknown magic. FPAC was expected.");
                FileCount = _stream.ReadU32();
                DataStartOffset = _stream.ReadU32();
                Field0C = _stream.ReadS32();
            }
        }
        public void Write()
        {
            if (_stream is not null)
            {
                _stream.WriteU32(MagicNumber);
                _stream.WriteU32(FileCount);
                _stream.WriteU32(DataStartOffset);
                _stream.WriteS32(Field0C);
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
