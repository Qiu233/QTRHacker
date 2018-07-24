#include "stdafx.h"
#include "CheatLibrary.h"
#include "DotNetDataGetter.h"

DotNetDataGetter *getter = NULL;
UINT processID;



UINT GetNumberFunction()
{
	return getter->methods.size();
}

void GetFunction(UINT index, PWCHAR name, UINT *address)
{
	auto a = getter->methods[index];;
	lstrcpyW(name, a.name);
	*address = (UINT)a.NativeCode;
}

VOID InitCL(UINT pid, BOOL dotNet, wchar_t* module)
{
	processID = pid;
	if (dotNet)
	{
		getter = new DotNetDataGetter(processID, module);
		getter->Init();
	}
}

BOOL SearchFunctionByName(const wchar_t* fullName, void** addr, unsigned int times)
{
	if (getter == NULL)return false;
	MethodInfo method;
	BOOL b = getter->SearchMethodByName(fullName, method, times);
	*addr = (void*)method.NativeCode;
	return b;
}
BOOL SearchFunctionByAddress(UINT address, PWCHAR name)
{
	if (getter == NULL)return false;
	MethodInfo method;
	BOOL b = getter->SearchMethodByAddress(address, method);
	wsprintf(name, L"%ws", method.name);
	return b;
}


UINT memmem(const char * a, int alen, const char * b, int blen)
{
	int i, j;
	for (i = 0; i < alen - blen; ++i)
	{
		for (j = 0; j < blen; ++j)
		{
			if (a[i + j] != b[j])
			{
				break;
			}
		}
		if (j >= blen)
		{
			return i;
		}
	}
	return -1;
}

UINT ctoh(char hex)
{
	if (hex >= '0' && hex <= '9') return hex - '0';
	if (hex >= 'A' && hex <= 'F') return hex - 'A' + 10;
	if (hex >= 'a' && hex <= 'f') return hex - 'a' + 10;
	return 0;
}

UINT getHexCode(const char *str, BYTE *res)
{
	char tmp[2] = "";
	int j = 0;
	int k = 0;
	for (unsigned i = 0; i < strlen(str); i++)
	{
		char c = str[i];
		if ((c >= '0'&&c <= '9') || (c >= 'A'&&c <= 'Z') || (c >= 'a'&&c <= 'z'))
		{
			if (j % 2 == 1)
			{
				tmp[1] = str[i];
				int v = 0x10 * ctoh(tmp[0]) + ctoh(tmp[1]);
				res[k] = v;
				k++;
			}
			else if (j % 2 == 0)
			{
				tmp[0] = str[i];
			}
			j++;
		}
	}
	return k;
}


PVOID Aobscan(const char *aob, UINT beginaddr)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
	BYTE b[256];
	int len = getHexCode(aob, b);
	MEMORY_BASIC_INFORMATION mbi;
	int i = beginaddr;
	while (i < 0x7FFFFFFF)
	{
		VirtualQueryEx(hProcess, (void*)i, &mbi, sizeof(MEMORY_BASIC_INFORMATION));
		if ((int)mbi.RegionSize <= 0)
		{
			break;
		}
		if (mbi.Protect != PAGE_EXECUTE_READWRITE || mbi.State != MEM_COMMIT)
		{
			int ti = (int)i;
			i += mbi.RegionSize;
			if (i <= ti)
			{
				break;
			}
			continue;
		}
		char *mem = (char*)malloc((int)mbi.RegionSize);
		ReadProcessMemory(hProcess, (void*)i, mem, (int)mbi.RegionSize, 0);
		int r = memmem(mem, (int)mbi.RegionSize, (char*)b, len);
		if (r >= 0)
		{
			return (PVOID)(i + r);
		}
		int ti = (int)i;
		i += mbi.RegionSize;
		if (i <= ti)
		{
			break;
		}
	}
	return (PVOID)NULL;
}

VOID Inline_Hook_Bytes(void*addr, BYTE*code, UINT dstLen, UINT len)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
	BYTE *ASM_CODE = (BYTE*)malloc(len + 5);
	memcpy(ASM_CODE, code, len);
	ASM_CODE[len] = 0xE9;//jmp
	UINT*retjmp = (UINT *)&ASM_CODE[len + 1];
	PWSTR pCode = (PWSTR)VirtualAllocEx(hProcess, NULL, len + 5, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
	*retjmp = (UINT)addr + dstLen - ((UINT)pCode + len + 5);
	WriteProcessMemory(hProcess, (void*)pCode, (void*)&ASM_CODE[0], len + 5, NULL);
	BYTE HookCode[5] = { 0xE9 };
	UINT*jmp = (UINT*)&HookCode[1];
	*jmp = (UINT)pCode - (UINT)addr - 5;
	WriteProcessMemory(hProcess, addr, HookCode, 5, NULL);
	Sleep(100);
	CloseHandle(hProcess);
	free(ASM_CODE);
}

DLL VOID Inline_Hook_Code(void*addr, const char*code, UINT dstLen)
{
	BYTE *bytes = (BYTE*)malloc(4096);
	int len = getHexCode(code, bytes);
	Inline_Hook_Bytes(addr, bytes, dstLen, len);
	free(bytes);
}

VOID ReadFromMultiOffset(void* dstAddr, void* data, UINT dataLen, UINT offCount, ...)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
	va_list args;
	va_start(args, offCount);
	void* baseaddr = dstAddr;
	while (offCount > 1)
	{
		int off = va_arg(args, int);
		ReadProcessMemory(hProcess, (void*)((int)baseaddr + off), &baseaddr, 4, NULL);
		offCount--;
	}
	int off = va_arg(args, int);
	ReadProcessMemory(hProcess, (void*)((int)baseaddr + off), data, dataLen, NULL);
	va_end(args);
	CloseHandle(hProcess);
}

VOID WriteByteCode(void* dstAddr, const char *byteCode)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
	BYTE bytes[256];
	int len = getHexCode(byteCode, bytes);
	WriteProcessMemory(hProcess, dstAddr, bytes, len, NULL);
	CloseHandle(hProcess);
}

DLL VOID InjectData(const char*magic, void* data, UINT len)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
	void *addr = Aobscan(magic, 0);
	void *tdata;
	if (addr)//is exists
	{
		tdata = (void*)((UINT)addr + 16);
	}
	else
	{
		void* p = VirtualAllocEx(hProcess, NULL, 16 + len, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
		WriteByteCode(p, magic);
		tdata = (void*)((UINT)p + 16);
	}
	WriteProcessMemory(hProcess, tdata, data, len, NULL);
	CloseHandle(hProcess);
}
DLL BOOL ReadData(const char*magic, void* data, UINT len)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
	void *addr = Aobscan(magic, 0);
	void *tdata;
	if (addr)//is exists
	{
		tdata = (void*)((UINT)addr + 16);
	}
	else
	{
		CloseHandle(hProcess);
		return FALSE;
	}
	ReadProcessMemory(hProcess, tdata, data, len, NULL);
	CloseHandle(hProcess);
	return TRUE;
}
DLL VOID DeleteData(const char *magic)
{
	HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
	void *addr = Aobscan(magic, 0);
	if (addr)
	{
		VirtualFree(addr, NULL, MEM_RELEASE);
	}
	CloseHandle(hProcess);
}
