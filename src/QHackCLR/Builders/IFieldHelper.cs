using QHackCLR.DAC.Defs;
using QHackCLR.DataTargets;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface IFieldHelper
{
	ISOSDacInterface SOSDac { get; }
	ITypeFactory TypeFactory { get; }
	DataAccess DataAccess { get; }
	bool GetFieldProps(CLRType declType, uint token, out string? name, out FieldAttributes? attributes);
	nuint GetStaticFieldAddress(CLRStaticField field);
}
