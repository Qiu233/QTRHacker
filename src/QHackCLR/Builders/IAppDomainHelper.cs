using QHackCLR.DAC;
using QHackCLR.DAC.Defs;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface IAppDomainHelper
{
	ISOSDacInterface SOSDac { get; }
	IModuleHelper ModuleHelper { get; }
	IAssemblyHelper AssemblyHelper { get; }
	DACLibrary DACLibrary { get; }
	CLRRuntime Runtime { get; }
	CLRModule GetModule(nuint moduleBase);
	IEnumerable<CLRModule> EnumerateModules(CLRAppDomain appDomain);
}
