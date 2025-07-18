using System;
using System.Runtime.InteropServices;

namespace CSharpClientConsole;

public class LinuxDirectInterop
{
    // Direct P/Invoke calls for demonstration
    [DllImport("libCppServer.so")]
    private static extern void HelloFromClientExport();
    
    [DllImport("libCppServer.so")]
    private static extern IntPtr CreateCppCOMObject();
    
    [DllImport("libCppServer.so")]
    private static extern IntPtr CreateMinimumCOMObject();

    // Direct VTable manipulation functions
    [DllImport("libCppServer.so")]
    private static extern int CallHelloDirectly(IntPtr obj, int data);

    [DllImport("libCppServer.so")]
    private static extern int CallAddDirectly(IntPtr obj, int a, int b);

    [DllImport("libCppServer.so")]
    private static extern int GetRefCount(IntPtr obj);

    public void TestDirectInterop()
    {
        Console.WriteLine("\n=== Linux Direct Interop Test ===");
        
        // Test basic native call
        Console.WriteLine("1. Testing basic native function call:");
        HelloFromClientExport();
        
        // Test object creation and basic Marshal operations
        Console.WriteLine("\n2. Testing object creation and Marshal operations:");
        var obj = CreateCppCOMObject();
        Console.WriteLine($"Created combined object at: {obj:X}");
        Console.WriteLine($"Initial ref count: {GetRefCount(obj)}");
        
        // Test AddRef/Release which should work on Linux
        Console.WriteLine("\n3. Testing AddRef/Release:");
        Marshal.AddRef(obj);
        Console.WriteLine($"After AddRef, ref count: {GetRefCount(obj)}");
        Marshal.Release(obj);
        Console.WriteLine($"After Release, ref count: {GetRefCount(obj)}");
        
        // Test direct function calls
        Console.WriteLine("\n4. Testing direct function calls:");
        int helloResult = CallHelloDirectly(obj, 999);
        Console.WriteLine($"CallHelloDirectly result: {helloResult}");
        
        int addResult = CallAddDirectly(obj, 10, 20);
        Console.WriteLine($"CallAddDirectly(10, 20) result: {addResult}");
        
        // Test simple object too
        Console.WriteLine("\n5. Testing simple object:");
        var simpleObj = CreateMinimumCOMObject();
        Console.WriteLine($"Created simple object at: {simpleObj:X}");
        Console.WriteLine($"Simple object ref count: {GetRefCount(simpleObj)}");
        
        int simpleHelloResult = CallHelloDirectly(simpleObj, 777);
        Console.WriteLine($"Simple object CallHelloDirectly result: {simpleHelloResult}");
        
        // Clean up
        Console.WriteLine("\n6. Cleaning up:");
        Marshal.Release(obj); // Final release for combined object
        Marshal.Release(simpleObj); // Release simple object
        
        Console.WriteLine("Direct interop test completed successfully!");
    }
}
