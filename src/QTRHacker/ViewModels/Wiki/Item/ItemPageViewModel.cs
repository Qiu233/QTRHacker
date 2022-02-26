using QTRHacker.Assets;
using QTRHacker.Commands;
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

	private readonly RelayCommand addOneCommand;
	private readonly RelayCommand addMaxCommand;
	public RelayCommand AddOneCommand => addOneCommand;
	public RelayCommand AddMaxCommand => addMaxCommand;

	public ItemPageViewModel()
	{
		addOneCommand = new RelayCommand(o => HackGlobal.IsActive, o => AddSelectedItemToGame_One());
		addMaxCommand = new RelayCommand(o => HackGlobal.IsActive, o => AddSelectedItemToGame_Max());

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

	public void AddSelectedItemToGame(int stack)
	{
		if (!HackGlobal.IsActive)
			return;
		var ctx = HackGlobal.GameContext;
		if (SelectedItemInfo == null)
			return;
		int id = SelectedItemInfo.Type;
		if (id == 0)
			return;
		var pos = ctx.MyPlayer.Position;
		int num = Functions.GameObjects.Terraria.Item.NewItem(ctx, (int)pos.X, (int)pos.Y, 0, 0, id, stack, false, 0, true);
		Functions.GameObjects.Terraria.NetMessage.SendData(ctx, 21, -1, -1, 0, num, 0, 0, 0, 0, 0, 0);
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
