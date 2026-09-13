namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemCategoryFilter : CategoryFilterViewModel
{
	public ItemCategory Category { get; }

	public ItemCategoryFilter(ItemCategory category) : base($"UI.ItemCategories.{category}")
	{
		Category = category;
	}
}
