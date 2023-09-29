using QHackCLR.DAC.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface IAssemblyHelper
{
	ISOSDacInterface SOSDac { get; }
	IModuleHelper ModuleHelper { get; }
}
