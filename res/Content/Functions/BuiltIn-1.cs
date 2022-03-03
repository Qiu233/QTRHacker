using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Scripts;
using QTRHacker.Core;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using static QTRHacker.Scripts.ScriptHelper;

public class InfiniteLife : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "无限生命";
				Tooltip = "免疫大部分伤害";
				break;
			case "en":
			default:
				Name = "Infinite Life";
				Tooltip = "Immune to most damages except continuous ones like burning";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "statLife");
		AobReplaceASM(ctx, $"sub [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"add [edx+{off}],eax");
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "statLife");
		AobReplaceASM(ctx, $"add [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"sub [edx+{off}],eax");
		this.IsEnabled = false;
	}
}

public class InfiniteMana : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "无限魔法";
				break;
			case "en":
			default:
				Name = "Infinite Mana";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "statMana");
		AobReplaceASM(ctx, $"sub [esi+{off}],edi", $"add [esi+{off}],edi");
		AobReplaceASM(ctx, $"sub [esi+{off}],eax", $"add [esi+{off}],eax");
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "statMana");
		AobReplaceASM(ctx, $"add [esi+{off}],edi", $"sub [esi+{off}],edi");
		AobReplaceASM(ctx, $"add [esi+{off}],eax", $"sub [esi+{off}],eax");
		this.IsEnabled = false;
	}
}

public class InfiniteOxygen : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "无限氧气";
				break;
			case "en":
			default:
				Name = "Infinite Oxygen";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "breath");
		AobReplaceASM(ctx, $"dec dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"inc dword ptr [eax+{off}]");
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "breath");
		AobReplaceASM(ctx, $"inc dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"dec dword ptr [eax+{off}]");
		this.IsEnabled = false;
	}
}

public class InfiniteMinion : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "无限召唤物";
				break;
			case "en":
			default:
				Name = "Infinite Minion";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "maxMinions");
		int offB = GetOffset(ctx, "Terraria.Player", "maxTurrets");
		AobReplaceASM(ctx, $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1", $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999");
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "maxMinions");
		int offB = GetOffset(ctx, "Terraria.Player", "maxTurrets");
		AobReplaceASM(ctx, $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999", $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1");
		this.IsEnabled = false;
	}
}

public class InfiniteItemAmmo : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "无限物品和子弹";
				break;
			case "en":
			default:
				Name = "Infinite Items and Ammo";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		AobReplace(ctx, "FF 88 B0 00 00 00 8B 45 E0 83 B8", "90 90 90 90 90 90");//dec dword ptr [eax+0xB0]\nmov eax,[ebp-0x20]\ncmp
		AobReplace(ctx, "FF 89 B0 00 00 00 8B 45 0C 8B 55 F4", "90 90 90 90 90 90");//dec dword ptr [ecx+0xB0]\nmov eax,[ebp+0xC]\nmov edx[ebp-0xC]
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		AobReplace(ctx, "90 90 90 90 90 90 8B 45 E0 83 B8", "FF 88 B0 00 00 00");
		AobReplace(ctx, "90 90 90 90 90 90 8B 45 0C 8B 55 F4", "FF 89 B0 00 00 00");
		this.IsEnabled = false;
	}
}

public class InfiniteFlyTime : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "无限飞行时间";
				break;
			case "en":
			default:
				Name = "Infinite Fly Time";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "wingTime");
		AobReplace(ctx, $"D9 99 {AobscanHelper.GetMByteCode(off)} 80 B9 F7060000 00", "90 90 90 90 90 90");
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "wingTime");
		AobReplace(ctx, "90 90 90 90 90 90 80 B9 F7060000 00", $"D9 99 {AobscanHelper.GetMByteCode(off)}");
		this.IsEnabled = false;
	}
}

public class ImmuneToDebuffs : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "免疫Debuff";
				break;
			case "en":
			default:
				Name = "Immune to debuffs";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = GetFunctionAddress(ctx, "Terraria.Player", "AddBuff");
		if (Read<byte>(ctx, a) == 0xE9)
			return;
		var player = ctx.MyPlayer;
		InlineHook.Hook(ctx.HContext,
			AssemblySnippet.FromCode(
				new AssemblyCode[]{
					(Instruction)$"pushad",
					(Instruction)$"mov ebx,{ctx.Debuff.BaseAddress}",
					(Instruction)$"cmp byte ptr [ebx+edx+8],0",
					(Instruction)$"je end",
					(Instruction)$"popad",
					(Instruction)$"ret 8",
					(Instruction)$"end:",
					(Instruction)$"popad",
				}), new HookParameters(a, 0x1000));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = GetFunctionAddress(ctx, "Terraria.Player", "AddBuff");
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

public class HighLight : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "全屏高亮";
				Tooltip = "请将游戏内视频设置为\"彩色\"";
				break;
			case "en":
			default:
				Name = "High Light";
				Tooltip = "Please set Video -> Lighting to \"Color\"";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint[] a = Aobscan(
			ctx,
			@"C7 ** ** ******** D9 07 D9 45 F0 DF F1 DD D8 7A").ToArray();
		if (!a.Any())
			return;
		InlineHook.Hook(ctx.HContext,
			AssemblySnippet.FromASMCode(
				@"mov dword ptr[ebp-0x10],0x3F800000
mov dword ptr[ebp-0x14],0x3F800000
mov dword ptr[ebp-0x18],0x3F800000"
),
				new HookParameters(a[0] + 7, 0x1000));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint[] a = Aobscan(ctx, "C7 ** ** ******** E9 ** ** ** ** DF F1 DD D8 7A").ToArray();
		if (!a.Any())
			return;
		InlineHook.FreeHook(ctx.HContext, a[0] + 7);
		this.IsEnabled = false;
	}
}

public class GhostMode : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "幽灵模式";
				break;
			case "en":
			default:
				Name = "Ghost Mode";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		ctx.MyPlayer.Ghost = true;
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.MyPlayer.Ghost = false;
		this.IsEnabled = false;
	}
}

FunctionCategory category = new FunctionCategory("Basic1");

category["zh"] = "基础1";
category["en"] = "Basic 1";

category.Add<InfiniteLife>();
category.Add<InfiniteMana>();
category.Add<InfiniteOxygen>();
category.Add<InfiniteMinion>();
category.Add<InfiniteItemAmmo>();
category.Add<InfiniteFlyTime>();
category.Add<ImmuneToDebuffs>();
category.Add<HighLight>();
category.Add<GhostMode>();

return category;