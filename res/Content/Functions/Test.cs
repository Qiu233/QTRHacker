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
	public override FunctionResult Enable(GameContext ctx)
	{
		int off = ScriptHelper.GetOffset(ctx, "Terraria.Player", "statLife");
		ScriptHelper.AobReplaceASM(ctx, $"sub [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"add [edx+{off}],eax");
		return FunctionResult.SUCCESS;
	}
	public override FunctionResult Disable(GameContext ctx)
	{
		int off = ScriptHelper.GetOffset(ctx, "Terraria.Player", "statLife");
		ScriptHelper.AobReplaceASM(ctx, $"add [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"sub [edx+{off}],eax");
		return FunctionResult.SUCCESS;
	}
}

return new Test();