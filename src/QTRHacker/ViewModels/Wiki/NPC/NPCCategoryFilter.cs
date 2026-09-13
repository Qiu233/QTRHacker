namespace QTRHacker.ViewModels.Wiki.NPC;

public class NPCCategoryFilter : CategoryFilterViewModel
{
	public NPCCategory Category { get; }

	public NPCCategoryFilter(NPCCategory category) : base($"UI.NPCCategories.{category}")
	{
		Category = category;
	}
}
