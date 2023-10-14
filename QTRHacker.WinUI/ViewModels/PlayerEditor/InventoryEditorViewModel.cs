using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor;

public class InventoryEditorViewModel : ObservableObject
{
	public InventorySlotsPanelViewModel InventorySlotsPanelViewModel { get; }
	public ItemPropertiesPanelViewModel ItemPropertiesPanelViewModel { get; }
	public InventoryEditorViewModel(InventorySlotsPanelViewModel slotsVM, ItemPropertiesPanelViewModel propsVM)
	{
		InventorySlotsPanelViewModel = slotsVM;
		ItemPropertiesPanelViewModel = propsVM;
	}
}
