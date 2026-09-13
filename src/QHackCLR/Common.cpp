#include "Common.h"
#include "DataTargets.h"
#include "Builders.h"
#include "Dac.h"
#include "DacHelpers.h"
#include "Utils.h"
#include <msclr/lock.h>
using namespace QHackCLR;
using QHackCLR::DacHelpers::GlobalHelpers;

namespace QHackCLR {
	namespace Common {
		UIntPtr ClrRuntime::BaseAddresss::get() {
			return this->ClrInfo->RuntimeBase;
		}
		DataTargets::DataTarget^ ClrRuntime::DataTarget::get() {
			return this->ClrInfo->DataTarget;
		}
		Dac::DacLibrary^ ClrRuntime::DacLibrary::get() {
			return this->RuntimeHelper->DacLibrary;
		}
		ClrAppDomain^ ClrRuntime::AppDomain::get() {
			if (m_AppDomain == nullptr) {
				auto domains = DacHelpers::SOSHelpers::GetAppDomainList(this->DacLibrary->SOSDac);
				if (domains->Length == 0)
					throw gcnew InvalidOperationException("The target CLR has no application domain.");
				m_AppDomain = this->RuntimeHelper->GetAppDomain(UIntPtr(domains[0]));
			}
			return m_AppDomain;
		}
		ClrHeap^ ClrRuntime::Heap::get() {
			if (m_Heap == nullptr) {
				m_Heap = gcnew ClrHeap(this, this->RuntimeHelper->HeapHelper);
			}
			return m_Heap;
		}
		ClrModule^ ClrRuntime::BaseClassLibrary::get() {
			return Heap->ObjectType->Module;
		}
		IRuntimeHelper^ ClrRuntime::RuntimeHelper::get() {
			return this->m_RuntimeHelper;
		}
		void ClrRuntime::Flush()
		{
			m_AppDomain = nullptr;
			m_Heap = nullptr;
			m_RuntimeHelper->Flush();
		}


		ClrHeap::ClrHeap(ClrRuntime^ runtime, IHeapHelper^ helper) {
			this->m_Runtime = runtime;
			DacpUsefulGlobalsData tables;
			GlobalHelpers::Check(helper->SOSDac->GetUsefulGlobals(&tables), "GetUsefulGlobals");

			m_FreeType = helper->TypeFactory->GetClrType(UIntPtr(tables.FreeMethodTable));
			m_ObjectType = helper->TypeFactory->GetClrType(UIntPtr(tables.ObjectMethodTable));

			m_StringType = helper->TypeFactory->GetClrType(UIntPtr(tables.StringMethodTable));
			m_ExceptionType = helper->TypeFactory->GetClrType(UIntPtr(tables.ExceptionMethodTable));
		}

		ClrAppDomain::ClrAppDomain(IAppDomainHelper^ helper, nuint handle) : ClrEntity(handle) {
			AppDomainHelper = helper;
			m_Name = DacHelpers::SOSHelpers::GetAppDomainName(helper->SOSDac, NativeHandle);

			DacHelpers::NativeData<DacpAppDomainData> data;
			GlobalHelpers::Check(helper->SOSDac->GetAppDomainData(NativeHandle, data.get()), "GetAppDomainData");

			IXCLRDataAppDomain* dataAppDomain = nullptr;
			GlobalHelpers::Check(helper->DacLibrary->ClrDataProcess->GetAppDomainByUniqueID(data.get()->dwId, &dataAppDomain), "GetAppDomainByUniqueID");
			if (dataAppDomain == nullptr)
				throw gcnew InvalidOperationException("DAC returned no application domain.");
			DataAppDomain = dataAppDomain;
			Data = data.release();
		}
		ClrAppDomain::~ClrAppDomain() {
			delete Data;
		}

		ClrRuntime^ ClrAppDomain::Runtime::get() {
			return AppDomainHelper->Runtime;
		}
		Generic::IReadOnlyList<ClrModule^>^ ClrAppDomain::Modules::get() {
			if (m_Modules == nullptr)
				m_Modules = Linq::Enumerable::ToList(AppDomainHelper->EnumerateModules(this));
			return m_Modules;
		}

