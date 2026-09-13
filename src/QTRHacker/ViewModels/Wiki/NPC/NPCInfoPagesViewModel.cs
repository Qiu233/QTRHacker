namespace QTRHacker.ViewModels.Wiki.NPC;

public class NPCInfoPagesViewModel : WikiFilterViewModel<NPCCategoryFilter>
{
	private NPCInfo npcInfo;

	public NPCInfo NPCInfo
	{
		get => npcInfo;
		set => SetProperty(ref npcInfo, value);
	}

	public NPCInfoPagesViewModel()
		: base(Enum.GetValues<NPCCategory>().Select(category => new NPCCategoryFilter(category)))
	{
	}
}
