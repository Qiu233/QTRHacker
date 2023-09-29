using QHackCLR.Builders;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DataTargets;

public record ClrInfo(DataTarget DataTarget, ClrFlavor Flavor, nuint RuntimeBase, string DacPath, string ClrModulePath)
{
	public CLRRuntime CreateRuntime()
	{
		return new RuntimeBuilder(this, new DAC.DACLibrary(DataTarget, DacPath, RuntimeBase)).Runtime;
	}
}