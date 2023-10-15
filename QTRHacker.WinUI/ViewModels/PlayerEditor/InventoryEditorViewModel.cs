using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor;

public partial class InventoryEditorViewModel : ObservableObject
{
	public InventorySlotsPanelViewModel InventorySlotsPanelViewModel { get; }
	public ItemPropertiesPanelViewModel ItemPropertiesPanelViewModel { get; }
	public InventoryEditorViewModel(InventorySlotsPanelViewModel slotsVM, ItemPropertiesPanelViewModel propsVM)
	{
		InventorySlotsPanelViewModel = slotsVM;
		ItemPropertiesPanelViewModel = propsVM;

		InventorySlotsPanelViewModel.ItemSelected += InventorySlotsPanelViewModel_ItemSelected;
	}

	private async void InventorySlotsPanelViewModel_ItemSelected(object? sender, ItemSlotViewModel e)
	{
		ItemPropertiesPanelViewModel.TargetItem = await InventorySlotsPanelViewModel.GetItemBySlotViewModel(e);
	}
}
