# COMWinRTLab
Learn COM/WinRT from scratch

Following [Kenny Kerr's course](https://app.pluralsight.com/profile/author/kenny-kerr) on Pluralsight.

Objectives:

Learn COM/WinRT from scratch, by actually implementing them with minimum dependencies. As they are ABI standard with lots of additional runtime features,
we want to implement them step by step and demonstrate and test how they work by interop between C++ and C#.

## Plan

Tasks:

- [ ] Basic dynamic linking, C++ and C# way, def files. etc
- [ ] Basic COM : exporting object - OO style interop: virtual table/interfaces
- [ ] Basic COM : adding lifetime management - ref counting
- [ ] Basic COM : hello IUnknown
- [ ] COM with runtime : adding activation - class objects
- [ ] COM with runtime : adding class factory
- [ ] WinRT : another layer based on (part of) COM - e.g. no CoCreateInstance
- [ ] registration?
- [ ] Out of proc server
- [ ] Debug Tools?



Topics:

- Threading model changes in COM -> UWP WinRT -> WinAppSDK WinRT
- Lowering of WinRT functionalities to COM interfaces:
    - class like functionalities, properties, events, async, error handling, etc
- Practical debug tools:
    - from course, there are depends for checking dll dependencies, il diasm for checking winmd, etc
    - from chat channel, there are gflags, loader snaps, Procmon, etc

## Milestones

### Simple Link and DLL
When using Cpp, we have different approaches to share code between projects:
* Header only library - direct embedding component source code into consumer source code - Can not work across different languages, normal optimization, duplicate disk/memory usage
* Static library - compile component code into a library, and embed full component native machine code into consumer's native machine code - can work across different native languages with C ABI, harder to interop with language with runtime like C#/Python, requires recompilation of consumer code when component code changes, potential link time optimization, duplicate disk/memory usage
* Dynamic library - compile component code into a library, and add reference/stub native code to the library into consumer's native machine code - can work across different languages with C ABI, no optimization like inline, shared disk/memory usage, loaded at runtime, slightly interop cost static library

### COM and WinRT Objectives (Historically)

COM: Stable ABI of couse, but not only for that. COM was designed on the key observation that all C++ compilers can generate the same binary layout for virtual functions (with correct calling convention and many other configurations).

When COM was designed, C++ was still in its early days,
COM was even claimed to be "a better OO model than C++", 
thus it was designed in a way that relative easy to write component in C++ (without addtional runtime support for its core functionalities), we can directly write C++ to generate code that is compatible with COM. .NET used to provide built-in support for COM in runtime.

On the time of WinRT, idea of COM involved to be a stable ABI to be used in MULTIPLE languages, not only C++ and C#.

## Notes

### Dump object layout using compiler:
```
cl /Fo /c /d1reportAllClassLayout .\some-target.cpp
```
arguments `/c` and `/Fo` are not required, `/d1reportAllClassLayout` is actually the key argument,
adding `/c` and `/Fo` is to make the command line compile only dump the layout, no link, and no output actual obj file.
