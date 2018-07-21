#include "stdafx.h"
#include "DotNetDataGetter.h"

const IID IID_ICorDebugProcess = { 0x3d6f5f64, 0x7538, 0x11d3, 0x8d, 0x5b, 0x00, 0x10, 0x4b, 0x35, 0xe7, 0xef };
const IID IID_ICorDebugProcess5 = { 0x21e9d9c0, 0xfcb8, 0x11df, 0x8c, 0xff, 0x08, 0x00, 0x20, 0x0c ,0x9a, 0x66 };
const IID IID_ICorDebugCode2 = { 0x5F696509,0x452F,0x4436,0xA3,0xFE,0x4D,0x11,0xFE,0x7E,0x23,0x47 }; //5F696509-452F-4436-A3FE-4D11FE7E2347


using namespace std;

DotNetDataGetter::DotNetDataGetter(ULONG processid,PCWCHAR module)
{
	processhandle = 0;
	CorDebugProcess5 = NULL;
	CorDebugProcess = NULL;
	CLRDebugging = NULL;
	libprovider = NULL;
	datacallback = NULL;

	this->processid = processid;
	this->ModuleName = new WCHAR[50];
	ZeroMemory(this->ModuleName, 50);
	lstrcpyW(this->ModuleName, module);
}

DotNetDataGetter::~DotNetDataGetter(void)
{
	if (processhandle)
		CloseHandle(processhandle);
	if (CorDebugProcess5)
		CorDebugProcess5->Release();

	if (CorDebugProcess)
		CorDebugProcess->Release();

	if (CLRDebugging)
		CLRDebugging->Release();


	if (libprovider)
		delete libprovider;

	if (datacallback)
		delete datacallback;
}

BOOL DotNetDataGetter::OpenOrAttachToProcess(void)
{
	ICLRMetaHost *pMetaHost;
	IEnumUnknown *RuntimeEnum;
	HANDLE ths;
	MODULEENTRY32 m;
	HRESULT r;
	BOOL result = FALSE;



	CLR_DEBUGGING_VERSION v;
	v.wStructVersion = 0;
	v.wMajor = 4;
	v.wMinor = 0;
	v.wRevision = 30319;
	v.wBuild = 65535;


	CLRDebugging = NULL;
	CorDebugProcess = NULL;
	CorDebugProcess5 = NULL;

	libprovider = NULL;
	datacallback = NULL;

	HMODULE hMscoree = LoadLibraryA("mscoree.dll");
	CLRCreateInstanceFnPtr CLRCreateInstance;

	if (hMscoree == NULL)
		return FALSE;

	CLRCreateInstance = (CLRCreateInstanceFnPtr)GetProcAddress(hMscoree, "CLRCreateInstance");
	if (CLRCreateInstance == NULL)
		return FALSE; //only 4.0 is supported for now

	processhandle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processid);
	if (processhandle == 0)
		return FALSE;

	if (CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&pMetaHost) != S_OK)
		return FALSE;


	if (pMetaHost->EnumerateLoadedRuntimes(processhandle, &RuntimeEnum) == S_OK)
	{
		ICLRRuntimeInfo *info;
		ULONG count = 0;

		RuntimeEnum->Next(1, (IUnknown **)&info, &count);
		if (count)
		{
			result = TRUE;
			libprovider = new CMyICLRDebuggingLibraryProvider(info);  //todo: scan for 4.0			
		}

		RuntimeEnum->Release();
	}
	pMetaHost->Release();

	if (!result)
		return FALSE; //no runtimes


	if (CLRCreateInstance(CLSID_CLRDebugging, IID_ICLRDebugging, (void **)&CLRDebugging) != S_OK)
		return FALSE;

	ths = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE | TH32CS_SNAPMODULE32, processid);
	if (ths == INVALID_HANDLE_VALUE)
		return FALSE;

	ZeroMemory(&m, sizeof(m));
	m.dwSize = sizeof(m);
	if (Module32First(ths, &m))
	{
		datacallback = new CMyIcorDebugDataTarget(processhandle);

		do
		{
			CLR_DEBUGGING_PROCESS_FLAGS flags;

			r = CLRDebugging->OpenVirtualProcess((ULONG64)m.hModule, datacallback, libprovider, &v, IID_ICorDebugProcess, (IUnknown **)&CorDebugProcess, &v, &flags);

			if (r == S_OK)
			{
				CorDebugProcess->QueryInterface(IID_ICorDebugProcess5, (void **)&CorDebugProcess5);
				break;
			}

		} while (Module32Next(ths, &m));
	}


	CloseHandle(ths);


	if (r != S_OK)
	{
		if (r != S_OK)
			return FALSE;
	}
	//still here
	return TRUE;
}


