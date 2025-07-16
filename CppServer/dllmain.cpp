// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <iostream>

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		std::cout << "cpp-server dll attach" << std::endl;
		break;
	case DLL_THREAD_ATTACH:
		std::cout << "cpp-server thread attach" << std::endl;
		break;
	case DLL_THREAD_DETACH:
		std::cout << "cpp-server thread detach" << std::endl;
		break;
	case DLL_PROCESS_DETACH:
		std::cout << "cpp-server dll detach" << std::endl;
		break;
	}
	return TRUE;
}

