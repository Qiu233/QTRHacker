using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Scripts;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using QTRHacker.Core.GameObjects.Terraria;
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using static QTRHacker.Scripts.ScriptHelper;

public abstract class EventFunction : BaseFunction
{
	public override bool CanDisable => false;
	public override void Disable(GameContext ctx)
	{
		throw new NotImplementedException();
	}
}

public class ToggleDayNight : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "切换昼夜";
				break;
			case "en":
			default:
				Name = "Toggle Day and Night";
				break;
		}
	}
	public override void Enable(GameContext ctx) => ctx.DayTime = !ctx.DayTime;
}

public class ToggleSunDial : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "开/关 日晷";
				break;
			case "en":
			default:
				Name = "Enable/Disable Sundial";
				break;
		}
	}
	public override void Enable(GameContext ctx) => ctx.FastForwardTime = !ctx.FastForwardTime;
}

public class ToggleBloodMoon : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "开/关 血月";
				break;
			case "en":
			default:
				Name = "Enable/Disable Blood Moon";
				break;
		}
	}
	public override void Enable(GameContext ctx) => ctx.BloodMoon = !ctx.BloodMoon;
}

public class ToggleEclipse : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "开/关 日食";
				break;
			case "en":
			default:
				Name = "Enable/Disable Eclipse";
				break;
		}
	}
	public override void Enable(GameContext ctx) => ctx.Eclipse = !ctx.Eclipse;
}

public class ToggleSnowMoon : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "开/关 霜月";
				break;
			case "en":
			default:
				Name = "Enable/Disable Snow Moon";
				break;
		}
	}
	public override void Enable(GameContext ctx) => ctx.SnowMoon = !ctx.SnowMoon;
}

public class TogglePumpkinMoon : EventFunction
{
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "开/关 南瓜月";
				break;
			case "en":
			default:
				Name = "Enable/Disable Pumpkin Moon";
				break;
		}
	}
	public override void Enable(GameContext ctx) => ctx.PumpkinMoon = !ctx.PumpkinMoon;
}

FunctionCategory category = new FunctionCategory("Events");

category["zh"] = "事件";
category["en"] = "Events";

category.Add<ToggleDayNight>();
category.Add<ToggleSunDial>();
category.Add<ToggleBloodMoon>();
category.Add<ToggleEclipse>();
category.Add<ToggleSnowMoon>();
category.Add<TogglePumpkinMoon>();

return category;