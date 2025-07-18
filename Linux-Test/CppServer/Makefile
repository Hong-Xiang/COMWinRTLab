CC = clang-18
CXX = clang++-18
CXXFLAGS = -std=c++17 -fPIC -shared -Wall -Wextra -O2
TARGET = libCppServer.so
SOURCE = CppServer.cpp

all: $(TARGET)

$(TARGET): $(SOURCE)
	$(CXX) $(CXXFLAGS) -o $(TARGET) $(SOURCE)

clean:
	rm -f $(TARGET)

.PHONY: all clean
