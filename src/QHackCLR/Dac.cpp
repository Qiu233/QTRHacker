#include "pre.h"
#include "Dac.h"
#include "DataTargets.h"
#include <clrdata.h>
#include <processthreadsapi.h>

using namespace System;
using namespace System::IO;
using namespace QHackCLR::DataTargets;

struct CUSTOM_CONTEXT_AMD64 {
	DWORD64 P1Home;
	DWORD64 P2Home;
	DWORD64 P3Home;
	DWORD64 P4Home;
	DWORD64 P5Home;
	DWORD64 P6Home;
	DWORD   ContextFlags;
	DWORD   MxCsr;
	WORD    SegCs;
	WORD    SegDs;
	WORD    SegEs;
	WORD    SegFs;
	WORD    SegGs;
	WORD    SegSs;
	DWORD   EFlags;
	DWORD64 Dr0;
	DWORD64 Dr1;
	DWORD64 Dr2;
	DWORD64 Dr3;
	DWORD64 Dr6;
	DWORD64 Dr7;
	DWORD64 Rax;
	DWORD64 Rcx;
	DWORD64 Rdx;
	DWORD64 Rbx;
	DWORD64 Rsp;
	DWORD64 Rbp;
	DWORD64 Rsi;
	DWORD64 Rdi;
	DWORD64 R8;
	DWORD64 R9;
	DWORD64 R10;
	DWORD64 R11;
	DWORD64 R12;
	DWORD64 R13;
	DWORD64 R14;
	DWORD64 R15;
	DWORD64 Rip;
	union {
		ULONGLONG       D[32];
		struct {
			M128A Header[2];
			M128A Legacy[8];
			M128A Xmm0;
			M128A Xmm1;
			M128A Xmm2;
			M128A Xmm3;
			M128A Xmm4;
			M128A Xmm5;
			M128A Xmm6;
			M128A Xmm7;
			M128A Xmm8;
			M128A Xmm9;
			M128A Xmm10;
			M128A Xmm11;
			M128A Xmm12;
			M128A Xmm13;
			M128A Xmm14;
			M128A Xmm15;
		} DUMMYSTRUCTNAME;
		DWORD           S[32];
	} DUMMYUNIONNAME;
	M128A   VectorRegister[26];
	DWORD64 VectorControl;
	DWORD64 DebugControl;
	DWORD64 LastBranchToRip;
	DWORD64 LastBranchFromRip;
	DWORD64 LastExceptionToRip;
	DWORD64 LastExceptionFromRip;
};

namespace QHackCLR {
	namespace Dac {
		DacLibrary::DacLibrary(DataTargets::DataTarget^ dataTarget, System::String^ dacPath, System::UInt64 runtimeBase) {
			pin_ptr<const wchar_t> _dacPath = PtrToStringChars(dacPath);
			HMODULE dac = LoadLibraryW(_dacPath);
			if (dac == nullptr)
				throw gcnew Exception("Failure loading DAC: LoadLibraryW failed when loading file: \"" + dacPath + "\"");
			FARPROC addr = GetProcAddress(dac, "CLRDataCreateInstance");
			if (addr == 0)
				throw gcnew Exception("Failed to obtain Dac CLRDataCreateInstance");
			this->DataTarget = new DacDataTargetImpl(dataTarget, runtimeBase);
			auto func = (HRESULT(__stdcall*)(REFIID, ICLRDataTarget*, IUnknown**))addr;
			IXCLRDataProcess* iUnk;
			HRESULT res = func(__uuidof(IXCLRDataProcess), DataTarget, (IUnknown**)&iUnk);
			if (res != S_OK)
				throw gcnew Exception("Failure loading DAC: CreateDacInstance failed 0x" + res.ToString("X8") + "");
			m_ClrDataProcess = iUnk;
		}

		HRESULT STDMETHODCALLTYPE DacDataTargetImpl::GetImageBase(
			/* [string][in] */ LPCWSTR imagePath,
			/* [out] */ CLRDATA_ADDRESS* baseAddress) {
			String^ path = gcnew String(imagePath);
			path = Path::GetFileNameWithoutExtension(path);
			auto modules = System::Diagnostics::Process::GetProcessById(m_DataTarget->ProcessID)->Modules;
			for each (System::Diagnostics::ProcessModule ^ module in modules) {
				String^ moduleName = Path::GetFileNameWithoutExtension(module->FileName);
				if (path->Equals(moduleName, StringComparison::CurrentCultureIgnoreCase)) {
					*baseAddress = module->BaseAddress.ToInt64();
					return S_OK;
				}
			}
			*baseAddress = 0;
			return E_FAIL;
		}
		HRESULT STDMETHODCALLTYPE DacDataTargetImpl::ReadVirtual(
			/* [in] */ CLRDATA_ADDRESS address,
			/* [length_is][size_is][out] */ BYTE* buffer,
			/* [in] */ ULONG32 bytesRequested,
			/* [out] */ ULONG32* bytesRead) {
			this->m_DataTarget->DataAccess->Read(UIntPtr(address), buffer, bytesRequested);
			*bytesRead = bytesRequested;
			return S_OK;
		}
		HRESULT STDMETHODCALLTYPE DacDataTargetImpl::WriteVirtual(
			/* [in] */ CLRDATA_ADDRESS address,
			/* [size_is][in] */ BYTE* buffer,
			/* [in] */ ULONG32 bytesRequested,
			/* [out] */ ULONG32* bytesWritten) {
			*bytesWritten = bytesRequested;
			return S_OK;
		}
		HRESULT STDMETHODCALLTYPE DacDataTargetImpl::GetThreadContext(
			/* [in] */ ULONG32 threadID,
			/* [in] */ ULONG32 contextFlags,
			/* [in] */ ULONG32 contextSize,
			/* [size_is][out] */ BYTE* context) {

			bool amd64 = IntPtr::Size == 8;
			if (contextSize < 4 || (amd64 && contextSize < 0x34))
				return E_FAIL;
			byte* ptr = context;
			if (amd64)
			{
				CUSTOM_CONTEXT_AMD64* ctx = (CUSTOM_CONTEXT_AMD64*)ptr;
				ctx->ContextFlags = contextFlags;
			}
			else
			{
				unsigned __int32* intPtr = (unsigned __int32*)ptr;
				*intPtr = contextFlags;
			}

			HANDLE thread = OpenThread(THREAD_ALL_ACCESS, true, threadID);
			if (thread == 0)
				return E_FAIL;
			if (thread == (HANDLE)(-1))
			{
				CloseHandle(thread);
				return E_FAIL;
			}
			if (!::GetThreadContext(thread, (LPCONTEXT)ptr))
			{
				CloseHandle(thread);
				return E_FAIL;
			}
			CloseHandle(thread);
			return S_OK;
		}
	}
}