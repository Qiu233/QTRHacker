using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("88E32849-0A0A-4cb0-9022-7CD2E9E139E2")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataModule
	{
		HRESULT StartEnumAssemblies([Out] out nuint handle);
		HRESULT EnumAssembly([In, Out] ref nuint handle, [Out] out IXCLRDataAssembly assembly);
		HRESULT EndEnumAssemblies([In] nuint handle);
		HRESULT StartEnumTypeDefinitions([Out] out nuint handle);
		HRESULT EnumTypeDefinition([In, Out] ref nuint handle, [Out] out IXCLRDataTypeDefinition typeDefinition);
		HRESULT EndEnumTypeDefinitions([In] nuint handle);
		HRESULT StartEnumTypeInstances([In] IXCLRDataAppDomain appDomain, [Out] out nuint handle);
		HRESULT EnumTypeInstance([In, Out] ref nuint handle, [Out] out IXCLRDataTypeInstance typeInstance);
		HRESULT EndEnumTypeInstances([In] nuint handle);
		HRESULT StartEnumTypeDefinitionsByName([In] char* name, [In] uint flags, [Out] out nuint handle);
		HRESULT EnumTypeDefinitionByName([In, Out] ref nuint handle, [Out] out IXCLRDataTypeDefinition type);
		HRESULT EndEnumTypeDefinitionsByName([In] nuint handle);
		HRESULT StartEnumTypeInstancesByName([In] char* name, [In] uint flags, [In] IXCLRDataAppDomain appDomain, [Out] out nuint handle);
		HRESULT EnumTypeInstanceByName([In, Out] ref nuint handle, [Out] out IXCLRDataTypeInstance type);
		HRESULT EndEnumTypeInstancesByName([In] nuint handle);
		HRESULT GetTypeDefinitionByToken([In] int token, [Out] out IXCLRDataTypeDefinition typeDefinition);
		HRESULT StartEnumMethodDefinitionsByName([In] char* name, [In] uint flags, [Out] out nuint handle);
		HRESULT EnumMethodDefinitionByName([In, Out] ref nuint handle, [Out] out IXCLRDataMethodDefinition method);
		HRESULT EndEnumMethodDefinitionsByName([In] nuint handle);
		HRESULT StartEnumMethodInstancesByName([In] char* name, [In] uint flags, [In] IXCLRDataAppDomain appDomain, [Out] out nuint handle);
		HRESULT EnumMethodInstanceByName([In, Out] ref nuint handle, [Out] out IXCLRDataMethodInstance method);
		HRESULT EndEnumMethodInstancesByName([In] nuint handle);
		HRESULT GetMethodDefinitionByToken([In] int token, [Out] out IXCLRDataMethodDefinition methodDefinition);
		HRESULT StartEnumDataByName([In] char* name, [In] uint flags, [In] IXCLRDataAppDomain appDomain, [In] IXCLRDataTask tlsTask, [Out] out nuint handle);
		HRESULT EnumDataByName([In, Out] ref nuint handle, [Out] out IXCLRDataValue value);
		HRESULT EndEnumDataByName([In] nuint handle);
		HRESULT GetName([In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetFileName([In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataModule mod);
		HRESULT StartEnumExtents([Out] out nuint handle);
		HRESULT EnumExtent([In, Out] ref nuint handle, [Out] out CLRDATA_MODULE_EXTENT extent);
		HRESULT EndEnumExtents([In] nuint handle);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT StartEnumAppDomains([Out] out nuint handle);
		HRESULT EnumAppDomain([In, Out] ref nuint handle, [Out] out IXCLRDataAppDomain appDomain);
		HRESULT EndEnumAppDomains([In] nuint handle);
		HRESULT GetVersionId([Out] out Guid vid);
	}
}
