using ClientCSharp;
using System.CommandLine;
using System.Runtime.InteropServices;


namespace TestConsoleApp;

internal class SimpleObject
{
    public Command Command { get; }

    public SimpleObject()
    {
        Command cmd = new("simple-object", "Simple IUnknown Object Test");
        var producerArgument = new Argument<ComProducer>("producer");
        var consumerArgument = new Argument<ComConsumer>("consumer");
        cmd.Arguments.Add(producerArgument);
        cmd.Arguments.Add(consumerArgument);
        cmd.SetAction(result =>
        {
            var producer = result.GetValue(producerArgument);
            var consumer = result.GetValue(consumerArgument);

            Console.WriteLine($"SimpleObject(IUnknown) Test using Producer: {producer}, Consumer: {consumer}");

            switch ((producer, consumer))
            {
                case (ComProducer.C, ComConsumer.Marshal):
                    CMarshal();
                    break;
                default:
                    throw new NotImplementedException();
            }
        });
        Command = cmd;
    }

    public void CMarshal()
    {
        var obj = ServerCppNative.ComCCreateObject();
        Marshal.Release(obj);
    }

}
