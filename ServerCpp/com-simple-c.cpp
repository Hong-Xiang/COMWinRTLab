#include "abi.h"
#include "com.h"
#include "guid.h"
#include <atomic>
#include <cstdint>
#include <cstdlib>
#include <iostream>
#include <sys/stat.h>

namespace comc {
struct Object;
typedef com::hresult(STDCALL *QueryInterfaceFunc)(void *This, const GUID *riid,
                                                  void **ppvObject);
typedef uint32_t(STDCALL *AddRefFunc)(void *This);
typedef uint32_t(STDCALL *ReleaseFunc)(void *This);

struct IUnknownVtbl {
  QueryInterfaceFunc QueryInterface;
  AddRefFunc AddRef;
  ReleaseFunc Release;
};

struct Object {
  void *lpVtbl;
  const char *name;
  uint32_t refCount;
};

uint32_t STDCALL IUnknown_AddRef(void *This) {
  // atomic increment using C++ standard atomic operations
  auto obj = (Object *)This;
  obj->refCount++;
  std::cout << obj->name << ".AddRef -> " << obj->refCount << std::endl;
  return obj->refCount;
}

uint32_t STDCALL IUnknown_Release(void *This) {
  auto obj = (Object *)This;
  obj->refCount--;
  std::cout << obj->name << ".Release -> " << obj->refCount << std::endl;
  if (obj->refCount == 0) {
    free(obj);
    std::cout << obj->name << ".Free" << std::endl;
  }
  return obj->refCount;
}

com::hresult STDCALL IUnknown_QueryInterface(void *This, const GUID *riid,
                                             void **ppvObject) {
  *ppvObject = nullptr;
  if (riid == nullptr) {
    return com::hresult::POINTER;
  }

  if (*riid == com::GetIUnknownIID()) {
    *ppvObject = This;
    IUnknown_AddRef(This);
    return com::hresult::OK;
  }

  return com::hresult::NOINTERFACE;
}

static IUnknownVtbl IUnknownIUnknownVtbl =
    IUnknownVtbl{IUnknown_QueryInterface, IUnknown_AddRef, IUnknown_Release};

// IHello

struct IHelloVtbl : IUnknownVtbl {
  com::hresult(STDCALL *SayHello)(void *This);
};

struct __declspec(uuid("b0bf416d-9e3a-4d46-9377-af3db3cb10e4")) IHello
    : Object {};

com::hresult STDCALL IHello_SayHello(void *This) {
  std::cout << "Hello From COM By C!" << std::endl;
  return com::hresult::OK;
}

com::hresult STDCALL IHello_QueryInterface(void *This, const GUID *riid,
                                           void **ppvObject) {
  std::cout << "IHello_QueryInterface called with riid " << guid_to_string(*riid)
            << std::endl;
  *ppvObject = nullptr;
  if (riid == nullptr) {
    return com::hresult::POINTER;
  }

  if (*riid == com::GetIUnknownIID()) {
    *ppvObject = This;
    IUnknown_AddRef(This);
    return com::hresult::OK;
  }
  if (*riid == __uuidof(IHello)) {
    *ppvObject = This;
    IUnknown_AddRef(This);
    return com::hresult::OK;
  }

  return com::hresult::NOINTERFACE;
}

static IHelloVtbl IHelloIHelloVtbl = IHelloVtbl{
    IHello_QueryInterface, IUnknown_AddRef, IUnknown_Release, IHello_SayHello};

} // namespace comc

extern "C" {
EXPORT_API comc::Object *STDCALL ComCCreateObject() {
  auto result = (comc::Object *)malloc(sizeof(comc::Object));
  result->refCount = 0;
  result->name = "Object";
  result->lpVtbl = &comc::IUnknownIUnknownVtbl;
  comc::IUnknown_AddRef(result);
  return result;
}
EXPORT_API comc::Object *STDCALL ComCCreateHello() {
  auto result = (comc::Object *)malloc(sizeof(comc::Object));
  result->refCount = 0;
  result->name = "IHello";
  result->lpVtbl = &comc::IHelloIHelloVtbl;
  comc::IUnknown_AddRef(result);
  return result;
}
}