		ClrModule::ClrModule(IModuleHelper^ helper, nuint handle) : ClrEntity(handle) {
			ModuleHelper = helper;
			DacHelpers::NativeData<DacpModuleData> data;
			GlobalHelpers::Check(helper->SOSDac->GetModuleData(NativeHandle, data.get()), "GetModuleData");
			IXCLRDataModule* dataModule = nullptr;
			GlobalHelpers::Check(helper->SOSDac->GetModule(NativeHandle, &dataModule), "GetModule");
			if (dataModule == nullptr)
				throw gcnew InvalidOperationException("DAC returned no module.");
			try {
				auto name = gcnew array<Char>(32768);
				pin_ptr<Char> ptr = &name[0];
				ULONG32 len = 0;
				GlobalHelpers::Check(dataModule->GetName(name->Length, &len, ptr), "GetModuleName");
				if (len > (unsigned int)name->Length)
					throw gcnew InvalidOperationException("DAC module name exceeds the buffer.");
				m_Name = gcnew String(ptr);
				name[0] = 0;
				auto result = dataModule->GetFileName(name->Length, &len, ptr);
				// Reflection modules can have no file name; the module itself was validated above.
				if (result == E_FAIL && data.get()->bIsReflection)
					m_FileName = String::Empty;
				else {
					GlobalHelpers::Check(result, "GetModuleFileName");
					if (len > (unsigned int)name->Length)
						throw gcnew InvalidOperationException("DAC module file name exceeds the buffer.");
					m_FileName = gcnew String(ptr);
				}
				Data = data.release();
			}
			finally {
				dataModule->Release();
			}
		}
		ClrModule::~ClrModule() {
			delete Data;
		}

		IMetaDataImport* ClrModule::MetadataImport::get() {
			return ModuleHelper->GetMetadataImport(this);
		}
		delegate void AddType_Del(
			UINT index,
			CLRDATA_ADDRESS methodTable,
			LPVOID token);
		namespace {
			ref class Anonymous_1 {
			public:
				Generic::List<CLRDATA_ADDRESS>^ Types;
				void Add(
					UINT index,
					CLRDATA_ADDRESS methodTable,
					LPVOID token) {

					Types->Add(methodTable);
				}
			};
		}

		Generic::IReadOnlyList<ClrType^>^ ClrModule::Traverse(ModuleMapType type) {
			auto holder = gcnew Generic::List<CLRDATA_ADDRESS>();
			Anonymous_1^ ano = gcnew Anonymous_1();
			ano->Types = holder;
			AddType_Del^ del = gcnew AddType_Del(ano, &Anonymous_1::Add);
			MODULEMAPTRAVERSE func = static_cast<MODULEMAPTRAVERSE>(Marshal::GetFunctionPointerForDelegate(del).ToPointer());
			GlobalHelpers::Check(this->ModuleHelper->SOSDac->TraverseModuleMap(type, NativeHandle, func, nullptr), "TraverseModuleMap");
			GC::KeepAlive(del);
			Generic::List<ClrType^>^ types = gcnew Generic::List<ClrType^>();
			for each (auto mt in holder)
				types->Add(this->ModuleHelper->TypeFactory->GetClrType(UIntPtr(mt)));
			return types;
		}

		Generic::IReadOnlyList<ClrType^>^ ClrModule::DefinedTypes::get() {
			if (m_DefinedTypes == nullptr) {
				m_DefinedTypes = Traverse(ModuleMapType::TYPEDEFTOMETHODTABLE);
			}
			return m_DefinedTypes;
		}
		Generic::IReadOnlyList<ClrType^>^ ClrModule::ReferencedTypes::get() {
			if (m_ReferencedTypes == nullptr) {
				m_ReferencedTypes = Traverse(ModuleMapType::TYPEREFTOMETHODTABLE);
			}
			return m_ReferencedTypes;
		}

