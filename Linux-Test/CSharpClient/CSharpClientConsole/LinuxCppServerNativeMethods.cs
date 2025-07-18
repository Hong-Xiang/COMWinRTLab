using System.Runtime.InteropServices;

namespace CSharpClientConsole;

public static partial class LinuxCppServerNativeMethods
{
    [LibraryImport("libCppServer.so")]
    public static partial void HelloFromClientExport();

    [LibraryImport("libCppServer.so")]
    public static partial nint CreateMinimumCOMObject();

    [LibraryImport("libCppServer.so")]
    public static partial nint CreateCppCOMObject();
}
