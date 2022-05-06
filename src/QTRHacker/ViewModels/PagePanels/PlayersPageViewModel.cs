using QTRHacker.Commands;
using QTRHacker.Core;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.PagePanels
{
	public class PlayersPageViewModel : PagePanelViewModel
	{
		public ObservableCollection<PlayerInfo> Players
		{
			get;
		} = new();
		private PlayerInfo selectedPlayerInfo;
		private readonly RelayCommand editPlayerCommand;
		private readonly RelayCommand tpToPlayerCommand;

		public PlayerInfo SelectedPlayerInfo
		{
			get => selectedPlayerInfo;
			set
			{
				selectedPlayerInfo = value;
				OnPropertyChanged(nameof(SelectedPlayerInfo));
				EditPlayerCommand.TriggerCanExecuteChanged();
				TPToPlayerCommand.TriggerCanExecuteChanged();
			}
		}

		public RelayCommand EditPlayerCommand => editPlayerCommand;
		public RelayCommand TPToPlayerCommand => tpToPlayerCommand;

		private bool GetIsPlayerSelected(object o) => SelectedPlayerInfo is not null;

		public void UpdatePlayersList()
		{
			if (!HackGlobal.IsActive)
				return;
			var players = HackGlobal.GameContext.Players;
			var active = players
				.Select((Player, Index) => new { Player, Index })
				.Where(t => t.Player.Active)
				.Select(t => new PlayerInfo(t.Index, t.Player.Name))
				.OrderBy(t => t);
			var currentPlayers = Players.OrderBy(t => t);
			if (active.SequenceEqual(currentPlayers))
				return;

			currentPlayers.Except(active).ToList().ForEach(t => Players.Remove(t));
			active.Except(currentPlayers).ToList().ForEach(t => Players.Add(t));
		}

		public DispatcherTimer PlayerUpdate { get; }

		public PlayersPageViewModel()
		{
			editPlayerCommand = new RelayCommand(GetIsPlayerSelected, (o) =>
			{
				var player = HackGlobal.GameContext.Players[SelectedPlayerInfo.ID];
				if (!player.Active)
					return;
				PlayerEditorWindow window = new();
				window.DataContext = new PlayerEditorWindowViewModel(player);
				window.Show();
			});
			tpToPlayerCommand = new(GetIsPlayerSelected, (o) =>
			{
				var player = HackGlobal.GameContext.Players[SelectedPlayerInfo.ID];
				if (!player.Active)
					return;
				HackGlobal.GameContext.MyPlayer.Position = player.Position;
			});
			if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
			{
				PlayerUpdate = new();
				PlayerUpdate.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.PlayersListUpdateInterval);
				WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(PlayerUpdate, nameof(DispatcherTimer.Tick), PlayerUpdate_Tick);
				//Still need to stop the timer, but so far I've found no clean way to do so.
				PlayerUpdate.Start();
			}
		}

		private void PlayerUpdate_Tick(object sender, EventArgs args)
		{
			UpdatePlayersList();
		}
	}
}
