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
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace QTRHacker.Functions.Test;

unsafe class Program
{
	public static unsafe int GetOffset(GameContext context, string module, string type, string field) => (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
	public static unsafe int GetOffset(GameContext context, string type, string field) => (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
	unsafe static void Main(string[] args)
	{
		if (args.Length > 0 && args[0] == "--verify-aobscan")
		{
			AobscanChecks.Verify();
			return;
		}
		if (args.Length > 0 && args[0] == "--gameplay-feature")
		{
			GameplayFeatureChecks.Run(args.Skip(1).ToArray());
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-game-compatibility")
		{
			GameCompatibility.Verify(args.Length > 1 ? args[1] : ".");
			return;
		}
		using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0]);
		//ctx.Patches.WorldPainter_BrushActive = true;
		ctx.MyPlayer.Inventory[0].SetDefaults(3063);
	}

}
