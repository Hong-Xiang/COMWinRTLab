#include "abi.h"
#include "com.h"
#include "guid.h"
#include <atomic>
#include <cstdint>
#include <iostream>
#include <sys/stat.h>

namespace comcpp {

struct __declspec(uuid("b0bf416d-9e3a-4d46-9377-af3db3cb10e4")) IHello
    : com::IUnknown {
  virtual com::hresult STDCALL SayHello() = 0;
};

struct Object : com::IUnknown {
  std::atomic<uint32_t> refCount{0};

  virtual com::hresult STDCALL
  QueryInterface(REFGUID riid, void **ppvObject) noexcept override {
    if (ppvObject == nullptr) {
      return com::hresult::POINTER;
    }
    *ppvObject = nullptr;

    if (riid == com::GetIUnknownIID()) {
      *ppvObject = static_cast<com::IUnknown *>(this);
      AddRef();
      return com::hresult::OK;
    }

    return com::hresult::NOINTERFACE;
  }

  virtual uint32_t AddRef() noexcept override {
    uint32_t newCount = refCount.fetch_add(1) + 1;
    std::cout << "Object.AddRef -> " << newCount << std::endl;
    return newCount;
  }

  virtual uint32_t Release() noexcept override {
    uint32_t newCount = refCount.fetch_sub(1) - 1;
    std::cout << "Object.Release -> " << newCount << std::endl;
    if (newCount == 0) {
      std::cout << "Object.Delete" << std::endl;
      delete this;
    }
    return newCount;
  }
};

struct Hello : IHello {
  std::atomic<uint32_t> refCount{0};

  virtual com::hresult STDCALL
  QueryInterface(REFGUID riid, void **ppvObject) noexcept override {
    std::cout << "Hello.QueryInterface called with riid "
              << guid_to_string(riid) << std::endl;

    if (ppvObject == nullptr) {
      return com::hresult::POINTER;
    }
    *ppvObject = nullptr;

    if (riid == __uuidof(com::IUnknown)) {
      *ppvObject = static_cast<com::IUnknown *>(this);
      std::cout << "Hello.QueryInterface(IUnknown) -> " << *ppvObject << std::endl;
      AddRef();
      return com::hresult::OK;
    }
    if (riid == __uuidof(IHello)) {
      *ppvObject = static_cast<IHello *>(this);
      std::cout << "Hello.QueryInterface(IHello) -> " << *ppvObject << std::endl;
      AddRef();
      return com::hresult::OK;
    }

    return com::hresult::NOINTERFACE;
  }

  virtual uint32_t AddRef() noexcept override {
    uint32_t newCount = refCount.fetch_add(1) + 1;
    std::cout << "Hello.AddRef -> " << newCount << std::endl;
    return newCount;
  }

  virtual uint32_t Release() noexcept override {
    uint32_t newCount = refCount.fetch_sub(1) - 1;
    std::cout << "Hello.Release -> " << newCount << std::endl;
    if (newCount == 0) {
      std::cout << "Hello.Delete" << std::endl;
      delete this;
    }
    return newCount;
  }

  virtual com::hresult STDCALL SayHello() override {
    std::cout << "Hello From COM By C++!" << std::endl;
    return com::hresult::OK;
  }
};

} // namespace comcpp

extern "C" {
EXPORT_API comcpp::Object *STDCALL ComCppCreateObject() {
  auto result = new comcpp::Object();
  result->AddRef();
  return result;
}
EXPORT_API comcpp::Hello *STDCALL ComCppCreateHello() {
  auto result = new comcpp::Hello();
  result->AddRef();
  return result;
}
}