#pragma once
#include <stdint.h>
#include <string>
#include <cstring>

// Use Windows GUID definition if available, otherwise define our own
#ifdef _WIN32
#include <guiddef.h>
#else
struct _GUID {
  uint32_t Data1;
  uint16_t Data2;
  uint16_t Data3;
  uint8_t Data4[8];
};
typedef struct _GUID GUID;
#define REFGUID const GUID &
__inline int IsEqualGUID(REFGUID rguid1, REFGUID rguid2) {
  return !std::memcmp(&rguid1, &rguid2, sizeof(GUID));
}
__inline bool operator==(REFGUID guidOne, REFGUID guidOther) {
  return !!IsEqualGUID(guidOne, guidOther);
}

__inline bool operator!=(REFGUID guidOne, REFGUID guidOther) {
  return !(guidOne == guidOther);
}
#endif

std::string guid_to_string(REFGUID guid);

