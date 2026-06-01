using System;
using System.Collections.Generic;
using QHackLib.Assemble;

namespace QTRHacker.Core;

public static class PlayerUpdateSnippets
{
	private const int FloatOneThird = 0x3EAAAAAB;
	private const int FloatTen = 0x41200000;

	public static AssemblyCode InfiniteLife(GameContext ctx)
	{
		int lifeOff = GetOffset(ctx, "Terraria.Player", "statLife");
		int lifeMaxOff = GetOffset(ctx, "Terraria.Player", "statLifeMax2");

		return WithLocalPlayer(ctx, "InfiniteLife", new AssemblyCode[] {
			(Instruction)$"mov ebx, [eax+{lifeMaxOff}]",
			(Instruction)$"cmp ebx, 0",
			(Instruction)$"jle InfiniteLife_done",
			(Instruction)$"mov [eax+{lifeOff}], ebx",
		});
	}

	public static AssemblyCode InfiniteMana(GameContext ctx)
	{
		int manaOff = GetOffset(ctx, "Terraria.Player", "statMana");
		int manaMaxOff = GetOffset(ctx, "Terraria.Player", "statManaMax2");

		return WithLocalPlayer(ctx, "InfiniteMana", new AssemblyCode[] {
			(Instruction)$"mov ebx, [eax+{manaMaxOff}]",
			(Instruction)$"cmp ebx, 0",
			(Instruction)$"jle InfiniteMana_done",
			(Instruction)$"mov [eax+{manaOff}], ebx",
		});
	}

	public static AssemblyCode InfiniteAmmo(GameContext ctx)
	{
		int inventoryOff = GetOffset(ctx, "Terraria.Player", "inventory");
		int ammoOff = GetOffset(ctx, "Terraria.Item", "ammo");
		int stackOff = GetOffset(ctx, "Terraria.Item", "stack");
		int maxStackOff = GetOffset(ctx, "Terraria.Item", "maxStack");

		return WithLocalPlayer(ctx, "InfiniteAmmo", new AssemblyCode[] {
			(Instruction)$"mov edx, [eax+{inventoryOff}]",
			(Instruction)$"test edx, edx",
			(Instruction)$"jz InfiniteAmmo_done",
			(Instruction)$"xor ecx, ecx",
			(Instruction)$"InfiniteAmmo_loop:",
			(Instruction)$"cmp ecx, [edx+4]",
			(Instruction)$"jae InfiniteAmmo_done",
			(Instruction)$"cmp ecx, 59",
			(Instruction)$"jae InfiniteAmmo_done",
			(Instruction)$"mov ebx, [edx+ecx*4+8]",
			(Instruction)$"test ebx, ebx",
			(Instruction)$"jz InfiniteAmmo_next",
			(Instruction)$"cmp dword ptr [ebx+{ammoOff}], 0",
			(Instruction)$"jle InfiniteAmmo_next",
			(Instruction)$"cmp dword ptr [ebx+{stackOff}], 0",
			(Instruction)$"jle InfiniteAmmo_next",
			(Instruction)$"mov edi, [ebx+{maxStackOff}]",
			(Instruction)$"cmp edi, 999",
			(Instruction)$"jge InfiniteAmmo_write",
			(Instruction)$"mov edi, 999",
			(Instruction)$"InfiniteAmmo_write:",
			(Instruction)$"mov [ebx+{stackOff}], edi",
			(Instruction)$"InfiniteAmmo_next:",
			(Instruction)$"inc ecx",
			(Instruction)$"jmp InfiniteAmmo_loop",
		});
	}

	public static AssemblyCode InfiniteOxygen(GameContext ctx)
	{
		int breathOff = GetOffset(ctx, "Terraria.Player", "breath");
		int breathMaxOff = GetOffset(ctx, "Terraria.Player", "breathMax");

		return WithLocalPlayer(ctx, "InfiniteOxygen", new AssemblyCode[] {
			(Instruction)$"mov ebx, [eax+{breathMaxOff}]",
			(Instruction)$"cmp ebx, 0",
			(Instruction)$"jle InfiniteOxygen_done",
			(Instruction)$"mov [eax+{breathOff}], ebx",
		});
	}

