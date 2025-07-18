using System;
using System.Runtime.InteropServices;

namespace CSharpClientConsole;

public class LinuxCppCOMUsage
{
    void SimpleIUnknownAPICallTest()
    {
        Console.WriteLine("=== Simple IUnknown API Call Test ===");
        var x = LinuxCppServerNativeMethods.CreateCppCOMObject();
        Console.WriteLine($"Created COM object: {x:X}");
        
        {
            Marshal.AddRef(x);
            Console.WriteLine("AddRef called");
            Marshal.Release(x);
            Console.WriteLine("Release called");
        }
        Marshal.Release(x);
        Console.WriteLine("Final Release called");
    }

    void ComWrapperUsageTest()
    {
        Console.WriteLine("\n=== COM Wrapper Usage Test ===");
        var x = LinuxCppServerNativeMethods.CreateCppCOMObject();
        Console.WriteLine($"Created COM object: {x:X}");
        
        try 
        {
            var o = (IHello)Marshal.GetObjectForIUnknown(x);
            Console.WriteLine("Successfully got IHello interface");
            
            o.Hello(42);
            Console.WriteLine("Called Hello method");
            
            var a = (IAdderInterface)o;
            Console.WriteLine("Successfully got IAdderInterface");
            
            int result = a.Add(1, 2);
            Console.WriteLine($"Calling Add(1, 2) = {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
        }
        finally
        {
            Marshal.Release(x);
            Console.WriteLine("Released COM object");
        }
    }

    void TestMinimumCOMObject()
    {
        Console.WriteLine("\n=== Minimum COM Object Test ===");
        var x = LinuxCppServerNativeMethods.CreateMinimumCOMObject();
        Console.WriteLine($"Created minimum COM object: {x:X}");
        
        try 
        {
            var o = (IHello)Marshal.GetObjectForIUnknown(x);
            Console.WriteLine("Successfully got IHello interface from minimum object");
            
            o.Hello(123);
            Console.WriteLine("Called Hello method on minimum object");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
        }
        finally
        {
            Marshal.Release(x);
            Console.WriteLine("Released minimum COM object");
        }
    }

    public void Run()
    {
        Console.WriteLine("Linux C++ COM Interop Tests");
        Console.WriteLine("============================");
        
        // Test basic native method call
        Console.WriteLine("\n=== Native Method Test ===");
        LinuxCppServerNativeMethods.HelloFromClientExport();
        
        SimpleIUnknownAPICallTest();
        ComWrapperUsageTest();
        TestMinimumCOMObject();
        
        Console.WriteLine("\n=== Forcing Garbage Collection ===");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        Console.WriteLine("\nAll tests completed!");
    }
}
