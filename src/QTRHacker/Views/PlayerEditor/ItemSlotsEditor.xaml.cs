using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QTRHacker.Views.PlayerEditor
{
	/// <summary>
	/// ItemSlotsEditor.xaml 的交互逻辑
	/// </summary>
	public partial class ItemSlotsEditor : UserControl
	{
		public ItemSlotsEditorViewModel ViewModel => DataContext as ItemSlotsEditorViewModel;
		public bool Updating
		{
			get => (bool)GetValue(UpdatingProperty);
			set => SetValue(UpdatingProperty, value);
		}
		public static readonly DependencyProperty UpdatingProperty =
			DependencyProperty.Register(nameof(Updating), typeof(bool), typeof(ItemSlotsEditor));

		public virtual void UpdateItemSlot(int index)
		{
			if (index < 0 || index >= ViewModel.SlotsLayout.Slots)
				return;
			var slot = ItemSlotsPanel.Slots[index];
			Item item = ViewModel.ItemProvider(index);
			if (item == null)
				return;
			slot.ItemType = item.Type;
			slot.ItemStack = item.Stack;
		}

		public void UpdateItemSlots()
		{
			int slots = ViewModel.SlotsLayout.Slots;
			for (int i = 0; i < slots; i++)
			{
				UpdateItemSlot(i);
			}
		}

		public void UpdateAll()
		{
			if (!Updating)
				return;
			UpdateItemSlots();
		}

		public ItemSlotsEditor()
		{
			InitializeComponent();
		}

		private void ApplyButton_Click(object sender, RoutedEventArgs e)
		{
			ItemPropertiesPanel.ViewModel.UpdatePropertiesToItem(ViewModel.SelectedItem);
		}

		private void InitButton_Click(object sender, RoutedEventArgs e)
		{
			int type = (int)ItemPropertiesPanel.ViewModel.GetValue("Type");
			int stack = (int)ItemPropertiesPanel.ViewModel.GetValue("Stack");
			stack = stack == 0 ? 1 : stack;
			byte prefix = (byte)ItemPropertiesPanel.ViewModel.GetValue("Prefix");
			var item = ViewModel.SelectedItem;
			item.SetDefaultsAndPrefix(type, prefix);
			item.Stack = stack;
		}
	}
}
