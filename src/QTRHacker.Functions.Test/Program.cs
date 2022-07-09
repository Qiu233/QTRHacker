using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
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
		public static unsafe int GetOffset(GameContext context, string module, string type, string field) => (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
		public static unsafe int GetOffset(GameContext context, string type, string field) => (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
		unsafe static void Main()
		{
			using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("dotnet")[0], "tModLoader");
			ctx.HContext.CLRHelpers.ToList().ForEach(t => { Console.WriteLine(t.Key.ToString()); });
		}
	}
}