using QTRHacker.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QTRHacker.Controls
{
	public class ItemSlot : RadioButton
	{
		public int ItemType
		{
			get => (int)GetValue(ItemTypeProperty);
			set => SetValue(ItemTypeProperty, value);
		}
		public static readonly DependencyProperty ItemTypeProperty =
			DependencyProperty.Register(nameof(ItemType), typeof(int), typeof(ItemSlot), new PropertyMetadata(0));

		public int ItemStack
		{
			get => (int)GetValue(ItemStackProperty);
			set => SetValue(ItemStackProperty, value);
		}
		public static readonly DependencyProperty ItemStackProperty =
			DependencyProperty.Register(nameof(ItemStack), typeof(int), typeof(ItemSlot), new PropertyMetadata(0));

		public Color TintColor
		{
			get => (Color)GetValue(TintColorProperty);
			set => SetValue(TintColorProperty, value);
		}
		public static readonly DependencyProperty TintColorProperty =
			DependencyProperty.Register(nameof(TintColor), typeof(Color), typeof(ItemSlot), new PropertyMetadata(Colors.White));

		public ItemSlot()
		{
		}
		static ItemSlot()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemSlot), new FrameworkPropertyMetadata(typeof(ItemSlot)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}
	}
}
