using QTRHacker.Models.Wiki;
using System.Collections.ObjectModel;

namespace QTRHacker.ViewModels.Wiki.Item
{
	public class RecipeFromInfo : ViewModelBase
	{
		public string Tab { get; }
		public ObservableCollection<ItemStackInfo> RequiredItems { get; } = new();

		public RecipeFromInfo(string tab)
		{
			Tab = tab;
		}
		public RecipeFromInfo(string tab, RecipeData data)
		{
			Tab = tab;
			foreach (var item in data.RequiredItems)
			{
				if (item.Type == 0)
					continue;
				RequiredItems.Add(new ItemStackInfo(item));
			}
		}
	}
}
