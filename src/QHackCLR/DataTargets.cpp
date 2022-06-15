#include "DataTargets.h"
#include "Builders.h"
#include "Dac.h"
#include "Utils.h"
#include <psapi.h>

using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Linq;

namespace QHackCLR {
	namespace DataTargets {
		DataTarget::DataTarget(int pid) {
			this->m_ProcessID = pid;
			m_Handle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
			if (m_Handle == nullptr)
			{
				DWORD hr = GetLastError();
				throw gcnew QHackCLRException("Could not attach to process " + pid + ", error: " + hr.ToString("X"));
			}
			BOOL targetx64;
			IsWow64Process(m_Handle, &targetx64);
			if (targetx64 == (System::UIntPtr::Size == 8))
			{
				throw gcnew QHackCLRMismatchedArchitectureException("Mismatched architecture between this process and the target process.\n" +
					"This process is " + (System::UIntPtr::Size == 8 ? "x64" : "x86") + " while the target is " + (targetx64 ? "x64" : "x86") + ".");
			}

			DataAccess = gcnew DataTargets::DataAccess(UIntPtr(m_Handle));
			List<String^>^ modules = gcnew List<String^>();
			List<ClrInfo^>^ versionBuilder = gcnew List<ClrInfo^>();
			DWORD needed;
			EnumProcessModules(m_Handle, nullptr, 0, &needed);
			unsigned __int32 len = needed / sizeof(HMODULE);
			HMODULE* buffer = new HMODULE[len];
			EnumProcessModules(m_Handle, buffer, needed, &needed);
			wchar_t* nameBuilder = new wchar_t[2048];
			for (size_t i = 0; i < len; i++) {
				HMODULE module = buffer[i];
				GetModuleFileNameEx(m_Handle, module, nameBuilder, 2048);
				String^ fileName = gcnew String(nameBuilder);
				modules->Add(fileName);
				ClrFlavor flavor;
				OSPlatform platform;
				if (ClrInfoProvider::IsSupportedRuntime(fileName, flavor, platform)) {
					String^ dacPath = Path::Combine(Path::GetDirectoryName(fileName), ClrInfoProvider::GetDacFileName(flavor, platform));
					versionBuilder->Add(gcnew ClrInfo(this, flavor, UIntPtr(module), dacPath));
				}
			}
			delete[] nameBuilder;
			delete[] buffer;
			ClrVersions = ImmutableArray::ToImmutableArray(versionBuilder);
			if (ClrVersions.Length == 0)
			{
				throw gcnew QHackCLRException("Could not find any supported clr runtime.\n[" + String::Join(",", modules->ToArray()) + "]");
			}
		}
		DataTarget::~DataTarget() {
			CloseHandle(m_Handle);
		}

		Common::ClrRuntime^ ClrInfo::CreateRuntime() {
			return (gcnew QHackCLR::Builders::RuntimeBuilder(this, gcnew Dac::DacLibrary(DataTarget, DacPath, RuntimeBase.ToUInt64())))->Runtime;
		}
	}
}