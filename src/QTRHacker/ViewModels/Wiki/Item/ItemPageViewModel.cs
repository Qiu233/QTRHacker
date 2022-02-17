using QTRHacker.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemPageViewModel : ViewModelBase
{
	private ItemInfoSubPageViewModel itemInfoSubPageViewModel;
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

	public ItemInfoSubPageViewModel ItemInfoSubPageViewModel => itemInfoSubPageViewModel;

	public ItemPageViewModel()
	{
		SelectedItemInfoChanged += ItemPageViewModel_SelectedItemInfoChanged;
		for (int i = 0; i < WikiResLoader.ItemDatum.Count; i++)
			Items.Add(new ItemInfo(i));
	}

	private void ItemPageViewModel_SelectedItemInfoChanged(object sender, EventArgs e)
	{
		itemInfoSubPageViewModel = new ItemInfoSubPageViewModel(SelectedItemInfo);
		OnPropertyChanged(nameof(ItemInfoSubPageViewModel));
	}
}
