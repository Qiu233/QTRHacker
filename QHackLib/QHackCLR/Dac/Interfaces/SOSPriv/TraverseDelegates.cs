using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Interfaces
{
	public unsafe delegate void MODULEMAPTRAVERSE(int index, CLRDATA_ADDRESS methodTable, void* token);
	public unsafe delegate void DUMPEHINFO(uint clauseIndex, uint totalClauses, DACEHInfo* pEHInfo, void* token);
	public unsafe delegate void VISITHEAP(CLRDATA_ADDRESS blockData, nuint blockSize, bool blockIsCurrentBlock);
	public unsafe delegate void VISITRCWFORCLEANUP(CLRDATA_ADDRESS RCW, CLRDATA_ADDRESS Context, CLRDATA_ADDRESS Thread,
		bool bIsFreeThreaded, void* token);
}
