using QHackCLR.COM;
using QHackCLR.Common;
using QHackCLR.DataTargets;
using System.Runtime.InteropServices;

namespace QHackCLR.DAC;
internal unsafe class DacDataTargetImpl : COMCallableIUnknown
{
	public const ulong MAGIC_CALLBACK_CONSTANT = 0x43;
	private int CallbackContext = 0;
	private Action? Magic_Callback;

	private static readonly Guid IID_IDacDataTarget = new("3E11CCEE-D08B-43e5-AF01-32717A64DA03");

	private readonly DataTarget DataTarget;

	public IntPtr IDacDataTarget { get; }

	public DacDataTargetImpl(DataTarget dataTarget)
	{
		DataTarget = dataTarget;

		VTableBuilder builder = AddInterface(IID_IDacDataTarget, false);
		builder.AddMethod(new GetMachineTypeDelegate(GetMachineType));
		builder.AddMethod(new GetPointerSizeDelegate(GetPointerSize));
		builder.AddMethod(new GetImageBaseDelegate(GetImageBase));
		builder.AddMethod(new ReadVirtualDelegate(ReadVirtual));
		builder.AddMethod(new WriteVirtualDelegate(WriteVirtual));
		builder.AddMethod(new GetTLSValueDelegate(GetTLSValue));
		builder.AddMethod(new SetTLSValueDelegate(SetTLSValue));
		builder.AddMethod(new GetCurrentThreadIDDelegate(GetCurrentThreadID));
		builder.AddMethod(new GetThreadContextDelegate(GetThreadContext));
		builder.AddMethod(new RequestDelegate(Request));
		IDacDataTarget = builder.Complete();
	}

	public void EnterMagicCallbackContext() => Interlocked.Increment(ref CallbackContext);

	public void ExitMagicCallbackContext() => Interlocked.Decrement(ref CallbackContext);

	public void SetMagicCallback(Action flushCallback) => Magic_Callback = flushCallback;

	public HRESULT GetMachineType(IntPtr self, out IMAGE_FILE_MACHINE machineType)
	{
		if (sizeof(nuint) == 8)
			machineType = IMAGE_FILE_MACHINE.AMD64;
		else
			machineType = IMAGE_FILE_MACHINE.I386;
		return HRESULT.S_OK;
	}

	public HRESULT GetPointerSize(IntPtr self, out int pointerSize)
	{
		pointerSize = sizeof(nuint);
		return HRESULT.S_OK;
	}

	public HRESULT GetImageBase(IntPtr self, string imagePath, out ulong baseAddress)
	{
		imagePath = Path.GetFileNameWithoutExtension(imagePath);
		var modules = System.Diagnostics.Process.GetProcessById(DataTarget.Pid).Modules;
		foreach (System.Diagnostics.ProcessModule module in modules)
		{
			string moduleName = Path.GetFileNameWithoutExtension(module.FileName);
			if (imagePath.Equals(moduleName, StringComparison.CurrentCultureIgnoreCase))
			{
				baseAddress = (ulong)module.BaseAddress.ToInt64();
				return HRESULT.S_OK;
			}
		}
		baseAddress = 0;
		return HRESULT.E_FAIL;
	}

	public HRESULT ReadVirtual(IntPtr _, CLRDATA_ADDRESS address, IntPtr buffer, int bytesRequested, out int bytesRead)
	{
		if (address == MAGIC_CALLBACK_CONSTANT && CallbackContext != 0)
		{
			if (this.Magic_Callback is not null)
				Magic_Callback.Invoke();
			bytesRead = 0;
			return HRESULT.E_FAIL;
		}
		this.DataTarget.DataAccess.Read((nuint)address, (byte*)buffer, (int)bytesRequested, out var _);
		bytesRead = bytesRequested;
		return HRESULT.S_OK;
	}

	public HRESULT WriteVirtual(IntPtr self, CLRDATA_ADDRESS address, IntPtr buffer, uint bytesRequested, out uint bytesWritten)
	{
		bytesWritten = bytesRequested;
		return HRESULT.S_OK;
	}

	public HRESULT GetTLSValue(IntPtr self, uint threadID, uint index, out ulong value)
	{
		value = 0;
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

	public HRESULT GetThreadContext(IntPtr self, uint threadID, uint contextFlags, int contextSize, IntPtr context)
	{
		bool amd64 = sizeof(nuint) == 8;
		if (contextSize < 4 || (amd64 && contextSize < 0x34))
			return HRESULT.E_FAIL;
		byte* ptr = (byte*)context;
		if (amd64)
		{
			NativeMethods.CONTEXT_AMD64* ctx = (NativeMethods.CONTEXT_AMD64*)ptr;
			ctx->ContextFlags = contextFlags;
		}
		else
		{
			uint* intPtr = (uint*)ptr;
			*intPtr = contextFlags;
		}

		nuint thread = NativeMethods.OpenThread(((0x000F0000) | (0x00100000) | 0xFFFF), true, threadID);
		if (thread == 0)
			return HRESULT.E_FAIL;
		if (thread == unchecked((nuint)(-1)))
		{
			NativeMethods.CloseHandle(thread);
			return HRESULT.E_FAIL;
		}
		if (!NativeMethods.GetThreadContext(thread, (NativeMethods.CONTEXT*)ptr))
		{
			NativeMethods.CloseHandle(thread);
			return HRESULT.E_FAIL;
		}
		NativeMethods.CloseHandle(thread);
		return HRESULT.S_OK;
	}

	public HRESULT Request(IntPtr self, uint reqCode, uint inBufferSize, IntPtr inBuffer, IntPtr outBufferSize, out IntPtr outBuffer)
	{
		outBuffer = IntPtr.Zero;
		return HRESULT.E_NOTIMPL;
	}

	private delegate HRESULT GetMetadataDelegate(IntPtr self, [In][MarshalAs(UnmanagedType.LPWStr)] string fileName, int imageTimestamp, int imageSize,
												 IntPtr mvid, uint mdRva, uint flags, uint bufferSize, IntPtr buffer, int* dataSize);
	private delegate HRESULT GetMachineTypeDelegate(IntPtr self, out IMAGE_FILE_MACHINE machineType);
	private delegate HRESULT GetPointerSizeDelegate(IntPtr self, out int pointerSize);
	private delegate HRESULT GetImageBaseDelegate(IntPtr self, [In][MarshalAs(UnmanagedType.LPWStr)] string imagePath, out ulong baseAddress);
	private delegate HRESULT ReadVirtualDelegate(IntPtr self, CLRDATA_ADDRESS address, IntPtr buffer, int bytesRequested, out int bytesRead);
	private delegate HRESULT WriteVirtualDelegate(IntPtr self, CLRDATA_ADDRESS address, IntPtr buffer, uint bytesRequested, out uint bytesWritten);
	private delegate HRESULT GetTLSValueDelegate(IntPtr self, uint threadID, uint index, out ulong value);
	private delegate HRESULT SetTLSValueDelegate(IntPtr self, uint threadID, uint index, CLRDATA_ADDRESS value);
	private delegate HRESULT GetCurrentThreadIDDelegate(IntPtr self, out uint threadID);
	private delegate HRESULT GetThreadContextDelegate(IntPtr self, uint threadID, uint contextFlags, int contextSize, IntPtr context);
	private delegate HRESULT SetThreadContextDelegate(IntPtr self, uint threadID, uint contextSize, IntPtr context);
	private delegate HRESULT RequestDelegate(IntPtr self, uint reqCode, uint inBufferSize, IntPtr inBuffer, IntPtr outBufferSize, out IntPtr outBuffer);

}