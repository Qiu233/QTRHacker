using QTRHacker.Scripts;
using QTRHacker.Functions;
using System.Globalization;

class Test : BaseFunction
{
	public override string Key => "Infinite_Life";
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		switch(culture)
		{
			case "zh-CN":
				Name = "无限生命";
				Category = "基础";
				Tooltip = "免疫大部分伤害";
				break;
			case "en-US":
			default:
				Name = "Infinite Life";
				Category = "Basic";
				Tooltip = "Immune to most damages except continuous ones like burning";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
	}
	public override void Disable(GameContext ctx)
	{
	}
}

return new Test();