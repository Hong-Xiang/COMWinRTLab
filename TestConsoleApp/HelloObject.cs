using System.CommandLine;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;


namespace TestConsoleApp;

[GeneratedComInterface]
[ComImport]
[Guid("b0bf416d-9e3a-4d46-9377-af3db3cb10e4")]
partial interface IHello
{
    void SayHello();
}

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("b0bf416d-9e3a-4d46-9377-af3db3cb10e4")]
interface IHelloJIT
{
    void SayHello();
}


internal class HelloObject
{
    public Command Command { get; }

    public HelloObject()
    {
        Command cmd = new("hello-object", "IHello Object Test");
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
                case (ComProducer.C, ComConsumer.ComWrapper):
                    CComWrapper();
                    break;
                case (ComProducer.Cpp, ComConsumer.Marshal):
                    CppMarshal();
                    break;
                case (ComProducer.Cpp, ComConsumer.ComWrapper):
                    CppComWrapper();
                    break;
                default:
                    throw new NotImplementedException();
            }
            GC.Collect();
        });
        Command = cmd;
    }

    void MarshalConsume(nint ptr)
    {
        Console.WriteLine("Getting dotnet wrapper");
        var unknown = Marshal.GetObjectForIUnknown(ptr);
        Console.WriteLine("Release original ptr");
        Marshal.Release(ptr);
        Console.WriteLine("Casting");
        var hello = (IHelloJIT)unknown;
        hello.SayHello();
    }

    void ComWrapperConsume(nint ptr)
    {
        Console.WriteLine("Getting dotnet wrapper");
        var cw = new StrategyBasedComWrappers();
        var unknown = cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
        Console.WriteLine("Release original ptr");
        Marshal.Release(ptr);
        Console.WriteLine("Casting");
        var hello = (IHello)unknown;
        hello.SayHello();
    }



    public void CMarshal()
    {
        Console.WriteLine("Calling Factory");
        var ptr = ServerCppNative.ComCCreateHello();
        MarshalConsume(ptr);
    }

    public void CppMarshal()
    {
        Console.WriteLine("Calling Factory");
        var ptr = ServerCppNative.ComCppCreateHello();
        MarshalConsume(ptr);
    }

    public void CComWrapper()
    {
        Console.WriteLine("Calling Factory");
        var ptr = ServerCppNative.ComCppCreateHello();
        ComWrapperConsume(ptr);
    }
    public void CppComWrapper()
    {
        Console.WriteLine("Calling Factory");
        var ptr = ServerCppNative.ComCppCreateHello();
        ComWrapperConsume(ptr);
    }

}
