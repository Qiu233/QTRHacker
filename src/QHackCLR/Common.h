#pragma once
#include "pre.h"
#include "defs.h"
#include "DacpStructs.h"

using namespace System;
using namespace System::Linq;
using namespace System::Reflection;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Runtime::CompilerServices;
using namespace QHackCLR::Builders;
using namespace QHackCLR::Builders::Helpers;

namespace QHackCLR {
	namespace Common {
		value class _MethodTable
		{
		public:
			unsigned __int32 Flags;
			unsigned __int32 BaseSize;
			unsigned __int16 Flags2;
			unsigned __int16 Token;
			unsigned __int16 NumVirtuals;
			unsigned __int16 NumInterfaces;
			UIntPtr ParentMethodTable;
			UIntPtr LoaderModule;
			UIntPtr WriteableData;

			UIntPtr EEClass; //nuint CanonMT;
			UIntPtr ElementTypeHnd; //nuint PerInstInfo;
			//nuint MultipurposeSlot1;
			UIntPtr InterfaceMap; //nuint MultipurposeSlot2;
		};
		public enum class ElementType
		{
			TYPE_END = 0x00,
			TYPE_VOID = 0x01,
			TYPE_BOOLEAN = 0x02,
			TYPE_CHAR = 0x03,
			TYPE_I1 = 0x04,
			TYPE_U1 = 0x05,
			TYPE_I2 = 0x06,
			TYPE_U2 = 0x07,
			TYPE_I4 = 0x08,
			TYPE_U4 = 0x09,
			TYPE_I8 = 0x0a,
			TYPE_U8 = 0x0b,
			TYPE_R4 = 0x0c,
			TYPE_R8 = 0x0d,
			TYPE_STRING = 0x0e,

			TYPE_PTR = 0x0f,
			TYPE_BYREF = 0x10,

			TYPE_VALUETYPE = 0x11,
			TYPE_CLASS = 0x12,
			TYPE_VAR = 0x13,
			TYPE_ARRAY = 0x14,
			TYPE_GENERICINST = 0x15,
			TYPE_TYPEDBYREF = 0x16,

			TYPE_I = 0x18,
			TYPE_U = 0x19,
			TYPE_FNPTR = 0x1b,
			TYPE_OBJECT = 0x1c,
			TYPE_SZARRAY = 0x1d,
			TYPE_MVAR = 0x1e,

			TYPE_CMOD_REQD = 0x1f,
			TYPE_CMOD_OPT = 0x20,

			TYPE_INTERNAL = 0x21,

			TYPE_MAX = 0x22,


			TYPE_MODIFIER = 0x40,
			TYPE_SENTINEL = 0x01 | TYPE_MODIFIER,
			TYPE_PINNED = 0x05 | TYPE_MODIFIER,

		};

		public interface class IClrHandled : IEquatable<IClrHandled^> {
			[NativeInteger]
			property System::UIntPtr ClrHandle {
				System::UIntPtr get();
			}
		};
		public interface class IHasMetaData {
			property __int32 MDToken {
				__int32 get();
			}
		};

		public ref class ClrEntity abstract : IClrHandled {
		private:
			UIntPtr m_ClrHandle;
		internal:
			virtual property CLRDATA_ADDRESS NativeHandle {
				[MethodImpl(MethodImplOptions::AggressiveInlining)]
				CLRDATA_ADDRESS get() sealed {
					return m_ClrHandle.ToUInt64();
				}
			}
		public:
			[NativeInteger]
			virtual property UIntPtr ClrHandle {
				[MethodImpl(MethodImplOptions::AggressiveInlining)]
				UIntPtr get() sealed {
					return m_ClrHandle;
				}
			}

			ClrEntity(nuint handle) {
				m_ClrHandle = handle;
			}

			String^ ToString() override {
				return String::Format("{0}(0x{1:X" + IntPtr::Size * 2 + "})", this->GetType()->Name, m_ClrHandle);
			}
			virtual bool Equals(IClrHandled^ a) sealed {
				if (Object::ReferenceEquals(a, nullptr))
					return false;
				return this->m_ClrHandle == a->ClrHandle;
			}
			bool Equals(Object^ obj) override {
				return Equals(dynamic_cast<IClrHandled^>(obj));
			}
			int GetHashCode() override {
				return m_ClrHandle.ToUInt32();
			}
			static bool operator ==(ClrEntity^ a, ClrEntity^ b) {
				if (Object::ReferenceEquals(a, nullptr))
					return Object::ReferenceEquals(b, nullptr);
				return a->Equals(b);
			}
			static bool operator !=(ClrEntity^ a, ClrEntity^ b) {
				return !(a == b);
			}
		};

