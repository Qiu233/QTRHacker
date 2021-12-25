using QHackCLR.Dac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface IModuleHelper
	{
		ISOSDacInterface SOSDac { get; }
		ITypeFactory TypeFactory { get; }
		ITypeHelper TypeHelper { get; }
		ClrHeap Heap { get; }
		ClrAppDomain AppDomain { get; }
		IMetadataImport GetMetadataImport(ClrModule module);
		ClrAppDomain GetAppDomain(nuint handle);
	}
}
