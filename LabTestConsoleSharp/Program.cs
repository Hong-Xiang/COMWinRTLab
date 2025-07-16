using CSharpConsumer;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace LabTestConsoleSharp;

//[ComImport]
//[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[GeneratedComInterface]
[Guid("0D65A935-2857-4487-9CCD-633D40563C4E")]
internal partial interface ICalculator
{
    void Hello();
    [PreserveSig]
    int Add(int a, int b);
}

//[ComImport]
//[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[GeneratedComInterface]
[Guid("497A7BFE-BB9C-4DDF-914E-5DE5602325FF")]
internal partial interface ICalculator2
{
    int Mul(int a, int b);
}



internal partial class Program
{
    [LibraryImport("CppComponent.dll")]
    public static partial int AddC(int a, int b);

    [LibraryImport("CppComponent.dll")]
    public static partial nint CreateCalculator();

    unsafe static void SimpleRelease()
    {
        var calculator = CreateCalculator();
        Marshal.Release(calculator);
    }

    static void UseComWrapper()
    {
        var calculatorPtr = CreateCalculator();
        Console.WriteLine("after create in dotnet");
        var cw = new StrategyBasedComWrappers();
        //ICalculator calculator = (ICalculator)Marshal.GetObjectForIUnknown(calculatorPtr);
        var calculator = (ICalculator)cw.GetOrCreateObjectForComInstance(calculatorPtr, CreateObjectFlags.None);
        Console.WriteLine("calling release for initial ptr from dotnet");
        Marshal.Release(calculatorPtr);
        Console.WriteLine("calling hello");
        calculator.Hello();
        Console.WriteLine($"Add 40 2 = {calculator.Add(40, 2)}");


        ICalculator2 calculator2 = (ICalculator2)calculator;
        Console.WriteLine($"40 * 2 = {calculator2.Mul(40, 2)}");
    }

    static unsafe void UseManualVTable()
    {
        //var unknownPtr = CreateCalculator();
        //var myUnknown = (MyIUnknown*)(unknownPtr);
        //{
        //    var myCalculator = myUnknown->QueryInterface(myUnknown, typeof(ICalculator).GUID, out var calculatorPtr);
        //    var c = (MyICalculator*)calculatorPtr;
        //    c->Hello(myUnknown);
        //    var result = c->Add(myUnknown, 40, 20);
        //    Console.WriteLine($"40 + 20 = {result}");
        //    var c2p = myUnknown->QueryInterface(myUnknown, typeof(ICalculator2).GUID, out var calculator2Ptr);
        //    var c2 = (MyICalculator2*)calculator2Ptr;

        //    var mulResult = c2->Mul(myUnknown, 40, 2);
        //    Console.WriteLine($"40 * 2 = {mulResult}");

        //    c->Release(myUnknown);
        //}

        //myUnknown->Release(myUnknown);
        var c = AdHocConsumer.CreateCalculator();
        c.Hello();
        Console.WriteLine(c.Add(40, 2));
        var c2 = ((MyCalculator)c).AsCalculator2();
        Console.WriteLine(c2.Mul(2, 3));
    }

    static void Main(string[] args)
    {
        CSharpClient.CppServerNativeMethods.HelloFromClientExport();
        new CSharpClient.MinimumCOMUsage().Run();

        //Console.WriteLine(AddC(40, 2));
        //SimpleRelease();
        //UseComWrapper();
        //UseManualVTable();
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        //Console.ReadLine();
    }
}
