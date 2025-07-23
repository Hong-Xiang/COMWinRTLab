using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TestConsoleApp;

public static partial class ServerCppNative
{
    [LibraryImport("ServerCpp.dll")]
    public static partial void PrintGUID(Guid guid);

    [LibraryImport("ServerCpp.dll")]
    public static partial nint ComCCreateObject();

    [LibraryImport("ServerCpp.dll")]
    public static partial nint ComCCreateHello();

    [LibraryImport("ServerCpp.dll")]
    public static partial nint ComCppCreateObject();

    [LibraryImport("ServerCpp.dll")]
    public static partial nint ComCppCreateHello();

    [LibraryImport("ServerCpp.dll")]
    public static partial nint ComCppCreateCalc();

    [LibraryImport("ole32.dll")]
    public static partial int CoCreateInstance(
        in Guid rclsid,
        nint pUnkOuter,
        int dwClsContext,
        in Guid riid,
        out nint ppv);
    [LibraryImport("ole32.dll")]
    public static partial int CoInitializeEx(nint pvReserved, UInt32 dwCoInit);
    [LibraryImport("ole32.dll")]
    public static partial uint CoUninitialize();

}
