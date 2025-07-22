#define WIN32_LEAN_AND_MEAN   
#include <Windows.h>

#include <iostream>
#include <wil/com.h>
#include <combaseapi.h>

struct __declspec(uuid("56f52a44-2e07-4fce-be7a-6473e4ba0be8")) Hello {};
struct __declspec(uuid("b0bf416d-9e3a-4d46-9377-af3db3cb10e4")) IHello : IUnknown {
	virtual HRESULT __stdcall SayHello() = 0;
};

int main()
{
	std::cout << "Start Cpp Test Console App" << std::endl;
	//wil::CoInitializeEx(COINITBASE_MULTITHREADED);
	HRESULT hr;
	hr = CoInitializeEx(nullptr, COINITBASE_MULTITHREADED);
	std::cout << "CoInitialize -> " << hr << std::endl;



	void* p = nullptr;
	hr = CoCreateInstance(__uuidof(Hello), nullptr, CLSCTX_INPROC, __uuidof(IHello), &p);
	std::cout << "CoCreateInstance -> " << hr << std::endl;
	if (SUCCEEDED(hr)) {
		auto hello = (IHello*)p;
		hello->SayHello();
		hello->Release();
	}
	else {
		std::cout << "Failed to create instance of Hello." << std::endl;
	}

	CoUninitialize();



	return 0;
}