		public ref class ClrRuntime {
		private:
			initonly DataTargets::ClrInfo^ m_ClrInfo;
			ClrAppDomain^ m_AppDomain;
			ClrHeap^ m_Heap;
			initonly IRuntimeHelper^ m_RuntimeHelper;
		public:
			ClrRuntime(DataTargets::ClrInfo^ clrInfo, IRuntimeHelper^ helper) {
				this->m_ClrInfo = clrInfo;
				this->m_RuntimeHelper = helper;
			}
			property DataTargets::ClrInfo^ ClrInfo {
				DataTargets::ClrInfo^ get() {
					return m_ClrInfo;
				}
			}
			[NativeInteger]
			property UIntPtr BaseAddresss {
				UIntPtr get();
			}
			property DataTargets::DataTarget^ DataTarget {
				DataTargets::DataTarget^ get();
			}
			property Dac::DacLibrary^ DacLibrary {
				Dac::DacLibrary^ get();
			}
			property ClrAppDomain^ AppDomain {
				ClrAppDomain^ get();
			}
			property ClrHeap^ Heap {
				ClrHeap^ get();
			}
			property ClrModule^ BaseClassLibrary {
				ClrModule^ get();
			}
			property IRuntimeHelper^ RuntimeHelper {
				IRuntimeHelper^ get();
			}

			void Flush();
		};
		public ref class ClrHeap {
		private:
			ClrRuntime^ m_Runtime;
			ClrType^ m_FreeType;
			ClrType^ m_ObjectType;
			ClrType^ m_StringType;
			ClrType^ m_ExceptionType;
		public:
			ClrHeap(ClrRuntime^ runtime, IHeapHelper^ helper);

			property ClrRuntime^ Runtime { ClrRuntime^ get() { return m_Runtime; } }

			property ClrType^ FreeType {
				ClrType^ get() { return m_FreeType; }
			}
			property ClrType^ ObjectType {
				ClrType^ get() { return m_ObjectType; }
			}

			property ClrType^ StringType {
				ClrType^ get() { return m_StringType; }
			}
			property ClrType^ ExceptionType {
				ClrType^ get() { return m_ExceptionType; }
			}
		};
		public ref class ClrAppDomain : public ClrEntity {
		private:
			initonly String^ m_Name;
			Generic::IReadOnlyList<ClrModule^>^ m_Modules;
		protected:
			initonly IAppDomainHelper^ AppDomainHelper;
		internal:
			initonly IXCLRDataAppDomain* DataAppDomain;
			initonly DacpAppDomainData* Data;
		public:
			property String^ Name {
				String^ get() { return m_Name; }
			}
			ClrAppDomain(IAppDomainHelper^ helper, nuint handle);
			~ClrAppDomain();

			property int ID {
				int get() {
					return (int)Data->dwId;
				}
			}
			property ClrRuntime^ Runtime {
				ClrRuntime^ get();
			}
			property Generic::IReadOnlyList<ClrModule^>^ Modules {
				Generic::IReadOnlyList<ClrModule^>^ get();
			}
		};
		public ref class ClrModule : public ClrEntity {
		private:
			initonly String^ m_Name;
			initonly String^ m_FileName;
			Generic::IReadOnlyList<ClrType^>^ m_DefinedTypes;
			Generic::IReadOnlyList<ClrType^>^ m_ReferencedTypes;
			Generic::IReadOnlyList<ClrType^>^ Traverse(ModuleMapType type);
		protected:
			IModuleHelper^ ModuleHelper;
		internal:
			initonly DacpModuleData* Data;
			property IMetaDataImport* MetadataImport {
				IMetaDataImport* get();
			}
		public:
			property String^ Name { String^ get() { return m_Name; } }
			property String^ FileName { String^ get() { return m_FileName; } }
			ClrModule(IModuleHelper^ helper, nuint handle);
			~ClrModule();

			ClrType^ GetTypeByName(String^ name);

			property Generic::IReadOnlyList<ClrType^>^ DefinedTypes {
				Generic::IReadOnlyList<ClrType^>^ get();
			}
			property Generic::IReadOnlyList<ClrType^>^ ReferencedTypes {
				Generic::IReadOnlyList<ClrType^>^ get();
			}
		};
		public ref class ClrType : public ClrEntity, IHasMetaData {
		private:
			initonly String^ m_Name;
			CorElementType m_ElementType;
			ClrModule^ m_Module;
			ClrType^ m_BaseType;
			ClrType^ m_ComponentType;
			Generic::IReadOnlyList<ClrField^>^ m_Fields;
			Generic::IReadOnlyList<ClrMethod^>^ m_Methods;
			CorElementType GetCorElementType();
		protected:
			initonly ITypeHelper^ TypeHelper;
		internal:
			DacpMethodTableData* Data;
			property IClrObjectHelper^ ClrObjectHelper {
				IClrObjectHelper^ get();
			}
		public:
			property String^ Name { String^ get() { return m_Name; } }
			ClrType(ITypeHelper^ helper, nuint handle);
			~ClrType();


			ClrInstanceField^ GetInstanceFieldByName(String^ name);
			ClrStaticField^ GetStaticFieldByName(String^ name);

			virtual property int MDToken {
				int get() sealed {
					return Data->cl;
				}
			}

			property unsigned int BaseSize {
				unsigned int get() {
					return Data->BaseSize;
				}
			}
			property unsigned int DataSize {
				unsigned int get() {
					return Data->BaseSize - 2 * sizeof(UIntPtr);
				}
			}

			property System::Reflection::TypeAttributes TypeAttributes {
				System::Reflection::TypeAttributes get() {
					return static_cast<System::Reflection::TypeAttributes>(Data->dwAttrClass);
				}
			}

			property ClrHeap^ Heap {
				ClrHeap^ get();
			}
			property CorElementType ElementType {
				CorElementType get() {
					if (m_ElementType == CorElementType::ELEMENT_TYPE_END)
						m_ElementType = GetCorElementType();
					return m_ElementType;
				}
			}
			property QHackCLR::Common::ElementType WrappedElementType {
				QHackCLR::Common::ElementType get() {
					return (QHackCLR::Common::ElementType)ElementType;
				}
			}

			property bool IsArray {
				bool get() {
					return ElementType == CorElementType::ELEMENT_TYPE_ARRAY || ElementType == CorElementType::ELEMENT_TYPE_SZARRAY;
				}
			}
			property bool IsValueType {
				bool get();
			}
			property bool IsPrimitive {
				bool get();
			}
			property bool IsObjectReference {
				bool get();
			}

			property bool IsShared {
				bool get() {
					return Data->bIsShared != 0;
				}
			}
			property bool IsDynamic {
				bool get() {
					return Data->bIsDynamic != 0;
				}
			}

			property unsigned int ComponentSize {
				unsigned int get() {
					return Data->ComponentSize;
				}
			}

			property int ContainsPointers {
				int get() {
					return Data->bContainsPointers;
				}
			}

			property ClrModule^ Module {
				ClrModule^ get();
			}
			property ClrType^ BaseType {
				ClrType^ get();
			}
			property ClrType^ ComponentType {
				ClrType^ get();
			}

			property int Rank {
				int get() {
					if (!IsArray)
						return 0;
					if (ElementType == CorElementType::ELEMENT_TYPE_SZARRAY)
						return 1;
					return (int)((BaseSize - sizeof(UIntPtr) * 3) / 8);
				}
			}


			property Generic::IReadOnlyList<ClrField^>^ Fields {
				Generic::IReadOnlyList<ClrField^>^ get();
			}
			property Generic::IReadOnlyList<ClrMethod^>^ MethodsInVTable {
				Generic::IReadOnlyList<ClrMethod^>^ get();
			}

			Generic::IEnumerable<ClrStaticField^>^ EnumerateStaticFields();
			Generic::IEnumerable<ClrInstanceField^>^ EnumerateInstanceFields();


#define CLR_TYPE_PROP(name, raw) property bool name { bool get() { return TypeAttributes.HasFlag(System::Reflection::TypeAttributes::raw); } }

			CLR_TYPE_PROP(IsNotPublic, NotPublic);
			CLR_TYPE_PROP(IsPublic, Public);
			CLR_TYPE_PROP(IsNestedPublic, NestedPublic);
			CLR_TYPE_PROP(IsNestedPrivate, NestedPrivate);
			CLR_TYPE_PROP(IsNestedFamily, NestedFamily);
			CLR_TYPE_PROP(IsNestedAssembly, NestedAssembly);
			CLR_TYPE_PROP(IsNestedFamANDAssem, NestedFamANDAssem);
			CLR_TYPE_PROP(IsNestedFamORAssem, NestedFamORAssem);

			CLR_TYPE_PROP(IsAbstract, Abstract);
			CLR_TYPE_PROP(IsSealed, Sealed);
			CLR_TYPE_PROP(IsSpecialName, SpecialName);

			CLR_TYPE_PROP(IsAutoLayout, AutoLayout);
			CLR_TYPE_PROP(IsSequentialLayout, SequentialLayout);
			CLR_TYPE_PROP(IsExplicitLayout, ExplicitLayout);

			CLR_TYPE_PROP(IsClass, Class);
			CLR_TYPE_PROP(IsInterface, Interface);

			CLR_TYPE_PROP(IsImport, Import);
			CLR_TYPE_PROP(IsSerializable, Serializable);
			CLR_TYPE_PROP(IsWindowsRuntime, WindowsRuntime);

			CLR_TYPE_PROP(IsAnsiClass, AnsiClass);
			CLR_TYPE_PROP(IsUnicodeClass, UnicodeClass);
			CLR_TYPE_PROP(IsAutoClass, AutoClass);

			CLR_TYPE_PROP(IsBeforeFieldInit, BeforeFieldInit);

			CLR_TYPE_PROP(IsRTSpecialName, RTSpecialName);
			CLR_TYPE_PROP(IsHasSecurity, HasSecurity);
#undef CLR_TYPE_PROP


			int GetLength(nuint objRef);
			int GetLength(nuint objRef, int dimension);
			int GetLowerBound(nuint objRef, int dimension);

			UIntPtr GetElementsBase(nuint objRef)
			{
				if (!IsArray)
					throw gcnew InvalidOperationException("Not an array");
				if (ElementType == CorElementType::ELEMENT_TYPE_SZARRAY)
					return UIntPtr(objRef + sizeof(UIntPtr) * 2);
				return UIntPtr(objRef + (sizeof(UIntPtr) * 2 + (8 * Rank)));
			}

			unsigned __int32 GetArrayElementOffset(nuint objRef, array<int>^ indices)
			{
				int rank = Rank;
				if (indices->Length != rank)
					throw gcnew ArgumentException("Rank does not match");
				if (ElementType == CorElementType::ELEMENT_TYPE_SZARRAY)
					return sizeof(UIntPtr) * 2 + (indices[0] * ComponentSize);
				int offset = 0;
				for (int i = 0; i < rank; i++)
				{
					int currentValueOffset = indices[i] - GetLowerBound(objRef, i);
					if (currentValueOffset >= GetLength(objRef, i))
						throw gcnew ArgumentOutOfRangeException();
					offset *= GetLength(objRef, i);
					offset += currentValueOffset;
				}
				return sizeof(UIntPtr) * 2 + (8 * rank) + (offset * ComponentSize);
			}

			UIntPtr GetArrayElementAddress(nuint objRef, array<int>^ indices) {
				return objRef + GetArrayElementOffset(objRef, indices);
			}
		};
		public ref class ClrMethod : public ClrEntity, IHasMetaData {
		private:
			initonly String^ m_Signature;
			initonly MethodAttributes m_Attributes;
			ClrType^ m_DeclaringType;
		protected:
			initonly IMethodHelper^ MethodHelper;
		internal:
			initonly DacpMethodDescData* Data;
		public:

			property String^ Signature {
				String^ get() {
					return m_Signature;
				}
			}
			property MethodAttributes Attributes {
				MethodAttributes get() {
					return m_Attributes;
				}
			}

			virtual property int MDToken {
				int get() sealed {
					return Data->MDToken;
				}
			}

			ClrMethod(IMethodHelper^ helper, nuint handle);
			~ClrMethod();

			property ClrType^ DeclaringType {
				ClrType^ get();
			}
			[NativeInteger]
			property UIntPtr NativeCode {
				UIntPtr get() {
					if (Data->NativeCodeAddr == -1)//this field is sign extended hence would cause exception on getting -1
						return UIntPtr(UIntPtr::Size == 4 ? (unsigned int)(-1) : (unsigned long long) - 1);
					return UIntPtr(Data->NativeCodeAddr);
				}
			}
			property String^ Name {
				String^ get() {
					String^ signature = Signature;
					if (signature == nullptr)
						return nullptr;
					int last = signature->LastIndexOf('(');
					if (last > 0) {
						int first = signature->LastIndexOf('.', last - 1);
						if (first != -1 && signature[first - 1] == '.')
							first--;
						return signature->Substring(first + 1, last - first - 1);
					}
					return "{error}";
				}
			}
		};
		public ref class ClrField abstract : public ClrEntity, IHasMetaData {
		private:
			initonly String^ m_Name;
			initonly ClrType^ m_DeclaringType;
			initonly ClrType^ m_Type;
			initonly FieldAttributes m_Attributes;
		protected:
			initonly IFieldHelper^ FieldHelper;
		internal:
			initonly DacpFieldDescData* Data;
		public:
			property String^ Name { String^ get() { return m_Name; } }
			property ClrType^ DeclaringType { ClrType^ get() { return m_DeclaringType; } }
			property ClrType^ Type { ClrType^ get() { return m_Type; } }
			property FieldAttributes Attributes { FieldAttributes get() { return m_Attributes; } }

			ClrField(ClrType^ decType, IFieldHelper^ helper, nuint handle);
			~ClrField();

			property bool IsStatic {
				bool get() {
					return Data->bIsStatic != 0;
				}
			}

			property unsigned int Offset {
				unsigned int get() {
					return Data->dwOffset;
				}
			}

			property CorElementType ElementType {
				CorElementType get() {
					return Data->Type;
				}
			}

			virtual property int MDToken {
				int get() sealed {
					return Data->mb;
				}
			}
		};

