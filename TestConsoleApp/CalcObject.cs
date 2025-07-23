using System.CommandLine;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;


namespace TestConsoleApp;

[GeneratedComInterface]
//[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("c30c6ecf-41d2-4628-982f-ecb48bddc4b8")]
partial interface IAdder
{
    int Add(int a, int b);
    [PreserveSig]
    int AddSig(int a, int b);
}

[GeneratedComInterface]
//[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("2f4f04ed-1b83-4569-926b-b632d9970321")]
partial interface ICalc
{
    void Hello();
    IAdder GetAdder();
    //nint GetAdder();
}

[Guid("45daa0f2-bdd0-4b33-9629-804ce225affb")]
sealed class Calc
{

}

internal class CalcObject
{
    public Command Command { get; }

    public CalcObject()
    {
        Command cmd = new("calc-object", "ICalc Object Test");
        var producerArgument = new Argument<ComProducer>("producer");
        var consumerArgument = new Argument<ComConsumer>("consumer");
        cmd.Arguments.Add(producerArgument);
        cmd.Arguments.Add(consumerArgument);
        cmd.SetAction(result =>
        {
            var producer = result.GetValue(producerArgument);
            var consumer = result.GetValue(consumerArgument);

            Console.WriteLine($"CalcObject(IUnknown) Test using Producer: {producer}, Consumer: {consumer}");

            switch ((producer, consumer))
            {
                case (ComProducer.Cpp, ComConsumer.Marshal):
                    MarshalConsume(ServerCppNative.ComCppCreateCalc());
                    break;
                case (ComProducer.Cpp, ComConsumer.ComWrapper):
                    ComWrapperConsume(ServerCppNative.ComCppCreateCalc());
                    break;
                case (ComProducer.CoCreateInstance, ComConsumer.Marshal):
                    MarshalConsume(CoCreateInstance());
                    break;
                case (ComProducer.CoCreateInstance, ComConsumer.ComWrapper):
                    ComWrapperConsume(CoCreateInstance());
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
        var calc = (ICalc)unknown;
        calc.Hello();
        Console.WriteLine("Create Adder");
        var adder = calc.GetAdder();
        //var adderPtr = calc.GetAdder();
        //var adder = (IAdder)Marshal.GetObjectForIUnknown(adderPtr);
        //Marshal.Release(adderPtr);
        Console.WriteLine($"Add 40 + 2 = {adder.Add(40, 2)}");
        Console.WriteLine($"AddSig 40 + 2 = {adder.AddSig(40, 2)}");
    }

    void ComWrapperConsume(nint ptr)
    {
        Console.WriteLine("Getting dotnet wrapper");
        var cw = new StrategyBasedComWrappers();
        var unknown = cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
        Console.WriteLine("Release original ptr");
        Marshal.Release(ptr);
        Console.WriteLine("Casting");
        var calc = (ICalc)unknown;
        calc.Hello();
        Console.WriteLine("Create Adder");
        //var adderPtr = calc.GetAdder();
        //var adderWrappers = new StrategyBasedComWrappers();
        //var adderRCW = adderWrappers.GetOrCreateObjectForComInstance(adderPtr, CreateObjectFlags.None);
        //var adder = (IAdder)adderRCW;
        //Marshal.Release(adderPtr);
        var adder = calc.GetAdder();
        Console.WriteLine($"Add 40 + 2 = {adder.Add(40, 2)}");
        Console.WriteLine($"AddSig 40 + 2 = {adder.AddSig(40, 2)}");
    }

    public nint CoCreateInstance()
    {
        Console.WriteLine("Calling CoCreateInstance");
        var hr = ServerCppNative.CoCreateInstance(typeof(Calc).GUID, IntPtr.Zero, 1 | 2, typeof(ICalc).GUID, out var ptr);
        if (hr < 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
        return ptr;
    }
}
