using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("AAF60008-FB2C-420b-8FB1-42D244A54A97")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataMethodDefinition
	{
		HRESULT GetTypeDefinition([Out] out IXCLRDataTypeDefinition typeDefinition);
		HRESULT StartEnumInstances([In] IXCLRDataAppDomain appDomain, [Out] out nuint handle);
		HRESULT EnumInstance([In, Out] ref nuint handle, [Out] out IXCLRDataMethodInstance instance);
		HRESULT EndEnumInstances([In] nuint handle);
		HRESULT GetName([In] uint flags, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetTokenAndScope([Out] out int token, [Out] out IXCLRDataModule mod);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataMethodDefinition method);
		HRESULT GetLatestEnCVersion([Out] out uint version);
		HRESULT StartEnumExtents([Out] out nuint handle);
		HRESULT EnumExtent([In, Out] ref nuint handle, [Out] out CLRDATA_METHDEF_EXTENT extent);
		HRESULT EndEnumExtents([In] nuint handle);
		HRESULT GetCodeNotification([Out] out uint flags);
		HRESULT SetCodeNotification([In] uint flags);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT GetRepresentativeEntryAddress([Out] out CLRDATA_ADDRESS addr);
		HRESULT HasClassOrMethodInstantiation([Out] out int bGeneric);
	}
}
