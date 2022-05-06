using QTRHacker.Controls;
using QTRHacker.ViewModels.Advanced.AimBot;
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

namespace QTRHacker.Views.Advanced.AimBot
{
	/// <summary>
	/// AimBotWindow.xaml 的交互逻辑
	/// </summary>
	public partial class AimBotWindow : MWindow
	{
		public AimBotWindowViewModel ViewModel => DataContext as AimBotWindowViewModel;
		public AimBotWindow()
		{
			InitializeComponent();
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ViewModel.UpdateTextBoxes = true;
		}

		private void MWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.Focus();
		}
	}
}
