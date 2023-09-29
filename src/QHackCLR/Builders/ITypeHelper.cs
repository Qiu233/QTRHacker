using QHackCLR.DAC.Defs;
using QHackCLR.DataTargets;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface ITypeHelper
{
	ISOSDacInterface SOSDac { get; }
	ITypeFactory TypeFactory { get; }
	IObjectHelper ObjectHelper { get; }
	CLRHeap Heap { get; }
	DataAccess DataAccess { get; }
	CLRModule GetModule(nuint handle);
	IEnumerable<CLRField> EnumerateFields(CLRType type);
	IEnumerable<CLRMethod> EnumerateVTableMethods(CLRType type);
}
