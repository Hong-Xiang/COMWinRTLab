# COMWinRTLab
Learn COM/WinRT from scratch by implementing a minimal COM/WinRT component and consumer.

Following [Kenny Kerr's course](https://app.pluralsight.com/profile/author/kenny-kerr) on Pluralsight.

## Objectives

Learn COM/WinRT from scratch by implementing them step-by-step with minimal dependencies. Since COM/WinRT are ABI standards with extensive runtime features, we implement them incrementally and validate through cross-language interop between C++ and C#.

## Project Structure

- **CppComponent/**: C++ COM component implementation
- **CSharpConsumer/**: C# consumer using different interop approaches
- **LabTestConsoleSharp/**: Console app for manual testing
- **CSharpConsumerTest/**: Automated unit tests

## Implementation Roadmap

### Phase 1: Foundation ✅ (Completed)
**Goal**: Establish basic cross-language interop

- [x] **Basic C Function Export** 
  - File: `CppComponent.cpp::AddC()` 
  - Validation: `CSharpConsumerTest` calls C function
  - ✅ Completed: Simple P/Invoke working

- [x] **Dynamic Library Loading**
  - Files: `CppComponent.vcxproj`, `.def` files
  - Validation: DLL loads in C# without crashes
  - ✅ Completed: DLL builds and loads successfully

### Phase 2: Core COM Implementation ✅ (Completed)
**Goal**: Implement fundamental COM interfaces and lifetime management

- [x] **IUnknown Interface Definition**
  - File: `CppComponent.cpp` (IUnknown struct)
  - Validation: Interface compiles with correct vtable layout
  - ✅ Completed: IUnknown with QueryInterface, AddRef, Release

- [x] **Virtual Table Implementation** 
  - File: `CppComponent.cpp::Calculator` class
  - Validation: C# can call methods through vtable
  - ✅ Completed: Calculator implements ICalculator, ICalculator2

- [x] **Reference Counting**
  - File: `Calculator::AddRef/Release` methods
  - Validation: Object properly destroyed when ref count reaches 0
  - ✅ Completed: Working ref counting with debug output

- [x] **QueryInterface Implementation**
  - File: `Calculator::QueryInterface` method  
  - Validation: Can cast between ICalculator and ICalculator2
  - ✅ Completed: Multi-interface support working

### Phase 3: C# Interop Approaches ✅ (Completed)
**Goal**: Demonstrate multiple ways to consume COM from C#

- [x] **Manual VTable Interop**
  - File: `CSharpConsumer/AdHocConsumer.cs`
  - Validation: Manual function pointer calls work
  - ✅ Completed: Unsafe vtable access working

- [x] **Built-in COM Interop**
  - File: `CSharpConsumer/ComWrapperConsumer.cs`
  - Validation: Standard .NET COM interop works  
  - ✅ Completed: Marshal.GetObjectForIUnknown working

- [x] **Generated COM Interfaces**
  - File: `LabTestConsoleSharp/Program.cs` (GeneratedComInterface)
  - Validation: Modern .NET COM generation works
  - ✅ Completed: Source-generated COM interfaces working

### Phase 4: Advanced COM Features 🔄 (In Progress)
**Goal**: Add enterprise-grade COM capabilities

- [ ] **Class Factory Implementation**
  - **Files to create**: `CppComponent/ClassFactory.cpp`, `CppComponent/ClassFactory.h`
  - **Changes needed**: Implement `IClassFactory` interface
  - **Validation**: Create objects via `CoCreateInstance`
  - **Acceptance criteria**: 
    - C# can create Calculator via `CoCreateInstance`
    - Multiple instances can be created independently
    - Class factory has proper ref counting

- [ ] **COM Registration**
  - **Files to create**: `register.reg`, `CppComponent/DllMain.cpp`
  - **Changes needed**: Add `DllRegisterServer`/`DllUnregisterServer`
  - **Validation**: Component appears in Windows registry
  - **Acceptance criteria**:
    - `regsvr32 CppComponent.dll` succeeds
    - Registry entries created correctly
    - `CoCreateInstance` works with CLSID lookup

- [ ] **Reg-Free COM (Side-by-Side Manifests)**
  - **Files to create**: `CppComponent.manifest`, `LabTestConsoleSharp.exe.manifest`, `CppComponent.dll.manifest`
  - **Changes needed**: Create application and component manifests for reg-free activation
  - **Validation**: COM component works without registry registration
  - **Acceptance criteria**:
    - Component works via manifests without `regsvr32`
    - `CoCreateInstance` succeeds using manifest-based activation
    - Side-by-side deployment enables isolated component versions
    - Manifest-based activation works in both debug and release builds
  - **Benefits**: Enables xcopy deployment, version isolation, no admin rights needed

- [ ] **Apartment Threading Model**
  - **Files to modify**: `Calculator` class, add thread safety
  - **Changes needed**: Add apartment awareness, thread marshaling
  - **Validation**: Component works from different threads
  - **Acceptance criteria**:
    - STA/MTA compatibility
    - Thread-safe ref counting
    - Proper proxy/stub behavior

### Phase 5: Error Handling & Robustness 📋 (Planned)
**Goal**: Production-ready error handling and diagnostics

- [ ] **HRESULT Error Propagation**
  - **Files to modify**: All interface methods in `Calculator`
  - **Changes needed**: Return proper HRESULTs, implement `ISupportErrorInfo`
  - **Validation**: C# receives and handles COM exceptions
  - **Acceptance criteria**:
    - All methods return appropriate HRESULTs
    - C# gets proper exception details
    - Error info includes descriptions

- [ ] **Interface Versioning**
  - **Files to create**: `ICalculator3` interface with new methods
  - **Changes needed**: Demonstrate backward compatibility
  - **Validation**: Old consumers still work with new component
  - **Acceptance criteria**:
    - `ICalculator` clients unchanged
    - New `ICalculator3` adds functionality
    - QueryInterface properly handles all versions



### Phase 6: WinRT Foundation 📋 (Planned)
**Goal**: Build WinRT layer on top of COM

- [ ] **WinRT Metadata Generation**
  - **Files to create**: `.winmd` files, IDL definitions
  - **Changes needed**: Define WinRT interfaces and classes
  - **Validation**: C# WinRT projections can consume component
  - **Acceptance criteria**:
    - IDL compiles to .winmd
    - C# sees projected types
    - Async operations work

- [ ] **WinRT Activation Factory**
  - **Files to create**: Activation factory implementation
  - **Changes needed**: Replace class factory with WinRT pattern
  - **Validation**: Can activate via `Windows.ApplicationModel.Core.CoreApplication`
  - **Acceptance criteria**:
    - Activation factory registered
    - WinRT activation works
    - Factory creates correct object types

### Phase 7: Out-of-Process Server 📋 (Planned)
**Goal**: Demonstrate process isolation and marshaling

- [ ] **Local Server Implementation**
  - **Files to create**: Separate EXE project for local server
  - **Changes needed**: Move Calculator to separate process
  - **Validation**: Cross-process COM calls work
  - **Acceptance criteria**:
    - Local server registers and starts
    - Cross-process calls succeed
    - Proper process lifetime management

### Phase 8: Advanced Scenarios 📋 (Planned)
**Goal**: Real-world COM/WinRT scenarios

- [ ] **Event Support (Connection Points)**
  - **Files to create**: Event interface, connection point implementation
  - **Changes needed**: Add event firing to Calculator
  - **Validation**: C# can subscribe to and receive events
  - **Acceptance criteria**:
    - Event interface defined
    - Connection points work
    - Events fire and are received

- [ ] **Async Operations (WinRT)**
  - **Files to create**: Async operation implementations
  - **Changes needed**: Add async methods to Calculator
  - **Validation**: C# can await WinRT async operations
  - **Acceptance criteria**:
    - Async operations return `IAsyncOperation<T>`
    - C# can await results
    - Cancellation works



- [ ] **Memory Leak Detection**
  - **Files to create**: Memory testing utilities
  - **Changes needed**: Add leak detection in tests
  - **Validation**: No memory leaks under stress testing
  - **Acceptance criteria**:
    - All AddRef/Release pairs balanced
    - Stress tests pass without leaks
    - Proper cleanup on DLL unload
## Testing Strategy

Each phase includes:
1. **Unit Tests**: `CSharpConsumerTest/` project with automated validation
2. **Manual Testing**: `LabTestConsoleSharp/` for interactive verification  
3. **Integration Tests**: Cross-language interop validation
4. **Performance Tests**: Memory leaks, ref counting accuracy

## Learning Resources

- [Kenny Kerr's COM/WinRT Course](https://app.pluralsight.com/profile/author/kenny-kerr)
- [COM Technical Articles](https://docs.microsoft.com/en-us/windows/win32/com/)
- [WinRT Documentation](https://docs.microsoft.com/en-us/windows/uwp/winrt-cref/)

## Development Tools

### GUID Tag using MSVC

```
// Use MSVC's built-in GUID support
__declspec(uuid("...")) class SomeClass {};
auto guid = __uuidof(SomeClass);
```

### Object Layout Analysis
```bash
# Dump C++ object layouts for debugging
cl /c /d1reportAllClassLayout .\layouttest.cpp
```

