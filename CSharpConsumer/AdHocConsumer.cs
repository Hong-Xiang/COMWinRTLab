using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsumer;

unsafe struct MyIUnknownVtbl
{
    public delegate* unmanaged[Stdcall]<void*, in Guid, out void*, int> QueryInterface;
    public delegate* unmanaged[Stdcall]<void*, uint> AddRef;
    public delegate* unmanaged[Stdcall]<void*, uint> Release;
}

unsafe struct MyICalculatorVtbl
{
    public delegate* unmanaged[Stdcall]<void*, in Guid, out void*, int> QueryInterface;
    public delegate* unmanaged[Stdcall]<void*, uint> AddRef;
    public delegate* unmanaged[Stdcall]<void*, uint> Release;
    public delegate* unmanaged[Stdcall]<void*, int> Hello;
    public delegate* unmanaged[Stdcall]<void*, int, int, int> Add;
}

unsafe struct MyICalculator2Vtbl
{
    public delegate* unmanaged[Stdcall]<void*, in Guid, out void*, int> QueryInterface;
    public delegate* unmanaged[Stdcall]<void*, uint> AddRef;
    public delegate* unmanaged[Stdcall]<void*, uint> Release;
    public delegate* unmanaged[Stdcall]<void*, int, int, out int, int> Mul;
}

unsafe struct PureVirtualObjectLayout<VTable>
{
    public VTable* VTablePtr;
}

public sealed unsafe class MyCalculator : SafeHandleZeroOrMinusOneIsInvalid, ICalculator
{
    private void* Instance { get; }
    private MyICalculatorVtbl* VTable { get; }

    public MyCalculator(IntPtr instance) : base(true)
    {
        Instance = (void*)instance;
        VTable = ((PureVirtualObjectLayout<MyICalculatorVtbl>*)instance)->VTablePtr;
        SetHandle(instance);
    }


    public int Add(int a, int b)
    {
        return VTable->Add(Instance, a, b);
    }

    public void Hello()
    {
        VTable->Hello(Instance);
    }

    protected override bool ReleaseHandle()
    {
        VTable->Release(Instance);
        return true;
    }

    public ICalculator2 AsCalculator2()
    {
        if (VTable->QueryInterface(Instance, typeof(ICalculator2).GUID, out var ptr) == 0)
        {
            return new MyCalculator2((IntPtr)ptr);
        }
        throw new Exception("Failed to query interface ICalculator2");
    }
}

sealed unsafe class MyCalculator2 : SafeHandleZeroOrMinusOneIsInvalid, ICalculator2
{
    private void* Instance { get; }
    private MyICalculator2Vtbl* VTable { get; }

    public MyCalculator2(IntPtr instance) : base(true)
    {
        Instance = (void*)instance;
        VTable = ((PureVirtualObjectLayout<MyICalculator2Vtbl>*)instance)->VTablePtr;
        SetHandle(instance);
    }

    public int Mul(int a, int b)
    {
        if (VTable->Mul(Instance, a, b, out var result) == 0)
        {
            return result;
        }
        throw new Exception("Failed to call Mul");
    }

    protected override bool ReleaseHandle()
    {
        VTable->Release(Instance);
        return true;
    }
}

public static class AdHocConsumer
{
    public static ICalculator CreateCalculator()
    {
        var ptr = NativeMethods.CreateCalculator();
        return new MyCalculator(ptr);
    }

    public static ICalculator2 CastAsCalculator2(ICalculator c)
    {
        var o = (MyCalculator)c;
        return o.AsCalculator2();
    }
}