		ClrType^ ClrModule::GetTypeByName(String^ name) {
			for each (ClrType ^ type in DefinedTypes) {
				if (type->Name == name)
					return type;
			}
			return nullptr;
		}

		int ClrType::GetLength(nuint objRef) {
			// GameString also uses this length slot through HackObject.GetArrayLength.
			if (!IsArray && ElementType != CorElementType::ELEMENT_TYPE_STRING)
				throw gcnew InvalidOperationException("Not an array or a string.");
			if (objRef == UIntPtr::Zero)
				throw gcnew ArgumentNullException("objRef");
			return TypeHelper->DataAccess->Read<int>(objRef + sizeof(UIntPtr));
		}

		int ClrType::GetLength(nuint objRef, int dimension) {
			int rank = Rank;
			if (dimension < 0 || dimension >= rank)
				throw gcnew ArgumentOutOfRangeException("dimension");
			if (objRef == UIntPtr::Zero)
				throw gcnew ArgumentNullException("objRef");
			if (ElementType == CorElementType::ELEMENT_TYPE_SZARRAY)//SZArray
				return GetLength(objRef);
			return TypeHelper->DataAccess->Read<int>(objRef + (sizeof(UIntPtr) * 2 + 4 * dimension));
		}

		int ClrType::GetLowerBound(nuint objRef, int dimension) {
			int rank = Rank;
			if (dimension < 0 || dimension >= rank)
				throw gcnew ArgumentOutOfRangeException("dimension");
			if (objRef == UIntPtr::Zero)
				throw gcnew ArgumentNullException("objRef");
			if (ElementType == CorElementType::ELEMENT_TYPE_SZARRAY)
				return 0;
			return TypeHelper->DataAccess->Read<int>(objRef + (sizeof(UIntPtr) * 2 + 4 * (rank + dimension)));
		}

		ClrType::ClrType(ITypeHelper^ helper, nuint handle) : ClrEntity(handle) {
			TypeHelper = helper;

			m_Name = DacHelpers::SOSHelpers::GetMethodTableName(helper->SOSDac, NativeHandle);

			DacHelpers::NativeData<DacpMethodTableData> data;
			GlobalHelpers::Check(helper->SOSDac->GetMethodTableData(NativeHandle, data.get()), "GetMethodTableData");
			Data = data.release();
		}
		ClrType::~ClrType() {
			delete Data;
		}

		ClrInstanceField^ ClrType::GetInstanceFieldByName(String^ name) {
			for each (auto field in EnumerateInstanceFields())
				if (field->Name == name)
					return field;
			return nullptr;
		}
		ClrStaticField^ ClrType::GetStaticFieldByName(String^ name) {
			for each (auto field in EnumerateStaticFields())
				if (field->Name == name)
					return field;
			return nullptr;
		}

		ClrHeap^ ClrType::Heap::get() {
			return TypeHelper->Heap;
		}

		Builders::Helpers::IClrObjectHelper^ ClrType::ClrObjectHelper::get() {
			return TypeHelper->ClrObjectHelper;
		}

		ClrModule^ ClrType::Module::get() {
			if (m_Module == nullptr) {
				m_Module = TypeHelper->GetModule(UIntPtr(Data->Module));
			}
			return m_Module;
		}
		ClrType^ ClrType::BaseType::get() {
			if (m_BaseType == nullptr) {
				m_BaseType = TypeHelper->TypeFactory->GetClrType(UIntPtr(Data->ParentMethodTable));
			}
			return m_BaseType;
		}
		ClrType^ ClrType::ComponentType::get() {
			if (m_ComponentType == nullptr) {
				_MethodTable table = TypeHelper->DataAccess->Read<_MethodTable>(ClrHandle);
				m_ComponentType = TypeHelper->TypeFactory->GetClrType(UIntPtr(table.ElementTypeHnd));
			}
			return m_ComponentType;
		}