void DotNetDataGetter::enumDomains(std::vector<DomainInfo> &result)
{
	if (CorDebugProcess)
	{
		ICorDebugAppDomainEnum *AppDomains;
		DWORD32 NumberOfDomains = 0;
		if (CorDebugProcess->EnumerateAppDomains(&AppDomains) == S_OK)
		{
			ULONG count;
			ULONG32 namelength = 0;

			AppDomains->GetCount(&count);
			ICorDebugAppDomain **domains = (ICorDebugAppDomain **)malloc(count * sizeof(ICorDebugAppDomain *));

			AppDomains->Next(count, domains, &count);

			NumberOfDomains = count;
			//WriteFile(pipe, &NumberOfDomains, sizeof(NumberOfDomains), &bw, NULL); //send the number of results to receive
			for (unsigned int i = 0; i < count; i++)
			{
				TCHAR *domainname = new TCHAR[255];
				UINT64 hDomain = (UINT64)domains[i];
				namelength = 0;
				domains[i]->GetName(255, &namelength, domainname);

				DomainInfo di = { hDomain,domainname };
				result.push_back(di);
			}

			if (domains)
				free(domains);

			AppDomains->Release();
		}
	}

}

void DotNetDataGetter::enumModules(UINT64 hDomain, std::vector<ModuleInfo> &modules)
{
	ICorDebugAppDomain *domain = (ICorDebugAppDomain *)hDomain;
	ICorDebugAssemblyEnum *pAssemblies;
	ICorDebugAssembly *assemblies[5];
	DWORD32 NumberOfModules = 0;
	unsigned int i, j;
	ULONG count;
	HRESULT r;

	std::vector<ICorDebugModule *> modulelist;

	if ((!domain) || (domain->EnumerateAssemblies(&pAssemblies) != S_OK))
	{
		//WriteFile(pipe, &NumberOfModules, sizeof(NumberOfModules), &bw, NULL);
		return;
	}

	modulelist.clear();

	do
	{
		r = pAssemblies->Next(5, assemblies, &count);
		for (i = 0; i < count; i++)
		{
			ICorDebugModuleEnum *pModules;
			if (assemblies[i]->EnumerateModules(&pModules) == S_OK)
			{
				ULONG modulecount;
				ICorDebugModule *modules[5];
				HRESULT r2;
				do
				{
					r2 = pModules->Next(5, modules, &modulecount);
					for (j = 0; j < modulecount; j++)
					{
						modulelist.push_back(modules[j]);
					}

				} while (r2 == S_OK);

				pModules->Release();
			}

			assemblies[i]->Release();
		}
	} while (r == S_OK);

	pAssemblies->Release();

	NumberOfModules = (DWORD32)modulelist.size();

	for (i = 0; i < NumberOfModules; i++)
	{
		UINT64 hModule = (UINT64)modulelist[i];
		TCHAR *modulename = new TCHAR[255];
		ULONG32 modulenamelength;
		CORDB_ADDRESS baseaddress;


		modulelist[i]->GetBaseAddress(&baseaddress);


		if (modulelist[i]->GetName(255, &modulenamelength, modulename) == S_OK)
			modulenamelength = sizeof(TCHAR)*modulenamelength;
		else
			modulenamelength = 0;
		int split = -1;
		for (int i = lstrlenW(modulename) - 1; i >= 0; i--)
		{
			if (modulename[i] == '\\')
			{
				split = i;
				break;
			}
		}
		if (split != -1)
		{
			modulename = &modulename[split + 1];
		}
		ModuleInfo mi = { hModule,baseaddress,modulename };
		modules.push_back(mi);
	}
}

IMetaDataImport *DotNetDataGetter::getMetaData(ICorDebugModule *module)
{
	IMetaDataImport *MetaData = moduleMetaData[module];

	if (MetaData == NULL)
	{
		module->GetMetaDataInterface(IID_IMetaDataImport, (IUnknown **)&MetaData);
		moduleMetaData[module] = MetaData;
	}


	return MetaData;
}



void DotNetDataGetter::enumTypeDefs(UINT64 hModule, std::vector<TypeInfo> &result)
{
	ICorDebugModule *module = (ICorDebugModule *)hModule;
	DWORD32 NumberOfTypeDefs = 0;
	IMetaDataImport *MetaData = NULL;
	HCORENUM henum = 0;
	HRESULT r;
	ULONG typedefcount, totalcount;

	mdTypeDef typedefs[512];

	unsigned int i;
	ULONG typedefnamesize;
	DWORD typedefflags;
	mdToken extends;

	if (module == NULL)
	{
		return;
	}

	MetaData = getMetaData(module);

	if (MetaData == NULL)
	{
		return;
	}
	MetaData->EnumTypeDefs(&henum, typedefs, 0, &typedefcount);
	MetaData->CountEnum(henum, &totalcount);
	do
	{
		r = MetaData->EnumTypeDefs(&henum, typedefs, 512, &typedefcount);

		for (i = 0; i < typedefcount; i++)
		{


			TCHAR *typedefname = new TCHAR[255];
			typedefnamesize = 0;
			MetaData->GetTypeDefProps(typedefs[i], typedefname, 255, &typedefnamesize, &typedefflags, &extends);

			typedefnamesize = sizeof(TCHAR)*typedefnamesize;
			TypeInfo ti = { typedefs[i],extends,typedefflags,typedefname };
			result.push_back(ti);
		}
	} while (r == S_OK);
	MetaData->CloseEnum(henum);
}

