using QTRHacker.ViewModels.Wiki.NPC;
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

namespace QTRHacker.Views.Wiki.NPC
{
	/// <summary>
	/// NPCFilterSubPage.xaml 的交互逻辑
	/// </summary>
	public partial class NPCFilterSubPage : UserControl
	{
		public NPCInfoPagesViewModel ViewModel => DataContext as NPCInfoPagesViewModel;
		public NPCFilterSubPage()
		{
			InitializeComponent();
		}

		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (ViewModel.ApplyKeyword.CanExecute(null))
					ViewModel.ApplyKeyword.Execute(null);
				e.Handled = true;
			}
		}
	}
}
