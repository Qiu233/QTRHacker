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
	DLL VOID InitCL(UINT pid, BOOL dotNet,wchar_t* module);

	DLL BOOL SearchFunctionByName(const wchar_t* fullName, void**addr, unsigned int times);

	DLL BOOL SearchFunctionByAddress(UINT address, PWCHAR name);

	DLL PVOID Aobscan(const char *aob, UINT beginaddr);

	DLL VOID Inline_Hook_Bytes(void*addr, BYTE*code, UINT dstLen, UINT len);
	DLL VOID Inline_Hook_Code(void*addr, const char*code, UINT dstLen);

	DLL VOID ReadFromMultiOffset(void* dstAddr, void* data, UINT dataLen, UINT offCount, ...);
	DLL VOID WriteByteCode(void* dstAddr,const char *byteCode);


	//magic's length must be 16 bytes
	DLL VOID InjectData(const char *magic, void* data, UINT len);
	//magic's length must be 16 bytes
	DLL BOOL ReadData(const char *magic, void* data, UINT len);
	//magic's length must be 16 bytes
	DLL VOID DeleteData(const char *magic);
#ifdef __cplusplus
}
#endif
