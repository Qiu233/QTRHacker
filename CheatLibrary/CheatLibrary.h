#pragma once
#include <stdio.h>
#include <wchar.h>
#ifdef  CHEATLIBRARY_EXPORTS
#define DLL _declspec(dllexport)
#else
#define DLL _declspec(dllimport)
#endif



#ifdef __cplusplus
extern "C" {
#endif
	DLL PVOID InitCL(UINT pid, wchar_t* module);

	DLL UINT GetNumberFunction(PVOID getter);

	DLL void GetFunction(PVOID getter, UINT index, PWCHAR name, UINT *address);

	DLL BOOL SearchFunctionByName(PVOID getter, const wchar_t* fullName, void**addr, unsigned int times);

	DLL BOOL SearchFunctionByAddress(PVOID getter, UINT address, PWCHAR name);

#ifdef __cplusplus
}
#endif
