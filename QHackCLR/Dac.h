#pragma once
#include "pre.h"
#include "defs.h"

using namespace System;
using namespace System::IO;

namespace QHackCLR {
	namespace Dac {
		[uuid("3E11CCEE-D08B-43e5-AF01-32717A64DA03")]
		public class DacDataTargetImpl final : public ICLRDataTarget {
		private:
			gcroot<DataTargets::DataTarget^> m_DataTarget;
			ULONG m_ref;

		public:
			CLRDATA_ADDRESS RuntimeBase;

			DacDataTargetImpl(DataTargets::DataTarget^ target, CLRDATA_ADDRESS runtimeBase) {
				this->m_DataTarget = target;
				this->RuntimeBase = runtimeBase;
				m_ref = 1;
			}


			HRESULT STDMETHODCALLTYPE QueryInterface(
				REFIID riid,
				OUT PVOID* ppvObject) override {
				if (riid == __uuidof(DacDataTargetImpl))
					*ppvObject = static_cast<DacDataTargetImpl*>(this);
				else
					return E_NOINTERFACE;
				AddRef();
				return S_OK;
			}

			ULONG STDMETHODCALLTYPE AddRef(void) override {
				return InterlockedIncrement(&m_ref);
			}

			ULONG STDMETHODCALLTYPE Release(void) override {
				ULONG v = InterlockedDecrement(&m_ref);
				if (m_ref == 0)
					delete this;
				return v;
			}

			HRESULT STDMETHODCALLTYPE GetMachineType(
				/* [out] */ ULONG32* machineType) override {
				*machineType = System::IntPtr::Size == 8 ? IMAGE_FILE_MACHINE_AMD64 : IMAGE_FILE_MACHINE_I386;
				return S_OK;
			}

			HRESULT STDMETHODCALLTYPE GetPointerSize(
				/* [out] */ ULONG32* pointerSize) override {
				*pointerSize = System::IntPtr::Size;
				return S_OK;
			}

			HRESULT STDMETHODCALLTYPE GetImageBase(
				/* [string][in] */ LPCWSTR imagePath,
				/* [out] */ CLRDATA_ADDRESS* baseAddress) override;

			HRESULT STDMETHODCALLTYPE ReadVirtual(
				/* [in] */ CLRDATA_ADDRESS address,
				/* [length_is][size_is][out] */ BYTE* buffer,
				/* [in] */ ULONG32 bytesRequested,
				/* [out] */ ULONG32* bytesRead) override;

			HRESULT STDMETHODCALLTYPE WriteVirtual(
				/* [in] */ CLRDATA_ADDRESS address,
				/* [size_is][in] */ BYTE* buffer,
				/* [in] */ ULONG32 bytesRequested,
				/* [out] */ ULONG32* bytesWritten) override;

			HRESULT STDMETHODCALLTYPE GetTLSValue(
				/* [in] */ ULONG32 threadID,
				/* [in] */ ULONG32 index,
				/* [out] */ CLRDATA_ADDRESS* value) override {
				return E_FAIL;
			}

			HRESULT STDMETHODCALLTYPE SetTLSValue(
				/* [in] */ ULONG32 threadID,
				/* [in] */ ULONG32 index,
				/* [in] */ CLRDATA_ADDRESS value) override {
				return E_FAIL;
			}

			HRESULT STDMETHODCALLTYPE GetCurrentThreadID(
				/* [out] */ ULONG32* threadID) override {
				return E_FAIL;
			}

			HRESULT STDMETHODCALLTYPE GetThreadContext(
				/* [in] */ ULONG32 threadID,
				/* [in] */ ULONG32 contextFlags,
				/* [in] */ ULONG32 contextSize,
				/* [size_is][out] */ BYTE* context);

			HRESULT STDMETHODCALLTYPE SetThreadContext(
				/* [in] */ ULONG32 threadID,
				/* [in] */ ULONG32 contextSize,
				/* [size_is][in] */ BYTE* context) override {
				return E_NOTIMPL;
			}

			HRESULT STDMETHODCALLTYPE Request(
				/* [in] */ ULONG32 reqCode,
				/* [in] */ ULONG32 inBufferSize,
				/* [size_is][in] */ BYTE* inBuffer,
				/* [in] */ ULONG32 outBufferSize,
				/* [size_is][out] */ BYTE* outBuffer) override {
				return E_NOTIMPL;
			}
		};

		public ref class DacLibrary {
		private:
			ICLRDataTarget* DataTarget;
			IXCLRDataProcess* m_ClrDataProcess;
		public:
			DacLibrary(DataTargets::DataTarget^ dataTarget, System::String^ dacPath, System::UInt64 runtimeBase);
			~DacLibrary() {
				delete this->DataTarget;
			}

			property IXCLRDataProcess* ClrDataProcess {
				IXCLRDataProcess* get() {
					return m_ClrDataProcess;
				}
			}

			property ISOSDacInterface* SOSDac {
				ISOSDacInterface* get() {
					ISOSDacInterface* result;
					ClrDataProcess->QueryInterface<ISOSDacInterface>(&result);
					return result;
				}
			}
		};
	}
}

