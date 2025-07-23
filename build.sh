mkdir -p build

clang++-18 -fms-extensions -std=c++20 -shared -g -O0 -DBUILDING_DLL -IServerCpp \
    "-Wl,--export-all-symbols" \
    "-Wl,--enable-auto-import" \
    ./ServerCpp/*.cpp -o ./build/ServerCpp.so
