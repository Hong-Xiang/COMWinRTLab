# Create build directory if it doesn't exist
if (!(Test-Path ".\build")) {
    New-Item -ItemType Directory -Path ".\build"
}

# Build the DLL with debug symbols and proper C exports
clang++.exe -fms-extensions -std=c++20 -shared -g -O0 -DBUILDING_DLL -IServerCpp `
    "-Wl,--export-all-symbols" `
    "-Wl,--enable-auto-import" `
    .\ServerCpp\*.cpp -o .\build\ServerCpp.dll

dumpbin.exe /EXPORTS .\build\ServerCpp.dll
cp .\ServerCpp\*.manifest .\build

