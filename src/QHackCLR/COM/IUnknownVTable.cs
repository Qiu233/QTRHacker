using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QHackCLR.Common;

namespace QHackCLR.COM;

public unsafe struct IUnknownVTable
{
	public delegate* unmanaged[Stdcall]<IntPtr, in Guid, out IntPtr, HRESULT> QueryInterface;
	public delegate* unmanaged[Stdcall]<IntPtr, int> AddRef;
	public delegate* unmanaged[Stdcall]<IntPtr, int> Release;
}