void DotNetDataGetter::enumTypeDefMethods(PCWCHAR typeName, UINT64 hModule, mdTypeDef TypeDef, vector<MethodInfo> &result)
{
	ICorDebugModule *module = (ICorDebugModule *)hModule;
	IMetaDataImport *MetaData = getMetaData(module);
	HRESULT r;
	if (MetaData)
	{
		unsigned int i;
		HCORENUM henum = 0;
		ULONG count = 0;
		ULONG methodnamesize;

		mdMethodDef methods[32];
		r = MetaData->EnumMethods(&henum, TypeDef, methods, 0, &count);
		MetaData->CountEnum(henum, &count);
		do
		{
			r = MetaData->EnumMethods(&henum, TypeDef, methods, 32, &count);
			for (i = 0; i < count; i++)
			{
				mdTypeDef classdef;
				unsigned int j;
				DWORD dwAttr;
				PCCOR_SIGNATURE sig;
				ULONG sigsize;
				ULONG CodeRVA;
				DWORD dwImplFlags;
				ICorDebugFunction *df = NULL;

				CORDB_ADDRESS ILCode = 0;
				CORDB_ADDRESS NativeCode = 0;
				ULONG32 NativeCodeSize = 1;
				CodeChunkInfo codechunks[8];

				ULONG32 SecondaryCodeBlocks = 0;


				TCHAR *methodname = new TCHAR[255];

				methodnamesize = 0;
				MetaData->GetMethodProps(methods[i], &classdef, methodname, 255, &methodnamesize, &dwAttr, &sig, &sigsize, &CodeRVA, &dwImplFlags);

				methodnamesize = sizeof(TCHAR)*methodnamesize;



				if (module->GetFunctionFromToken(methods[i], &df) == S_OK)
				{
					ICorDebugCode *Code;
					if (df->GetILCode(&Code) == S_OK)
					{
						Code->GetAddress(&ILCode);
						Code->Release();
					}

					if (df->GetNativeCode(&Code) == S_OK)
					{
						ICorDebugCode2 *Code2;
						Code->GetAddress(&NativeCode);

						if (Code->QueryInterface(IID_ICorDebugCode2, (void **)&Code2) == S_OK)
						{
							ULONG32 count;
							ZeroMemory(codechunks, 8 * sizeof(CodeChunkInfo));
							Code2->GetCodeChunks(8, &count, codechunks);  //count is useless
							Code2->Release();

							SecondaryCodeBlocks = 0;
							for (j = 0; j < count; j++)
								if (codechunks[j].startAddr)
									SecondaryCodeBlocks++;
						}
						Code->Release();
					}

				}
				wchar_t *name = new wchar_t[255];
				wsprintf(name, L"%ws::%ws", typeName, methodname);
				MethodInfo mi = { methods[i],ILCode,NativeCode,name };
				result.push_back(mi);
			}
		} while (r == S_OK);

	}
}

void DotNetDataGetter::Init()
{
	OpenOrAttachToProcess();
	vector<DomainInfo> domains;
	enumDomains(domains);
	for (UINT i = 0; i < domains.size(); i++)
	{
		vector<ModuleInfo> modules;
		enumModules(domains[i].ID, modules);
		for (UINT j = 0; j < modules.size(); j++)
		{
			if (lstrcmpW(modules[j].name, this->ModuleName) == 0)
			{
				vector<TypeInfo> types;
				enumTypeDefs(modules[j].handle, types);
				for (UINT r = 0; r < types.size(); r++)
				{
					enumTypeDefMethods(types[r].name, modules[j].handle, types[r].token, methods);
				}
			}
		}
	}
}

bool DotNetDataGetter::SearchMethodByName(PCWCHAR fullName, MethodInfo &method, UINT32 times)
{
	UINT32 i = 0;
	auto it = methods.begin();
	for (; it != methods.end(); it++)
	{
		if (lstrcmpW(it->name, fullName) == 0)
		{
			i++;
			if (i == times) 
			{
				method.ILCode = it->ILCode;
				method.name = it->name;
				method.NativeCode = it->NativeCode;
				method.token = it->token;
				return true;
			}
		}
	}
	return false;
}
bool DotNetDataGetter::SearchMethodByAddress(UINT address, MethodInfo &method)
{
	auto it = methods.begin();
	for (; it != methods.end(); it++)
	{
		if (it->NativeCode == address)
		{
			method.ILCode = it->ILCode;
			method.name = it->name;
			method.NativeCode = it->NativeCode;
			method.token = it->token;
			return true;
		}
	}
	return false;
}
