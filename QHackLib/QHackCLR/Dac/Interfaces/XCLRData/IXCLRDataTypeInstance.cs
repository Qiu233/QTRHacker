using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("4D078D91-9CB3-4b0d-97AC-28C8A5A82597")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataTypeInstance
	{
		HRESULT StartEnumMethodInstances([Out] out nuint handle);
		HRESULT EnumMethodInstance([In, Out] ref nuint handle, [Out] out IXCLRDataMethodInstance methodInstance);
		HRESULT EndEnumMethodInstances([In] nuint handle);
		HRESULT StartEnumMethodInstancesByName([In] char* name, [In] uint flags, [Out] out nuint handle);
		HRESULT EnumMethodInstanceByName([In, Out] ref nuint handle, [Out] out IXCLRDataMethodInstance method);
		HRESULT EndEnumMethodInstancesByName([In] nuint handle);
		HRESULT GetNumStaticFields([Out] out uint numFields);
		HRESULT GetStaticFieldByIndex([In] uint index, [In] IXCLRDataTask tlsTask, [Out] out IXCLRDataValue field, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out int token);
		HRESULT StartEnumStaticFieldsByName([In] char* name, [In] uint flags, [In] IXCLRDataTask tlsTask, [Out] out nuint handle);
		HRESULT EnumStaticFieldByName([In, Out] ref nuint handle, [Out] out IXCLRDataValue value);
		HRESULT EndEnumStaticFieldsByName([In] nuint handle);
		HRESULT GetNumTypeArguments([Out] out uint numTypeArgs);
		HRESULT GetTypeArgumentByIndex([In] uint index, [Out] out IXCLRDataTypeInstance typeArg);
		HRESULT GetName([In] uint flags, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
		HRESULT GetModule([Out] out IXCLRDataModule mod);
		HRESULT GetDefinition([Out] out IXCLRDataTypeDefinition typeDefinition);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataTypeInstance type);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT GetNumStaticFields2([In] uint flags, [Out] out uint numFields);
		HRESULT StartEnumStaticFields([In] uint flags, [In] IXCLRDataTask tlsTask, [Out] out nuint handle);
		HRESULT EnumStaticField([In, Out] ref nuint handle, [Out] out IXCLRDataValue value);
		HRESULT EndEnumStaticFields([In] nuint handle);
		HRESULT StartEnumStaticFieldsByName2([In] char* name, [In] uint nameFlags, [In] uint fieldFlags, [In] IXCLRDataTask tlsTask, [Out] out nuint handle);
		HRESULT EnumStaticFieldByName2([In, Out] ref nuint handle, [Out] out IXCLRDataValue value);
		HRESULT EndEnumStaticFieldsByName2([In] nuint handle);
		HRESULT GetStaticFieldByToken([In] int token, [In] IXCLRDataTask tlsTask, [Out] out IXCLRDataValue field, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
		HRESULT GetBase([Out] out IXCLRDataTypeInstance _base);
		HRESULT EnumStaticField2([In, Out] ref nuint handle, [Out] out IXCLRDataValue value, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out IXCLRDataModule tokenScope, [Out] out int token);
		HRESULT EnumStaticFieldByName3([In, Out] ref nuint handle, [Out] out IXCLRDataValue value, [Out] out IXCLRDataModule tokenScope, [Out] out int token);
		HRESULT GetStaticFieldByToken2([In] IXCLRDataModule tokenScope, [In] int token, [In] IXCLRDataTask tlsTask, [Out] out IXCLRDataValue field, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
	}
}
