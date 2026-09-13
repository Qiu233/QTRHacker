using QTRHacker.Core;

namespace QTRHacker.Scripts.Functions;
public class SuperRange : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.SuperRange;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "超远距离",
			_ => "Super range",
		};
	}
}

public class FastTileAndWallPlacingSpeed : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.FastTileAndWallPlacingSpeed;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "加快方块/墙壁放置速度",
			_ => "Super Fast Tile/Wall Placing Speed",
		};
	}
}


public class MachanicalRuler : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.MachanicalRuler;
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
}

public class MachanicalLens : GameplayFeatureFunction
{
	protected override GameplayFeature Feature => GameplayFeature.MachanicalLens;
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
