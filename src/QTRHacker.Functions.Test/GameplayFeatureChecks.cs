using QTRHacker.Core;
using QHackLib.Memory;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test;

internal static class GameplayFeatureChecks
{
	public static void Run(string[] args)
	{
		using var context = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0]);
		Console.WriteLine($"Terraria PID {context.GameProcess.Id}; gameMenu={context.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "gameMenu")}");
		if (args.Length == 1 && args[0] == "inspect")
		{
			Console.WriteLine($"ResetEffects=0x{context.GameModuleHelper.GetFunctionAddress("Terraria.Player", "ResetEffects"):X}");
			Console.WriteLine($"Player=0x{context.MyPlayer.InternalObject.BaseAddress:X}; patchesLoaded={context.Patches.IsInitialized}");
			return;
		}
		if (args.Length == 1 && args[0] == "verify-loader-buffer")
		{
			byte[] bytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "QTRHacker.Patches.dll"));
			using var allocation = new MemoryAllocation(context.HContext, (uint)bytes.Length + 64);
			byte[] copy = new byte[bytes.Length];
			if (!allocation.Write(bytes, (uint)bytes.Length, 0) || !allocation.Read(copy, (uint)copy.Length, 0) || !bytes.SequenceEqual(copy))
				throw new InvalidOperationException("Remote loader buffer did not round-trip the patch DLL.");
			Console.WriteLine($"PASS: {bytes.Length} DLL bytes round-tripped through remote buffer 0x{allocation.AllocationBase:X}.");
			return;
		}
		if (args.Length == 1 && args[0] == "verify-toggle-cycle")
		{
			const string type = "QTRHacker.Patches.GameplayPatches";
			if (context.Patches.IsInitialized && context.Patches.PatchHelper.GetStaticFieldValue<int>(type, "EnabledFeatures") != 0)
				throw new InvalidOperationException("Disable gameplay features before running the toggle cycle.");
			var features = Enum.GetValues<GameplayFeature>();
			try
			{
				Parallel.ForEach(features, feature => context.Patches.SetGameplayFeature(feature, true));
				int expected = features.Aggregate(0, (bits, feature) => bits | (1 << (int)feature));
				if (context.Patches.PatchHelper.GetStaticFieldValue<int>(type, "EnabledFeatures") != expected)
					throw new InvalidOperationException("Concurrent enable did not set every feature bit.");
			}
			finally
			{
				if (!context.GameProcess.HasExited)
					Parallel.ForEach(features, feature => context.Patches.SetGameplayFeature(feature, false));
			}
			if (context.Patches.PatchHelper.GetStaticFieldValue<int>(type, "EnabledFeatures") != 0)
				throw new InvalidOperationException("Concurrent disable did not clear every feature bit.");
			Console.WriteLine("PASS: concurrent controller requests enabled and disabled all 16 features; flags=0.");
			return;
		}
		if (args.Length != 2 || !Enum.TryParse<GameplayFeature>(args[0], out var feature)
			|| !Enum.IsDefined(feature) || (args[1] != "on" && args[1] != "off"))
			throw new ArgumentException("Use --gameplay-feature inspect|verify-toggle-cycle, or --gameplay-feature <GameplayFeature> on|off. This modifies the running game.");
		context.Patches.SetGameplayFeature(feature, args[1] == "on");
		Thread.Sleep(250);
		Console.WriteLine($"{feature} {args[1]}; flags=0x{context.Patches.PatchHelper.GetStaticFieldValue<int>("QTRHacker.Patches.GameplayPatches", "EnabledFeatures"):X}");
		Console.WriteLine($"slowFall={context.MyPlayer.SlowFall}; moveSpeed={context.MyPlayer.MoveSpeed}; minions={context.MyPlayer.MaxMinions}; wallSpeed={context.MyPlayer.WallSpeed}; tileSpeed={context.MyPlayer.TileSpeed}; ruler={context.MyPlayer.RulerGrid}; wires={context.MyPlayer.InfoAccMechShowWires}");
	}
}
