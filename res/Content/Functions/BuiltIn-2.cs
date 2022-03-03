using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Scripts;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using QTRHacker.Core.GameObjects.Terraria;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using static QTRHacker.Scripts.ScriptHelper;

public class SlowFall : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "缓慢下落";
				break;
			case "en":
			default:
				Name = "Slow falling";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "slowFall");
		int offB = GetOffset(ctx, "Terraria.Player", "findTreasure");
		nuint a = Aobscan(
			ctx,
			$"88 96 {AobscanHelper.GetMByteCode(offA)} 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			$"mov dword ptr [esi+{offA}],1"),
			new HookParameters(a, 4096, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			$"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(GetOffset(ctx, "Terraria.Player", "findTreasure"))}").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

public class FastSpeed : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "加快移动速度";
				break;
			case "en":
			default:
				Name = "Super Fast Speed";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		int offA = GetOffset(ctx, "Terraria.Player", "moveSpeed");
		int offB = GetOffset(ctx, "Terraria.Player", "boneArmor");
		nuint a = Aobscan(
			ctx,
			$"D9 E8 D9 9E {AobscanHelper.GetMByteCode(offA)} 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			$"mov dword ptr [esi+{offA}],0x41A00000"),
			new HookParameters(a, 0x1000, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int offB = GetOffset(ctx, "Terraria.Player", "boneArmor");
		nuint a = Aobscan(
			ctx,
			$"E9 ******** 90 90 90 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
		if (a == 0) return;
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

public class SuperGrabRange : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "超远拾取距离";
				break;
			case "en":
			default:
				Name = "Super Grab Range";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
		if (Read<byte>(ctx, a) == 0xE9)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			"mov eax,1000\nret"),
			new HookParameters(a, 4096, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

public class BonusTwoSlots : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "额外两个饰品栏";
				break;
			case "en":
			default:
				Name = "Bonus Two Acc Slots";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
		if (Read<byte>(ctx, a) == 0xE9)
			return;
		InlineHook.Hook(ctx.HContext,
			AssemblySnippet.FromASMCode(
			"mov eax,1\nret"),
			new HookParameters(a, 4096, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

public class CoinPortalDropsBags : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "金币洞掉落财宝袋子";
				Tooltip = "金币洞弹幕类型518";
				break;
			case "en":
			default:
				Name = "Coin portal drops Treasure bags";
				Tooltip = "Coin portal projectile type: 518";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = AobscanASM(
			ctx,
			@"push 0
push 0
push 0x49
push 1
push 0
push 0
push 0
push 0").FirstOrDefault();
		if (a == 0)
			return;
		a += 2 * 5;
		InlineHook.Hook(ctx.HContext,
			AssemblySnippet.FromASMCode(
			"mov dword ptr [esp+8],3332"),
			new HookParameters(a, 4096, false, false));
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = AobscanASM(
				ctx,
				@"push 0
push 0
push 0x49
push 1
push 0").FirstOrDefault();
		if (a == 0)
			return;
		a += 2 * 5;
		InlineHook.FreeHook(ctx.HContext, a);
		this.IsEnabled = false;
	}
}

public class FishCratesOnly : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "只钓板条箱";
				break;
			case "en":
			default:
				Name = "Fish Crates only";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"8B 45 0C C6 00 00 8B 45 08 C6 00 00 B9").FirstOrDefault();
		if (a == 0)
			return;
		Write<byte>(ctx, a + 11, 1);
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"8B 45 0C C6 00 00 8B 45 08 C6 00 01 B9").FirstOrDefault();
		if (a == 0)
			return;
		Write<byte>(ctx, a + 11, 0);
		this.IsEnabled = false;
	}
}

public class EnableAllRecipes : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "解锁全部合成配方";
				break;
			case "en":
			default:
				Name = "Enable All Recipes";
				Tooltip = "Actually not all, only 3000 recipes are enabled.";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		var helper = ctx.GameModuleHelper;
		Write<byte>(ctx,
			helper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
			0xC3);
		helper.SetStaticFieldValue("Terraria.Main", "numAvailableRecipes", 3000);
		var array = new GameObjectArrayV<int>(ctx, helper.GetStaticHackObject("Terraria.Main", "availableRecipe"));
		int len = array.Length;
		for (int i = 0; i < len; i++)
			array[i] = i;
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		Write<byte>(ctx,
			ctx.GameModuleHelper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
			0x55);
		this.IsEnabled = false;
	}
}

public class StrengthenVampireKnives : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "强化吸血飞刀";
				Tooltip = "360°发射飞刀";
				break;
			case "en":
			default:
				Name = "Strengthen Vampire Knives";
				Tooltip = "Shoots in all directions";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"81 F9 21060000").FirstOrDefault();
		if (a == 0)
			return;
		ctx.HContext.DataAccess.Write<int>(a + 18, 100);
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = AobscanHelper.Aobscan(
			ctx.HContext.Handle,
			"81 F9 21060000").FirstOrDefault();
		if (a == 0)
			return;
		ctx.HContext.DataAccess.Write<int>(a + 18, 4);
		this.IsEnabled = false;
	}
}

public class SwingingAttacksAllMob : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "挥砍攻击所有怪物";
				break;
			case "en":
			default:
				Name = "Swinging Attacks All Mobs";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		this.IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		this.IsEnabled = false;
	}
}

FunctionCategory category = new FunctionCategory("Basic2");

category["zh"] = "基础2";
category["en"] = "Basic 2";

category.Add<SlowFall>();
category.Add<FastSpeed>();
category.Add<SuperGrabRange>();
category.Add<BonusTwoSlots>();
category.Add<CoinPortalDropsBags>();
category.Add<FishCratesOnly>();
category.Add<EnableAllRecipes>();

return category;