#pragma once
#include "abi.h"
#include "guid.h"
#include <cstdint>

namespace com {
GUID GetIUnknownIID();

enum hresult : uint32_t {
  OK = 0,
  NOTIMPL = 0x80004001,
  NOINTERFACE = 0x80004002,
  POINTER = 0x80004003,
  FAIL = 0x80004005,
  ABORT = 0x80004004,
  OUTOFMEMORY = 0x8007000E,
  INVALIDARG = 0x80070057,
};

struct __declspec(uuid("00000000-0000-0000-C000-000000000046")) IUnknown {
  virtual STDCALL hresult QueryInterface(REFGUID riid,
                                         void **ppvObject) noexcept = 0;
  virtual uint32_t AddRef() noexcept = 0;
  virtual uint32_t Release() noexcept = 0;
};
} // namespace com