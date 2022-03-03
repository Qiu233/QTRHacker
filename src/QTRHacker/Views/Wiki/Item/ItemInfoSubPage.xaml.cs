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
	/// ItemInfoSubPage.xaml 的交互逻辑
	/// </summary>
	public partial class ItemInfoSubPage : UserControl
	{
		public ItemInfoPagesViewModel ViewModel => DataContext as ItemInfoPagesViewModel;
		public ItemInfoSubPage()
		{
			InitializeComponent();
		}

		private void JumpToRecipe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender is not ListBoxItem item)
				return;
			if (!ViewModel.JumpToCommand.CanExecute(item.DataContext))
				return;
			ViewModel.JumpToCommand.Execute(item.DataContext);
		}
	}
}
