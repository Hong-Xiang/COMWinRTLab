// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using TestConsoleApp;

RootCommand root = new("Mini COM/WinRT Test App");

root.Subcommands.Add(EchoGuid.Command);
root.Subcommands.Add(new SimpleObject().Command);
root.Subcommands.Add(new HelloObject().Command);
root.Subcommands.Add(new CalcObject().Command);
var parseResult = root.Parse(args);
parseResult.Invoke();
