using QTRHacker.Controls;
using QTRHacker.ViewModels.Common;
using QTRHacker.ViewModels.Common.PropertyEditor;
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
using System.Windows.Shapes;

namespace QTRHacker.Views.Common
{
	/// <summary>
	/// Interaction logic for PropertyEditorWindow.xaml
	/// </summary>
	public partial class PropertyEditorWindow : MWindow
	{
		public PropertyEditorWindowViewModel ViewModel => DataContext as PropertyEditorWindowViewModel;
		public PropertyEditorWindow()
		{
			InitializeComponent();
		}
		private static void FinishEditing(TextBox tb)
		{
			if (tb.DataContext is not EditableProperty ep)
				return;
			if (ep.IsEditable)
			{
				BindingExpression be = tb.GetBindingExpression(TextBox.TextProperty);
				be.UpdateSource();
			}
			ep.IsEditing = false;
		}

		private void EditorBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (sender is not TextBox tb)
				return;
			FinishEditing(tb);
		}

		private void EditorBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (sender is not TextBox tb)
				return;
			if (e.Key == Key.Enter)
				FinishEditing(tb);
		}

		private void EditorBox_Loaded(object sender, RoutedEventArgs e)
		{
			if (sender is not TextBox tb)
				return;
			tb.CaretIndex = tb.Text.Length;
			tb.Focus();
		}

		private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (sender is not TreeViewItem tvi || tvi.DataContext is not EditableProperty ep)
					return;
				ep.IsEditing = true;
			}
		}

		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (e.NewValue is PropertyBase pb)
				ViewModel.SelectedProperty = pb;
		}

		private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

			if (treeViewItem != null)
			{
				treeViewItem.Focus();
				e.Handled = true;
			}
		}
		static TreeViewItem VisualUpwardSearch(DependencyObject source)
		{
			while (source != null && source is not TreeViewItem)
				source = VisualTreeHelper.GetParent(source);

			return source as TreeViewItem;
		}

		private void TreeView_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			e.Handled = true;
		}
	}
}
