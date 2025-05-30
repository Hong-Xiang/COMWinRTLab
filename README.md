# COMWinRTLab
Learn COM/WinRT from scratch

Following [Kenny Kerr's course](https://app.pluralsight.com/profile/author/kenny-kerr) on Pluralsight.

Objectives:

Learn COM/WinRT from scratch, by actually implementing them with minimum dependencies. As they are ABI standard with lots of additional runtime features,
we want to implement them step by step and demonstrate and test how they work by interop between C++ and C#.

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
