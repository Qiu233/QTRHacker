using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.QHackCLR.Clr.Structs
{
	public readonly ref struct ILToNativeMap
	{
		public readonly int ILOffset;
		public readonly nuint StartAddress;
		public readonly nuint EndAddress;

		public ILToNativeMap(int ilOffset, nuint startAddress, nuint endAddress)
		{
			ILOffset = ilOffset;
			StartAddress = startAddress;
			EndAddress = endAddress;
		}
	}
}
