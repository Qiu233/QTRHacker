using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("E5F3039D-2C0C-4230-A69E-12AF1C3E563C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRLibrarySupport
	{
		HRESULT LoadHardboundDependency(char* name, ref Guid mvid, [Out] out nuint loadedBase);
		HRESULT LoadSoftboundDependency(char* name, byte* assemblymetadataBinding, byte* hash, uint hashLength, [Out] out nuint loadedBase);
	}
}
