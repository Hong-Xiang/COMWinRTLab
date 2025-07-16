#include "pch.h"
#include <iostream>

namespace MinimumCOMServer
{
	// Forward declaration
	struct IUnknownVTbl;

	struct MySimpleObject {
		IUnknownVTbl* VTable;
		int RefCount;
	};

	// Function pointer typedefs for COM IUnknownVTbl interface
	typedef long(__stdcall* QueryInterfaceFunc)(MySimpleObject* This, const GUID* riid, void** ppvObject);
	typedef unsigned long(__stdcall* AddRefFunc)(MySimpleObject* This);
	typedef unsigned long(__stdcall* ReleaseFunc)(MySimpleObject* This);
	typedef long(__stdcall* HelloFunc)(MySimpleObject* This);

	struct IUnknownVTbl
	{
		// COM IUnknownVTbl interface function pointers
		QueryInterfaceFunc QueryInterface; // Retrieves pointers to supported interfaces
		AddRefFunc AddRef;				   // Increments reference count
		ReleaseFunc Release;			   // Decrements reference count
	};

	struct IHelloVTbl : IUnknownVTbl {

	};

	long __stdcall QueryInterfaceImpl(MySimpleObject* self, const GUID* riid, void** ppvObject)
	{
		std::cout << "Query Interface Called with self" << self << std::endl;
		*ppvObject = self;
		return 0;
	}

	unsigned long __stdcall AddRefImpl(MySimpleObject* self)
	{
		std::cout << "AddRef called" << std::endl;
		return ++self->RefCount;
	}

	unsigned long __stdcall ReleaseImpl(MySimpleObject* self)
	{
		std::cout << "Release called" << std::endl;
		if (--self->RefCount == 0)
		{
			std::cout << "free memory" << std::endl;
			free(self);
			return 0;
		}
		return self->RefCount;
	}


	static IUnknownVTbl MySimpleObjectIUnknownVTable{
		.QueryInterface = QueryInterfaceImpl,
		.AddRef = AddRefImpl,
		.Release = ReleaseImpl
	};


	MinimumCOMServer::MySimpleObject* CreateObject() {
		MySimpleObject* result = (MySimpleObject*)malloc(sizeof(MySimpleObject));
		result->VTable = &MySimpleObjectIUnknownVTable;
		result->RefCount = 1;
		return result;
	}
}

MinimumCOMServer::MySimpleObject* CreateMinimumCOMObject()
{
	return MinimumCOMServer::CreateObject();
}