#pragma once
#include <Windows.h>

#ifdef _QINJECT
#define QLIB __declspec(dllexport)
#else
#define QLIB __declspec(dllimport)
#endif

#define MAX_RUNTIMES 8
#define MAX_APPDOMAINS 32

struct AppDomain {
	OLECHAR friendlyName[256];
	bool injected;
};

struct Runtime {
	WCHAR version[32];
	BOOL started;
	DWORD startedFlags;

	int numAppDomains;
	AppDomain appDomains[MAX_APPDOMAINS];
};

struct InjectionResult {
	long retVal;
	DWORD status;
	char statusMessage[256];

	int numRuntimes;
	Runtime runtimes[MAX_RUNTIMES];
};

struct InjectionOptions {
	bool enumerate;
	int appDomainIndex;
	DWORD processId;
	void* assembly;
	int assemblySize;
	OLECHAR typeName[256];
};

int Inject(const InjectionOptions * options, InjectionResult * result);

extern "C" QLIB void Inject(DWORD processId, void*assembly, int assemblySize, OLECHAR* typeName);