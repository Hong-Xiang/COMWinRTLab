using System;
using System.Runtime.InteropServices;

namespace CSharpClientConsole;

public class LinuxComWrappersUsage
{
    void StrategyBasedComWrappersTest()
    {
        Console.WriteLine("=== StrategyBasedComWrappers Test ===");
        
        // Create the ComWrappers instance
        var cw = LinuxStrategyBasedComWrappers.Instance;
        
        // Get pointer to COM interface from native code
        nint ptr = LinuxCppServerNativeMethods.CreateCppCOMObject();
        Console.WriteLine($"Got native COM pointer: {ptr:X}");
        
        try
        {
            // Use the ComWrappers API to create a Runtime Callable Wrapper
            var helloObj = (IHello)cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
            Console.WriteLine("Successfully created IHello wrapper using StrategyBasedComWrappers!");
            
            // Test the Hello method
            helloObj.Hello(42);
            
            // Try to get IAdderInterface from the same object
            try 
            {
                var adderObj = (IAdderInterface)cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
                Console.WriteLine("Successfully created IAdderInterface wrapper!");
                
                int result = adderObj.Add(10, 20);
                Console.WriteLine($"Add(10, 20) = {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get IAdderInterface: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            Console.WriteLine($"Exception type: {ex.GetType().Name}");
        }
        finally
        {
            // Clean up the native object
            Marshal.Release(ptr);
            Console.WriteLine("Released native COM object");
        }
    }

    void MultipleInterfaceTest()
    {
        Console.WriteLine("\n=== Multiple Interface Test ===");
        
        var cw = LinuxStrategyBasedComWrappers.Instance;
        
        // Test with combined object (supports both interfaces)
        Console.WriteLine("Testing combined object...");
        nint combinedPtr = LinuxCppServerNativeMethods.CreateCppCOMObject();
        TestInterfacesWithComWrappers(cw, combinedPtr, "Combined");
        
        // Test with simple object (supports only IHello)
        Console.WriteLine("\nTesting simple object...");
        nint simplePtr = LinuxCppServerNativeMethods.CreateMinimumCOMObject();
        TestInterfacesWithComWrappers(cw, simplePtr, "Simple");
    }

    private void TestInterfacesWithComWrappers(ComWrappers cw, nint ptr, string objectType)
    {
        try
        {
            Console.WriteLine($"Testing {objectType} object at {ptr:X}");
            
            // Test IHello interface
            try 
            {
                var hello = (IHello)cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
                Console.WriteLine($"✅ {objectType} object supports IHello");
                hello.Hello(123);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ {objectType} object failed IHello: {ex.Message}");
            }
            
            // Test IAdderInterface
            try 
            {
                var adder = (IAdderInterface)cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
                Console.WriteLine($"✅ {objectType} object supports IAdderInterface");
                int result = adder.Add(5, 7);
                Console.WriteLine($"   Add(5, 7) = {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ {objectType} object failed IAdderInterface: {ex.Message}");
            }
        }
        finally
        {
            Marshal.Release(ptr);
            Console.WriteLine($"{objectType} object released");
        }
    }

    void ComWrappersCacheTest()
    {
        Console.WriteLine("\n=== ComWrappers Caching Test ===");
        
        var cw = LinuxStrategyBasedComWrappers.Instance;
        nint ptr = LinuxCppServerNativeMethods.CreateCppCOMObject();
        
        try
        {
            // Get the same COM object multiple times - should return cached instances
            var hello1 = (IHello)cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
            var hello2 = (IHello)cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
            var adder1 = (IAdderInterface)cw.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.None);
            
            Console.WriteLine($"IHello instance 1: {hello1.GetHashCode()}");
            Console.WriteLine($"IHello instance 2: {hello2.GetHashCode()}");
            Console.WriteLine($"IAdderInterface instance: {adder1.GetHashCode()}");
            
            if (ReferenceEquals(hello1, hello2))
            {
                Console.WriteLine("✅ ComWrappers correctly cached the same object");
            }
            else
            {
                Console.WriteLine("⚠️ ComWrappers created different instances (may be expected)");
            }
            
            // Test functionality
            hello1.Hello(100);
            int sum = adder1.Add(15, 25);
            Console.WriteLine($"Sum calculation: {sum}");
        }
        finally
        {
            Marshal.Release(ptr);
        }
    }

    public void Run()
    {
        Console.WriteLine("Modern ComWrappers API Tests");
        Console.WriteLine("=============================");
        
        Console.WriteLine("Using StrategyBasedComWrappers with [GeneratedComInterface]");
        Console.WriteLine("This provides cross-platform COM-like functionality!\n");
        
        StrategyBasedComWrappersTest();
        MultipleInterfaceTest();
        ComWrappersCacheTest();
        
        Console.WriteLine("\n=== Forcing Garbage Collection ===");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        Console.WriteLine("\nModern ComWrappers tests completed!");
    }
}
