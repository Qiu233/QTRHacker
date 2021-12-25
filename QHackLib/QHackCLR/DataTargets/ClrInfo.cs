using QHackCLR.Clr;
using QHackCLR.Clr.Builders;
using QHackCLR.Dac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DataTargets
{
	public class ClrInfo
	{
		public DataTarget DataTarget { get; }
		public nuint RuntimeBase { get; }
		public string DacPath { get; }
		public ClrFlavor Flavor { get; }

		internal ClrInfo(DataTarget dt, ClrFlavor flavor, nuint moduleBase, string dacPath)
		{
			DataTarget = dt;
			Flavor = flavor;
			RuntimeBase = moduleBase;
			DacPath = dacPath;
		}
		public ClrRuntime CreateRuntime()
		{
			return new RuntimeBuilder(this, new DacLibrary(DataTarget, DacPath, RuntimeBase)).Runtime;
		}
	}
}
