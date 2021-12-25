using QHackCLR.Dac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Helpers
{
	public unsafe static class IXCLRDataTypeInstanceHelper
	{
		public static IEnumerable<IXCLRDataMethodInstance> GetMethods(this IXCLRDataTypeInstance typeInst)
		{
			var enumerator = GeneralEnumerator<IXCLRDataMethodInstance>.Create(
				typeInst.StartEnumMethodInstances,
				typeInst.EnumMethodInstance,
				typeInst.EndEnumMethodInstances);
			return enumerator.GetAll();
		}
		public static string GetName(this IXCLRDataTypeInstance typeInst)
		{
			return HelperGlobals.GetNameWithFlags(typeInst.GetName, 0);
		}
	}
}
