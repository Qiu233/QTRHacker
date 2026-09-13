using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using static QTRHacker.Scripts.ScriptHelper;

namespace QTRHacker.Scripts.Functions;
public class SlowFall : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.SlowFall;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "缓慢下落",
			_ => "Slow falling",
		};
	}
}

public class FastSpeed : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.FastSpeed;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "加快移动速度",
			_ => "Super Fast Speed",
		};
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
		nuint a = ctx.GameModuleHelper.GetClrMethodBySignature("Terraria.Player", "Terraria.Player.GetItemGrabRange(Terraria.Item)").NativeCode;
		if (Read<byte>(ctx, a) == 0xE9)
			return;
		InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
			"mov eax,1000\nret"),
			new HookParameters(a, 4096, false, false));
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		nuint a = ctx.GameModuleHelper.GetClrMethodBySignature("Terraria.Player", "Terraria.Player.GetItemGrabRange(Terraria.Item)").NativeCode;
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
		nuint a = ctx.GameModuleHelper.GetClrMethodBySignature("Terraria.Player", "Terraria.Player.IsItemSlotUnlockedAndUsable(Int32)").NativeCode;
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
		nuint a = ctx.GameModuleHelper.GetClrMethodBySignature("Terraria.Player", "Terraria.Player.IsItemSlotUnlockedAndUsable(Int32)").NativeCode;
		InlineHook.FreeHook(ctx.HContext, a);
		IsEnabled = false;
	}
}

public class CoinPortalDropsBags : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.CoinPortalDropsBags;
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
}

public class FishCratesOnly : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.FishCratesOnly;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "只钓板条箱",
			_ => "Fish Crates only",
		};
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
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		ctx.Patches.SetAllRecipesEnabled(true);
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.SetAllRecipesEnabled(false);
		IsEnabled = false;
	}
}

public class StrengthenVampireKnives : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.StrengthenVampireKnives;
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
