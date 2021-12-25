using QHackCLR.Dac.Utils;
using System;

namespace QHackCLR.Dac.COM
{
	public unsafe struct IUnknownVTable
	{
		public delegate* unmanaged[Stdcall]<IntPtr, in Guid, out IntPtr, HRESULT> QueryInterface;
		public delegate* unmanaged[Stdcall]<IntPtr, int> AddRef;
		public delegate* unmanaged[Stdcall]<IntPtr, int> Release;
	}
}