using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Helpers
{
	public static class IXCLRDataModuleHelper
	{
		public static IEnumerable<IXCLRDataTypeInstance> GetTypeInstances(this IXCLRDataModule module, IXCLRDataAppDomain appDomain)
		{
			var enumerator = GeneralEnumerator<IXCLRDataTypeInstance>.Create(
				(out nuint handle) => module.StartEnumTypeInstances(appDomain, out handle),
				module.EnumTypeInstance,
				module.EndEnumTypeInstances);
			return enumerator.GetAll();
		}
		public static IEnumerable<IXCLRDataTypeDefinition> GetTypeDefinitions(this IXCLRDataModule module)
		{
			var enumerator = GeneralEnumerator<IXCLRDataTypeDefinition>.Create(
				module.StartEnumTypeDefinitions,
				module.EnumTypeDefinition,
				module.EndEnumTypeDefinitions);
			return enumerator.GetAll();
		}
	}
}
