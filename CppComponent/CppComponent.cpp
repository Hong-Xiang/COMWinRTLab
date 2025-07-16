#include "pch.h"

int AddC(int a, int b)
{
	return a + b;
}

const GUID IUnknownGuid = __uuidof(IUnknown); // "00000000-0000-0000-C000-000000000046"

static const GUID IUknownIID{ 0x0, 0x0, 0x0, { 0xc0, 0x00, 0x00, 0x00, 0x0, 0x0, 0x0, 0xa46 } };

struct IUknown {
	virtual HRESULT __stdcall QueryInterface(const GUID& iid, void** ppv) noexcept = 0;
	virtual ULONG __stdcall AddRef() noexcept = 0;
	virtual ULONG __stdcall Release() noexcept = 0;
};

// {0D65A935-2857-4487-9CCD-633D40563C4E}
static const GUID CalculatorIID =
{ 0xd65a935, 0x2857, 0x4487, { 0x9c, 0xcd, 0x63, 0x3d, 0x40, 0x56, 0x3c, 0x4e } };

struct ICalculator : IUnknown {
	virtual HRESULT __stdcall Hello() noexcept = 0;
	virtual INT32 __stdcall Add(INT32, INT32) noexcept = 0;
};


// {497A7BFE-BB9C-4DDF-914E-5DE5602325FF}
static const GUID Calculator2IID =
{ 0x497a7bfe, 0xbb9c, 0x4ddf, { 0x91, 0x4e, 0x5d, 0xe5, 0x60, 0x23, 0x25, 0xff } };

struct ICalculator2 : IUnknown {
	virtual HRESULT __stdcall Mul(INT32, INT32, INT32*) noexcept = 0;
};

void printf_guid(GUID guid) {
	printf("{%08lX-%04hX-%04hX-%02hhX%02hhX-%02hhX%02hhX%02hhX%02hhX%02hhX%02hhX}",
		guid.Data1, guid.Data2, guid.Data3,
		guid.Data4[0], guid.Data4[1], guid.Data4[2], guid.Data4[3],
		guid.Data4[4], guid.Data4[5], guid.Data4[6], guid.Data4[7]);
}

static const GUID CalculatorCLSID{ 0xbf94877b, 0x82a4, 0x44c5, { 0x9e, 0xe4, 0x5d, 0x7d, 0xf9, 0x61, 0x4, 0xa7 } };

class Calculator : ICalculator, ICalculator2 {
public:
	ULONG m_count{ 1 };


public:
	virtual HRESULT __stdcall QueryInterface(const GUID& iid, void** ppv) noexcept {
		printf("Query Interface for ");
		printf_guid(iid);
		printf("\n");
		if (ppv == nullptr) {
			return E_INVALIDARG;
		}
		if (iid == IUnknownGuid
			|| iid == CalculatorIID) {
			auto queryInterface = &Calculator::QueryInterface;
			*ppv = (void*)static_cast<ICalculator*>(this);
			auto val = *ppv;
			AddRef();
			return S_OK;
		}
		if (iid == Calculator2IID) {
			*ppv = (void*)static_cast<ICalculator2*>(this);
			AddRef();
			return S_OK;
		}
		return E_NOINTERFACE;
	};
	virtual ULONG __stdcall AddRef() noexcept {
		auto result = InterlockedIncrement(&m_count);
		printf("AddRef Called %d\n", result);
		return result;
	}
	virtual ULONG __stdcall Release() noexcept {
		auto result = InterlockedDecrement(&m_count);
		printf("Release Called %d\n", result);
		if (result == 0) {
			printf("Delete object\n");
			delete this;
		}
		return result;
	}
	virtual INT32 __stdcall Add(INT32 a, INT32 b) noexcept {
		return a + b;
	}
	virtual HRESULT __stdcall Mul(INT32 a, INT32 b, INT32* c) noexcept {
		if (c == nullptr) {
			return E_INVALIDARG;
		}
		*c = a * b;
		return S_OK;
	}
	virtual HRESULT __stdcall Hello() noexcept {
		printf("Hello from Calculator\n");
		return S_OK;
	}
};

Calculator* CreateCalculator() {
	auto result = new Calculator();
	printf("Create Object with count %d\n", result->m_count);
	return result;
}

struct IShape {
	virtual HRESULT __stdcall Area(double* result) noexcept = 0;
	virtual HRESULT __stdcall Perimeter(double* result) noexcept = 0;
};
