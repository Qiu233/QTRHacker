using QTRHacker.Core;

namespace QTRHacker.Scripts.Functions;
public abstract class EventFunction : BaseFunction
{
	public override bool CanDisable => false;
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
}

public class ToggleDayNight : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "切换昼夜",
			_ => "Toggle Day and Night",
		};
	}
	public override void Enable(GameContext ctx) => ctx.DayTime = !ctx.DayTime;
}

public class ToggleSunDial : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "开/关 日晷",
			_ => "Enable/Disable Sundial",
		};
	}
	public override void Enable(GameContext ctx) => ctx.FastForwardTime = !ctx.FastForwardTime;
}

public class ToggleBloodMoon : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "开/关 血月",
			_ => "Enable/Disable Blood Moon",
		};
	}
	public override void Enable(GameContext ctx) => ctx.BloodMoon = !ctx.BloodMoon;
}

public class ToggleEclipse : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "开/关 日食",
			_ => "Enable/Disable Eclipse",
		};
	}
	public override void Enable(GameContext ctx) => ctx.Eclipse = !ctx.Eclipse;
}

public class ToggleSnowMoon : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "开/关 霜月",
			_ => "Enable/Disable Snow Moon",
		};
	}
	public override void Enable(GameContext ctx) => ctx.SnowMoon = !ctx.SnowMoon;
}

public class TogglePumpkinMoon : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "开/关 南瓜月",
			_ => "Enable/Disable Pumpkin Moon",
		};
	}
	public override void Enable(GameContext ctx) => ctx.PumpkinMoon = !ctx.PumpkinMoon;
}


public class BuiltIn_4 : FunctionCategory
{
	public override string Category => "Events";
	public BuiltIn_4()
	{
		this["zh"] = "事件";
		this["en"] = "Events";
		Add<ToggleDayNight>();
		Add<ToggleSunDial>();
		Add<ToggleBloodMoon>();
		Add<ToggleEclipse>();
		Add<ToggleSnowMoon>();
		Add<TogglePumpkinMoon>();
	}
}