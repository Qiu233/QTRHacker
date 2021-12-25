using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("2FA17588-43C2-46ab-9B51-C8F01E39C9AC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataAssembly
	{
		HRESULT StartEnumModules([Out] out nuint handle);
		HRESULT EnumModule([In, Out] ref nuint handle, [Out] out IXCLRDataModule mod);
		HRESULT EndEnumModules([In] nuint handle);
		HRESULT GetName([In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetFileName([In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataAssembly assembly);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT StartEnumAppDomains([Out] out nuint handle);
		HRESULT EnumAppDomain([In, Out] ref nuint handle, [Out] out IXCLRDataAppDomain appDomain);
		HRESULT EndEnumAppDomains([In] nuint handle);
		HRESULT GetDisplayName([In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
	}
}
