using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using System.Linq;
using static QTRHacker.Scripts.ScriptHelper;

namespace QTRHacker.Scripts.Functions;
public class SuperRange : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "超远距离",
			_ => "Super range",
		};
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
		IsEnabled = true;
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
		IsEnabled = false;
	}
}

public class FastTileAndWallPlacingSpeed : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "加快方块/墙壁放置速度",
			_ => "Super Fast Tile/Wall Placing Speed",
		};
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"E9 ******** 90 90 90 E9 ******** 90 90 90 88 96").FirstOrDefault();
		if (a == 0) return;
		InlineHook.FreeHook(ctx.HContext, a);
		InlineHook.FreeHook(ctx.HContext, a + 8);
		IsEnabled = false;
	}
}


public class MachanicalRuler : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "机械尺",
			_ => "Machanical Ruler",
		};
		Tooltip = culture switch
		{
			"zh" => "显示网格",
			_ => "Show grids",
		};
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
		IsEnabled = true;
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
		IsEnabled = false;
	}
}

public class MachanicalLens : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "机械眼镜",
			_ => "Machanical Lens",
		};
		Tooltip = culture switch
		{
			"zh" => "显示电线",
			_ => "Show wires",
		};
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "InfoAccMechShowWires");
		int offB = GetOffset(ctx, "Terraria.Player", "accJarOfSouls");
		nuint a = Aobscan(
			ctx,
			$"88 96 {AobscanHelper.GetMByteCode(offA)} 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			$"mov byte ptr [esi+{offA}],0x1"),
			new HookParameters(a, 4096, false, false));
		IsEnabled = true;
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
		IsEnabled = false;
	}
}
/*
public class BuiltIn_5 : FunctionCategory
{
	public override string Category => "Builder";
	public BuiltIn_5()
	{
		this["zh"] = "建筑";
		this["en"] = "Builder"; 
		Add<SuperRange>();
		Add<FastTileAndWallPlacingSpeed>();
		Add<MachanicalRuler>();
		Add<MachanicalLens>();
	}
}
*/