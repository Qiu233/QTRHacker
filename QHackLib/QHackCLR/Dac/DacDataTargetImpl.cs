using QHackCLR;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
using QHackCLR.DataTargets;
using QHackLib;

namespace QHackCLR.Dac.Interfaces
{
#pragma warning disable CA1822
	public sealed unsafe class DacDataTargetImpl : COMCallableIUnknown
	{
		private readonly Guid IID_IDacDataTarget = new("3E11CCEE-D08B-43e5-AF01-32717A64DA03");
		private readonly Guid IID_IMetadataLocator = new("aa8fa804-bc05-4642-b2c5-c353ed22fc63");
		private readonly Guid IID_ICLRRuntimeLocator = new("b760bf44-9377-4597-8be7-58083bdc5146");
		public CLRDATA_ADDRESS RuntimeBase { get; }
		public DataTarget DataTarget { get; }
		internal IntPtr IDacDataTarget { get; }
		public DacDataTargetImpl(DataTarget dataTarget, CLRDATA_ADDRESS runtimeBase)
		{
			DataTarget = dataTarget;
			RuntimeBase = runtimeBase;

			VTableBuilder builder = AddInterface(IID_IDacDataTarget);
			builder.AddMethod(new GetMachineTypeDelegate(GetMachineType));
			builder.AddMethod(new GetPointerSizeDelegate(GetPointerSize));
			builder.AddMethod(new GetImageBaseDelegate(GetImageBase));
			builder.AddMethod(new ReadVirtualDelegate(ReadVirtual));
			builder.AddMethod(new WriteVirtualDelegate(WriteVirtual));
			builder.AddMethod(new GetTLSValueDelegate(GetTLSValue));
			builder.AddMethod(new SetTLSValueDelegate(SetTLSValue));
			builder.AddMethod(new GetCurrentThreadIDDelegate(GetCurrentThreadID));
			builder.AddMethod(new GetThreadContextDelegate(GetThreadContext));
			builder.AddMethod(new SetThreadContextDelegate(SetThreadContext));
			builder.AddMethod(new RequestDelegate(Request));
			IDacDataTarget = builder.Complete();
		}

		public HRESULT GetMachineType(IntPtr self, out IMAGE_FILE_MACHINE machineType)
		{
			machineType = IntPtr.Size == 8 ? IMAGE_FILE_MACHINE.AMD64 : IMAGE_FILE_MACHINE.I386;
			return HRESULT.S_OK;
		}

		public HRESULT GetPointerSize(IntPtr self, out uint pointerSize)
		{
			pointerSize = (uint)IntPtr.Size;
			return HRESULT.S_OK;
		}

		public HRESULT GetImageBase(IntPtr self, [In, MarshalAs(UnmanagedType.LPWStr)] string imagePath, out CLRDATA_ADDRESS baseAddress)
		{
			imagePath = Path.GetFileNameWithoutExtension(imagePath);
			var modules = Process.GetProcessById(DataTarget.ProcessId).Modules;
			foreach (ProcessModule module in modules)
			{
				string moduleName = Path.GetFileNameWithoutExtension(module.FileName);
				if (imagePath.Equals(moduleName, StringComparison.CurrentCultureIgnoreCase))
				{
					baseAddress = (ulong)module.BaseAddress;
					return HRESULT.S_OK;
				}
			}
			baseAddress = 0u;
			return HRESULT.E_FAIL;
		}

		public HRESULT ReadVirtual(IntPtr self, CLRDATA_ADDRESS address, byte* buffer, uint bytesRequested, out uint bytesRead)
		{
			DataTarget.DataAccess.Read(address, buffer, bytesRequested);
			bytesRead = bytesRequested;
			return HRESULT.S_OK;
		}

		public HRESULT WriteVirtual(IntPtr self, CLRDATA_ADDRESS address, byte* buffer, uint bytesRequested, out uint bytesWritten)
		{
			bytesWritten = bytesRequested;
			return HRESULT.S_OK;
		}

		public HRESULT GetTLSValue(IntPtr self, uint threadID, uint index, out CLRDATA_ADDRESS value)
		{
			value = 0u;
			return HRESULT.E_FAIL;
		}

		public HRESULT SetTLSValue(IntPtr self, uint threadID, uint index, CLRDATA_ADDRESS value)
		{
			return HRESULT.E_FAIL;
		}

		public HRESULT GetCurrentThreadID(IntPtr self, out uint threadID)
		{
			threadID = 0;
			return HRESULT.E_FAIL;
		}

		public HRESULT GetThreadContext(IntPtr self, uint threadID, uint contextFlags, uint contextSize, byte* context)
		{
			bool amd64 = IntPtr.Size == 8;
			if (contextSize < 4 || (amd64 && contextSize < 0x34))
				return HRESULT.E_FAIL;

			byte* ptr = context;
			if (amd64)
			{
				AMD64Context* ctx = (AMD64Context*)ptr;
				ctx->ContextFlags = contextFlags;
			}
			else
			{
				uint* intPtr = (uint*)ptr;
				*intPtr = contextFlags;
			}

			nuint thread = NativeFunctions.OpenThread(NativeFunctions.ThreadAccess.THREAD_ALL_ACCESS, true, threadID);
			if (thread == unchecked((nuint)(-1)) || thread == 0)
			{
				NativeFunctions.CloseHandle(thread);
				return HRESULT.E_FAIL;
			}

			if (!NativeFunctions.GetThreadContext(thread, (nuint)ptr))
			{
				NativeFunctions.CloseHandle(thread);
				return HRESULT.E_FAIL;
			}
			NativeFunctions.CloseHandle(thread);
			return HRESULT.S_OK;
		}

		public HRESULT SetThreadContext(IntPtr self, uint threadID, uint contextSize, byte* context)
		{
			return HRESULT.E_NOTIMPL;
		}

		public HRESULT Request(IntPtr self, uint reqCode, uint inBufferSize, byte* inBuffer, uint outBufferSize, byte* outBuffer)
		{
			return HRESULT.E_NOTIMPL;
		}



		private delegate HRESULT GetMachineTypeDelegate(IntPtr self, out IMAGE_FILE_MACHINE machineType);
		private delegate HRESULT GetPointerSizeDelegate(IntPtr self, out uint pointerSize);
		private delegate HRESULT GetImageBaseDelegate(IntPtr self, [In, MarshalAs(UnmanagedType.LPWStr)] string imagePath, out CLRDATA_ADDRESS baseAddress);
		private delegate HRESULT ReadVirtualDelegate(IntPtr self, CLRDATA_ADDRESS address, byte* buffer, uint bytesRequested, out uint bytesRead);
		private delegate HRESULT WriteVirtualDelegate(IntPtr self, CLRDATA_ADDRESS address, byte* buffer, uint bytesRequested, out uint bytesWritten);
		private delegate HRESULT GetTLSValueDelegate(IntPtr self, uint threadID, uint index, out CLRDATA_ADDRESS value);
		private delegate HRESULT SetTLSValueDelegate(IntPtr self, uint threadID, uint index, CLRDATA_ADDRESS value);
		private delegate HRESULT GetCurrentThreadIDDelegate(IntPtr self, out uint threadID);
		private delegate HRESULT GetThreadContextDelegate(IntPtr self, uint threadID, uint contextFlags, uint contextSize, byte* context);
		private delegate HRESULT SetThreadContextDelegate(IntPtr self, uint threadID, uint contextSize, byte* context);
		private delegate HRESULT RequestDelegate(IntPtr self, uint reqCode, uint inBufferSize, byte* inBuffer, uint outBufferSize, byte* outBuffer);

	}
}
