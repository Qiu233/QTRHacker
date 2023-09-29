using QHackCLR.DAC.Defs;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface IModuleHelper
{
	ISOSDacInterface SOSDac { get; }
	ITypeFactory TypeFactory { get; }
	ITypeHelper TypeHelper { get; }
	CLRHeap Heap { get; }
	CLRAppDomain AppDomain { get; }
	CLRAppDomain GetAppDomain(nuint handle);
	IMetaDataImport GetMetadataImport(CLRModule module);
}
