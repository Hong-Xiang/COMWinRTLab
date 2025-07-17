using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClient;


public partial class CppCOMUsage
{
    void SimpleIUnknownAPICallTest()
    {
        var x = CppServerNativeMethods.CreateCppCOMObject();
        {
            Marshal.AddRef(x);
            Marshal.Release(x);
        }
        Marshal.Release(x);
    }

    void ComWrapperUsageTest()
    {
        var x = CppServerNativeMethods.CreateCppCOMObject();
        var o = (IHello)Marshal.GetObjectForIUnknown(x);
        o.Hello(42);
        var a = (IAdderInterface)o;
        Console.WriteLine($"calling add(1, 2) = {a.Add(1, 2)}");
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
