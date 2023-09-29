using QHackCLR.DAC.Defs;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface IObjectHelper
{
	ISOSDacInterface SOSDac { get; }
	ITypeFactory TypeFactory { get; }
	DataAccess DataAccess { get; }
}
