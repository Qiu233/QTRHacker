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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Views.PlayerEditor;

public sealed partial class InventorySlotsPanel : UserControl
{
	public ItemSlotsGridViewModel ViewModel => (ItemSlotsGridViewModel)DataContext;
	public InventorySlotsPanel()
	{
		this.InitializeComponent();
		this.DataContext = new ItemSlotsGridViewModel();
		SetupSlots();

	}

	private void SetupSlots()
	{
		string groupName = "Items@" + this.GetHashCode();
		for (int i = 0; i < 50; i++)
		{
			ItemSlot slot = new();
			slot.GroupName = groupName;
			slot.Width = slot.Height = 48;
			InvGrid.Children.Add(slot);
		}
		for (int i = 0; i < 8; i++)
		{
			ItemSlot slot = new();
			slot.GroupName = groupName;
			slot.Width = slot.Height = 36;
			InvSubGrid.Children.Add(slot);
		}
		TrashItem.GroupName = groupName;
		TrashItem.Width = TrashItem.Height = 48;
		for (int i = 0; i < 30; i++)
		{
			ItemSlot slot = new();
			slot.GroupName = groupName;
			slot.Width = slot.Height = 48;
			ArmorsGrid.Children.Add(slot);
		}
		for (int i = 0; i < 10; i++)
		{
			ItemSlot slot = new();
			slot.GroupName = groupName;
			slot.Width = slot.Height = 48;
			MiscsGrid.Children.Add(slot);
		}
		for (int i = 0; i < 40; i++)
		{
			ItemSlot slot = new();
			slot.GroupName = groupName;
			slot.Width = slot.Height = 48;
			PiggyGrid.Children.Add(slot);
		}

	}
}
