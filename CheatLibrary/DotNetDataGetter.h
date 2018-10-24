#pragma once


#include "stdafx.h"
#include "CheatLibrary.h"
#include "MyICLRDebuggingLibraryProvider.h"
#include "MyICorDebugDataTarget.h"

#include <Windows.h>
#include <MSCorEE.h>
#include <cor.h>
#include <CorHdr.h>
#include <CorDebug.h>

typedef struct
{
	mdTypeDef token;
	CORDB_ADDRESS ILCode;
	CORDB_ADDRESS NativeCode;
	PWCHAR name;
}MethodInfo;

typedef struct
{
	UINT64 handle;
	CORDB_ADDRESS baseAddr;
	PWCHAR name;
}ModuleInfo;

typedef struct
{
	UINT64 ID;
	PWCHAR name;
}DomainInfo;

typedef struct
{
	mdTypeDef token;
	mdToken extends;
	DWORD flags;
	PWCHAR name;
}TypeInfo;

class DotNetDataGetter
{
private:
	ULONG processid;
	HANDLE processhandle;
	ICLRDebugging *CLRDebugging;
	ICorDebugProcess *CorDebugProcess;
	ICorDebugProcess5 *CorDebugProcess5;
	CMyICLRDebuggingLibraryProvider *libprovider;
	CMyIcorDebugDataTarget *datacallback; 
	std::vector<DomainInfo> domains;
	std::vector<ModuleInfo> modules;
	std::vector<TypeInfo> types;
	ModuleInfo MInfo;
	std::map<ICorDebugModule*, IMetaDataImport*> moduleMetaData;

	IMetaDataImport *getMetaData(ICorDebugModule *module);
	BOOL OpenOrAttachToProcess(void);
	PWCHAR ModuleName;
	void enumDomains(std::vector<DomainInfo> &result);
	void enumModules(UINT64 hDomain, std::vector<ModuleInfo> &modules);
	void enumTypeDefs(UINT64 hModule, std::vector<TypeInfo> &result);
	void enumTypeDefMethods(PCWCHAR typeName, UINT64 hModule, mdTypeDef TypeDef, std::vector<MethodInfo> &result);
public:
	std::vector<MethodInfo> methods;
	DotNetDataGetter(ULONG processid,PCWCHAR module);
	~DotNetDataGetter(void);
	void Init();
	bool SearchMethodByName(PCWCHAR fullName, MethodInfo &method, UINT32 times);
	bool SearchMethodByAddress(UINT address, MethodInfo &method);
};
