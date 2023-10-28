using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Helpers;
using QTRHacker.Models;
using QTRHacker.Models.Wiki;
using System.Collections.ObjectModel;

namespace QTRHacker.ViewModels.Wiki.Items;

public class ItemStackInfo
{
	public ItemInfo ItemInfo { get; }
	public int Stack { get; }

	public ItemStackInfo(ItemInfo itemInfo, int stack)
	{
		ItemInfo = itemInfo;
		Stack = stack;
	}
}

public partial class RecipeFromInfo : ObservableObject
{
	[ObservableProperty]
	private ItemStackInfo? selectedRecipeFromItem;

	public string Tab { get; }
	public TaskCompletionNotifier<ObservableCollection<ItemStackInfo>> RequiredItemsNotifier { get; }

	private static async Task<ObservableCollection<ItemStackInfo>> LoadItems(RecipeData data)
	{
		ObservableCollection<ItemStackInfo> collection = new();
		foreach (var item in data.RequiredItems!)
		{
			if (item.Type == 0)
				continue;
			ItemInfo info = await ItemInfo.Create(item.Type);
			collection.Add(new ItemStackInfo(info, item.Stack));
		}
		return collection;
	}

	public RecipeFromInfo(string tab, RecipeData data)
	{
		Tab = tab;
		RequiredItemsNotifier = new TaskCompletionNotifier<ObservableCollection<ItemStackInfo>>(LoadItems(data));
	}
}
