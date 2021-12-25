using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("ECD73800-22CA-4b0d-AB55-E9BA7E6318A5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataMethodInstance
	{
		HRESULT GetTypeInstance([Out] out IXCLRDataTypeInstance typeInstance);
		HRESULT GetDefinition([Out] out IXCLRDataMethodDefinition methodDefinition);
		HRESULT GetTokenAndScope([Out] out int token, [Out] out IXCLRDataModule mod);
		HRESULT GetName([In] uint flags, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataMethodInstance method);
		HRESULT GetEnCVersion([Out] out uint version);
		HRESULT GetNumTypeArguments([Out] out uint numTypeArgs);
		HRESULT GetTypeArgumentByIndex([In] uint index, [Out] out IXCLRDataTypeInstance typeArg);
		HRESULT GetILOffsetsByAddress([In] CLRDATA_ADDRESS address, [In] uint offsetsLen, [Out] out uint offsetsNeeded, [Out/*, */] out uint ilOffsets);
		HRESULT GetAddressRangesByILOffset([In] uint ilOffset, [In] uint rangesLen, [Out] out uint rangesNeeded, [Out/*, */] out CLRDATA_ADDRESS_RANGE addressRanges);
		HRESULT GetILAddressMap([In] uint mapLen, [Out] out uint mapNeeded, [Out/*, */] CLRDATA_IL_ADDRESS_MAP* maps);
		HRESULT StartEnumExtents([Out] out nuint handle);
		HRESULT EnumExtent([In, Out] ref nuint handle, [Out] out CLRDATA_ADDRESS_RANGE extent);
		HRESULT EndEnumExtents([In] nuint handle);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT GetRepresentativeEntryAddress([Out] out CLRDATA_ADDRESS addr);
	}
}
