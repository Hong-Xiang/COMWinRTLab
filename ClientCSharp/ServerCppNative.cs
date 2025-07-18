using System.Runtime.InteropServices;

namespace ClientCSharp;

public static partial class ServerCppNative
{
    [LibraryImport("ServerCpp.dll")]
    public static partial void PrintGUID(Guid guid);

    [LibraryImport("ServerCpp.dll")]
    public static partial nint ComCCreateObject();

    [LibraryImport("ServerCpp.dll")]
    public static partial nint ComCCreateHello();
}