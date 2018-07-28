using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id))
			{
				int fAddr = gc.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Item::SetDefaults");
				AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(fAddr, null, false, gc.MyPlayer.Inventory[0].BaseAddress, 3063, false);
				using (RemoteExecution re = RemoteExecution.Create(gc.HContext, snippet))
				{
					re.Execute();
					System.Threading.Thread.Sleep(50);
				}
			}
		}
	}
}