	public static AssemblyCode InfiniteMinion(GameContext ctx)
	{
		int maxMinionsOff = GetOffset(ctx, "Terraria.Player", "maxMinions");
		int maxTurretsOff = GetOffset(ctx, "Terraria.Player", "maxTurrets");

		return WithLocalPlayer(ctx, "InfiniteMinion", new AssemblyCode[] {
			(Instruction)$"mov dword ptr [eax+{maxMinionsOff}], 9999",
			(Instruction)$"mov dword ptr [eax+{maxTurretsOff}], 9999",
		});
	}

	public static AssemblyCode InfiniteFlyTime(GameContext ctx)
	{
		int wingTimeOff = GetOffset(ctx, "Terraria.Player", "wingTime");
		int wingTimeMaxOff = GetOffset(ctx, "Terraria.Player", "wingTimeMax");
		int rocketTimeOff = GetOffset(ctx, "Terraria.Player", "rocketTime");
		int rocketTimeMaxOff = GetOffset(ctx, "Terraria.Player", "rocketTimeMax");

		return WithLocalPlayer(ctx, "InfiniteFlyTime", new AssemblyCode[] {
			(Instruction)$"mov ebx, [eax+{wingTimeMaxOff}]",
			(Instruction)$"cmp ebx, 0",
			(Instruction)$"jle InfiniteFlyTime_rocket",
			(Instruction)$"mov [eax+{wingTimeOff}], ebx",
			(Instruction)$"InfiniteFlyTime_rocket:",
			(Instruction)$"mov ebx, [eax+{rocketTimeMaxOff}]",
			(Instruction)$"cmp ebx, 0",
			(Instruction)$"jle InfiniteFlyTime_done",
			(Instruction)$"mov [eax+{rocketTimeOff}], ebx",
		});
	}

	public static AssemblyCode SlowFall(GameContext ctx)
	{
		return SetBool(ctx, "SlowFall", "slowFall", true);
	}

	public static AssemblyCode FastSpeed(GameContext ctx)
	{
		return SetInt32(ctx, "FastSpeed", "moveSpeed", FloatTen);
	}

	public static AssemblyCode FastTileAndWallPlacingSpeed(GameContext ctx)
	{
		int wallSpeedOff = GetOffset(ctx, "Terraria.Player", "wallSpeed");
		int tileSpeedOff = GetOffset(ctx, "Terraria.Player", "tileSpeed");

		return WithLocalPlayer(ctx, "FastTileAndWallPlacingSpeed", new AssemblyCode[] {
			(Instruction)$"mov dword ptr [eax+{wallSpeedOff}], {FloatOneThird}",
			(Instruction)$"mov dword ptr [eax+{tileSpeedOff}], {FloatOneThird}",
		});
	}

	public static AssemblyCode SuperRange(GameContext ctx)
	{
		nuint tileRangeX = ctx.GameModuleHelper.GetStaticFieldAddress("Terraria.Player", "tileRangeX");
		nuint tileRangeY = ctx.GameModuleHelper.GetStaticFieldAddress("Terraria.Player", "tileRangeY");
		int lastTileRangeXOff = GetOffset(ctx, "Terraria.Player", "lastTileRangeX");
		int lastTileRangeYOff = GetOffset(ctx, "Terraria.Player", "lastTileRangeY");

		return WithLocalPlayer(ctx, "SuperRange", new AssemblyCode[] {
			(Instruction)$"mov dword ptr [{tileRangeX}], 4096",
			(Instruction)$"mov dword ptr [{tileRangeY}], 4096",
			(Instruction)$"mov dword ptr [eax+{lastTileRangeXOff}], 4096",
			(Instruction)$"mov dword ptr [eax+{lastTileRangeYOff}], 4096",
		});
	}

	public static AssemblyCode MechanicalRuler(GameContext ctx)
	{
		int gridOff = GetOffset(ctx, "Terraria.Player", "rulerGrid");
		int lineOff = GetOffset(ctx, "Terraria.Player", "rulerLine");
		int builderAccStatusOff = GetOffset(ctx, "Terraria.Player", "builderAccStatus");

		return WithLocalPlayer(ctx, "MechanicalRuler", new AssemblyCode[] {
			(Instruction)$"mov byte ptr [eax+{gridOff}], 1",
			(Instruction)$"mov byte ptr [eax+{lineOff}], 1",
			SetBuilderAccVisible("MechanicalRuler_grid", builderAccStatusOff, 0),
			SetBuilderAccVisible("MechanicalRuler_line", builderAccStatusOff, 1),
		});
	}

