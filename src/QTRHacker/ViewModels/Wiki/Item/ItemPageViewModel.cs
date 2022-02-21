using QTRHacker.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemPageViewModel : ViewModelBase
{
	private ItemInfo selectedItemInfo;

	public ObservableCollection<ItemInfo> Items { get; } = new();

	public ItemInfo SelectedItemInfo
	{
		get => selectedItemInfo;
		set
		{
			selectedItemInfo = value;
			OnPropertyChanged(nameof(SelectedItemInfo));
			SelectedItemInfoChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	public event EventHandler SelectedItemInfoChanged;
	public ItemInfoPagesViewModel ItemInfoPagesViewModel { get; }

	public ItemPageViewModel()
	{
		ItemInfoPagesViewModel = new ItemInfoPagesViewModel();
		ItemInfoPagesViewModel.FilterResumed += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.CategoryFilters.CollectionChanged += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.KeywordChanged += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.ItemCategoryFilterSelectedChanged += (s, e) => UpdateFilter();
		SelectedItemInfoChanged += ItemPageViewModel_SelectedItemInfoChanged;

		// this loop begins from 1 instead of 0, because 0 is ItemName.None
		for (int i = 1; i < WikiResLoader.ItemDatum.Count; i++)
			Items.Add(new ItemInfo(i));
	}

	private void ItemPageViewModel_SelectedItemInfoChanged(object sender, EventArgs e)
	{
		ItemInfoPagesViewModel.ItemInfo = SelectedItemInfo;
		ItemInfoPagesViewModel.SelectedRecipeFrom = 0;
	}

	private void UpdateFilter()
	{
		if (ItemInfoPagesViewModel.IsFilterSuspended)
			return;
		Items.Clear();
		var kw = ItemInfoPagesViewModel.Keyword;
		for (int i = 1; i < WikiResLoader.ItemDatum.Count; i++)
		{
			var item = new ItemInfo(i);
			if (!item.Name.Contains(kw, StringComparison.OrdinalIgnoreCase) &&
				!item.Tooltip.Contains(kw, StringComparison.OrdinalIgnoreCase))
				continue;
			var cate = item.GetItemCategory();
			var flags = ItemInfoPagesViewModel.CategoryFilters
				.Where(t => t.IsSelected)
				.Select(t => (t.Category != ItemCategory.Others && cate.HasFlag(t.Category)) ||
							(t.Category == ItemCategory.Others && cate == ItemCategory.Others))
				.ToList();
			if (!flags.Any(t => t))
				continue;
			Items.Add(item);
		}
	}
}
