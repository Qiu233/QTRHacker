using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("4675666C-C275-45b8-9F6C-AB165D5C1E09")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataTypeDefinition
	{
		HRESULT GetModule([Out] out IXCLRDataModule mod);
		HRESULT StartEnumMethodDefinitions([Out] out nuint handle);
		HRESULT EnumMethodDefinition([In, Out] ref nuint handle, [Out] out IXCLRDataMethodDefinition methodDefinition);
		HRESULT EndEnumMethodDefinitions([In] nuint handle);
		HRESULT StartEnumMethodDefinitionsByName([In] char* name, [In] uint flags, [Out] out nuint handle);
		HRESULT EnumMethodDefinitionByName([In, Out] ref nuint handle, [Out] out IXCLRDataMethodDefinition method);
		HRESULT EndEnumMethodDefinitionsByName([In] nuint handle);
		HRESULT GetMethodDefinitionByToken([In] int token, [Out] out IXCLRDataMethodDefinition methodDefinition);
		HRESULT StartEnumInstances([In] IXCLRDataAppDomain appDomain, [Out] out nuint handle);
		HRESULT EnumInstance([In, Out] ref nuint handle, [Out] out IXCLRDataTypeInstance instance);
		HRESULT EndEnumInstances([In] nuint handle);
		HRESULT GetName([In] uint flags, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
		HRESULT GetTokenAndScope([Out] out int token, [Out] out IXCLRDataModule mod);
		HRESULT GetCorElementType([Out] out CorElementType type);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataTypeDefinition type);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT GetArrayRank([Out] out uint rank);
		HRESULT GetBase([Out] out IXCLRDataTypeDefinition _base);
		HRESULT GetNumFields([In] uint flags, [Out] out uint numFields);
		HRESULT StartEnumFields([In] uint flags, [Out] out nuint handle);
		HRESULT EnumField([In, Out] ref nuint handle, [In] uint nameBufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out IXCLRDataTypeDefinition type, [Out] out uint flags, [Out] out int token);
		HRESULT EndEnumFields([In] nuint handle);
		HRESULT StartEnumFieldsByName([In] char* name, [In] uint nameFlags, [In] uint fieldFlags, [Out] out nuint handle);
		HRESULT EnumFieldByName([In, Out] ref nuint handle, [Out] out IXCLRDataTypeDefinition type, [Out] out uint flags, [Out] out int token);
		HRESULT EndEnumFieldsByName([In] nuint handle);
		HRESULT GetFieldByToken([In] int token, [In] uint nameBufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out IXCLRDataTypeDefinition type, [Out] out uint flags);
		HRESULT GetTypeNotification([Out] out uint flags);
		HRESULT SetTypeNotification([In] uint flags);
		HRESULT EnumField2([In, Out] ref nuint handle, [In] uint nameBufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out IXCLRDataTypeDefinition type, [Out] out uint flags, [Out] out IXCLRDataModule tokenScope, [Out] out int token);
		HRESULT EnumFieldByName2([In, Out] ref nuint handle, [Out] out IXCLRDataTypeDefinition type, [Out] out uint flags, [Out] out IXCLRDataModule tokenScope, [Out] out int token);
		HRESULT GetFieldByToken2([In] IXCLRDataModule tokenScope, [In] int token, [In] uint nameBufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out IXCLRDataTypeDefinition type, [Out] out uint flags);
	}
}
