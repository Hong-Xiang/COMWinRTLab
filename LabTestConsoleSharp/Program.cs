using System.Runtime.InteropServices;

namespace LabTestConsoleSharp
{
    internal class Program
    {
        [DllImport("component.dll")]
        static extern int FuncWithoutExport();
        [DllImport("component.dll", EntryPoint = "?FuncWithDllExport@@YAHXZ")]
        static extern int FuncWithDllExport();
        [DllImport("component.dll")]
        static extern int FuncCWithDllExport();
        [DllImport("component.dll")]
        static extern int FuncWithDefExport();

        static void Main(string[] args)
        {
            // Console.WriteLine(FuncWithoutExport());
            Console.WriteLine(FuncWithDllExport());
            Console.WriteLine(FuncCWithDllExport());
            Console.WriteLine(FuncWithDefExport());
        }
    }
}
