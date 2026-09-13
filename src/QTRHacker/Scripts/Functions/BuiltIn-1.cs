using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using static QTRHacker.Scripts.ScriptHelper;
using System.Windows;

namespace QTRHacker.Scripts.Functions;
public class CreativeMenu : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.CreativeMenu;
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

public class InfiniteLife : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.InfiniteLife;
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
}

public class InfiniteMana : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.InfiniteMana;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限魔法",
			_ => "Infinite Mana",
		};
	}
}

public class InfiniteOxygen : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.InfiniteOxygen;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限氧气",
			_ => "Infinite Oxygen",
		};
	}
}

public class InfiniteMinion : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.InfiniteMinion;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限召唤物",
			_ => "Infinite Minion",
		};
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
		ctx.Patches.InfiniteAmmo_Enabled = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.InfiniteAmmo_Enabled = false;
		IsEnabled = false;
	}
}

public class InfiniteFlyTime : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.InfiniteFlyTime;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "无限飞行时间",
			_ => "Infinite Fly Time",
		};
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

public class HighLight : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.HighLight;
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
