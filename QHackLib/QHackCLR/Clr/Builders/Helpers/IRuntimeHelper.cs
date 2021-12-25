using QHackCLR.Dac.Interfaces;
using QHackCLR.DataTargets;
using System.Collections.Generic;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface IRuntimeHelper
	{
		ISOSDacInterface SOSDac { get; }
		ITypeFactory TypeFactory { get; }
		IHeapHelper HeapHelper { get; }
		IXCLRDataProcess CLRDataProcess { get; }
		DataAccess DataAccess { get; }
		DacLibrary DacLibrary { get; }
		ClrAppDomain GetAppDomain(nuint handle);
		void Flush();
	}
}
