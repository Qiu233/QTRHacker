using QTRHacker.ViewModels.Wiki.Item;
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

namespace QTRHacker.Views.Wiki.Item
{
	/// <summary>
	/// ItemWikiTabPage.xaml 的交互逻辑
	/// </summary>
	public partial class ItemWikiTabPage : UserControl
	{
		public ItemPageViewModel ViewModel => DataContext as ItemPageViewModel;
		public ItemWikiTabPage()
		{
			InitializeComponent();
		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ViewModel.AddSelectedItemToGame_Max();
		}
	}
}
