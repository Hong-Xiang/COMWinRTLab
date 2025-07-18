#pragma once
#include "guid.h"

namespace com {
GUID GetIUnknownIID();

struct IUnknown {
  virtual HRESULT QueryInterface(GUID const &riid, void **ppvObject) = 0;
  virtual ULONG AddRef() = 0;
  virtual ULONG Release() = 0;
}