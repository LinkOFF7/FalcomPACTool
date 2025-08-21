using FalcomPACTool.Archive;

namespace FalcomPACTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3) return;
            if (args[0] == "extract")
            {
                FPAC fpac = new FPAC(File.OpenRead(args[1]));
                fpac.ExtractToDirectory(args[2]);
            }
        }
    }
}