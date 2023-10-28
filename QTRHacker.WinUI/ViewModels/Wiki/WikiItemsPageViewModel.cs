using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Assets;
using QTRHacker.ViewModels.Wiki.Items;
using StrongInject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace QTRHacker.ViewModels.Wiki;

public partial class WikiItemsPageViewModel : ObservableObject
{
	public ObservableCollection<ItemInfo> Items { get; } = new();

	private ItemInfo? selectedItem;
	public ItemInfo? SelectedItem
	{
		get => selectedItem;
		set
		{
			if (SetProperty(ref selectedItem, value))
				SelectedItemChanged?.Invoke(this, value);
		}
	}

	public event TypedEventHandler<WikiItemsPageViewModel, ItemInfo?>? SelectedItemChanged;

	[ObservableProperty]
	private RecipeFromInfo? selectedRecipeFrom;

	public WikiItemsPageViewModel()
	{
		_ = InitItems();
		SelectedItemChanged += WikiItemsPageViewModel_SelectedItemChanged;
	}

	private async void WikiItemsPageViewModel_SelectedItemChanged(WikiItemsPageViewModel sender, ItemInfo? args)
	{
		if (args is null)
		{
			SelectedRecipeFrom = null;
			return;
		}
		await args.LoadRecipes();
		SelectedRecipeFrom = args.RecipeFroms.FirstOrDefault();
	}

	private async Task InitItems()
	{
		var datum = await WikiResLoader.ItemDatum;

		for (int i = 1; i < datum.Count; i++)
		{
			Items.Add(await ItemInfo.Create(i));
		}
	}
}
