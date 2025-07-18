#pragma once

// Cross-platform export macros
#ifdef _WIN32
#define EXPORT_API __declspec(dllexport)
#define STDCALL __stdcall
#else
#define EXPORT_API __attribute__((visibility("default")))
#define STDCALL __attribute__((stdcall))
#endif
