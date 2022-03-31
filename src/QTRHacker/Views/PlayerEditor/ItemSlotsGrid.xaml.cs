using QTRHacker.ViewModels.PlayerEditor;
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
	/// ItemSlotsGrid.xaml 的交互逻辑
	/// </summary>
	public partial class ItemSlotsGrid : UserControl
	{
		public static readonly DependencyProperty ItemContextMenuProperty =
			DependencyProperty.Register(nameof(ItemContextMenu), typeof(ContextMenu), typeof(ItemSlotsGrid));

		public ContextMenu ItemContextMenu
		{
			get => (ContextMenu)GetValue(ItemContextMenuProperty);
			set => SetValue(ItemContextMenuProperty, value);
		}

		public ItemSlotsGrid()
		{
			InitializeComponent();
		}
	}
}
