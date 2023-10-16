using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor;

public partial class SelectedItemHolder : ObservableObject
{
	public event EventHandler<Item?>? SelectedItemChanged;
	private Item? selectedItem;
	public Item? SelectedItem
	{
		get => selectedItem;
		set
		{
			if (SetProperty(ref selectedItem, value))
				SelectedItemChanged?.Invoke(this, value);
		}
	}
}

public partial class InventoryEditorViewModel : ObservableObject
{
	public InventorySlotsPanelViewModel InventorySlotsPanelViewModel { get; }
	public ItemPropertiesPanelViewModel ItemPropertiesPanelViewModel { get; }
	public SelectedItemHolder SelectedItemHolder { get; }
	public InventoryEditorViewModel(InventorySlotsPanelViewModel slotsVM, ItemPropertiesPanelViewModel propsVM, SelectedItemHolder holder)
	{
		InventorySlotsPanelViewModel = slotsVM;
		ItemPropertiesPanelViewModel = propsVM;
		SelectedItemHolder = holder;
	}

	[RelayCommand]
	public async Task Initialize()
	{
		if (SelectedItemHolder.SelectedItem is not Item item)
			return;
		await Task.Run(() =>
		{
			var prefix = item.Prefix;
			item.SetDefaults(item.Type);
			item.SetPrefix(prefix);
		});
		await ItemPropertiesPanelViewModel.InvokeUpdate();
	}

}
