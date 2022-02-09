using QTRHacker.ViewModels.PagePanels;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QTRHacker.Views.PagePanels
{
	/// <summary>
	/// PlayerPage.xaml 的交互逻辑
	/// </summary>
	public partial class PlayersPage : UserControl
	{
		public PlayersPageViewModel ViewModel => DataContext as PlayersPageViewModel;
		public PlayersPage()
		{
			InitializeComponent();
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			var playerInfo = PlayersList.SelectedItem as PlayersPageViewModel.PlayerInfo;
			if (playerInfo == null)
				return;
			var player = HackGlobal.GameContext.Players[playerInfo.ID];
			if (!player.Active)
				return;
			PlayerEditorWindow window = new();
			window.DataContext = new PlayerEditorWindowViewModel(player);
			window.Show();
		}
	}
}
