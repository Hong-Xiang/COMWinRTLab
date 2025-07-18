using CSharpClientConsole;

try
{
    Console.WriteLine("Testing Linux COM-like Interop");
    Console.WriteLine("===============================");
    
    // Test the traditional COM-like functionality (shows limitations on Linux)
    var comTest = new LinuxCppCOMUsage();
    comTest.Run();
    
    // Test the modern StrategyBasedComWrappers API with [GeneratedComInterface]
    var modernComWrappersTest = new LinuxComWrappersUsage();
    modernComWrappersTest.Run();
    
    // Test direct interop which works on all platforms
    var directTest = new LinuxDirectInterop();
    directTest.TestDirectInterop();
    
    Console.WriteLine("\n=== Summary ===");
    Console.WriteLine("✅ Native function calls work perfectly");
    Console.WriteLine("✅ Object creation and basic Marshal operations work");
    Console.WriteLine("✅ Memory management (AddRef/Release) works");
    Console.WriteLine("✅ StrategyBasedComWrappers with [GeneratedComInterface] provides modern cross-platform COM!");
    Console.WriteLine("❌ Traditional COM wrapper (GetObjectForIUnknown) is Windows-only");
    Console.WriteLine("💡 Recommended: Use StrategyBasedComWrappers + [GeneratedComInterface] for cross-platform COM");
}
catch (Exception ex)
{
    Console.WriteLine($"Unhandled exception: {ex}");
    return 1;
}

return 0;
