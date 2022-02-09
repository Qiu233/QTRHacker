using QTRHacker.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
