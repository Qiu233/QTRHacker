#pragma once
#include "pre.h"
#include "defs.h"

using namespace System;
using namespace System::IO;
using namespace System::Collections::Immutable;
using namespace System::Runtime::CompilerServices;
using namespace System::Runtime::InteropServices;

namespace QHackCLR {
	namespace DataTargets {
		public enum class ClrFlavor
		{
			Desktop = 0,
			Core = 3
		};

		public ref class DataTarget : IDisposable {
		private:
			int m_ProcessID;
			HANDLE m_Handle;
		public:
			initonly DataAccess^ DataAccess;
			initonly ImmutableArray<ClrInfo^> ClrVersions;
			DataTarget(int pid);
			~DataTarget();
			property int ProcessID {
				int get() {
					return m_ProcessID;
				}
			}
		};



		public ref class ClrInfo
		{
		private:
			DataTargets::DataTarget^ m_DataTarget;
			ClrFlavor m_Flavor;
			UIntPtr m_RuntimeBase;
			String^ m_DacPath;
			String^ m_ClrModulePath;
		internal:
			ClrInfo(DataTargets::DataTarget^ dt, ClrFlavor flavor, [NativeInteger] UIntPtr moduleBase, String^ dacPath, String^ clrPath)
			{
				m_DataTarget = dt;
				m_Flavor = flavor;
				m_RuntimeBase = moduleBase;
				m_DacPath = dacPath;
				m_ClrModulePath = clrPath;
			}
		public:
			property DataTargets::DataTarget^ DataTarget {
				DataTargets::DataTarget^ get() { return m_DataTarget; }
			}
			[NativeInteger]
			property UIntPtr RuntimeBase {
				UIntPtr get() { return m_RuntimeBase; }
			}
			property System::String^ DacPath {
				System::String^ get() { return m_DacPath; }
			}
			property String^ ClrModulePath {
				String^ get() { return m_ClrModulePath; }
			}
			property ClrFlavor Flavor {
				ClrFlavor get() { return m_Flavor; }
			}
			Common::ClrRuntime^ CreateRuntime();
		};

		public ref class ClrInfoProvider abstract sealed {
		private:
			literal String^ c_desktopModuleName1 = "clr.dll";
			literal String^ c_desktopModuleName2 = "mscorwks.dll";
			literal String^ c_coreModuleName = "coreclr.dll";
			literal String^ c_linuxCoreModuleName = "libcoreclr.so";
			literal String^ c_macOSCoreModuleName = "libcoreclr.dylib";

			literal String^ c_desktopDacFileNameBase = "mscordacwks";
			literal String^ c_coreDacFileNameBase = "mscordaccore";
			literal String^ c_desktopDacFileName = "mscordacwks.dll";
			literal String^ c_coreDacFileName = "mscordaccore.dll";
			literal String^ c_linuxCoreDacFileName = "libmscordaccore.so";
			literal String^ c_macOSCoreDacFileName = "libmscordaccore.dylib";

		public:
			static bool IsSupportedRuntime(String^ moduleFile, ClrFlavor& flavor, OSPlatform& platform)
			{
				if (moduleFile == nullptr)
					throw gcnew ArgumentNullException();

				String^ moduleName = Path::GetFileName(moduleFile);
				if (moduleName == nullptr)
					return false;

				if (moduleName->Equals(c_desktopModuleName1, StringComparison::OrdinalIgnoreCase) ||
					moduleName->Equals(c_desktopModuleName2, StringComparison::OrdinalIgnoreCase))
				{
					flavor = ClrFlavor::Desktop;
					platform = OSPlatform::Windows;
					return true;
				}
				else if (moduleName->Equals(c_coreModuleName, StringComparison::OrdinalIgnoreCase))
				{
					flavor = ClrFlavor::Core;
					platform = OSPlatform::Windows;
					return true;
				}
				else if (moduleName->Equals(c_linuxCoreModuleName))
				{
					flavor = ClrFlavor::Core;
					platform = OSPlatform::Linux;
					return true;
				}
				else if (moduleName->Equals(c_macOSCoreModuleName))
				{
					flavor = ClrFlavor::Core;
					platform = OSPlatform::OSX;
					return true;
				}
				return false;
			}

			static String^ GetDacFileName(ClrFlavor flavor, OSPlatform platform)
			{
				if (platform == OSPlatform::Linux)
					return RuntimeInformation::IsOSPlatform(OSPlatform::Windows) ? c_coreDacFileName : c_linuxCoreDacFileName;
				if (platform == OSPlatform::OSX)
					return c_macOSCoreDacFileName;
				return flavor == ClrFlavor::Core ? c_coreDacFileName : c_desktopDacFileName;
			}
		};

