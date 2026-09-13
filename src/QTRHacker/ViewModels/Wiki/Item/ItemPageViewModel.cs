using System.Collections.ObjectModel;
using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Core;

namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemPageViewModel : ViewModelBase
{
	private ItemInfo selectedItemInfo;
	private int selectedItemIndex = -1;

	public ObservableCollection<ItemInfo> Items { get; } = new();

	public ItemInfo SelectedItemInfo
	{
		get => selectedItemInfo;
		set
		{
			if (SetProperty(ref selectedItemInfo, value))
				SelectedItemInfoChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	public int SelectedItemIndex
	{
		get => selectedItemIndex;
		set => SetProperty(ref selectedItemIndex, value);
	}

	public event EventHandler SelectedItemInfoChanged;
	public ItemInfoPagesViewModel ItemInfoPagesViewModel { get; }

	public RelayCommand AddOneCommand { get; }
	public RelayCommand AddMaxCommand { get; }

	public ItemPageViewModel()
	{
		AddOneCommand = new HackCommand(o => AddSelectedItemToGame_One());
		AddMaxCommand = new HackCommand(o => AddSelectedItemToGame_Max());

		ItemInfoPagesViewModel = new ItemInfoPagesViewModel();
		ItemInfoPagesViewModel.FilterChanged += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.JumpToItem += ItemInfoPagesViewModel_JumpToItem;
		SelectedItemInfoChanged += ItemPageViewModel_SelectedItemInfoChanged;

		// this loop begins from 1 instead of 0, because 0 is ItemName.None
		for (int i = 1; i < WikiResLoader.ItemDatum.Count; i++)
			Items.Add(new ItemInfo(i));
	}

	public void SetSelectedItemType(int type)
	{
		var index = Items.ToList().FindIndex(t => t.Type == type);
		if (index == -1)
			SelectedItemIndex = 0;
		else
			SelectedItemIndex = index;
	}

	private void ItemInfoPagesViewModel_JumpToItem(object sender, JumpToItemEventArgs e)
	{
		if (e == null || e.ItemInfo == null)
			return;
		var type = e.ItemInfo.Type;
		var index = Items.ToList().FindIndex(t => t.Type == type);
		if (index == -1)
			SelectedItemInfo = e.ItemInfo;
		else
			SelectedItemIndex = index;
	}

	public void AddSelectedItemToGame(int stack)
	{
		if (!HackGlobal.IsActive)
			return;
		var ctx = HackGlobal.GameContext;
		if (SelectedItemInfo == null)
			return;
		int type = SelectedItemInfo.Type;
		if (type <= 0)//ignoring the negative-indexed legacy items
			return;
		ctx.AddItemStackToInv(type, stack);
	}

	public void AddSelectedItemToGame_Max()
	{
		if (SelectedItemInfo == null)
			return;
		AddSelectedItemToGame(SelectedItemInfo.Data.MaxStack);
	}
	public void AddSelectedItemToGame_One()
	{
		if (SelectedItemInfo == null)
			return;
		AddSelectedItemToGame(1);
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
			bool matchesCategory = ItemInfoPagesViewModel.CategoryFilters.Any(filter =>
				filter.IsSelected && (filter.Category == ItemCategory.Others
					? cate == ItemCategory.Others
					: cate.HasFlag(filter.Category)));
			if (!matchesCategory)
				continue;
			Items.Add(item);
		}
	}
}
