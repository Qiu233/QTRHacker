using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.Defs;

[ComImport, Guid("88E32849-0A0A-4cb0-9022-7CD2E9E139E2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal unsafe interface IXCLRDataModule
{
	HRESULT StartEnumAssemblies(
	   /* [out] */ byte* handle);

	HRESULT EnumAssembly(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** assembly);

	HRESULT EndEnumAssemblies(
	   /* [in] */ ulong handle);

	HRESULT StartEnumTypeDefinitions(
	   /* [out] */ byte* handle);

	HRESULT EnumTypeDefinition(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** typeDefinition);

	HRESULT EndEnumTypeDefinitions(
	   /* [in] */ ulong handle);

	HRESULT StartEnumTypeInstances(
	   /* [in] */ byte* appDomain,
	   /* [out] */ byte* handle);

	HRESULT EnumTypeInstance(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** typeInstance);

	HRESULT EndEnumTypeInstances(
	   /* [in] */ ulong handle);

	HRESULT StartEnumTypeDefinitionsByName(
	   /* [in] */ string name,
	   /* [in] */ uint flags,
	   /* [out] */ byte* handle);

	HRESULT EnumTypeDefinitionByName(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** type);

	HRESULT EndEnumTypeDefinitionsByName(
	   /* [in] */ ulong handle);

	HRESULT StartEnumTypeInstancesByName(
	   /* [in] */ string name,
	   /* [in] */ uint flags,
	   /* [in] */ byte* appDomain,
	   /* [out] */ byte* handle);

	HRESULT EnumTypeInstanceByName(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** type);

	HRESULT EndEnumTypeInstancesByName(
	   /* [in] */ ulong handle);

	HRESULT GetTypeDefinitionByToken(
	   /* [in] */ uint token,
	   /* [out] */ byte** typeDefinition);

	HRESULT StartEnumMethodDefinitionsByName(
	   /* [in] */ string name,
	   /* [in] */ uint flags,
	   /* [out] */ byte* handle);

	HRESULT EnumMethodDefinitionByName(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** method);

	HRESULT EndEnumMethodDefinitionsByName(
	   /* [in] */ ulong handle);

	HRESULT StartEnumMethodInstancesByName(
	   /* [in] */ string name,
	   /* [in] */ uint flags,
	   /* [in] */ byte* appDomain,
	   /* [out] */ byte* handle);

	HRESULT EnumMethodInstanceByName(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** method);

	HRESULT EndEnumMethodInstancesByName(
	   /* [in] */ ulong handle);

	HRESULT GetMethodDefinitionByToken(
	   /* [in] */ uint token,
	   /* [out] */ byte** methodDefinition);

	HRESULT StartEnumDataByName(
	   /* [in] */ string name,
	   /* [in] */ uint flags,
	   /* [in] */ byte* appDomain,
	   /* [in] */ byte* tlsTask,
	   /* [out] */ byte* handle);

	HRESULT EnumDataByName(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** value);

	HRESULT EndEnumDataByName(
	   /* [in] */ ulong handle);

	HRESULT GetName(
	   /* [in] */ uint bufLen,
	   /* [out] */ out uint nameLen,
	   /* [size_is][out] */ char* name);

	HRESULT GetFileName(
	   /* [in] */ uint bufLen,
	   /* [out] */ out uint nameLen,
	   /* [size_is][out] */ char* name);

	HRESULT GetFlags(
	   /* [out] */ byte* flags);

	HRESULT IsSameObject(
	   /* [in] */ byte* mod);

	HRESULT StartEnumExtents(
	   /* [out] */ byte* handle);

	HRESULT EnumExtent(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte* extent);

	HRESULT EndEnumExtents(
	   /* [in] */ ulong handle);

	HRESULT Request(
	   /* [in] */ uint reqCode,
	   /* [in] */ uint inBufferSize,
	   /* [size_is][in] */ byte* inBuffer,
	   /* [in] */ uint outBufferSize,
	   /* [size_is][out] */ byte* outBuffer);

	HRESULT StartEnumAppDomains(
	   /* [out] */ byte* handle);

	HRESULT EnumAppDomain(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** appDomain);

	HRESULT EndEnumAppDomains(
	   /* [in] */ ulong handle);

	HRESULT GetVersionId(
	   /* [out] */ byte* vid);

}
