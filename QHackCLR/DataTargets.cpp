#include "DataTargets.h"
#include "Builders.h"
#include "Dac.h"
#include <psapi.h>

using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Linq;

namespace QHackCLR {
	namespace DataTargets {
		DataTarget::DataTarget(int pid) {
			this->m_ProcessID = pid;
			m_Handle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
			DataAccess = gcnew DataTargets::DataAccess(UIntPtr(m_Handle));
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
				ClrFlavor flavor;
				OSPlatform platform;
				if (ClrInfoProvider::IsSupportedRuntime(fileName, flavor, platform)) {
					String^ dacPath = Path::Combine(Path::GetDirectoryName(fileName), ClrInfoProvider::GetDacFileName(flavor, platform));
					versionBuilder->Add(gcnew ClrInfo(this, flavor, UIntPtr(module), dacPath));
				}
			}
			delete nameBuilder;
			delete buffer;
			ClrVersions = ImmutableArray::ToImmutableArray(versionBuilder);
		}
		DataTarget::~DataTarget() {
			CloseHandle(m_Handle);
		}

		Common::ClrRuntime^ ClrInfo::CreateRuntime() {
			return (gcnew QHackCLR::Builders::RuntimeBuilder(this, gcnew Dac::DacLibrary(DataTarget, DacPath, RuntimeBase.ToUInt64())))->Runtime;
		}
	}
}