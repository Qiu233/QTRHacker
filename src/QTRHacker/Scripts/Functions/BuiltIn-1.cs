using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using QTRHacker.Scripts;
using static QTRHacker.Scripts.ScriptHelper;
using System.Windows;

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
		ctx.Patches.CreativeMenu = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.CreativeMenu = false;
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
		var task = Task.Run(() => ctx.Patches.UnlockAllDuplications());
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
	private const string HookName = "InfiniteLife";
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
		ctx.Patches.InfiniteLife = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.InfiniteLife = false;
		IsEnabled = false;
	}
}

public class InfiniteMana : BaseFunction
{
	private const string HookName = "InfiniteMana";
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
		ctx.Patches.InfiniteMana = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.InfiniteMana = false;
		IsEnabled = false;
	}
}

public class InfiniteOxygen : BaseFunction
{
	private const string HookName = "InfiniteOxygen";
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
		ctx.Patches.InfiniteOxygen = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.InfiniteOxygen = false;
		IsEnabled = false;
	}
}

public class InfiniteMinion : BaseFunction
{
	private const string HookName = "InfiniteMinion";
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
		ctx.Patches.InfiniteMinion = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.InfiniteMinion = false;
		IsEnabled = false;
	}
}

public class InfiniteAmmo : BaseFunction
{
	private const string HookName = "InfiniteAmmo";
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
		ctx.Patches.InfiniteAmmo = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.InfiniteAmmo = false;
		IsEnabled = false;
	}
}

public class InfiniteFlyTime : BaseFunction
{
	private const string HookName = "InfiniteFlyTime";
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
		ctx.Patches.InfiniteFlyTime = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.InfiniteFlyTime = false;
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
		ctx.Patches.ImmuneToDebuffs = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.ImmuneToDebuffs = false;
		IsEnabled = false;
	}
}

public class HighLight : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "HighLight";
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
		ctx.Patches.HighLight = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.HighLight = false;
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
