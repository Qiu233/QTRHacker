using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("5c552ab6-fc09-4cb3-8e36-22fa03c798b8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataProcess2
	{
		HRESULT GetGcNotification([In, Out] ref GcEvtArgs gcEvtArgs);
		HRESULT SetGcNotification([In] GcEvtArgs gcEvtArgs);
	}
}
