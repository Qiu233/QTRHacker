using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Scripts;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using QTRHacker.Core.GameObjects.Terraria;
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using static QTRHacker.Scripts.ScriptHelper;

namespace QTRHacker.Scripts.Functions;
public class BurnAllNPCs : BaseFunction
{
	public override bool CanDisable => false;
	public override bool HasProgress => true;
	public override void ApplyLocalization(string culture)
	{
		(Name, Tooltip) = culture switch
		{
			"zh" => ("燃烧所有NPC", "包括怪物和城镇/友好NPC"),
			_ => ("Burn All NPCs", "Including mobs and town/friendly npcs"),
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		var npc = ctx.NPC;
		int max = npc.Length;
		for (int i = 0; i < max; i++)
		{
			if (npc[i].Active)
				npc[i].AddBuff(153, 216000);
			Progress = ((double)i / max) * 100;
		}
	}
}

public class BurnAllPlayers : BaseFunction
{
	public override bool CanDisable => false;
	public override bool HasProgress => true;
	public override void ApplyLocalization(string culture)
	{
		(Name, Tooltip) = culture switch
		{
			"zh" => ("燃烧所有玩家", "包括自己"),
			_ => ("Burn All Players", "Including my player also"),
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		var player = ctx.Players;
		int max = player.Length;
		for (int i = 0; i < max; i++)
		{
			if (player[i].Active)
				player[i].AddBuff(44, 216000);
			Progress = ((double)i / max) * 100;
		}
	}
}

public class RevealTheWholeMap : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		(Name, Tooltip) = culture switch
		{
			"zh" => ("揭示整张地图", "有点慢，请稍等"),
			_ => ("Reveal The Whole Map", "A bit slow, please wait"),
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		AssemblySnippet asm = AssemblySnippet.FromEmpty();
		asm.Add(Instruction.Create("push ecx"));
		asm.Add(Instruction.Create("push edx"));
		asm.Add(
			AssemblySnippet.Loop(
				AssemblySnippet.Loop(
					AssemblySnippet.FromCode(
						new AssemblyCode[] {
							(Instruction)"mov edx, [esp+4]",
							(Instruction)"push [esp]",
							(Instruction)"push 255",
							AssemblySnippet.FromClrCall(
								ctx.GameModuleHelper.GetFunctionAddress("Terraria.Map.WorldMap", "UpdateLighting"), false, ctx.Map.BaseAddress, null, null,
								Array.Empty<object>())
						}),
					ctx.MaxTilesY, false),
				ctx.MaxTilesX, false));
		asm.Add(Instruction.Create("pop edx"));
		asm.Add(Instruction.Create("pop ecx"));

		ctx.RunByHookUpdate(asm);
		ctx.RefreshMap = true;
	}
}

public class RightClickToTP : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "大地图右键传送",
			_ => "Right Click on Map to TP",
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		int off = GetOffset(ctx, "Terraria.Entity", "position");
		var ass = AssemblySnippet.FromCode(
				new AssemblyCode[] {
						Instruction.Create("pushad"),
						Instruction.Create($"cmp byte ptr [{ctx.MapFullScreen_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{ctx.MouseRight_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{ctx.MouseRightRelease_Address}],0"),
						Instruction.Create("je _rwualfna"),
						AssemblySnippet.FromCode(
							new AssemblyCode[]{
								Instruction.Create($"mov byte ptr [{ctx.MapFullScreen_Address}],0"),
								Instruction.Create($"mov byte ptr [{ctx.MouseRightRelease_Address}],0"),
								AssemblySnippet.FromClrCall(
									ctx.GameModuleHelper.GetFunctionAddress("Terraria.Main","get_LocalPlayer"), false, null, null, null, Array.Empty<object>()),
								Instruction.Create("mov ebx,eax"),
								Instruction.Create("push eax"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{ctx.ScreenWidth_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{ctx.MouseX_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullscreenPos_Address + 4}]"),
								Instruction.Create("fsubrp"),
								Instruction.Create("mov dword ptr [esp],16"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fmulp"),
								Instruction.Create($"fstp dword ptr [ebx+{off}]"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{ctx.ScreenHeight_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{ctx.MouseY_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullscreenPos_Address + 8}]"),
								Instruction.Create("fsubrp"),
								Instruction.Create("mov dword ptr [esp],16"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fmulp"),
								Instruction.Create($"fstp dword ptr [ebx+{off + 0x4}]"),

								Instruction.Create("pop eax"),
							}),
						Instruction.Create("_rwualfna:"),
						Instruction.Create("popad")
				});
		HookParameters ps = new(ctx.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update") + 5, 4096);
		InlineHook.Hook(ctx.HContext, ass, ps);
	}
}

public class RandomizeUUID : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "随机UUID",
			_ => "Randomize UUID",
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		ctx.UUID = Guid.NewGuid().ToString();
	}
}


public class BuiltIn_3 : FunctionCategory
{
	public override string Category => "Advanced";
	public BuiltIn_3()
	{
		this["zh"] = "高级";
		this["en"] = "Advanced";
		Add<BurnAllNPCs>();
		Add<BurnAllPlayers>();
		Add<RandomizeUUID>();
		Add<RightClickToTP>();
		Add<RevealTheWholeMap>();
	}
}