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
[Guid("b0bf416d-9e3a-4d46-9377-af3db3cb10e4")]
[CoClass(typeof(Hello))]
internal interface Hello
{
}

[ComImport]
[Guid("56f52a44-2e07-4fce-be7a-6473e4ba0be8")]
sealed class HelloClass { }

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
                case (ComProducer.CoCreateInstance, ComConsumer.Marshal):
                    CoCreateInstanceMarshal();
                    break;
                case (ComProducer.CoCreateInstance, ComConsumer.ComWrapper):
                    CoCreateInstanceComWrapper();
                    break;
                case (ComProducer.Class, _):
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

    public nint CoCreateInstance()
    {
        Console.WriteLine("Calling CoCreateInstance");
        var hr = ServerCppNative.CoCreateInstance(typeof(Hello).GUID, IntPtr.Zero, 1, typeof(IHello).GUID, out var ptr);
        if (hr != 0)
        {
            throw new Exception($"CoCreateInstance failed with hr: {hr}");
        }
        return ptr;
    }

    public void ClassProducer()
    {
        Console.WriteLine("Calling Com Import Class");
        //var ptr = ServerCppNative.ComCCreateHello();
        //ComWrapperConsume(ptr);
    }


    public void CoCreateInstanceComWrapper()
    {
        Console.WriteLine("Calling CoCreateInstance");
        var ptr = CoCreateInstance();
        ComWrapperConsume(ptr);
    }

    public void CoCreateInstanceMarshal()
    {
        Console.WriteLine("Calling CoCreateInstance");
        var ptr = CoCreateInstance();
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
