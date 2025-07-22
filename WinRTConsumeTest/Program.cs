using System.Runtime.InteropServices;
using WinRT;

namespace WinRTConsumeTest;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("3B5DF111-F36D-5A0E-B6DE-801429A12289")]
partial interface IShape
{
    void GetIids(out int iidCount, out IntPtr iids);
    void GetRuntimeClassName(out IntPtr className);
    void GetTrustLevel(out TrustLevel trustLevel);
    void Show();
    float Area();
}

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("269414e8-dd21-5303-8acf-3089254dd6a1")]
partial interface IShapeFactory
{
    void GetIids(out int iidCount, out IntPtr iids);
    void GetRuntimeClassName(out IntPtr className);
    void GetTrustLevel(out TrustLevel trustLevel);
    IShape CreateInstance(float radius);
}


internal class Program
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate int DllGetActivationFactoryDelegate(IntPtr classId, out IntPtr factory);

    static void ShapeUsage(IShape shape)
    {
        shape.Show();
        Console.WriteLine("Area: " + shape.Area());
    }
    static void ShapeUsage(WinRTCppRuntimeComponent.Circle shape)
    {
        shape.Show();
        Console.WriteLine("Area: " + shape.Area());
    }

    static WinRTCppRuntimeComponent.Circle CreateCircleFromCsWinRT(float radius)
    {
        return new WinRTCppRuntimeComponent.Circle(radius);
    }

    static IShape CreateCircleFromNative(float radius)
    {
        var lib = NativeLibrary.Load("WinRTCppRuntimeComponent.dll");
        var activationFactoryPtr = NativeLibrary.GetExport(lib, "DllGetActivationFactory");

        var dllGetActivationFactory = Marshal.GetDelegateForFunctionPointer<DllGetActivationFactoryDelegate>(activationFactoryPtr);

        var className = "WinRTCppRuntimeComponent.Circle";
        var classNameHString = MarshalString.GetAbi(MarshalString.CreateMarshaler(className));

        var hr = dllGetActivationFactory(classNameHString, out IntPtr factoryPtr);
        if (hr < 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }

        Console.WriteLine("Successfully got activation factory!");
        var shapeFactoryCOM = Marshal.GetObjectForIUnknown(factoryPtr);
        var shapeFactory = (IShapeFactory)shapeFactoryCOM;
        var shape = shapeFactory.CreateInstance(radius);
        return shape;
    }

    static void Main(string[] args)
    {
        //var factory = WinRT.ActivationFactory.Get("WinRTCppRuntimeComponent.Circle", global::ABI.WinRTCppRuntimeComponent.ICircleFactoryMethods.IID);

        {
            Console.WriteLine("=== CsWinRT Standard Usage ===");
            var c = CreateCircleFromCsWinRT(2.0f);
            ShapeUsage(c);
        }

        {
            Console.WriteLine("=== WinRT Used as COM test ===");
            var c = new WinRTCppRuntimeComponent.Circle(1.0f);
            var objRef = ((IWinRTObject)c).NativeObject;
            var ptr = objRef.GetRef();
            var unknown = Marshal.GetObjectForIUnknown(ptr);
            var shape = (IShape)unknown;
            shape.GetRuntimeClassName(out var classNamePtr);
            Console.WriteLine(MarshalString.FromAbi(classNamePtr));
        }

        {
            Console.WriteLine("=== Native Usage ===");
            var c = CreateCircleFromNative(2.0f);
            ShapeUsage(c);
        }
    }
}