		public ref class ClrInstanceField : public ClrField {
		private:
			AddressableTypedEntity^ GetValue(nuint offsetBase);
		public:
			UIntPtr GetAddress(nuint offsetBase) {
				return offsetBase + Offset;
			}
			generic<typename T> where T : value class
				T GetRawValue(nuint offsetBase);
			ClrInstanceField(ClrType^ declType, IFieldHelper^ helper, nuint handle) : ClrField(declType, helper, handle) {
			}
			UIntPtr GetAddress(AddressableTypedEntity^ entity);
			generic<typename T> where T : value class
				T GetRawValue(AddressableTypedEntity^ entity);
			AddressableTypedEntity^ GetValue(AddressableTypedEntity^ entity);
		};
		public ref class ClrStaticField : public ClrField {
		public:
			ClrStaticField(ClrType^ declType, IFieldHelper^ helper, nuint handle) : ClrField(declType, helper, handle) {
			}

			UIntPtr GetAddress();

			generic<typename T> where T : value class
				T GetRawValue();
			AddressableTypedEntity^ GetValue();
		};

		public ref class AddressableTypedEntity abstract : IEquatable<AddressableTypedEntity^> {
		private:
			initonly UIntPtr m_Address;
			initonly IClrObjectHelper^ m_ObjectHelper;
		public:
			AddressableTypedEntity(IClrObjectHelper^ helper, nuint address) {
				m_ObjectHelper = helper;
				m_Address = address;
			}

			[NativeInteger]
			property UIntPtr Address {
				UIntPtr get() { return m_Address; }
			}
			property IClrObjectHelper^ ObjectHelper {
				IClrObjectHelper^ get() { return m_ObjectHelper; }
			}
			property DataTargets::DataAccess^ DataAccess {
				DataTargets::DataAccess^ get();
			}

			virtual property ClrType^ Type {
				ClrType^ get() abstract;
			}
			[NativeInteger]
			virtual property UIntPtr OffsetBase {
				UIntPtr get() abstract;
			}
			generic<typename T> where T : value class
				T Read(unsigned __int32 offset);
			generic<typename T> where T : value class
				void Write(unsigned __int32 offset, T value);

			virtual bool Equals(AddressableTypedEntity^ a) sealed {
				if (Object::ReferenceEquals(a, nullptr))
					return false;
				return this->Address == a->Address;
			}
			bool Equals(Object^ obj) override {
				return Equals(dynamic_cast<AddressableTypedEntity^>(obj));
			}
			int GetHashCode() override {
				return UIntPtr(m_Address).ToUInt32();
			}
			static bool operator ==(AddressableTypedEntity^ a, AddressableTypedEntity^ b) {
				if (Object::ReferenceEquals(a, nullptr))
					return Object::ReferenceEquals(b, nullptr);
				return a->Equals(b);
			}
			static bool operator !=(AddressableTypedEntity^ a, AddressableTypedEntity^ b) {
				return !(a == b);
			}
		};