		Generic::IReadOnlyList<ClrField^>^ ClrType::Fields::get() {
			if (m_Fields == nullptr) {
				m_Fields = Enumerable::ToList(TypeHelper->EnumerateFields(this));
			}
			return m_Fields;
		}
		Generic::IReadOnlyList<ClrMethod^>^ ClrType::MethodsInVTable::get() {
			if (m_Methods == nullptr) {
				m_Methods = Enumerable::ToList(TypeHelper->EnumerateVTableMethods(this));
			}
			return m_Methods;
		}

		Generic::IEnumerable<ClrStaticField^>^ ClrType::EnumerateStaticFields() {
			List<ClrStaticField^>^ staticFields = gcnew List<ClrStaticField^>();
			for each (auto field in Fields) {
				if (field->IsStatic)
					staticFields->Add(static_cast<ClrStaticField^>(field));
			}
			return staticFields;
		}
		Generic::IEnumerable<ClrInstanceField^>^ ClrType::EnumerateInstanceFields() {
			List<ClrInstanceField^>^ instanceFields = gcnew List<ClrInstanceField^>();
			for each (auto field in Fields) {
				if (!field->IsStatic)
					instanceFields->Add(static_cast<ClrInstanceField^>(field));
			}
			return instanceFields;
		}

		CorElementType ClrType::GetCorElementType()
		{
			if (this == Heap->ObjectType)
				return CorElementType::ELEMENT_TYPE_OBJECT;
			if (this == Heap->StringType)
				return CorElementType::ELEMENT_TYPE_STRING;
			if (ComponentSize != 0)
				return (BaseSize > (unsigned int)(3 * IntPtr::Size)) ? CorElementType::ELEMENT_TYPE_ARRAY : CorElementType::ELEMENT_TYPE_SZARRAY;
			ClrType^ baseType = BaseType;
			if (baseType == nullptr)
				return CorElementType::ELEMENT_TYPE_OBJECT;
			if (baseType == Heap->ObjectType)
				return CorElementType::ELEMENT_TYPE_CLASS;
			if (baseType->Name != "System.ValueType")
				return baseType->ElementType;
			return Utils::GetCorElementTypeFromName(Name);
		}

		bool ClrType::IsValueType::get() {
			return Utils::CorElementTypeIsValueType(ElementType);
		}
		bool ClrType::IsPrimitive::get() {
			return Utils::CorElementTypeIsPrimitive(ElementType);
		}
		bool ClrType::IsObjectReference::get() {
			return Utils::CorElementTypeIsObjectReference(ElementType);
		}

		ClrField::ClrField(ClrType^ decType, IFieldHelper^ helper, nuint handle) : ClrEntity(handle) {
			m_DeclaringType = decType;
			FieldHelper = helper;

			DacHelpers::NativeData<DacpFieldDescData> data;
			GlobalHelpers::Check(helper->SOSDac->GetFieldDescData(NativeHandle, data.get()), "GetFieldDescData");

			m_Type = helper->TypeFactory->GetClrType(UIntPtr(data.get()->MTOfType));

			String^ name;
			FieldAttributes attr;
			if (!helper->GetFieldProps(decType, data.get()->mb, name, attr))
				throw gcnew InvalidOperationException("Could not read field metadata.");
			m_Name = name;
			m_Attributes = attr;
			Data = data.release();
		}
		ClrField::~ClrField() {
			delete Data;
		}

		generic<typename T> where T : value class
			T ClrInstanceField::GetRawValue(nuint offsetBase) {
			return FieldHelper->DataAccess->Read<T>(GetAddress(offsetBase));
		}
		AddressableTypedEntity^ ClrInstanceField::GetValue(nuint offsetBase) {
			if (Type->IsValueType)
				return gcnew ClrValue(Type, GetAddress(offsetBase));
			return gcnew ClrObject(Type, GetRawValue<UIntPtr>(offsetBase));
		}
		UIntPtr ClrInstanceField::GetAddress(AddressableTypedEntity^ entity) {
			return entity->OffsetBase + Offset;
		}
		generic<typename T> where T : value class
			T ClrInstanceField::GetRawValue(AddressableTypedEntity^ entity) {
			return FieldHelper->DataAccess->Read<T>(GetAddress(entity));
		}
		AddressableTypedEntity^ ClrInstanceField::GetValue(AddressableTypedEntity^ entity) {
			if (Type->IsValueType)
				return gcnew ClrValue(Type, GetAddress(entity));
			return gcnew ClrObject(Type, GetRawValue<UIntPtr>(entity));
		}

