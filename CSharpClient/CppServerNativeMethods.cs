using System.Runtime.InteropServices;

namespace CSharpClient;

public static partial class CppServerNativeMethods
{
    [LibraryImport("CppServer.dll")]
    public static partial void HelloFromClientExport();

    [LibraryImport("CppServer.dll")]
    public static partial nint CreateMinimumCOMObject();

    [LibraryImport("CppServer.dll")]
    public static partial nint CreateCppCOMObject();
}
