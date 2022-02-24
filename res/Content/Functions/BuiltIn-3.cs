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

public class BurnAllNPCs : BaseFunction
{
	public override bool CanDisable => false;
	public override bool HasProgress => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "燃烧所有NPC";
				Tooltip = "包括怪物和城镇/友好NPC";
				break;
			case "en":
			default:
				Name = "Burn All NPCs";
				Tooltip = "Including mobs and town/friendly npcs";
				break;
		}
	}
	public override void Disable(GameContext ctx)
	{
		throw new NotImplementedException();
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
		switch (culture)
		{
			case "zh":
				Name = "燃烧所有玩家";
				Tooltip = "包括自己";
				break;
			case "en":
			default:
				Name = "Burn All Players";
				Tooltip = "Including my player also";
				break;
		}
	}
	public override void Disable(GameContext ctx)
	{
		throw new NotImplementedException();
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
		switch (culture)
		{
			case "zh":
				Name = "揭示整张地图";
				Tooltip = "有点慢，请稍等";
				break;
			case "en":
			default:
				Name = "Reveal The Whole Map";
				Tooltip = "A bit slow, please wait";
				break;
		}
	}
	public override void Disable(GameContext ctx)
	{
		throw new NotImplementedException();
	}
	public override void Enable(GameContext ctx)
	{
		AssemblySnippet asm = AssemblySnippet.FromEmpty();
		asm.Content.Add(Instruction.Create("push ecx"));
		asm.Content.Add(Instruction.Create("push edx"));
		asm.Content.Add(
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
		asm.Content.Add(Instruction.Create("pop edx"));
		asm.Content.Add(Instruction.Create("pop ecx"));

		ctx.RunByHookOnUpdate(asm);
		ctx.RefreshMap = true;
	}
}

FunctionCategory category = new FunctionCategory("Advanced");

category["zh"] = "高级";
category["en"] = "Advanced";

category.Add<BurnAllNPCs>();
category.Add<BurnAllPlayers>();
category.Add<RevealTheWholeMap>();

return category;