		public ref class ClrObject : public AddressableTypedEntity {
		private:
			ClrType^ m_Type;
		internal:
			DacpObjectData* Data;
		public:
			virtual property ClrType^ Type {
				ClrType^ get() override {
					return m_Type;
				}
			}
			ClrObject(ClrType^ type, nuint address);
			~ClrObject() {
				delete Data;
			}

			property bool IsArray {
				bool get();
			}
			property bool IsBoxed {
				bool get();
			}

			property bool IsNullPtr {
				bool get() {
					return this->Address == UIntPtr::Zero;
				}
			}

			[NativeInteger]
			virtual property UIntPtr OffsetBase {
				UIntPtr get() override {
					return UIntPtr(Address + sizeof(UIntPtr));
				}
			}
			property unsigned __int64 Size {
				unsigned __int64 get() {
					return Data->Size;
				}
			}

			int GetLength() {
				return Type->GetLength(this->Address);
			}

			int GetLength(int dimension) {
				return Type->GetLength(this->Address, dimension);
			}

			int GetLowerBound(int dimension) {
				return Type->GetLowerBound(this->Address, dimension);
			}

			UIntPtr GetElementsBase() {
				return Type->GetElementsBase(this->Address);
			}

			unsigned __int32 GetArrayElementOffset(array<int>^ indices) {
				return Type->GetArrayElementOffset(this->Address, indices);
			}

			UIntPtr GetArrayElementAddress(array<int>^ indices) {
				return Type->GetArrayElementAddress(this->Address, indices);
			}

			AddressableTypedEntity^ GetArrayElement(array<int>^ indices);
			generic<typename T> where T : value class
				T ReadArrayElement(array<int>^ indices) {
				return Read<T>(GetArrayElementOffset(indices));
			}
			AddressableTypedEntity^ GetArrayElement(int index) {
				return GetArrayElement(gcnew array<int> { index });
			}
		};
		public ref class ClrValue : public AddressableTypedEntity {
		private:
			ClrType^ m_Type;
		public:
			virtual property ClrType^ Type {
				ClrType^ get() override {
					return m_Type;
				}
			}
			[NativeInteger]
			virtual property UIntPtr OffsetBase {
				UIntPtr get() override {
					return Address;
				}
			}
			ClrValue(ClrType^ type, nuint address);

			generic<typename T> where T : value class
				T GetValue() {
				if (Type->DataSize > sizeof(T))
					throw gcnew InvalidOperationException("Size exceeded.");
				return Read<T>(0);
			}

			array<byte>^ ReadBytes(int size);
			array<byte>^ ReadBytes();
		};
	}
}