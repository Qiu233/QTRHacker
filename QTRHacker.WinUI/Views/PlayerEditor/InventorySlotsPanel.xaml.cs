using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.Controls;
using QTRHacker.ViewModels.Common;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Views.PlayerEditor;

public sealed partial class InventorySlotsPanel : UserControl
{
	public InventorySlotsPanelViewModel ViewModel => (InventorySlotsPanelViewModel)DataContext;
	public InventorySlotsPanel()
	{
		this.InitializeComponent();
	}

	private void ItemSlot_Checked(object sender, RoutedEventArgs e)
	{
		if (sender is not ItemSlot ic || ic.DataContext is not ItemSlotViewModel slot)
			return;
		ViewModel.OnItemSelected(slot);
    }
}
