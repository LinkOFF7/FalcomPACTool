using FalcomPACTool.Archive;

namespace FalcomPACTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                PrintUsage();
                return;
            }
            if (args[0] == "extract")
            {
                FPAC fpac = new FPAC(File.OpenRead(args[1]));
                fpac.ExtractToDirectory(args[2]);
            }
            else
            {
                PrintUsage();
                return;
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Falcom PAC archive extractor (Trails in the Sky 1st Chapter) by LinkOFF\n");
            Console.WriteLine("Usage: FalcomPACTool.exe extract <PAC File> <Output Folder>");
        }
    }
}
