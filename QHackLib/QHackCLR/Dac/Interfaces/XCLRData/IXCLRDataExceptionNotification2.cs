using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("31201a94-4337-49b7-aef7-0c755054091f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataExceptionNotification2
	{
		HRESULT OnAppDomainLoaded([In] IXCLRDataAppDomain domain);
		HRESULT OnAppDomainUnloaded([In] IXCLRDataAppDomain domain);
		HRESULT OnException([In] IXCLRDataExceptionState exception);
	}
}
