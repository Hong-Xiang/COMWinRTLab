#!/bin/bash

echo "Building C++ COM Server for Linux..."
cd CppServer
make clean
make
cd ..

echo "Building C# Client Console Application..."
cd CSharpClient/CSharpClientConsole
dotnet build
echo "Build completed!"

echo "Running C# Client..."
dotnet run