using QTRHacker.Functions.GameObjects.Terraria;
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
	public class OnItemDataUpdatingEventArgs : EventArgs
	{
		public int Index { get; }
		public Item Item { get; set; }
		public OnItemDataUpdatingEventArgs(int index)
		{
			Index = index;
		}
	}
	/// <summary>
	/// ItemSlotsEditor.xaml 的交互逻辑
	/// </summary>
	public partial class ItemSlotsEditor : UserControl
	{
		public ISlotsLayout SlotsLayout
		{
			get => (ISlotsLayout)GetValue(SlotsLayoutProperty);
			set => SetValue(SlotsLayoutProperty, value);
		}
		public static readonly DependencyProperty SlotsLayoutProperty =
			DependencyProperty.Register(nameof(SlotsLayout), typeof(ISlotsLayout), typeof(ItemSlotsEditor));

		public bool Updating
		{
			get => (bool)GetValue(UpdatingProperty);
			set => SetValue(UpdatingProperty, value);
		}
		public static readonly DependencyProperty UpdatingProperty =
			DependencyProperty.Register(nameof(Updating), typeof(bool), typeof(ItemSlotsEditor));

		public event EventHandler<OnItemDataUpdatingEventArgs> OnItemDataFetching;

		public virtual void UpdateItemSlot(int index)
		{
			if (index < 0 || index >= SlotsLayout.Slots)
				return;
			OnItemDataUpdatingEventArgs args = new(index);
			OnItemDataFetching?.Invoke(this, args);
			var slot = ItemSlotsPanel.Slots[index];
			Item item = args.Item;
			if (item == null)
				return;
			slot.ItemType = item.Type;
			slot.ItemStack = item.Stack;
		}

		public void UpdateItemSlots()
		{
			int slots = SlotsLayout.Slots;
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
	}
}
