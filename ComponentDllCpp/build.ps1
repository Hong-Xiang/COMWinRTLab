# cl.exe /LD /W4 .\component.cpp /Fe:component.dll
# Option 1: Simple syntax (preferred for basic builds)
# cl.exe /LD /W4 .\component.cpp /Fe:component.dll /link /DEF:exports.def

# Option 2: Explicit linker options
cl.exe /W4 /EHcs /c .\component.cpp
link.exe component.obj /DLL /DEF:component.def /OUT:component.dll
dumpbin.exe /exports .\component.dll
copy .\component.dll ..\LabTestConsoleSharp\bin\Debug\net9.0\component.dll