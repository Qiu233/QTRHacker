using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects.Terraria;
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
	private int selectedItemIndex;

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

	public int SelectedItemIndex
	{
		get => selectedItemIndex;
		set
		{
			selectedItemIndex = value;
			OnPropertyChanged(nameof(SelectedItemIndex));
		}
	}

	public event EventHandler SelectedItemInfoChanged;
	public ItemInfoPagesViewModel ItemInfoPagesViewModel { get; }

	private readonly RelayCommand addOneCommand;
	private readonly RelayCommand addMaxCommand;
	public RelayCommand AddOneCommand => addOneCommand;
	public RelayCommand AddMaxCommand => addMaxCommand;

	public ItemPageViewModel()
	{
		addOneCommand = new HackCommand(o => AddSelectedItemToGame_One());
		addMaxCommand = new HackCommand(o => AddSelectedItemToGame_Max());

		ItemInfoPagesViewModel = new ItemInfoPagesViewModel();
		ItemInfoPagesViewModel.FilterResumed += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.CategoryFilters.CollectionChanged += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.KeywordChanged += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.ItemCategoryFilterSelectedChanged += (s, e) => UpdateFilter();
		ItemInfoPagesViewModel.JumpToItem += ItemInfoPagesViewModel_JumpToItem;
		SelectedItemInfoChanged += ItemPageViewModel_SelectedItemInfoChanged;

		// this loop begins from 1 instead of 0, because 0 is ItemName.None
		for (int i = 1; i < WikiResLoader.ItemDatum.Count; i++)
			Items.Add(new ItemInfo(i));
	}

	private void ItemInfoPagesViewModel_JumpToItem(object sender, JumpToItemEventArgs e)
	{
		if (e == null || e.ItemInfo == null)
			return;
		var type = e.ItemInfo.Type;
		var item = Items.FirstOrDefault(t => t.Type == type);
		int index = Items.IndexOf(item);
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
