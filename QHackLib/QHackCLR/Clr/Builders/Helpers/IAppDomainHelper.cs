using QHackCLR.Dac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface IAppDomainHelper
	{
		ISOSDacInterface SOSDac { get; }
		IModuleHelper ModuleHelper { get; }
		DacLibrary DacLibrary { get; }
		ClrRuntime Runtime { get; }
		IAssemblyHelper AssemblyHelper { get; }
		ClrModule GetModule(nuint moduleBase);
		IEnumerable<ClrModule> EnumerateModules(ClrAppDomain appDomain);
	}
}
