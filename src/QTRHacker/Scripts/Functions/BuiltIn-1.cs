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
using System;
using System.Windows;
using System.Threading.Tasks;
using QTRHacker.Controls;
using System.Windows.Controls;
using System.Windows.Media;
using QTRHacker.Localization;
using QTRHacker.ViewModels;
using QTRHacker.Core.GameObjects.Terraria;
using System.Windows.Data;
using System.IO;

namespace QTRHacker.Scripts.Functions;
public class CreativeMenu : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "旅行模式菜单";
				Tooltip = "在非旅行模式下可用";
				break;
			case "en":
			default:
				Name = "Journey Mode Menu";
				Tooltip = "Force journey mode menu enbaled";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "difficulty");
		AobReplace(ctx, $"80 B8 {AobscanHelper.GetMByteCode(off)} 03 74", $"80 B8 {AobscanHelper.GetMByteCode(off)} 03 EB");
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "difficulty");
		AobReplace(ctx, $"80 B8 {AobscanHelper.GetMByteCode(off)} 03 EB", $"80 B8 {AobscanHelper.GetMByteCode(off)} 03 74");
		IsEnabled = false;
	}
}
public class UnlockAllDuplications : BaseFunction
{
	private string ErrorMsg1 { get; set; }
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "解锁所有研究";
				Tooltip = "旅行模式菜单";
				ErrorMsg1 = "请先关闭/折叠旅行模式菜单";
				break;
			case "en":
			default:
				Name = "Unlock all duplications";
				Tooltip = "In journey mode menu";
				ErrorMsg1 = "Please first close or fold journey mode menu";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		dynamic enabled = ctx.GameModuleHelper
			.GetStaticHackObject("Terraria.Main", "CreativeMenu")
			.InternalGetMember("<Enabled>k__BackingField");
		if ((bool)enabled)// This restriction prevents crashes
		{
			MessageBox.Show(ErrorMsg1);
			return;
		}
		HackObject c = ctx.MyPlayer.InternalObject.creativeTracker.ItemSacrifices;
		nuint addr = ctx.GameModuleHelper
			.GetFunctionAddress("Terraria.GameContent.Creative.ItemsSacrificedUnlocksTracker",
			"RegisterItemSacrifice");
		var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				AssemblySnippet.Loop(
					AssemblySnippet.FromCode(new AssemblyCode[] {
						(Instruction)$"mov ecx, {c.BaseAddress}",
						(Instruction)$"mov edx, [esp]",
						(Instruction)$"push 9999",
						(Instruction)$"call {addr}",
					}),
					GameConstants.MaxItemTypes, true)
			});
		var task = Task.Run(() => ctx.RunOnManagedThread(code).WaitToDispose());
		if (!task.Wait(2000))
		{
			//TODO: abort the task
		}
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
}

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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "statLife");
		AobReplaceASM(ctx, $"add [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"sub [edx+{off}],eax");
		IsEnabled = false;
	}
}

public class InfiniteMana : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限魔法",
			_ => "Infinite Mana",
		};
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "statMana");
		AobReplaceASM(ctx, $"sub [esi+{off}],edi", $"add [esi+{off}],edi");
		AobReplaceASM(ctx, $"sub [esi+{off}],eax", $"add [esi+{off}],eax");
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "statMana");
		AobReplaceASM(ctx, $"add [esi+{off}],edi", $"sub [esi+{off}],edi");
		AobReplaceASM(ctx, $"add [esi+{off}],eax", $"sub [esi+{off}],eax");
		IsEnabled = false;
	}
}

public class InfiniteOxygen : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限氧气",
			_ => "Infinite Oxygen",
		};
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "breath");
		AobReplaceASM(ctx, $"dec dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"inc dword ptr [eax+{off}]");
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Player", "breath");
		AobReplaceASM(ctx, $"inc dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"dec dword ptr [eax+{off}]");
		IsEnabled = false;
	}
}

