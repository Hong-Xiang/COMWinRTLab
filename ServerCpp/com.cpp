#include "com.h"

static GUID IUnknownGUID{0x00000000,
                         0x0000,
                         0x0000,
                         {0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46}};

GUID com::GetIUnknownIID() { return IUnknownGUID; }

extern "C" {
__declspec(dllexport) GUID GetIUnknownIID() { return IUnknownGUID; }
}
