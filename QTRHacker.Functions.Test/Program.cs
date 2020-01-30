using Keystone;
using Microsoft.Diagnostics.Runtime;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.ProjectileImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id))
			{
				//Console.WriteLine(gc.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate").ToString("X8"));
				CLRFunctionCaller.Call(gc, "Terraria.exe",
					"Terraria.WorldGen", "KillTile",
					gc.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"),
					(int)gc.MyPlayer.X / 16, (int)gc.MyPlayer.Y / 16 + 3 + 2, false, false, false);


				//gc.HContext.Runtime.Modules.ToList().ForEach(t => Console.WriteLine(t.Name));
				/*var ah = gc.HContext.GetAddressHelper("TRInjections");
				ah.Module.EnumerateTypes().ToList().ForEach(type =>
				{
					Console.WriteLine("---" + type.Name);
					type.Methods.ToList().ForEach(method =>
					{
						Console.WriteLine(method.Name +"--"+ method.NativeCode.ToString("X8"));
					});
				});*/
				Console.Read();
			}
		}
	}
}
