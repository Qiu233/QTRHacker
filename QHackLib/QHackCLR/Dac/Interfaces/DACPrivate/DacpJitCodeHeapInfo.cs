using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Interfaces
{
	public unsafe struct DacpJitCodeHeapInfo
	{
		public uint CodeHeapType;
		public CLRDATA_ADDRESS BaseAddr;
		public CLRDATA_ADDRESS CurrentAddr;
	};
}