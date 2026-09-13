#pragma once
#include "pre.h"
#include "DacpStructs.h"
using namespace System;
using namespace System::Linq;
namespace QHackCLR {
	namespace DacHelpers {
		// Keep native DAC snapshots exception-safe until a managed wrapper takes ownership.
		template<typename T> class NativeData {
			T* value = new T;
		public:
			NativeData() = default;
			NativeData(const NativeData&) = delete;
			NativeData& operator=(const NativeData&) = delete;
			~NativeData() { delete value; }
			T* get() { return value; }
			T* release() {
				T* result = value;
				value = nullptr;
				return result;
			}
		};
	}
}
namespace QHackCLR {
	namespace DacHelpers {
		ref class GlobalHelpers abstract sealed {

		public:
			static void Check(HRESULT result, String^ operation) {
				if (FAILED(result))
					throw gcnew System::Runtime::InteropServices::COMException(
						operation + " failed (HRESULT 0x" + result.ToString("X8") + ").", result);
			}
		};
		ref class SOSHelpers abstract sealed {
		public:
#define GET_STRING_ADDR_PROB_U(name, func, addrName) \
static String^ name(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS addrName) {\
	unsigned int needed = 0;\
	GlobalHelpers::Check(SOSDac->func(addrName, 0, nullptr, &needed), #func);\
	if (needed <= 1)\
		return nullptr;\
	if (needed > Int32::MaxValue) throw gcnew OverflowException("DAC name is too long.");\
	array<Char>^ buffer = gcnew array<Char>(needed);\
	pin_ptr<Char> ptr = &buffer[0];\
	GlobalHelpers::Check(SOSDac->func(addrName, needed, static_cast<wchar_t*>(ptr), &needed), #func);\
	if (needed == 0 || needed > (unsigned int)buffer->Length) throw gcnew InvalidOperationException("DAC name changed while reading.");\
	return gcnew String(static_cast<wchar_t*>(ptr), 0, (int)needed - 1);\
}
			GET_STRING_ADDR_PROB_U(GetAppDomainName, GetAppDomainName, appDomain);
			GET_STRING_ADDR_PROB_U(GetMethodTableName, GetMethodTableName, mt);
#undef GET_STRING_ADDR_PROB_U

			static String^ GetMethodDescName(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS md) {
				if (md == 0)
					return nullptr;
				unsigned int needed = 0;
				GlobalHelpers::Check(SOSDac->GetMethodDescName(md, 0, nullptr, &needed), "GetMethodDescName");
				if (needed <= 1)
					return String::Empty;
				if (needed > Int32::MaxValue / sizeof(Char))
					throw gcnew OverflowException("DAC method name is too long.");
				array<byte>^ buffer = gcnew array<byte>(needed * sizeof(Char));
				unsigned int actuallyNeeded = 0;
				pin_ptr<byte> ptr = &buffer[0];
				GlobalHelpers::Check(SOSDac->GetMethodDescName(md, needed, (WCHAR*)ptr, &actuallyNeeded), "GetMethodDescName");
				if (needed != actuallyNeeded)
				{
					if (actuallyNeeded == 0 || actuallyNeeded > Int32::MaxValue / sizeof(Char))
						throw gcnew InvalidOperationException("DAC returned an invalid method name length.");
					buffer = gcnew array<byte>(actuallyNeeded * sizeof(Char));
					ptr = &buffer[0];
					GlobalHelpers::Check(SOSDac->GetMethodDescName(md, actuallyNeeded, (WCHAR*)ptr, &actuallyNeeded), "GetMethodDescName");
				}
				if (actuallyNeeded == 0 || actuallyNeeded > (unsigned int)buffer->Length / sizeof(Char))
					throw gcnew InvalidOperationException("DAC method name changed while reading.");
				return System::Text::Encoding::Unicode->GetString(buffer, 0, (actuallyNeeded - 1) * sizeof(Char));
			}

			static IMetaDataImport* GetMetaDataImport(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS module)
			{
				IXCLRDataModule* dataModule = nullptr;
				GlobalHelpers::Check(SOSDac->GetModule(module, &dataModule), "GetModule");
				if (dataModule == nullptr)
					throw gcnew InvalidOperationException("DAC returned no module.");
				try {
					IMetaDataImport* result = nullptr;
					GlobalHelpers::Check(dataModule->QueryInterface(IID_IMetaDataImport, (void**)&result), "QueryInterface(IMetaDataImport)");
					if (result == nullptr)
						throw gcnew InvalidOperationException("DAC returned no metadata import.");
					// GetMdInterface adds a reference independently of dataModule. The caller owns it.
					return result;
				}
				finally {
					dataModule->Release();
				}
			}

			static array<CLRDATA_ADDRESS>^ GetAppDomainList(ISOSDacInterface* SOSDac)
			{
				DacpAppDomainStoreData adsData;
				GlobalHelpers::Check(SOSDac->GetAppDomainStoreData(&adsData), "GetAppDomainStoreData");

				unsigned int needed = adsData.DomainCount;
				if (needed == 0)
					return Array::Empty<CLRDATA_ADDRESS>();
				array<CLRDATA_ADDRESS>^ buffer = gcnew array<CLRDATA_ADDRESS>(needed);
				pin_ptr<CLRDATA_ADDRESS> ptr = &buffer[0];
				GlobalHelpers::Check(SOSDac->GetAppDomainList(needed, static_cast<CLRDATA_ADDRESS*>(ptr), &needed), "GetAppDomainList");
				if (needed > (unsigned int)buffer->Length)
					throw gcnew InvalidOperationException("AppDomain list changed while reading.");
				Array::Resize(buffer, (int)needed);
				return buffer;
			}
			static array<CLRDATA_ADDRESS>^ GetAssemblyList(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS appDomain)
			{
				DacpAppDomainData data;
				GlobalHelpers::Check(SOSDac->GetAppDomainData(appDomain, &data), "GetAppDomainData");

				int needed = data.AssemblyCount;
				if (needed == 0)
					return Array::Empty<CLRDATA_ADDRESS>();
				array<CLRDATA_ADDRESS>^ buffer = gcnew array<CLRDATA_ADDRESS>(needed);
				pin_ptr<CLRDATA_ADDRESS> ptr = &buffer[0];
				GlobalHelpers::Check(SOSDac->GetAssemblyList(appDomain, needed, static_cast<CLRDATA_ADDRESS*>(ptr), &needed), "GetAssemblyList");
				if (needed < 0 || needed > buffer->Length)
					throw gcnew InvalidOperationException("Assembly list changed while reading.");
				Array::Resize(buffer, needed);
				return buffer;
			}
			static array<CLRDATA_ADDRESS>^ GetAssemblyModuleList(ISOSDacInterface* SOSDac, CLRDATA_ADDRESS appDomain, CLRDATA_ADDRESS assembly)
			{
				DacpAssemblyData data;
				GlobalHelpers::Check(SOSDac->GetAssemblyData(appDomain, assembly, &data), "GetAssemblyData");

				unsigned int needed = data.ModuleCount;
				if (needed == 0)
					return Array::Empty<CLRDATA_ADDRESS>();
				array<CLRDATA_ADDRESS>^ buffer = gcnew array<CLRDATA_ADDRESS>(needed);
				pin_ptr<CLRDATA_ADDRESS> ptr = &buffer[0];
				GlobalHelpers::Check(SOSDac->GetAssemblyModuleList(assembly, needed, static_cast<CLRDATA_ADDRESS*>(ptr), &needed), "GetAssemblyModuleList");
				if (needed > (unsigned int)buffer->Length)
					throw gcnew InvalidOperationException("Module list changed while reading.");
				Array::Resize(buffer, (int)needed);
				return buffer;
			}
		};
	}
}
