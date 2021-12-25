using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("c12f35a9-e55c-4520-a894-b3dc5165dfce")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface8
	{
		HRESULT GetNumberGenerations(uint* pGenerations);
		HRESULT GetGenerationTable(uint cGenerations, DacpGenerationData* pGenerationData, uint* pNeeded);
		HRESULT GetFinalizationFillPointers(uint cFillPointers, CLRDATA_ADDRESS* pFinalizationFillPointers, uint* pNeeded);
		HRESULT GetGenerationTableSvr(CLRDATA_ADDRESS heapAddr, uint cGenerations, DacpGenerationData* pGenerationData, uint* pNeeded);
		HRESULT GetFinalizationFillPointersSvr(CLRDATA_ADDRESS heapAddr, uint cFillPointers, CLRDATA_ADDRESS* pFinalizationFillPointers, uint* pNeeded);
		HRESULT GetAssemblyLoadContext(CLRDATA_ADDRESS methodTable, CLRDATA_ADDRESS* assemblyLoadContext);
	}
}
