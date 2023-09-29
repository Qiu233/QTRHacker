using QHackCLR.DAC.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface IMethodHelper
{
	ISOSDacInterface SOSDac { get; }
	ITypeFactory TypeFactory { get; }
}