		UIntPtr ClrStaticField::GetAddress() {
			return FieldHelper->GetStaticFieldAddress(this);
		}

		generic<typename T> where T : value class
			T ClrStaticField::GetRawValue() {
			return FieldHelper->DataAccess->Read<T>(GetAddress());
		}
		AddressableTypedEntity^ ClrStaticField::GetValue() {
			if (Type->IsPrimitive)
				return gcnew ClrValue(Type, GetAddress());
			return gcnew ClrObject(Type, GetRawValue<UIntPtr>());
		}


		ClrMethod::ClrMethod(IMethodHelper^ helper, nuint handle) : ClrEntity(handle) {
			MethodHelper = helper;
			DacHelpers::NativeData<DacpMethodDescData> data;
			GlobalHelpers::Check(helper->SOSDac->GetMethodDescData(NativeHandle, 0, data.get(), 0, nullptr, nullptr), "GetMethodDescData");

			m_Signature = DacHelpers::SOSHelpers::GetMethodDescName(helper->SOSDac, NativeHandle);
			Data = data.release();
		}
		ClrMethod::~ClrMethod() {
			delete Data;
		}

		ClrType^ ClrMethod::DeclaringType::get() {
			if (m_DeclaringType == nullptr)
				m_DeclaringType = MethodHelper->TypeFactory->GetClrType(UIntPtr(Data->MethodTablePtr));
			return m_DeclaringType;
		}


		generic<typename T> where T : value class
			T AddressableTypedEntity::Read(unsigned __int32 offset) {
			return DataAccess->Read<T>(Address + offset);
		}
		generic<typename T> where T : value class
			void AddressableTypedEntity::Write(unsigned __int32 offset, T value) {
			DataAccess->Write<T>(UIntPtr(m_Address + offset), value);
		}

		DataTargets::DataAccess^ AddressableTypedEntity::DataAccess::get() {
			return ObjectHelper->DataAccess;
		}


		ClrObject::ClrObject(ClrType^ type, nuint address) : AddressableTypedEntity(type->ClrObjectHelper, address) {
			m_Type = type;
			DacHelpers::NativeData<DacpObjectData> data;
			// A null object is a supported wrapper (IsNullPtr), not a failed DAC lookup.
			if (address != UIntPtr::Zero)
				GlobalHelpers::Check(type->ClrObjectHelper->SOSDac->GetObjectData(address.ToUInt64(), data.get()), "GetObjectData");
			Data = data.release();
		}

		bool ClrObject::IsArray::get() {
			return Type->IsArray;
		}
		bool ClrObject::IsBoxed::get() {
			return Type->IsValueType;
		}

		AddressableTypedEntity^ ClrObject::GetArrayElement(array<int>^ indices)
		{
			if (!Type->IsArray)
				throw gcnew InvalidOperationException("Not an array");
			unsigned int offset = GetArrayElementOffset(indices);
			ClrType^ componentType = Type->ComponentType;
			if (componentType->IsValueType)
				return gcnew ClrValue(componentType, Address + offset);
			return gcnew ClrObject(componentType, Read<UIntPtr>(offset));
		}

		ClrValue::ClrValue(ClrType^ type, nuint address) : AddressableTypedEntity(type->ClrObjectHelper, address) {
			m_Type = type;
		}
		array<byte>^ ClrValue::ReadBytes(int size) {
			return DataAccess->ReadBytes(Address, size);
		}
		array<byte>^ ClrValue::ReadBytes() {
			return DataAccess->ReadBytes(Address, Type->DataSize);
		}
	}
}
