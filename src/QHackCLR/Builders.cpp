#include "Common.h"
#include "DataTargets.h"
#include "Builders.h"
#include "Dac.h"
#include "DacHelpers.h"
#include "Utils.h"

namespace QHackCLR {
	namespace Builders {

		namespace {
			ref class Anonymous_1 {
			public:
				IXCLRDataProcess* obj;
				void Flush() {
					this->obj->Flush();
				}
			};
		}

		RuntimeBuilder::RuntimeBuilder(DataTargets::ClrInfo^ clrInfo, Dac::DacLibrary^ dacLibrary)
		{
			m_ClrInfo = clrInfo;
			m_DacLibrary = dacLibrary;
			m_Runtime = gcnew Common::ClrRuntime(clrInfo, this);
			Anonymous_1^ tmp = gcnew Anonymous_1();
			tmp->obj = m_DacLibrary->ClrDataProcess;
			m_DacLibrary->DataTarget->SetMagicCallback(gcnew Action(tmp, &Anonymous_1::Flush));
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
		void RuntimeBuilder::Flush() {
			// CLRDataProcess->Flush();
			// The above cannot be called directly because it would potentially cause heap corruption.
			// A tricky solution is to call it in DataTarget's Read function, on getting a magic number.
			this->m_DacLibrary->DataTarget->EnterMagicCallbackContext();
			try {
				DacpWorkRequestData _;
				this->m_DacLibrary->SOSDac->GetWorkRequestData(MAGIC_CALLBACK_CONSTANT, &_);
			}
			finally {
				this->m_DacLibrary->DataTarget->ExitMagicCallbackContext();
			}
			AppDomains->Clear();
			Modules->Clear();
			Types->Clear();
		}

		System::Collections::Generic::IEnumerable<Common::ClrModule^>^ RuntimeBuilder::EnumerateModules(Common::ClrAppDomain^ appDomain)
		{
			List<Common::ClrModule^>^ res = gcnew List<Common::ClrModule^>();
			auto assemblies = DacHelpers::SOSHelpers::GetAssemblyList(SOSDac, appDomain->NativeHandle);
			for each (auto assembly in assemblies) {
				auto modules = DacHelpers::SOSHelpers::GetAssemblyModuleList(SOSDac, appDomain->NativeHandle, assembly);
				for each (auto module in modules) {
					res->Add(GetModule(DacHelpers::GlobalHelpers::ToNativeAddress(module)));
				}
			}
			return res;
		}

		bool RuntimeBuilder::IsInitialized(DacpDomainLocalModuleData* data, int token) {
			CLRDATA_ADDRESS flagsAddr = (data->pClassData + (token & ~0x02000000u) - 1);
			byte flags = this->DataAccess->Read<byte>(DacHelpers::GlobalHelpers::ToNativeAddress(flagsAddr));
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
				DacHelpers::GlobalHelpers::Check(SOSDac->GetModuleData(module->NativeHandle, &data), "GetModuleData");
				DacHelpers::GlobalHelpers::Check(SOSDac->GetDomainLocalModuleDataFromAppDomain(AppDomain->NativeHandle, (int)data.dwModuleID, &dlmd), "GetDomainLocalModuleDataFromAppDomain");
				if (!shared && !IsInitialized(&dlmd, (int)type->MDToken))
					return UIntPtr::Zero;

				if (Utils::CorElementTypeIsPrimitive(field->ElementType))
					return DacHelpers::GlobalHelpers::ToNativeAddress(dlmd.pNonGCStaticDataStart + field->Offset);
				else
					return DacHelpers::GlobalHelpers::ToNativeAddress(dlmd.pGCStaticDataStart + field->Offset);
			}
			else
			{
				DacHelpers::GlobalHelpers::Check(SOSDac->GetDomainLocalModuleDataFromModule(module->NativeHandle, &dlmd), "GetDomainLocalModuleDataFromModule");
			}
			if (Utils::CorElementTypeIsPrimitive(field->ElementType))
				return DacHelpers::GlobalHelpers::ToNativeAddress(dlmd.pNonGCStaticDataStart + field->Offset);
			else
				return DacHelpers::GlobalHelpers::ToNativeAddress(dlmd.pGCStaticDataStart + field->Offset);
		}

