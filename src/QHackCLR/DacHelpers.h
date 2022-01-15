#pragma once
#include "pre.h"
#include "DacpStructs.h"
using namespace System;
using namespace System::Linq;
namespace QHackCLR {
	namespace DacHelpers {
	}
}
namespace QHackCLR {
	namespace DacHelpers {
		ref class GlobalHelpers abstract sealed {

		public:
		};
		ref class SOSHelpers abstract sealed {
		public:
#define GET_STRING_ADDR_PROB_U(name, func, addrName) \
static String^ name(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS addrName) {\
	unsigned int needed = 0;\
	SOSDac->func(addrName, 0, nullptr, &needed);\
	if (needed <= 1)\
		return nullptr;\
	array<Char>^ buffer = gcnew array<Char>(needed);\
	pin_ptr<Char> ptr = &buffer[0];\
	SOSDac->func(addrName, needed, static_cast<wchar_t*>(ptr), &needed);\
	return gcnew String(Enumerable::ToArray(Enumerable::SkipLast<Char>(buffer, 1)));\
}
			GET_STRING_ADDR_PROB_U(GetAppDomainName, GetAppDomainName, appDomain);
			GET_STRING_ADDR_PROB_U(GetMethodTableName, GetMethodTableName, mt);
			GET_STRING_ADDR_PROB_U(GetMethodDescName, GetMethodDescName, md);
#undef GET_STRING_ADDR_PROB_U

			static IMetaDataImport* GetMetaDataImport(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS module)
			{
				IXCLRDataModule* dataModule;
				SOSDac->GetModule(module, &dataModule);
				IMetaDataImport* result = nullptr;
				dataModule->QueryInterface(IID_IMetaDataImport, (void**)&result);
				return result;
			}

			static array<CLRDATA_ADDRESS>^ GetAppDomainList(ISOSDacInterface* SOSDac)
			{
				DacpAppDomainStoreData adsData;
				SOSDac->GetAppDomainStoreData(&adsData);

				unsigned int needed = adsData.DomainCount;
				if (needed == 0)
					return Array::Empty<CLRDATA_ADDRESS>();
				array<CLRDATA_ADDRESS>^ buffer = gcnew array<CLRDATA_ADDRESS>(needed);
				pin_ptr<CLRDATA_ADDRESS> ptr = &buffer[0];
				SOSDac->GetAppDomainList(needed, static_cast<CLRDATA_ADDRESS*>(ptr), &needed);
				return buffer;
			}
			static array<CLRDATA_ADDRESS>^ GetAssemblyList(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS appDomain)
			{
				DacpAppDomainData data;
				SOSDac->GetAppDomainData(appDomain, &data);

				int needed = data.AssemblyCount;
				if (needed == 0)
					return Array::Empty<CLRDATA_ADDRESS>();
				array<CLRDATA_ADDRESS>^ buffer = gcnew array<CLRDATA_ADDRESS>(needed);
				pin_ptr<CLRDATA_ADDRESS> ptr = &buffer[0];
				SOSDac->GetAssemblyList(appDomain, needed, static_cast<CLRDATA_ADDRESS*>(ptr), &needed);
				return buffer;
			}
			static array<CLRDATA_ADDRESS>^ GetAssemblyModuleList(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS appDomain, CLRDATA_ADDRESS assembly)
			{
				DacpAssemblyData data;
				SOSDac->GetAssemblyData(appDomain, assembly, &data);

				unsigned int needed = data.ModuleCount;
				if (needed == 0)
					return Array::Empty<CLRDATA_ADDRESS>();
				array<CLRDATA_ADDRESS>^ buffer = gcnew array<CLRDATA_ADDRESS>(needed);
				pin_ptr<CLRDATA_ADDRESS> ptr = &buffer[0];
				SOSDac->GetAssemblyModuleList(assembly, needed, static_cast<CLRDATA_ADDRESS*>(ptr), &needed);
				return buffer;
			}
		};
	}
}