using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using static QTRHacker.Scripts.ScriptHelper;

namespace QTRHacker.Scripts.Functions;
public class BurnAllNPCs : BaseFunction
{
	public override bool CanDisable => false;
	public override bool HasProgress => true;
	public override void ApplyLocalization(string culture)
	{
		(Name, Tooltip) = culture switch
		{
			"zh" => ("燃烧所有NPC", "包括怪物和城镇/友好NPC"),
			_ => ("Burn All NPCs", "Including mobs and town/friendly npcs"),
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		var npc = ctx.NPC;
		int max = npc.Length;
		for (int i = 0; i < max; i++)
		{
			if (npc[i].Active)
				npc[i].AddBuff(153, 216000);
			Progress = ((double)i / max) * 100;
		}
	}
}

public class BurnAllPlayers : BaseFunction
{
	public override bool CanDisable => false;
	public override bool HasProgress => true;
	public override void ApplyLocalization(string culture)
	{
		(Name, Tooltip) = culture switch
		{
			"zh" => ("燃烧所有玩家", "包括自己"),
			_ => ("Burn All Players", "Including my player also"),
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		var player = ctx.Players;
		int max = player.Length;
		for (int i = 0; i < max; i++)
		{
			if (player[i].Active)
				player[i].AddBuff(44, 216000);
			Progress = ((double)i / max) * 100;
		}
	}
}

public class RevealTheWholeMap : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		(Name, Tooltip) = culture switch
		{
			"zh" => ("揭示整张地图", "有点慢，请稍等"),
			_ => ("Reveal The Whole Map", "A bit slow, please wait"),
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		ctx.Patches.RevealTheWholeMap();
		ctx.RefreshMap = true;
	}
}

public class RightClickToTP : BaseFunction
{
	private const string HookName = "RightClickToTP";
	public override bool CanDisable => true;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "大地图右键传送",
			_ => "Right Click on Map to TP",
		};
	}
	public override void Disable(GameContext ctx)
	{
		ctx.Patches.RightClickToTP = false;
		IsEnabled = false;
	}
	public override void Enable(GameContext ctx)
	{
		ctx.Patches.RightClickToTP = true;
		IsEnabled = true;
	}
}

public class RandomizeUUID : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		Name = culture switch
		{
			"zh" => "随机UUID",
			_ => "Randomize UUID",
		};
	}
	public override void Disable(GameContext ctx)
	{
		throw new InvalidOperationException();
	}
	public override void Enable(GameContext ctx)
	{
		ctx.UUID = Guid.NewGuid().ToString();
	}
}


public class BuiltIn_3 : FunctionCategory
{
	public override string Category => "Advanced";
	public BuiltIn_3()
	{
		this["zh"] = "高级";
		this["en"] = "Advanced";
		Add<BurnAllNPCs>();
		Add<BurnAllPlayers>();
		Add<RandomizeUUID>();
		Add<RightClickToTP>();
		Add<RevealTheWholeMap>();
	}
}
