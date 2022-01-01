#include "Common.h"
#include "DataTargets.h"
#include "Builders.h"
#include "Dac.h"
#include "DacHelpers.h"
#include "Utils.h"

namespace QHackCLR {
	namespace Builders {

		RuntimeBuilder::RuntimeBuilder(DataTargets::ClrInfo^ clrInfo, Dac::DacLibrary^ dacCibrary)
		{
			m_ClrInfo = clrInfo;
			m_DacLibrary = dacCibrary;
			m_Runtime = gcnew Common::ClrRuntime(clrInfo, this);
		}
		Common::ClrHeap^ RuntimeBuilder::Heap::get() {
			return this->m_Runtime->Heap;
		}
		Common::ClrAppDomain^ RuntimeBuilder::AppDomain::get() {
			return this->m_Runtime->AppDomain;
		}
		IXCLRDataProcess* RuntimeBuilder::CLRDataProcess::get() {
			return m_DacLibrary->ClrDataProcess;
		}
		ISOSDacInterface* RuntimeBuilder::SOSDac::get() {
			return m_DacLibrary->SOSDac;
		}
		DataTargets::DataAccess^ RuntimeBuilder::DataAccess::get() {
			return this->Runtime->DataTarget->DataAccess;
		}

		System::Collections::Generic::IEnumerable<Common::ClrModule^>^ RuntimeBuilder::EnumerateModules(Common::ClrAppDomain^ appDomain)
		{
			List<Common::ClrModule^>^ res = gcnew List<Common::ClrModule^>();
			auto assemblies = DacHelpers::SOSHelpers::GetAssemblyList(SOSDac, appDomain->NativeHandle);
			for each (auto assembly in assemblies) {
				auto modules = DacHelpers::SOSHelpers::GetAssemblyModuleList(SOSDac, appDomain->NativeHandle, assembly);
				for each (auto module in modules) {
					res->Add(GetModule(UIntPtr(module)));
				}
			}
			return res;
		}

		bool RuntimeBuilder::IsInitialized(DacpDomainLocalModuleData* data, int token) {
			CLRDATA_ADDRESS flagsAddr = (data->pClassData + (token & ~0x02000000u) - 1);
			byte flags = this->DataAccess->Read<byte>(UIntPtr(flagsAddr));
			return (flags & 1) != 0;
		}

		IMetaDataImport* RuntimeBuilder::GetMetadataImport(Common::ClrModule^ module)
		{
			return DacHelpers::SOSHelpers::GetMetaDataImport(SOSDac, module->NativeHandle);
		}

		UIntPtr RuntimeBuilder::GetStaticFieldAddress(Common::ClrStaticField^ field) {
			Common::ClrType^ type = field->DeclaringType;
			Common::ClrModule^ module = type->Module;
			bool shared = type->IsShared;
			DacpDomainLocalModuleData dlmd;
			if (shared)
			{
				if (AppDomain == nullptr)
					return UIntPtr::Zero;
				DacpModuleData data;
				if (FAILED(SOSDac->GetModuleData(module->NativeHandle, &data)))
					return UIntPtr::Zero;
				if (FAILED(SOSDac->GetDomainLocalModuleDataFromAppDomain(AppDomain->NativeHandle, (int)data.dwModuleID, &dlmd)))
					return UIntPtr::Zero;
				if (!shared && !IsInitialized(&dlmd, (int)type->MDToken))
					return UIntPtr::Zero;

				if (Utils::CorElementTypeIsPrimitive(field->ElementType))
					return UIntPtr(dlmd.pNonGCStaticDataStart + field->Offset);
				else
					return UIntPtr(dlmd.pGCStaticDataStart + field->Offset);
			}
			else
			{
				if (FAILED(SOSDac->GetDomainLocalModuleDataFromModule(module->NativeHandle, &dlmd)))
					return UIntPtr::Zero;
			}
			if (Utils::CorElementTypeIsPrimitive(field->ElementType))
				return UIntPtr(dlmd.pNonGCStaticDataStart + field->Offset);
			else
				return UIntPtr(dlmd.pGCStaticDataStart + field->Offset);
		}

		bool RuntimeBuilder::GetFieldProps(Common::ClrType^ parentType, int token, String^% name, FieldAttributes% attributes) {
			IMetaDataImport* import = parentType->Module->MetadataImport;
			DWORD attr;
			ULONG needed;
			if (import == nullptr || FAILED(import->GetFieldProps(token, nullptr, nullptr, 0, &needed, &attr, nullptr, nullptr, nullptr, nullptr, nullptr)))
			{
				name = nullptr;
				attributes = static_cast<FieldAttributes>(0);
				return false;
			}
			attributes = static_cast<FieldAttributes>(attr);
			wchar_t* buffer = new wchar_t[needed];
			import->GetFieldProps(token, nullptr, buffer, needed, &needed, nullptr, nullptr, nullptr, nullptr, nullptr, nullptr);
			name = gcnew String(buffer);
			delete buffer;
			return true;
		}

		Generic::IEnumerable<Common::ClrField^>^ RuntimeBuilder::EnumerateFields(Common::ClrType^ type) {
			DacpMethodTableFieldData info;
			SOSDac->GetMethodTableFieldData(type->NativeHandle, &info);
			List<Common::ClrField^>^ fields = gcnew List<Common::ClrField^>();
			auto field = info.FirstField;
			while (field != 0)
			{
				DacpFieldDescData data;
				if (FAILED(SOSDac->GetFieldDescData(field, &data))) {
					break;
				}
				if (data.bIsStatic != 0)
					fields->Add(gcnew Common::ClrStaticField(type, this, UIntPtr(field)));
				else
					fields->Add(gcnew Common::ClrInstanceField(type, this, UIntPtr(field)));
				field = data.NextField;
			}
			return fields;
		}
		Generic::IEnumerable<Common::ClrMethod^>^ RuntimeBuilder::EnumerateVTableMethods(Common::ClrType^ type) {
			auto mt = type->NativeHandle;
			DacpMethodTableData mtData;
			SOSDac->GetMethodTableData(mt, &mtData);
			List<Common::ClrMethod^>^ methods = gcnew List<Common::ClrMethod^>();
			for (int i = 0; i < mtData.wNumMethods; i++)
			{
				CLRDATA_ADDRESS slot;
				DacpCodeHeaderData chdata;
				SOSDac->GetMethodTableSlot(mt, i, &slot);
				SOSDac->GetCodeHeaderData(slot, &chdata);
				methods->Add(gcnew Common::ClrMethod(this, UIntPtr(chdata.MethodDescPtr)));
			}
			return methods;
		}


#define TRY_GET_CACHE(type, dict, name)							\
		type^ RuntimeBuilder::name(UIntPtr handle) {			\
			if (handle == UIntPtr::Zero) return nullptr;		\
			type^ value;										\
			if (dict->TryGetValue(handle, value))				\
				return value;									\
			dict[handle] = gcnew type(this, handle);			\
			return dict[handle];								\
		}														\

		TRY_GET_CACHE(Common::ClrAppDomain, AppDomains, GetAppDomain);
		TRY_GET_CACHE(Common::ClrType, Types, GetClrType);
		TRY_GET_CACHE(Common::ClrModule, Modules, GetModule);
#undef TRY_GET_CACHE
	}
}