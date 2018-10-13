#include "stdafx.h"
#include "CheatLibrary.h"
#include "DotNetDataGetter.h"



UINT GetNumberFunction(PVOID getter)
{
	return ((DotNetDataGetter*)getter)->methods.size();
}

void GetFunction(PVOID getter, UINT index, PWCHAR name, UINT *address)
{
	auto a = ((DotNetDataGetter*)getter)->methods[index];
	lstrcpyW(name, a.name);
	*address = (UINT)a.NativeCode;
}

PVOID InitCL(UINT pid, wchar_t* module)
{
	DotNetDataGetter *getter = new DotNetDataGetter(pid, module);
	getter->Init();
	return getter;
}

BOOL SearchFunctionByName(PVOID getter, const wchar_t* fullName, void** addr, unsigned int times)
{
	if (getter == NULL)return false;
	MethodInfo method;
	BOOL b = ((DotNetDataGetter*)getter)->SearchMethodByName(fullName, method, times);
	*addr = (void*)method.NativeCode;
	return b;
}
BOOL SearchFunctionByAddress(PVOID getter, UINT address, PWCHAR name)
{
	if (getter == NULL)return false;
	MethodInfo method;
	BOOL b = ((DotNetDataGetter*)getter)->SearchMethodByAddress(address, method);
	wsprintf(name, L"%ws", method.name);
	return b;
}
