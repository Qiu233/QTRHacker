using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Memory;
using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		unsafe static void Main()
		{
			using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0]);
			var helper = ctx.GameModuleHelper;
			var h = helper.GetStaticHackObject("Terraria.Main", "versionNumber2");
			Console.WriteLine(new GameObjects.GameString(ctx, h).GetString());
			Console.WriteLine(Path.GetDirectoryName(helper.Module.FileName));
		}
	}
}
