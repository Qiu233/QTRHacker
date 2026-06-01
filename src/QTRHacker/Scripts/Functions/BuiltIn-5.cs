using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using static QTRHacker.Scripts.ScriptHelper;

namespace QTRHacker.Scripts.Functions;
public class SuperRange : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "SuperRange";
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "超远距离",
			_ => "Super range",
		};
	}
	public override void Enable(GameContext ctx)
	{
		ctx.Patches.SuperRange = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.SuperRange = false;
		IsEnabled = false;
	}
}

public class FastTileAndWallPlacingSpeed : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "FastTileAndWallPlacingSpeed";
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "加快方块/墙壁放置速度",
			_ => "Super Fast Tile/Wall Placing Speed",
		};
	}
	public override void Enable(GameContext ctx)
	{
		ctx.Patches.FastTileAndWallPlacingSpeed = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.FastTileAndWallPlacingSpeed = false;
		IsEnabled = false;
	}
}


public class MachanicalRuler : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "MachanicalRuler";
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "机械尺",
			_ => "Machanical Ruler",
		};
		Tooltip = culture switch
		{
			"zh" => "显示网格",
			_ => "Show grids",
		};
	}
	public override void Enable(GameContext ctx)
	{
		ctx.Patches.MechanicalRuler = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.MechanicalRuler = false;
		IsEnabled = false;
	}
}

public class MachanicalLens : BaseFunction
{
	public override bool CanDisable => true;
	private const string HookName = "MachanicalLens";
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "机械眼镜",
			_ => "Machanical Lens",
		};
		Tooltip = culture switch
		{
			"zh" => "显示电线",
			_ => "Show wires",
		};
	}
	public override void Enable(GameContext ctx)
	{
		ctx.Patches.MechanicalLens = true;
		IsEnabled = true;
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.MechanicalLens = false;
		IsEnabled = false;
	}
}

public class BuiltIn_5 : FunctionCategory
{
	public override string Category => "Builder";
	public BuiltIn_5()
	{
		this["zh"] = "建筑";
		this["en"] = "Builder"; 
		Add<SuperRange>();
		Add<FastTileAndWallPlacingSpeed>();
		Add<MachanicalRuler>();
		Add<MachanicalLens>();
	}
}
