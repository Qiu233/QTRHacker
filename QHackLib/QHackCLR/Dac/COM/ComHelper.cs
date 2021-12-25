using QHackCLR.Dac.Utils;
using System;
using System.Runtime.InteropServices;

namespace QHackCLR.Dac.COM
{
	public abstract unsafe class COMHelper
	{
		protected static readonly Guid IUnknownGuid = new("00000000-0000-0000-C000-000000000046");

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		protected delegate int AddRefDelegate(IntPtr self);
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		protected delegate int ReleaseDelegate(IntPtr self);
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		protected delegate HRESULT QueryInterfaceDelegate(IntPtr self, in Guid guid, out IntPtr ptr);

		public static unsafe int Release(IntPtr pUnk)
		{
			if (pUnk == IntPtr.Zero)
				return 0;
			IUnknownVTable* vtable = *(IUnknownVTable**)pUnk;
			return vtable->Release(pUnk);
		}

		public static unsafe HRESULT QueryInterface(IntPtr pUnk, in Guid riid, out IntPtr result)
		{
			result = IntPtr.Zero;
			if (pUnk == IntPtr.Zero)
				return HRESULT.E_INVALIDARG;
			IUnknownVTable* vtable = *(IUnknownVTable**)pUnk;
			return vtable->QueryInterface(pUnk, riid, out result);
		}
	}
}