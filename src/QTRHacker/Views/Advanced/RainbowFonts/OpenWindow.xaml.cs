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
	/// Interaction logic for OpenWindow.xaml
	/// </summary>
	public partial class OpenWindow : MWindow
	{
		public OpenWindowViewModel ViewModel => DataContext as OpenWindowViewModel;
		public OpenWindow()
		{
			InitializeComponent();
			Loaded += OpenWindow_Loaded;
		}

		private void OpenWindow_Loaded(object sender, RoutedEventArgs e)
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
	}
}
