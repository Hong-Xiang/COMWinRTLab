#include "guid.h"
#include <iomanip>
#include <iostream>
#include <sstream>

std::string guid_to_string(REFGUID guid) {
  std::ostringstream oss;
  oss << std::hex << std::setfill('0');

  // Data1: 8 hex digits
  oss << std::setw(8) << guid.Data1 << "-";

  // Data2: 4 hex digits
  oss << std::setw(4) << guid.Data2 << "-";

  // Data3: 4 hex digits
  oss << std::setw(4) << guid.Data3 << "-";

  // Data4[0-1]: 4 hex digits
  oss << std::setw(2) << static_cast<unsigned>(guid.Data4[0]) << std::setw(2)
      << static_cast<unsigned>(guid.Data4[1]) << "-";

  // Data4[2-7]: 12 hex digits
  for (int i = 2; i < 8; ++i) {
    oss << std::setw(2) << static_cast<unsigned>(guid.Data4[i]);
  }

  return oss.str();
}

extern "C" {
__declspec(dllexport) void PrintGUID(REFGUID guid) {
  std::string str = guid_to_string(guid);
  std::cout << str << std::endl;
}
}
