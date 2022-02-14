using QTRHacker.Commands;
using QTRHacker.Functions;
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
	public class PlayersPageViewModel : ViewModelBase
	{
		public ObservableCollection<PlayerInfo> Players
		{
			get;
		} = new();
		private static readonly object _lock_update = new();
		private PlayerInfo selectedPlayerInfo;

		public PlayerInfo SelectedPlayerInfo
		{
			get => selectedPlayerInfo;
			set
			{
				selectedPlayerInfo = value;
				OnPropertyChanged(nameof(SelectedPlayerInfo));
			}
		}

		public ICommand EditPlayerCommand
		{
			get
			{
				return new RelayCommand(GetIsPlayerSelected, (o) =>
				{
					var player = HackGlobal.GameContext.Players[SelectedPlayerInfo.ID];
					if (!player.Active)
						return;
					PlayerEditorWindow window = new();
					window.DataContext = new PlayerEditorWindowViewModel(player);
					window.Show();
				});
			}
		}
		public ICommand TPToPlayerCommand
		{
			get
			{
				return new RelayCommand(GetIsPlayerSelected, (o) =>
				{
					var player = HackGlobal.GameContext.Players[SelectedPlayerInfo.ID];
					if (!player.Active)
						return;
				});
			}
		}

		private bool GetIsPlayerSelected(object o) => SelectedPlayerInfo is not null;

		public void UpdatePlayersList()
		{
			lock (_lock_update)
			{
				if (!HackGlobal.IsActive)
					return;
				var players = HackGlobal.GameContext.Players;
				var active = players.
					Select((Player, Index) => new { Player, Index }).
					Where(t => t.Player.Active).Select(t => new PlayerInfo(t.Index, t.Player.Name)).OrderBy(t => t);
				var currentPlayers = Players.ToList().OrderBy(t => t);
				if (active.SequenceEqual(currentPlayers))
					return;

				currentPlayers.Except(active).ToList().ForEach(t => Players.Remove(t));
				active.Except(currentPlayers).ToList().ForEach(t => Players.Add(t));
			}
		}

		public DispatcherTimer PlayerUpdate { get; }

		public PlayersPageViewModel()
		{
			if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
			{
				PlayerUpdate = new();
				PlayerUpdate.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.PlayersListUpdateInterval);
				PlayerUpdate.Tick += (s, e) => UpdatePlayersList();
				PlayerUpdate.Start();
			}
		}


		public record PlayerInfo(int ID, string Name);
	}
}
