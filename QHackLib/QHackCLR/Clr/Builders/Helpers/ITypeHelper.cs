using QHackCLR.Dac.Interfaces;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface ITypeHelper
	{
		ISOSDacInterface SOSDac { get; }
		ClrHeap Heap { get; }
		ITypeFactory TypeFactory { get; }
		DataAccess DataAccess { get; }
		IClrObjectHelper ClrObjectHelper { get; }
		IEnumerable<ClrField> EnumerateFields(ClrType type);
		IEnumerable<ClrMethod> EnumerateVTableMethods(ClrType type);
		ClrModule GetModule(nuint moduleBase);
	}
}
