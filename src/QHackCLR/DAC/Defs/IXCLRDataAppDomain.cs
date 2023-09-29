using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.Defs;

[ComImport, Guid("7CA04601-C702-4670-A63C-FA44F7DA7BD5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal unsafe interface IXCLRDataAppDomain
{
	HRESULT GetProcess(
		/* [out] */ out IXCLRDataProcess process);

	HRESULT GetName(
		/* [in] */ uint bufLen,
		/* [out] */ uint* nameLen,
		/* [size_is][out] */ char* name);

	HRESULT GetUniqueID(
		/* [out] */ ulong* id);

	HRESULT GetFlags(
		/* [out] */ uint* flags);

	HRESULT IsSameObject(
		/* [in] */ IXCLRDataAppDomain appDomain);

	HRESULT GetManagedObject(
		/* [out] */ byte** value);

	HRESULT Request(
		/* [in] */ uint reqCode,
		/* [in] */ uint inBufferSize,
		/* [size_is][in] */ byte* inBuffer,
		/* [in] */ uint outBufferSize,
		/* [size_is][out] */ byte* outBuffer);
}
