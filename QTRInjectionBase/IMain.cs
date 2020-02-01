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
		public static event Action AfterMainCalled = () => { };


		[Hook("Terraria.exe", "Terraria.Main", "Update", HookType.BEFORE, PassArguments = false)]
		public void MainPreHook()
		{
			PreMainCalled();
		}
		[Hook("Terraria.exe", "Terraria.Main", "Update", HookType.AFTER, PassArguments = false)]
		public void MainAfterHook()
		{
			AfterMainCalled();
		}
	}
}
