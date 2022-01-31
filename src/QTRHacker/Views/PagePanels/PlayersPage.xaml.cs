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
		private record PlayerInfo(int ID, string Name);
		public DispatcherTimer PlayerUpdate { get; }
		private static readonly object _lock_update = new();
		public PlayersPage()
		{
			InitializeComponent();

			if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
			{
				// I don't why this keeps throwing exception, breaking the designer.
				PlayerUpdate = new();
				PlayerUpdate.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.PlayersListUpdateInterval);
				PlayerUpdate.Tick += (s, e) => UpdatePlayersList();
				PlayerUpdate.Start();
			}
		}

		private void UpdatePlayersList()
		{
			lock (_lock_update)
			{
				if (!HackGlobal.IsActive || PlayersList == null)
					return;
				var players = HackGlobal.GameContext.Players;
				var active = players.
					Select((Player, Index) => new { Player, Index }).
					Where(t => t.Player.Active).Select(t => new PlayerInfo(t.Index, t.Player.Name)).OrderBy(t => t);
				var currentPlayers = PlayersList.Items.OfType<PlayerInfo>().OrderBy(t => t);
				if (active.SequenceEqual(currentPlayers))
					return;
				PlayersList.Dispatcher.Invoke(() =>
				{
					currentPlayers.Except(active).ToList().ForEach(t => PlayersList.Items.Remove(t));
					active.Except(currentPlayers).ToList().ForEach(t => PlayersList.Items.Add(t));
				});
			}
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			var playerInfo = PlayersList.SelectedItem as PlayerInfo;
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
