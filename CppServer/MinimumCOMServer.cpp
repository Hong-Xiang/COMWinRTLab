#include "pch.h"
#include <iostream>
#include <combaseapi.h>
#include <objbase.h>

namespace MinimumCOMServer
{
	// Forward declaration
	struct IUnknownVTbl;
	struct IHelloVTbl;

	struct MySimpleObject {
		IHelloVTbl* VTable;
		int RefCount;
	};

	// Function pointer typedefs for COM IUnknownVTbl interface
	typedef HRESULT(__stdcall* QueryInterfaceFunc)(MySimpleObject* This, const GUID* riid, void** ppvObject);
	typedef unsigned long(__stdcall* AddRefFunc)(MySimpleObject* This);
	typedef unsigned long(__stdcall* ReleaseFunc)(MySimpleObject* This);
	typedef HRESULT(__stdcall* HelloFunc)(MySimpleObject* This, int data);

	struct IUnknownVTbl
	{
		QueryInterfaceFunc QueryInterface;
		AddRefFunc AddRef;
		ReleaseFunc Release;
	};

	struct IHelloVTbl {
		QueryInterfaceFunc QueryInterface;
		AddRefFunc AddRef;
		ReleaseFunc Release;
		HelloFunc Hello;
	};

	static GUID IUnknownGUID{ 0x00000000, 0x0000, 0x0000, {0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46} };


	// "2E55DE61-D7ED-44BE-9B62-F340AF70721D"
	static GUID IHelloInterface{ 0x2E55DE61, 0xD7ED, 0x44BE, {0x9B, 0x62, 0xF3, 0x40, 0xAF, 0x70, 0x72, 0x1D} };

	HRESULT __stdcall QueryInterfaceImpl(MySimpleObject* self, const GUID* riid, void** ppvObject)
	{
		OLECHAR guidString[40];
		StringFromGUID2(*riid, guidString, 40);
		std::wcout << L"Query Interface Called with self " << self << L" and riid " << guidString << std::endl;
		if (IsEqualGUID(*riid, IUnknownGUID)
			|| IsEqualGUID(*riid, IHelloInterface)) {
			*ppvObject = self;
			self->RefCount++;
			return S_OK;
		}
		return E_NOINTERFACE;
	}

	unsigned long __stdcall AddRefImpl(MySimpleObject* self)
	{
		std::cout << "AddRef called" << std::endl;
		self->RefCount++;
		return self->RefCount;
	}

	unsigned long __stdcall ReleaseImpl(MySimpleObject* self)
	{
		std::cout << "Release called" << std::endl;
		self->RefCount--;
		if (self->RefCount == 0)
		{
			std::cout << "free memory" << std::endl;
			free(self);
			return 0;
		}
		return self->RefCount;
	}

	HRESULT __stdcall HelloImpl(MySimpleObject* self, int data) {
		std::cout << "Hello with data " << data << " from COM, with ref count" << self->RefCount << std::endl;
		return S_OK;
	}


	static IUnknownVTbl MySimpleObjectIUnknownVTable{
		.QueryInterface = QueryInterfaceImpl,
		.AddRef = AddRefImpl,
		.Release = ReleaseImpl
	};

	static IHelloVTbl MySimpleObjectHelloVTable{
		.QueryInterface = QueryInterfaceImpl,
		.AddRef = AddRefImpl,
		.Release = ReleaseImpl,
		.Hello = HelloImpl
	};

	MinimumCOMServer::MySimpleObject* CreateObject() {
		MySimpleObject* result = (MySimpleObject*)malloc(sizeof(MySimpleObject));
		//result->VTable = &MySimpleObjectIUnknownVTable;
		result->VTable = &MySimpleObjectHelloVTable;
		result->RefCount = 1;
		return result;
	}
}

MinimumCOMServer::MySimpleObject* CreateMinimumCOMObject()
{
	return MinimumCOMServer::CreateObject();
}