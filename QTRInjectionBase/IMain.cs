using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRInjectionBase
{
	public class IMain
	{
		public static event Action PreMainCalled = () => { };


		public static void ResetHook()
		{
			PreMainCalled = () => { };
		}

		[Hook("Terraria.exe", "Terraria.Main", "Update", PassArguments = false)]
		public void MainHook()
		{
			PreMainCalled();
		}
	}
}
