using System;
using System.Collections.Generic;
using System.Linq;
using QHackLib.Memory;

namespace QTRHacker.Core;

internal static class TerrariaHookPoints
{
	public static nuint GetMainUpdateAddress(GameContext ctx)
	{
		nuint address = ctx.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update");
		if (address == 0)
			throw new InvalidOperationException("Could not locate Terraria.Main.Update native address.");
		return address;
	}

	public static nuint GetPlayerUpdateHookAddress(GameContext ctx)
	{
		var candidates = new List<nuint>();
		var helper = ctx.GameModuleHelper;

		AddCandidate(candidates, ctx,
			$"88 96 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "slowFall"))} 88 96 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "findTreasure"))}",
			12);
		AddCandidate(candidates, ctx,
			$"D9 E8 D9 9E {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "moveSpeed"))} 88 96 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "boneArmor"))}",
			14);
		AddCandidate(candidates, ctx,
			$"C7 86 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "maxMinions"))} 01 00 00 00 C7 86 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "maxTurrets"))} 01 00 00 00",
			20);
		AddCandidate(candidates, ctx,
			$"D9 E8 D9 9E {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "wallSpeed"))} D9 E8 D9 9E {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "tileSpeed"))} 88 96",
			22);
		AddCandidate(candidates, ctx,
			$"88 96 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "InfoAccMechShowWires"))} 88 96 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "accJarOfSouls"))}",
			12);
		AddCandidate(candidates, ctx,
			$"88 96 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "rulerGrid"))} C6 86 {MOff(helper.GetInstanceFieldOffset("Terraria.Player", "rulerLine"))} 01",
			13);

		if (candidates.Count == 0)
			throw new InvalidOperationException("Could not locate a Terraria per-frame native hook point.");

		return candidates.Max();
	}

	public static nuint GetPlayerItemCheckHookAddress(GameContext ctx)
	{
		nuint address = ctx.GameModuleHelper.GetFunctionAddress("Terraria.Player", "ItemCheck");
		if (address == 0)
			throw new InvalidOperationException("Could not locate Terraria.Player.ItemCheck native address.");
		return address;
	}

	private static void AddCandidate(List<nuint> candidates, GameContext ctx, string pattern, int bytesToSkip)
	{
		var matches = AobscanHelper.Aobscan(ctx.HContext.Handle, pattern).Take(2).ToArray();
		if (matches.Length == 1)
			candidates.Add(matches[0] + (uint)bytesToSkip);
	}

	private static string MOff(uint rawOffset)
	{
		return AobscanHelper.GetMByteCode(checked((int)rawOffset + IntPtr.Size));
	}
}
