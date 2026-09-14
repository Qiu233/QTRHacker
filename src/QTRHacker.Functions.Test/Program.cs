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
	[STAThread]
	unsafe static void Main(string[] args)
	{
		if (args.Length == 0 || args[0] == "--diagnose")
		{
			Environment.ExitCode = FieldDiagnostics.Run(args.Skip(1).ToArray());
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-process-exit")
		{
			if (args.Length != 2)
				throw new ArgumentException("Usage: --verify-process-exit <QHackCLR.TestTarget.exe>");
			ProcessExitChecks.Verify(args[1]);
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-clr-high-addresses")
		{
			if (args.Length != 2)
				throw new ArgumentException("Usage: --verify-clr-high-addresses <QHackCLR.TestTarget.exe>");
			DacAddressChecks.Verify(args[1]);
			return;
		}
		if (args.Length > 0 && args[0] == "--stress-live-items")
		{
			LiveItemChecks.Run(args.Skip(1).ToArray());
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-veh-items-window")
		{
			VehItemChecks.VerifyWindow(args.Skip(1).ToArray());
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-veh-items")
		{
			VehItemChecks.Verify(args.Skip(1).ToArray());
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-veh-join")
		{
			if (args.Length > 2)
				throw new ArgumentException("Usage: --verify-veh-join [VehJoinTarget.exe]");
			VehJoinChecks.Verify(args.Length == 2 ? args[1] : Path.Combine(AppContext.BaseDirectory, "VehJoinTarget.exe"));
			return;
		}
		if (args.Length > 0 && args[0] == "--veh-join-controller")
		{
			if (args.Length != 3)
				throw new ArgumentException("Internal VEH fixture controller requires target and report paths.");
			VehJoinChecks.RunController(args[1], args[2]);
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-inline-hook")
		{
			if (args.Length != 2)
				throw new ArgumentException("Usage: --verify-inline-hook <QHackCLR.TestTarget.exe>");
			InlineHookChecks.Verify(args[1]);
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-game-attach")
		{
			GameAttachChecks.Verify();
			return;
		}
		if (args.Length > 0 && args[0] == "--verify-clr")
		{
			if (args.Length != 2)
				throw new ArgumentException("Usage: --verify-clr <QHackCLR.TestTarget.exe>");
			ClrChecks.Verify(args[1]);
			return;
		}
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
		if (args.Length != 1 || args[0] != "--manual-item")
			throw new ArgumentException("Unknown command. Use --diagnose for customer diagnostics.");
		using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0]);
		//ctx.Patches.WorldPainter_BrushActive = true;
		ctx.MyPlayer.Inventory[0].SetDefaults(3063);
	}

}
