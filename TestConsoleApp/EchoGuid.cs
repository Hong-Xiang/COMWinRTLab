using ClientCSharp;
using System.CommandLine;

namespace TestConsoleApp;

internal class EchoGuid
{
    private static Command CreateCommand()
    {
        Command cmd = new("echo-guid", "Echo a GUID") { };
        cmd.SetAction((_) =>
        {
            var guid = Guid.NewGuid();
            Console.WriteLine($"Generated guid {guid}");
            ServerCppNative.PrintGUID(guid);
        });
        return cmd;
    }
    public static readonly Command Command = CreateCommand();
}
