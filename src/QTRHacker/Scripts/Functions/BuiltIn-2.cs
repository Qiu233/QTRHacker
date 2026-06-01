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
	private const string HookName = "SlowFall";
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
		ctx.Patches.SlowFall = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.SlowFall = false;
		IsEnabled = false;
	}
}

public class FastSpeed : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "FastSpeed";
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
		ctx.Patches.FastSpeed = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.FastSpeed = false;
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
		ctx.Patches.SuperGrabRange = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.SuperGrabRange = false;
		IsEnabled = false;
	}
}

public class BonusTwoSlots : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "BonusTwoSlots";
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
		ctx.Patches.BonusTwoSlots = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.BonusTwoSlots = false;
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
		ctx.Patches.CoinPortalDropsBags = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.CoinPortalDropsBags = false;
		IsEnabled = false;
	}

	private static nuint FindCoinPortalItemSpawnAddr(GameContext ctx)
	{
		// Scan for the coin portal AI code. The coin portal projectile has aiStyle == 94.
		// In the JIT code, the Item.NewItem call will have the item type 73 (0x49) as an immediate.
		// We look for the pattern: the comparison/assignment for aiStyle 94 followed by the NewItem call.
		// The key unique pattern: "push 0x49" (6A 49) appears in the coin portal's item spawn code.
		// We use an AOB that matches the coin portal AI state update code pattern.

		// Pattern: fld [localAI+0] / fadd [float_1.0] / fstp [localAI+0] / fld [localAI+0] / fcomp [ai+0]
		// This is the "localAI[0] += 1f; if (localAI[0] >= this.ai[1])" check in the coin portal AI.
		nuint[] aiPattern = Aobscan(ctx, "D9 45 ** D9 05 ** ** ** ** D8 C1 D9 5D ** D9 45 ** D9 47 ** DF E1 F6 C4 41 75").ToArray();
		if (aiPattern.Length > 0)
		{
			// Search forward from the pattern for push 0x49 (6A 49) within 200 bytes
			foreach (var a in aiPattern)
			{
				for (nuint offset = 0; offset < 200; offset++)
				{
					byte op = ctx.HContext.DataAccess.Read<byte>(a + offset);
					byte imm = ctx.HContext.DataAccess.Read<byte>(a + offset + 1);
					if (op == 0x6A && imm == 0x49)
						return a + offset;
				}
			}
		}

		// Fallback: search for any push 0x49 followed by call near coin portal-related code
		// The coin portal AI has unique FPU operations for the timer state machine.
		// Try alternative pattern: the velocity assignment after Item.NewItem
		// "fild [base.Center.X]" pattern (D9 45 or DB 45 for integer load)
		nuint[] altPattern = Aobscan(ctx, "D9 05 ** ** ** ** D8 C1 D9 5D ** DB 45 ** D9 5D ** D9 45 ** D9 43 ** DF E1").ToArray();
		if (altPattern.Length > 0)
		{
			foreach (var a in altPattern)
			{
				for (nuint offset = 0; offset < 200; offset++)
				{
					byte op = ctx.HContext.DataAccess.Read<byte>(a + offset);
					byte imm = ctx.HContext.DataAccess.Read<byte>(a + offset + 1);
					if (op == 0x6A && imm == 0x49)
						return a + offset;
				}
			}
		}

		return 0;
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
		ctx.Patches.FishCratesOnly = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.FishCratesOnly = false;
		IsEnabled = false;
	}
}

public class EnableAllRecipes : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "EnableAllRecipes";
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
		ctx.Patches.EnableAllRecipes = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.EnableAllRecipes = false;
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
		ctx.Patches.StrengthenVampireKnives = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.StrengthenVampireKnives = false;
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
