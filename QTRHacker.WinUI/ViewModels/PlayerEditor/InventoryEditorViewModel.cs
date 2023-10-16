using CommunityToolkit.Mvvm.ComponentModel;
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
	public InventoryEditorViewModel(InventorySlotsPanelViewModel slotsVM, ItemPropertiesPanelViewModel propsVM)
	{
		InventorySlotsPanelViewModel = slotsVM;
		ItemPropertiesPanelViewModel = propsVM;
	}
}
