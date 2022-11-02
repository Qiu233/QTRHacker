using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using static QTRHacker.Scripts.ScriptHelper;

namespace QTRHacker.Scripts.Functions;
public class SlowFall : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "缓慢下落",
			_ => "Slow falling",
		};
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			$"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(GetOffset(ctx, "Terraria.Player", "findTreasure"))}").FirstOrDefault();
		if (a == 0)
			return;
		InlineHook.FreeHook(ctx.HContext, a);
		IsEnabled = false;
	}
}

public class FastSpeed : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "加快移动速度",
			_ => "Super Fast Speed",
		};
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		int offB = GetOffset(ctx, "Terraria.Player", "boneArmor");
		nuint a = Aobscan(
			ctx,
			$"E9 ******** 90 90 90 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
		if (a == 0) return;
		InlineHook.FreeHook(ctx.HContext, a);
		IsEnabled = false;
	}
}

public class SuperGrabRange : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "超远拾取距离",
			_ => "Super Grab Range",
		};
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
		if (Read<byte>(ctx, a) == 0xE9)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			"mov eax,1000\nret"),
			new HookParameters(a, 4096, false, false));
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
		InlineHook.FreeHook(ctx.HContext, a);
		IsEnabled = false;
	}
}

public class BonusTwoSlots : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "额外两个饰品栏",
			_ => "Bonus Two Acc Slots",
		};
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
		InlineHook.FreeHook(ctx.HContext, a);
		IsEnabled = false;
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
		IsEnabled = true;
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
		IsEnabled = false;
	}
}

public class FishCratesOnly : BaseFunction
{
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "只钓板条箱",
			_ => "Fish Crates only",
		};
	}
	public override void Enable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"8B 45 0C C6 00 00 8B 45 08 C6 00 00 B9").FirstOrDefault();
		if (a == 0)
			return;
		Write<byte>(ctx, a + 11, 1);
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = Aobscan(
			ctx,
			"8B 45 0C C6 00 00 8B 45 08 C6 00 01 B9").FirstOrDefault();
		if (a == 0)
			return;
		Write<byte>(ctx, a + 11, 0);
		IsEnabled = false;
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		Write<byte>(ctx,
			ctx.GameModuleHelper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
			0x55);
		IsEnabled = false;
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
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = AobscanHelper.Aobscan(
			ctx.HContext.Handle,
			"81 F9 21060000").FirstOrDefault();
		if (a == 0)
			return;
		ctx.HContext.DataAccess.Write<int>(a + 18, 4);
		IsEnabled = false;
	}
}


public class BuiltIn_2 : FunctionCategory
{
	public override string Category => "Basic2";
	public BuiltIn_2()
	{
		this["zh"] = "基础2";
		this["en"] = "Basic 2";

		Add<SlowFall>();
		Add<FastSpeed>();
		Add<SuperGrabRange>();
		Add<BonusTwoSlots>();
		Add<CoinPortalDropsBags>();
		Add<FishCratesOnly>();
		Add<EnableAllRecipes>();
		Add<StrengthenVampireKnives>();
	}
}
