using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("7CA04601-C702-4670-A63C-FA44F7DA7BD5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataAppDomain
	{
		HRESULT GetProcess([Out] out IXCLRDataProcess process);
		HRESULT GetName([In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetUniqueID([Out] out ulong id);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataAppDomain appDomain);
		HRESULT GetManagedObject([Out] out IXCLRDataValue value);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
	}
}
