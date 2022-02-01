using QTRHacker.Controls;
using QTRHacker.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QTRHacker.Views.PlayerEditor
{
	public class ItemSlotsPanel : UserControl
	{
		public ISlotsLayout SlotsLayout
		{
			get => (ISlotsLayout)GetValue(SlotsLayoutProperty);
			set => SetValue(SlotsLayoutProperty, value);
		}
		public static readonly DependencyProperty SlotsLayoutProperty =
			DependencyProperty.Register(nameof(SlotsLayout), typeof(ISlotsLayout), typeof(ItemSlotsPanel), new PropertyMetadata(OnSlotsLayoutChanged));

		public int ItemSlotWidth
		{
			get => (int)GetValue(ItemSlotWidthProperty);
			set => SetValue(ItemSlotWidthProperty, value);
		}
		public static readonly DependencyProperty ItemSlotWidthProperty =
			DependencyProperty.Register(nameof(ItemSlotWidth), typeof(int), typeof(ItemSlotsPanel), new PropertyMetadata(50));

		public int ItemSlotMargin
		{
			get => (int)GetValue(ItemSlotMarginProperty);
			set => SetValue(ItemSlotMarginProperty, value);
		}
		public static readonly DependencyProperty ItemSlotMarginProperty =
			DependencyProperty.Register(nameof(ItemSlotMargin), typeof(int), typeof(ItemSlotsPanel), new PropertyMetadata(2));

		public int SelectedIndex
		{
			get => (int)GetValue(SelectedIndexProperty);
			set => SetValue(SelectedIndexProperty, value);
		}
		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(ItemSlotsPanel), new PropertyMetadata(-1, OnSelectedIndexChanged));

		private static void OnSelectedIndexChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			ItemSlotsPanel panel = obj as ItemSlotsPanel;
			if (panel.Slots.Count == 0)
				return;
			var slot = panel.Slots[panel.SelectedIndex % panel.Slots.Count];
			slot.IsChecked = true;
		}

		public readonly Grid MainGrid;

		public readonly List<ItemSlot> Slots = new();

		private static void OnSlotsLayoutChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			ItemSlotsPanel panel = obj as ItemSlotsPanel;
			lock (panel.Slots)
			{
				panel.Slots.Clear();
				panel.MainGrid.Children.Clear();
				panel.MainGrid.ColumnDefinitions.Clear();
				panel.MainGrid.RowDefinitions.Clear();
				if (args.NewValue is not ISlotsLayout layout)
					return;
				List<(int Column, int Row)> slots = new();
				for (int i = 0; i < layout.Slots; i++)
					slots.Add(layout.GetSlotLocation(i));
				int columns = slots.Max(t => t.Column) + 1;
				int rows = slots.Max(t => t.Row) + 1;
				for (int i = 0; i < columns; i++)
					panel.MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
				for (int i = 0; i < rows; i++)
					panel.MainGrid.RowDefinitions.Add(new RowDefinition());
				for (int i = 0; i < slots.Count; i++)
				{
					ItemSlot slot = new();
					int copy = i;
					slot.Checked += (s, e) =>
					{
						if (panel.SelectedIndex != copy)
							panel.SelectedIndex = copy;
					};
					panel.Slots.Add(slot);
					panel.MainGrid.Children.Add(slot);

					BindingOperations.SetBinding(slot, WidthProperty,
						new Binding(nameof(ItemSlotWidth)) { Source = panel });
					BindingOperations.SetBinding(slot, HeightProperty,
						new Binding(nameof(ItemSlotWidth)) { Source = panel });
					BindingOperations.SetBinding(slot, MarginProperty,
						new Binding(nameof(ItemSlotMargin)) { Source = panel });

					Grid.SetColumn(slot, slots[i].Column);
					Grid.SetRow(slot, slots[i].Row);
				}
			}
		}

		public ItemSlotsPanel()
		{
			Padding = new Thickness(1, 3, 1, 3);
			var border = new Border
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Background = new SolidColorBrush(Colors.Transparent),
			};
			AddChild(border);
			MainGrid = new Grid();
			border.Child = MainGrid;
		}


	}
}
