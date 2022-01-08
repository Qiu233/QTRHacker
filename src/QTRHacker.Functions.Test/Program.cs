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
			ctx.Patches.Init();
			//ctx.Patches.WorldPainter_EyeDropperActive = true;
			/*using QHackContext ctx = QHackContext.Create(Process.GetProcessesByName("Terraria")[0].Id);
			foreach (var m in ctx.CLRHelpers)
			{
				Console.WriteLine(m.Key.FileName);
			}
			var helper = ctx.CLRHelpers.Last(t => t.Key.Name == "QTRHacker.Patches").Key;
			foreach (var t in helper.DefinedTypes)
			{
				Console.WriteLine(t.Name);
			}*/
			/*var helper = ctx.GetCLRHelper("Terraria");
			var h = helper.GetStaticFieldAddress("Terraria.GameContent.TextureAssets", "MagicPixel");
			Console.WriteLine(h.ToString("X8"));*/
		}
	}
}
