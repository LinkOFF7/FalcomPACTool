using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalcomPACTool.Archive
{
    internal class FPAC : IDisposable
    {
        private Stream _stream;

        public FPACHeader Header {  get; private set; }
        public List<FPACEntry> Entries { get; private set; }

        public FPAC(Stream stream)
        {
            _stream = stream;
            Header = new FPACHeader(stream);
            Entries = new List<FPACEntry>();
            for(int i = 0; i < Header.FileCount; i++)
                Entries.Add(new FPACEntry(stream));
        }

        public void ExtractToDirectory(string outputDir)
        {
            if (_stream is null) return;
            foreach(var entry in Entries)
            {
                string outPath = Path.Combine(outputDir, entry.FilePath);
                Console.WriteLine("Extracting: {0}", outPath);
                if(!Directory.Exists(Path.GetDirectoryName(outPath))) Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                File.WriteAllBytes(outPath, entry.GetBytes());
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
