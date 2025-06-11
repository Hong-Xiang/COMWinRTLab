using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsumer;

static partial class NativeMethods
{
    [LibraryImport("CppComponent.dll")]
    public static partial int AddC(int a, int b);

    [LibraryImport("CppComponent.dll")]
    public static partial nint CreateCalculator();
}