	public static AssemblyCode MechanicalLens(GameContext ctx)
	{
		int wiresOff = GetOffset(ctx, "Terraria.Player", "InfoAccMechShowWires");
		int builderAccStatusOff = GetOffset(ctx, "Terraria.Player", "builderAccStatus");

		return WithLocalPlayer(ctx, "MechanicalLens", new AssemblyCode[] {
			(Instruction)$"mov byte ptr [eax+{wiresOff}], 1",
			SetBuilderAccVisible("MechanicalLens_red", builderAccStatusOff, 4),
			SetBuilderAccVisible("MechanicalLens_green", builderAccStatusOff, 5),
			SetBuilderAccVisible("MechanicalLens_blue", builderAccStatusOff, 6),
			SetBuilderAccVisible("MechanicalLens_yellow", builderAccStatusOff, 7),
			SetBuilderAccVisible("MechanicalLens_hideAll", builderAccStatusOff, 8),
			SetBuilderAccVisible("MechanicalLens_actuators", builderAccStatusOff, 9),
		});
	}

	public static AssemblyCode BonusTwoSlots(GameContext ctx)
	{
		return SetBool(ctx, "BonusTwoSlots", "extraAccessory", true);
	}

	private static AssemblyCode WithLocalPlayer(GameContext ctx, string labelPrefix, IEnumerable<AssemblyCode> body)
	{
		nuint playerArrAddr = ctx.GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "player");
		nuint myPlayerAddr = ctx.GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "myPlayer");

		var snippet = AssemblySnippet.FromCode(new AssemblyCode[] {
			(Instruction)$"mov eax, [{myPlayerAddr}]",
			(Instruction)$"cmp eax, 0",
			(Instruction)$"jl {labelPrefix}_done",
			(Instruction)$"mov edx, [{playerArrAddr}]",
			(Instruction)$"test edx, edx",
			(Instruction)$"jz {labelPrefix}_done",
			(Instruction)$"cmp eax, [edx+4]",
			(Instruction)$"jae {labelPrefix}_done",
			(Instruction)$"mov eax, [edx+eax*4+8]",
			(Instruction)$"test eax, eax",
			(Instruction)$"jz {labelPrefix}_done",
		});
		snippet.Content.AddRange(body);
		snippet.Content.Add((Instruction)$"{labelPrefix}_done:");
		return snippet;
	}

	private static AssemblyCode SetBool(GameContext ctx, string labelPrefix, string field, bool value)
	{
		int off = GetOffset(ctx, "Terraria.Player", field);
		return WithLocalPlayer(ctx, labelPrefix, new AssemblyCode[] {
			(Instruction)$"mov byte ptr [eax+{off}], {(value ? 1 : 0)}",
		});
	}

	private static AssemblyCode SetInt32(GameContext ctx, string labelPrefix, string field, int value)
	{
		int off = GetOffset(ctx, "Terraria.Player", field);
		return WithLocalPlayer(ctx, labelPrefix, new AssemblyCode[] {
			(Instruction)$"mov dword ptr [eax+{off}], {value}",
		});
	}

	private static AssemblyCode SetBuilderAccVisible(string labelPrefix, int builderAccStatusOff, int index)
	{
		return AssemblySnippet.FromASMCode($@"
			mov edx, [eax+{builderAccStatusOff}]
			test edx, edx
			jz {labelPrefix}_done
			cmp dword ptr [edx+4], {index + 1}
			jb {labelPrefix}_done
			mov dword ptr [edx+{8 + index * 4}], 0
			{labelPrefix}_done:
		");
	}

	private static int GetOffset(GameContext ctx, string type, string field)
	{
		return checked((int)ctx.GameModuleHelper.GetInstanceFieldOffset(type, field) + IntPtr.Size);
	}
}
