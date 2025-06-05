// This function is not exported and won't be visible in symbol listings
int FuncWithoutExport()
{
    return 39;
}

// This function is exported using __declspec(dllexport)
__declspec(dllexport)
int FuncWithDllExport()
{
    return 40;
}

// This function is exported using __declspec(dllexport), without cpp name mangling
extern "C" {
__declspec(dllexport)
int FuncCWithDllExport()
{
    return 41;
}
}

// This function can be exported through .def file
int FuncWithDefExport()
{
    return 42;
}
