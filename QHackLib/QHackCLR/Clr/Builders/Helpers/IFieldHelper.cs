using QHackCLR.Dac.Interfaces;
using QHackCLR.DataTargets;
using QHackCLR.Metadata.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface IFieldHelper
	{
		ISOSDacInterface SOSDac { get; }
		ITypeFactory TypeFactory { get; }
		DataAccess DataAccess { get; }
		bool GetFieldProps(ClrType declaringType, int token, out string name, out FieldAttributes attributes, out SigParser sigParser);
		nuint GetStaticFieldAddress(ClrStaticField field);
	}
}
