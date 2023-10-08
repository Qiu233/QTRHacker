using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Pages;

public readonly record struct PlayerInfo(int ID, string Name);

public class PlayersPageViewModel : PageViewModel
{
	public ObservableCollection<PlayerInfo> Players { get; } = new();
	private readonly DispatcherQueueTimer UpdateTimer;
	public PlayersPageViewModel()
	{
		UpdateTimer = App.MainWindow.DispatcherQueue.CreateTimer();
		UpdateTimer.Interval = TimeSpan.FromMilliseconds(500);

		WeakEventListener<PlayersPageViewModel, DispatcherQueueTimer, object> listener = new(this);
		UpdateTimer.Tick += listener.OnEvent;
		listener.OnEventAction = (i, s, e) => UpdateTimer_Tick(s!, e);
		listener.OnDetachAction = l =>
		{
			UpdateTimer.Tick -= listener.OnEvent;
			UpdateTimer.Stop();
		};

		UpdateTimer.Start();
	}

	private void UpdateTimer_Tick(DispatcherQueueTimer sender, object args)
	{
		UpdatePlayersList();
	}
	public void UpdatePlayersList()
	{
		if (!HackGlobal.IsActive)
			return;
		var players = HackGlobal.GameContext!.Players;
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
}
