using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public class ClrHeap
	{
		public ClrRuntime Runtime { get; }

		public ClrType FreeType { get; }
		public ClrType ObjectType { get; }

		public ClrType StringType { get; }
		public ClrType ExceptionType { get; }

		public ClrHeap(ClrRuntime runtime, IHeapHelper helper)
		{
			Runtime = runtime;

			helper.SOSDac.GetUsefulGlobals(out DacpUsefulGlobalsData tables);

			FreeType = helper.TypeFactory.GetClrType(tables.FreeMethodTable);
			ObjectType = helper.TypeFactory.GetClrType(tables.ObjectMethodTable);

			StringType = helper.TypeFactory.GetClrType(tables.StringMethodTable);
			ExceptionType = helper.TypeFactory.GetClrType(tables.ExceptionMethodTable);
		}
	}
}
