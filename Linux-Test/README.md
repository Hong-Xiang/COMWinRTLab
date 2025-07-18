# Linux COM-like Interop Test

This directory demonstrates how to create a COM-like server in C++ that can be used with .NET's marshal functionality on Linux, specifically tested with Ubuntu WSL and clang-18.

## Project Structure

```
Linux-Test/
├── build.sh                           # Build script for the entire project
├── CppServer/
│   ├── CppServer.cpp                  # Linux-compatible COM-like server implementation
│   ├── Makefile                       # Build configuration for C++ server
│   └── libCppServer.so               # Generated shared library
└── CSharpClient/
    └── CSharpClientConsole/
        ├── CSharpClientConsole.csproj # Project configuration
        ├── Program.cs                 # Main entry point
        ├── LinuxCppCOMUsage.cs       # Tests showing COM limitations on Linux
        ├── LinuxDirectInterop.cs     # Direct P/Invoke interop (recommended approach)
        ├── ComClientContract.cs      # COM interface definitions
        └── LinuxCppServerNativeMethods.cs # P/Invoke declarations
```

## What Works on Linux ✅

1. **Native Function Calls**: Direct P/Invoke calls to C++ functions work perfectly
2. **Object Creation**: Can create and manage C++ objects through pointers
3. **Basic Marshal Operations**: `Marshal.AddRef()` and `Marshal.Release()` work correctly
4. **Memory Management**: Reference counting and object lifetime management works
5. **Direct VTable Calls**: Can call C++ methods directly through exported helper functions
6. **Multiple Interface Support**: Objects can support multiple interfaces (IHello and IAdder)

## What Doesn't Work on Linux ❌

1. **COM Wrapper Creation**: `Marshal.GetObjectForIUnknown()` is Windows-only
2. **COM Interface Casting**: Cannot use `(IHello)Marshal.GetObjectForIUnknown(ptr)`
3. **Automatic Interface Marshaling**: .NET's COM interop attributes don't work on Linux

## Build Requirements

- **Ubuntu WSL** or Linux environment
- **clang-18** (tested version)
- **.NET 8.0** SDK
- **make** build system

## Building and Running

1. Make the build script executable:
   ```bash
   chmod +x build.sh
   ```

2. Run the complete build and test:
   ```bash
   ./build.sh
   ```

3. Or build components separately:
   ```bash
   # Build C++ server
   cd CppServer
   make
   
   # Build and run C# client
   cd ../CSharpClient/CSharpClientConsole
   dotnet build
   dotnet run
   ```

## Key Implementation Details

### C++ Server (CppServer.cpp)

- **No Windows Dependencies**: Removed `combaseapi.h`, `objbase.h`, and `__stdcall`
- **Linux-Compatible GUIDs**: Custom GUID structure without Windows OLECHAR
- **VTable-Based Interface**: Implements COM-like VTable structure manually
- **Export Functions**: Uses `extern "C"` for C-style exports
- **Memory Management**: Proper reference counting and cleanup

### C# Client

- **Direct P/Invoke**: Uses `[DllImport("libCppServer.so")]` for direct calls
- **LibraryImport**: Modern `[LibraryImport]` attribute for better performance
- **Platform Awareness**: Handles platform limitations gracefully
- **Custom Wrappers**: Demonstrates how to create custom interop wrappers

## Test Results

The test application demonstrates:

```
=== What Works ===
✅ Native function calls work perfectly
✅ Object creation and basic Marshal operations work  
✅ Memory management (AddRef/Release) works
✅ Direct function calls through exported helpers work
✅ Multiple interface support works

=== What Doesn't Work ===
❌ Full COM wrapper (GetObjectForIUnknown) is Windows-only

=== Recommendation ===
💡 For Linux, use direct P/Invoke or create custom interop wrappers
```

## Alternative Approaches for Linux

Since full COM interop isn't available on Linux, consider these alternatives:

1. **Direct P/Invoke**: Call C++ functions directly (demonstrated in this project)
2. **Custom Wrapper Classes**: Create C# wrapper classes that manage C++ objects
3. **C++/CLI**: Use managed C++ as an interop layer (Windows-specific)
4. **gRPC or REST APIs**: Use network protocols for cross-platform communication
5. **Shared Memory**: Use memory-mapped files for high-performance scenarios

## Conclusion

While .NET's COM interop is Windows-specific, this project shows that you can still achieve effective C++/C# interop on Linux using:

- Direct P/Invoke calls
- Manual VTable management  
- Custom interop wrappers
- Proper memory management

This approach provides a foundation for building cross-platform native interop solutions while understanding the platform-specific limitations of COM.
