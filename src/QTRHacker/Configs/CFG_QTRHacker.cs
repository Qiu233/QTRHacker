namespace QTRHacker.Configs;

public sealed class CFG_QTRHacker
{
	public int ItemUpdateInterval = 200;//milliseconds
	public int PlayersListUpdateInterval = 500;
	public int SchesUpdateInterval = 500;
	public bool ForceEnglish = false;
	public string Theme = "Dark";
	public string Language; // A missing value is migrated from ForceEnglish when loading older configs.
}