		bool RuntimeBuilder::GetFieldProps(Common::ClrType^ parentType, int token, String^% name, FieldAttributes% attributes) {
			IMetaDataImport* import = parentType->Module->MetadataImport;
			if (import == nullptr)
				throw gcnew InvalidOperationException("DAC returned no metadata import.");
			try {
				DWORD attr = 0;
				ULONG needed = 0;
				DacHelpers::GlobalHelpers::Check(import->GetFieldProps(token, nullptr, nullptr, 0, &needed, &attr, nullptr, nullptr, nullptr, nullptr, nullptr), "GetFieldProps");
				if (needed == 0 || needed > Int32::MaxValue)
					throw gcnew InvalidOperationException("Metadata returned an invalid field name length.");
				auto buffer = gcnew array<Char>(needed);
				pin_ptr<Char> ptr = &buffer[0];
				DacHelpers::GlobalHelpers::Check(import->GetFieldProps(token, nullptr, ptr, needed, &needed, nullptr, nullptr, nullptr, nullptr, nullptr, nullptr), "GetFieldProps");
				if (needed == 0 || needed > (unsigned int)buffer->Length)
					throw gcnew InvalidOperationException("Field name changed while reading.");
				name = gcnew String(ptr, 0, (int)needed - 1);
				attributes = static_cast<FieldAttributes>(attr);
				return true;
			}
			finally {
				import->Release();
			}
		}

		Generic::IEnumerable<Common::ClrField^>^ RuntimeBuilder::EnumerateFields(Common::ClrType^ type) {
			DacpMethodTableFieldData info;
			DacHelpers::GlobalHelpers::Check(SOSDac->GetMethodTableFieldData(type->NativeHandle, &info), "GetMethodTableFieldData");
			List<Common::ClrField^>^ fields = gcnew List<Common::ClrField^>();
			int inheritedCount = 0;
			if (type->BaseType != nullptr) {
				DacpMethodTableFieldData parentInfo;
				DacHelpers::GlobalHelpers::Check(SOSDac->GetMethodTableFieldData(type->BaseType->NativeHandle, &parentInfo), "GetMethodTableFieldData(parent)");
				inheritedCount = parentInfo.wNumInstanceFields;
			}
			if (info.wNumInstanceFields < inheritedCount)
				throw gcnew InvalidOperationException("DAC returned inconsistent field counts.");
			int fieldCount = info.wNumInstanceFields - inheritedCount + info.wNumStaticFields;
			auto field = info.FirstField;
			// NextField is pointer arithmetic, not a null-terminated linked list.
			for (int i = 0; i < fieldCount; i++)
			{
				if (field == 0)
					throw gcnew InvalidOperationException("DAC returned no field descriptor before the end of the field list.");
				DacpFieldDescData data;
				DacHelpers::GlobalHelpers::Check(SOSDac->GetFieldDescData(field, &data), "GetFieldDescData");
				if (data.bIsStatic != 0)
					fields->Add(gcnew Common::ClrStaticField(type, this, DacHelpers::GlobalHelpers::ToNativeAddress(field)));
				else
					fields->Add(gcnew Common::ClrInstanceField(type, this, DacHelpers::GlobalHelpers::ToNativeAddress(field)));
				field = data.NextField;
			}
			return fields;
		}
		Generic::IEnumerable<Common::ClrMethod^>^ RuntimeBuilder::EnumerateVTableMethods(Common::ClrType^ type) {
			auto mt = type->NativeHandle;
			DacpMethodTableData mtData;
			DacHelpers::GlobalHelpers::Check(SOSDac->GetMethodTableData(mt, &mtData), "GetMethodTableData");
			List<Common::ClrMethod^>^ methods = gcnew List<Common::ClrMethod^>();
			for (int i = 0; i < mtData.wNumMethods; i++)
			{
				CLRDATA_ADDRESS slot = 0;
				DacpCodeHeaderData chdata;
				DacHelpers::GlobalHelpers::Check(SOSDac->GetMethodTableSlot(mt, i, &slot), "GetMethodTableSlot");
				DacHelpers::GlobalHelpers::Check(SOSDac->GetCodeHeaderData(slot, &chdata), "GetCodeHeaderData");
				if (chdata.MethodDescPtr == 0)
					throw gcnew InvalidOperationException("DAC returned no method descriptor for a method slot.");
				methods->Add(gcnew Common::ClrMethod(this, DacHelpers::GlobalHelpers::ToNativeAddress(chdata.MethodDescPtr)));
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
