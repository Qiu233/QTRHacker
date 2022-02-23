using QTRHacker.Controls;
using QTRHacker.ViewModels.PagePanels;
using QTRHacker.ViewModels.Wiki;
using QTRHacker.Views.Wiki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace QTRHacker.Views.PagePanels
{
	public partial class MainPage : UserControl
	{

		public MainPageViewModel ViewModel => DataContext as MainPageViewModel;

		public MainPage()
		{
			InitializeComponent();
		}

		private void Cross_CrossReleased(object sender, CrossReleasedEventArgs e)
		{
			Point p = (sender as Control).PointToScreen(e.Point);
			ViewModel.InitGame(p);
		}

		private void WikiButton_Click(object sender, RoutedEventArgs e)
		{
			WikiWindow window = new();
			window.DataContext = new WikiWindowViewModel();
			window.Show();
		}
	}
}
