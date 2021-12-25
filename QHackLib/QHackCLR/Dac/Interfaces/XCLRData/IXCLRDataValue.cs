using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("96EC93C7-1000-4e93-8991-98D8766E6666")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataValue
	{
		HRESULT GetFlags([Out] out uint flags);
		HRESULT GetAddress([Out] out CLRDATA_ADDRESS address);
		HRESULT GetSize([Out] out ulong size);
		HRESULT GetBytes([In] uint bufLen, [Out] out uint dataSize, [Out/*, */] out byte buffer);
		HRESULT SetBytes([In] uint bufLen, [Out] out uint dataSize, [In/*, */] in byte buffer);
		HRESULT GetType([Out] out IXCLRDataTypeInstance typeInstance);
		HRESULT GetNumFields([Out] out uint numFields);
		HRESULT GetFieldByIndex([In] uint index, [Out] out IXCLRDataValue field, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out int token);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT GetNumFields2([In] uint flags, [In] IXCLRDataTypeInstance fromType, [Out] out uint numFields);
		HRESULT StartEnumFields([In] uint flags, [In] IXCLRDataTypeInstance fromType, [Out] out nuint handle);
		HRESULT EnumField([In, Out] ref nuint handle, [Out] out IXCLRDataValue field, [In] uint nameBufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out int token);
		HRESULT EndEnumFields([In] nuint handle);
		HRESULT StartEnumFieldsByName([In] char* name, [In] uint nameFlags, [In] uint fieldFlags, [In] IXCLRDataTypeInstance fromType, [Out] out nuint handle);
		HRESULT EnumFieldByName([In, Out] ref nuint handle, [Out] out IXCLRDataValue field, [Out] out int token);
		HRESULT EndEnumFieldsByName([In] nuint handle);
		HRESULT GetFieldByToken([In] int token, [Out] out IXCLRDataValue field, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
		HRESULT GetAssociatedValue([Out] out IXCLRDataValue assocValue);
		HRESULT GetAssociatedType([Out] out IXCLRDataTypeInstance assocType);
		HRESULT GetString([In] uint bufLen, [Out] out uint strLen, [Out/*, */] char* str);
		HRESULT GetArrayProperties([Out] out uint rank, [Out] out uint totalElements, [In] uint numDim, [Out/*, */] out uint dims, [In] uint numBases, [Out/*, */] out int bases);
		HRESULT GetArrayElement([In] uint numInd, [In/*, */] in int indices, [Out] out IXCLRDataValue value);
		HRESULT EnumField2([In, Out] ref nuint handle, [Out] out IXCLRDataValue field, [In] uint nameBufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out IXCLRDataModule tokenScope, [Out] out int token);
		HRESULT EnumFieldByName2([In, Out] ref nuint handle, [Out] out IXCLRDataValue field, [Out] out IXCLRDataModule tokenScope, [Out] out int token);
		HRESULT GetFieldByToken2([In] IXCLRDataModule tokenScope, [In] int token, [Out] out IXCLRDataValue field, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
		HRESULT GetNumLocations([Out] out uint numLocs);
		HRESULT GetLocationByIndex([In] uint loc, [Out] out uint flags, [Out] out CLRDATA_ADDRESS arg);
	}
}