public class InfiniteMinion : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限召唤物",
			_ => "Infinite Minion",
		};
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "maxMinions");
		int offB = GetOffset(ctx, "Terraria.Player", "maxTurrets");
		AobReplaceASM(ctx, $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1", $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999");
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "maxMinions");
		int offB = GetOffset(ctx, "Terraria.Player", "maxTurrets");
		AobReplaceASM(ctx, $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999", $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1");
		IsEnabled = false;
	}
}

public class InfiniteAmmo : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限子弹",
			_ => "Infinite Ammo",
		};
	}
	public override void Enable(GameContext ctx)
	{
		AobReplace(ctx, "FF 88 B0 00 00 00 8B 45 E0 83 B8", "90 90 90 90 90 90");//dec dword ptr [eax+0xB0]\nmov eax,[ebp-0x20]\ncmp
		AobReplace(ctx, "FF 89 B0 00 00 00 8B 45 0C 8B 55 F4", "90 90 90 90 90 90");//dec dword ptr [ecx+0xB0]\nmov eax,[ebp+0xC]\nmov edx[ebp-0xC]
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		AobReplace(ctx, "90 90 90 90 90 90 8B 45 E0 83 B8", "FF 88 B0 00 00 00");
		AobReplace(ctx, "90 90 90 90 90 90 8B 45 0C 8B 55 F4", "FF 89 B0 00 00 00");
		IsEnabled = false;
	}
}

public class InfiniteFlyTime : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限飞行时间",
			_ => "Infinite Fly Time",
		};
	}
	public override void Enable(GameContext ctx)
	{
		int off1 = GetOffset(ctx, "Terraria.Player", "wingTime");
		int off2 = GetOffset(ctx, "Terraria.Player", "empressBrooch");
		AobReplace(ctx, $"D9 99 {AobscanHelper.GetMByteCode(off1)} 80 B9 {AobscanHelper.GetMByteCode(off2)} 00", "90 90 90 90 90 90");
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int off1 = GetOffset(ctx, "Terraria.Player", "wingTime");
		int off2 = GetOffset(ctx, "Terraria.Player", "empressBrooch");
		AobReplace(ctx, $"90 90 90 90 90 90 80 B9 {AobscanHelper.GetMByteCode(off2)} 00", $"D9 99 {AobscanHelper.GetMByteCode(off1)}");
		IsEnabled = false;
	}
}

public class ImmuneToDebuffs : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "免疫Debuff",
			_ => "Immune to debuffs",
		};
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = GetFunctionAddress(ctx, "Terraria.Player", "AddBuff");
		if (Read<byte>(ctx, a) == 0xE9)
			return;
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = GetFunctionAddress(ctx, "Terraria.Player", "AddBuff");
		InlineHook.FreeHook(ctx.HContext, a);
		IsEnabled = false;
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint[] a = Aobscan(ctx, "C7 ** ** ******** E9 ** ** ** ** DF F1 DD D8 7A").ToArray();
		if (!a.Any())
			return;
		InlineHook.FreeHook(ctx.HContext, a[0] + 7);
		IsEnabled = false;
	}
}

public class GhostMode : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "幽灵模式",
			_ => "Ghost Mode",
		};
	}
	public override void Enable(GameContext ctx)
	{
		ctx.MyPlayer.Ghost = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.MyPlayer.Ghost = false;
		IsEnabled = false;
	}
}
/*
public class BuiltIn_1 : FunctionCategory
{
	public override string Category => "Basic1";
	public BuiltIn_1()
	{
		this["zh"] = "基础1";
		this["en"] = "Basic 1";

		Add<CreativeMenu>();
		Add<UnlockAllDuplications>();
		Add<InfiniteLife>();
		Add<InfiniteMana>();
		Add<InfiniteOxygen>();
		Add<InfiniteMinion>();
		Add<InfiniteAmmo>();
		Add<InfiniteFlyTime>();
		Add<ImmuneToDebuffs>();
		Add<HighLight>();
		Add<GhostMode>();
	}
}
*/