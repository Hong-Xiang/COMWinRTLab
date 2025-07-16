using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClient;

public class MinimumCOMUsage
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



    public void Run()
    {
        SimpleIUnknownAPICallTest();
    }
}
