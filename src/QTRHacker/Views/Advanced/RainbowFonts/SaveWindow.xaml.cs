using QTRHacker.Controls;
using QTRHacker.ViewModels.Advanced.RainbowFonts;
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

namespace QTRHacker.Views.Advanced.RainbowFonts
{
	/// <summary>
	/// Interaction logic for SaveWindow.xaml
	/// </summary>
	public partial class SaveWindow : MWindow
	{
		public SaveWindowViewModel ViewModel => DataContext as SaveWindowViewModel;
		public SaveWindow()
		{
			InitializeComponent();
			Loaded += SaveWindow_Loaded;
		}

		private void SaveWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModel.RequestClose += ViewModel_RequestClose;
			ViewModel.RequestSetDialogResult += ViewModel_RequestSetDialogResult;
		}
		private void ViewModel_RequestSetDialogResult(bool? obj)
		{
			DialogResult = obj;
		}

		private void ViewModel_RequestClose()
		{
			Close();
		}

		private void TextBox_Loaded(object sender, RoutedEventArgs e)
		{
			(sender as TextBox)?.Focus();
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ViewModel.ApplyNew((sender as TextBox).DataContext as SaveWindowViewModel.NewLibViewModel);
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				ViewModel.ApplyNew((sender as TextBox).DataContext as SaveWindowViewModel.NewLibViewModel);
			}
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView view = sender as ListView;
			view.ScrollIntoView(view.SelectedItem);
		}
	}
}
