using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CSharpClient;

public partial class MinimumCOMUsage
{

  
    void SimpleIUnknownAPICallTest()
    {
        var x = CppServerNativeMethods.CreateMinimumCOMObject();
        {
            Marshal.AddRef(x);
            Marshal.Release(x);
        }
        Marshal.Release(x);
    }

    void ComWrapperUsageTest()
    {
        var x = CppServerNativeMethods.CreateMinimumCOMObject();
        var o = (IHelloInterface)Marshal.GetObjectForIUnknown(x);
        o.Hello(42);
        Marshal.Release(x);
    }



    public void Run()
    {
        Console.WriteLine("Simple Marshal API Tests");
        SimpleIUnknownAPICallTest();
        Console.WriteLine();

        Console.WriteLine("COM Wrapper API Tests");
        ComWrapperUsageTest();
        GC.Collect();
    }
}
