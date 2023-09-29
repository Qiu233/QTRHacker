using QHackCLR.Builders;
using QHackCLR.DAC.DACP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public unsafe class CLRHeap
{
	public CLRRuntime Runtime { get; }
	internal CLRHeap(CLRRuntime runtime, IHeapHelper helper)
	{
		Runtime = runtime;
		DacpUsefulGlobalsData tables;
		helper.SOSDac.GetUsefulGlobals(&tables);

		FreeType = helper.TypeFactory.GetCLRType(tables.FreeMethodTable)!;
		ObjectType = helper.TypeFactory.GetCLRType(tables.ObjectMethodTable)!;

		StringType = helper.TypeFactory.GetCLRType(tables.StringMethodTable)!;
		ExceptionType = helper.TypeFactory.GetCLRType(tables.ExceptionMethodTable)!;
	}

	public CLRType FreeType { get; }
	public CLRType ObjectType { get; }
	public CLRType StringType { get; }
	public CLRType ExceptionType { get; }
}
