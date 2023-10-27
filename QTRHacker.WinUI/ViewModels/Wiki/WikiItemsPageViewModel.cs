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

namespace QTRHacker.ViewModels.Wiki;

public partial class WikiItemsPageViewModel : ObservableObject
{
	public ObservableCollection<ItemInfo> Items { get; } = new();

	[ObservableProperty]
	private ItemInfo? selectedItem;
	public WikiItemsPageViewModel()
	{
		_ = InitItems();
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
