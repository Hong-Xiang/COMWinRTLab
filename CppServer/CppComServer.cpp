#include "pch.h"
#include <wil/com.h>
#include <iostream>

namespace CppCOMServer {
	struct
		__declspec(uuid("2E55DE61-D7ED-44BE-9B62-F340AF70721D"))
		IHello : IUnknown {
		virtual HRESULT __stdcall Hello(int data) noexcept = 0;
	};

	struct 
		__declspec(uuid("EA71AB58-8226-4FCB-A156-DC53E1EE91A9"))
		IAdder : IUnknown {
		virtual HRESULT __stdcall Add(int a, int b, int* r) noexcept = 0;
	};

	struct MyHelloObject : IHello, IAdder {
		unsigned long m_count{ 1 };

		// IUnknown methods
		virtual HRESULT __stdcall QueryInterface(const GUID& riid, void** ppv) noexcept override {
			std::cout << "QueryInterface called on MyHelloObject" << std::endl;
			if (ppv == nullptr) {
				return E_INVALIDARG;
			}

			if (riid == __uuidof(IUnknown) || riid == __uuidof(CppCOMServer::IHello)) {
				*ppv = static_cast<IHello*>(this);
				std::cout << "Query for IUnknown or IHello: " << *ppv << std::endl;
				AddRef();
				return S_OK;
			}

			if (riid == __uuidof(IAdder)) {
				*ppv = static_cast<IAdder*>(this);
				std::cout << "Query for IAdder: " << *ppv << std::endl;
				AddRef();
				return S_OK;
			}

			*ppv = nullptr;
			return E_NOINTERFACE;
		}

		virtual unsigned long __stdcall AddRef() noexcept override {
			unsigned long result = InterlockedIncrement(&m_count);
			std::cout << "AddRef called, count: " << result << std::endl;
			return result;
		}

		virtual unsigned long __stdcall Release() noexcept override {
			unsigned long result = InterlockedDecrement(&m_count);
			std::cout << "Release called, count: " << result << std::endl;
			if (result == 0) {
				std::cout << "Deleting MyHelloObject" << std::endl;
				delete this;
			}
			return result;
		}

		// IHello methods
		virtual HRESULT __stdcall Hello(int data) noexcept override {
			std::cout << "Hello called with data: " << data << ", ref count: " << m_count << std::endl;
			return S_OK;
		}

		virtual HRESULT __stdcall Add(int a, int b, int* r) {
			*r = a + b;
			return S_OK;
		}

		virtual ~MyHelloObject() {
			std::cout << "MyHelloObject destructor called" << std::endl;
		}
	};
}

// Export function to create the object
EXTERN_C CppCOMServer::IHello* CreateCppCOMObject() {
	return new CppCOMServer::MyHelloObject();
}

