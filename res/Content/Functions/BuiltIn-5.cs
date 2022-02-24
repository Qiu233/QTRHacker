using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Scripts;
using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using static QTRHacker.Scripts.ScriptHelper;

public class SuperRange : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "超远距离";
				break;
			case "en":
			default:
				Name = "Super range";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"C7 05 ******** 05000000 C7 05 ******** 04000000 A1").FirstOrDefault();
		if (a == 0)
			return;
		nuint b = a + 6;
		nuint c = a + 16;
		int v = 0x1000;
		Write<int>(ctx, b, v);
		Write<int>(ctx, c, v);
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"C7 05 ******** 00100000 C7 05 ******** 00100000 A1").FirstOrDefault();
		if (a == 0)
			return;
		nuint b = a + 6;
		nuint c = a + 16;
		int v1 = 5;
		int v2 = 4;
		Write<int>(ctx, b, v1);
		Write<int>(ctx, c, v2);
		this.IsEnabled = false;
	}
}

public class FastTileAndWallPlacingSpeed : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "加快方块/墙壁放置速度";
				break;
			case "en":
			default:
				Name = "Super Fast Tile/Wall Placing Speed";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "wallSpeed");
		int offB = GetOffset(ctx, "Terraria.Player", "tileSpeed");
		nuint a = Aobscan(
			ctx,
			$"D9 E8 D9 9E {AobscanHelper.GetMByteCode(offA)} D9 E8 D9 9E {AobscanHelper.GetMByteCode(offB)} 88 96").FirstOrDefault();
		if (a == 0) return;

		InlineHook.Hook(ctx.HContext,
			AssemblySnippet.FromASMCode(
			$"mov dword ptr [esi+{offA})],0x41200000"),
			new HookParameters(a, 4096, false, false));

		InlineHook.Hook(ctx.HContext,
			AssemblySnippet.FromASMCode(
			$"mov dword ptr [esi+{offB}],0x41200000"),
			new HookParameters(a + 8, 4096, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"E9 ******** 90 90 90 E9 ******** 90 90 90 88 96").FirstOrDefault();
		if (a == 0) return;
		InlineHook.FreeHook(ctx.HContext, a);
		InlineHook.FreeHook(ctx.HContext, a + 8);
		this.IsEnabled = false;
	}
}


public class MachanicalRuler : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "机械尺";
				break;
			case "en":
			default:
				Name = "Machanical Ruler";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "rulerGrid");
		int offB = GetOffset(ctx, "Terraria.Player", "rulerLine");
		nuint a = Aobscan(
			ctx,
			$"88 96 {AobscanHelper.GetMByteCode(offA)} C6 86 {AobscanHelper.GetMByteCode(offB)} 01").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			$"mov byte ptr [esi+{offA}],0x1"),
			new HookParameters(a, 4096, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int offB = GetOffset(ctx, "Terraria.Player", "rulerLine");
		nuint a = Aobscan(
			   ctx,
			   $"E9 ******** 90 C6 86 {AobscanHelper.GetMByteCode(offB)} 01").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

public class MachanicalLens : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "机械眼镜";
				break;
			case "en":
			default:
				Name = "Machanical Lens";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "infoAccMechShowWires");
		int offB = GetOffset(ctx, "Terraria.Player", "accJarOfSouls");
		nuint a = Aobscan(
			ctx,
			$"88 96 {AobscanHelper.GetMByteCode(offA)} 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			$"mov byte ptr [esi+{offA}],0x1"),
			new HookParameters(a, 4096, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int offB = GetOffset(ctx, "Terraria.Player", "accJarOfSouls");
		nuint a = Aobscan(
			   ctx,
			   $"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

FunctionCategory category = new FunctionCategory("Builder");

category["zh"] = "建筑";
category["en"] = "Builder";

category.Add<SuperRange>();
category.Add<FastTileAndWallPlacingSpeed>();
category.Add<MachanicalRuler>();
category.Add<MachanicalLens>();

return category;