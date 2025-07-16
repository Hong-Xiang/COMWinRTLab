# Dynamic Linking

## Create Server DLL in Cpp

C/Cpp might be the most straight language to create a library dll,
the only thing we need to do is export the functions we want in a proper way:
the widely accepted C FFI fashion, with proper calling conventions.

To create a DLL in C++, we typically follow these steps:

1. Write the code for the functions we want to export.
2. Use module def file or use the `__declspec(dllexport)` keyword to mark these functions for export.
3. Compile the code into a DLL using a compatible compiler.

Compilation command line
```
TODO
```

## Create Client in Cpp

## Create Server DLL in CSharp

## Create Client in CSharp
Using P/Invoke