		public ref class DataAccess {
		private:
			HANDLE m_ProcessHandle;
			static int ValidateBuffer(Array^ buffer, unsigned int length, unsigned int elementSize) {
				if (buffer == nullptr)
					throw gcnew ArgumentNullException("buffer");
				if (length > (unsigned int)buffer->Length || length > Int32::MaxValue / elementSize)
					throw gcnew ArgumentOutOfRangeException("length");
				return (int)(length * elementSize);
			}
			static void ThrowMemoryError(String^ operation, nuint addr, unsigned int length, int error) {
				throw gcnew IOException(String::Format("{0} failed at 0x{1:X} ({2} bytes, Win32 error {3}).",
					operation, addr.ToUInt64(), length, error), HRESULT_FROM_WIN32(error));
			}
			generic<typename T> where T : value class
				static void ValidateValueType() {
				if (RuntimeHelpers::IsReferenceOrContainsReferences<T>())
					throw gcnew ArgumentException("Memory access requires a value type without managed references.", "T");
				}
		internal:
			// DAC probes unreadable addresses as part of normal operation. Keep HRESULT failures
			// inside that callback; public memory access must report failures to its caller.
			bool TryRead(nuint addr, void* buffer, unsigned int length) {
				SIZE_T transferred = 0;
				bool success = ReadProcessMemory(m_ProcessHandle, addr.ToPointer(), buffer, length, &transferred);
				if (success && transferred != length) {
					SetLastError(ERROR_PARTIAL_COPY);
					return false;
				}
				return success;
			}
		public:
			DataAccess(nuint handle) {
				m_ProcessHandle = handle.ToPointer();
			}
			property UIntPtr ProcessHandle {
				UIntPtr get() {
					return UIntPtr(m_ProcessHandle);
				}
			}
			[MethodImpl(MethodImplOptions::AggressiveInlining)]
			bool Read(nuint addr, void* buffer, __int32 length) {
				if (length < 0)
					throw gcnew ArgumentOutOfRangeException("length");
				if (length == 0)
					return true;
				if (buffer == nullptr)
					throw gcnew ArgumentNullException("buffer");
				if (!TryRead(addr, buffer, length))
					ThrowMemoryError("ReadProcessMemory", addr, length, GetLastError());
				return true;
			}
			[MethodImpl(MethodImplOptions::AggressiveInlining)]
			bool Write(nuint addr, void* buffer, unsigned __int32 length) {
				if (length == 0)
					return true;
				if (buffer == nullptr)
					throw gcnew ArgumentNullException("buffer");
				SIZE_T transferred = 0;
				if (!WriteProcessMemory(m_ProcessHandle, addr.ToPointer(), buffer, length, &transferred))
					ThrowMemoryError("WriteProcessMemory", addr, length, GetLastError());
				if (transferred != length)
					ThrowMemoryError("WriteProcessMemory", addr, length, ERROR_PARTIAL_COPY);
				return true;
			}

			generic<typename T> where T : value class[MethodImpl(MethodImplOptions::AggressiveInlining)]
				bool Read(nuint addr, void* pData) {
				ValidateValueType<T>();
				return Read(addr, pData, sizeof(T));
			}
			generic<typename T> where T : value class[MethodImpl(MethodImplOptions::AggressiveInlining)]
				bool Write(nuint addr, void* pValue) {
				ValidateValueType<T>();
				return Write(addr, pValue, sizeof(T));
			}

		private:
			generic<typename T> where T : value class
				bool ReadGeneric1(nuint addr, [Out] T% value) {
				ValidateValueType<T>();
				pin_ptr<T> ptr = &value;
				return Read<T>(addr, ptr);
			}
			generic<typename T> where T : value class
				T ReadGeneric2(nuint addr) {
				T value;
				ReadGeneric1<T>(addr, value);
				return value;
			}
			generic<typename T> where T : value class
				bool WriteGeneric(nuint addr, T value) {
				ValidateValueType<T>();
				pin_ptr<T> ptr = &value;
				return Write(addr, ptr, sizeof(T));
			}
		public:

			generic<typename T> where T : value class
				bool Read(nuint addr, [Out] T% value) {
				return ReadGeneric1<T>(addr, value);
			}
			generic<typename T> where T : value class
				T Read(nuint addr) {
				return ReadGeneric2<T>(addr);
			}
			generic<typename T> where T : value class
				bool Write(nuint addr, T value) {
				return WriteGeneric(addr, value);
			}

			[MethodImpl(MethodImplOptions::AggressiveInlining)]
			bool Read(nuint addr, array<byte>^ buffer, unsigned __int32 length)
			{
				ValidateBuffer(buffer, length, 1);
				if (length == 0)
					return true;
				pin_ptr<byte> ptr = &buffer[0];
				return Read(addr, ptr, length);
			}

			[MethodImpl(MethodImplOptions::AggressiveInlining)]
			bool Write(nuint addr, array<byte>^ buffer, unsigned __int32 length)
			{
				ValidateBuffer(buffer, length, 1);
				if (length == 0)
					return true;
				pin_ptr<byte> ptr = &buffer[0];
				return Write(addr, ptr, length);
			}

			generic<typename T> where T : value class
				[MethodImpl(MethodImplOptions::AggressiveInlining)]
			bool Read(nuint addr, array<T>^ buffer, unsigned __int32 length)
			{
				ValidateValueType<T>();
				int byteCount = ValidateBuffer(buffer, length, sizeof(T));
				if (length == 0)
					return true;
				pin_ptr<T> ptr = &buffer[0];
				return Read(addr, ptr, byteCount);
			}

			generic<typename T> where T : value class
				[MethodImpl(MethodImplOptions::AggressiveInlining)]
			bool Write(nuint addr, array<T>^ buffer, unsigned __int32 length)
			{
				ValidateValueType<T>();
				int byteCount = ValidateBuffer(buffer, length, sizeof(T));
				if (length == 0)
					return true;
				pin_ptr<T> ptr = &buffer[0];
				return Write(addr, ptr, byteCount);
			}

			array<byte>^ ReadBytes(nuint addr, unsigned __int32 length) {
				if (length > Int32::MaxValue)
					throw gcnew ArgumentOutOfRangeException("length");
				array<byte>^ bs = gcnew array<byte>(length);
				if (length == 0)
					return bs;
				pin_ptr<byte> ptr = &bs[0];
				Read(addr, ptr, length);
				return bs;
			}
			void WriteBytes(nuint addr, array<byte>^ data) {
				if (data == nullptr)
					throw gcnew ArgumentNullException("data");
				if (data->Length == 0)
					return;
				pin_ptr<byte> ptr = &data[0];
				Write(addr, ptr, data->Length);
			}

			static int GetTypeSize(Type^ t)
			{
				using namespace System::Reflection::Emit;
				auto method = gcnew DynamicMethod("", Int32::typeid, Type::EmptyTypes);
				ILGenerator^ generator = method->GetILGenerator();
				generator->Emit(OpCodes::Sizeof, t);
				generator->Emit(OpCodes::Ret);
				auto func = static_cast<Func<int>^>(method->CreateDelegate(Func<int>::typeid));
				return func();
			}

			[MethodImpl(MethodImplOptions::AggressiveInlining)]
			Object^ Read(Type^ type, nuint addr)
			{
				using namespace System::Reflection;
				if (type == nullptr)
					throw gcnew ArgumentNullException("type");
				if (!type->IsValueType)
					throw gcnew ArgumentException("Not a ValueType");
				auto method = GetType()->GetMethod("ReadGeneric2", BindingFlags::Instance | BindingFlags::NonPublic)->MakeGenericMethod(type);
				return method->Invoke(this, gcnew array<Object^> { addr });
			}

			[MethodImpl(MethodImplOptions::AggressiveInlining)]
			void Write(nuint addr, Object^ value)
			{
				using namespace System::Reflection;
				if (value == nullptr)
					throw gcnew ArgumentNullException("value");
				Type^ type = value->GetType();
				if (!type->IsValueType)
					throw gcnew ArgumentException("Not a ValueType");
				auto method = GetType()->GetMethod("WriteGeneric", BindingFlags::Instance | BindingFlags::NonPublic)->MakeGenericMethod(type);
				method->Invoke(this, gcnew array<Object^> { addr, value });
			}
		};
	}
}

