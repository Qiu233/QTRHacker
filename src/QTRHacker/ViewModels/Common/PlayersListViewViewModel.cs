using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.Common;

public class PlayersListViewViewModel : ViewModelBase
{

	private double idColumnWidth = 40;
	private double nameColumnWidth = 140;
	private PlayerInfo selectedPlayerInfo;

	public event EventHandler<PlayerInfo> SelectedPlayerInfoChanged;

	public double IDColumnWidth
	{
		get => idColumnWidth;
		set
		{
			idColumnWidth = value;
			OnPropertyChanged(nameof(IDColumnWidth));
		}
	}
	public double NameColumnWidth
	{
		get => nameColumnWidth;
		set
		{
			nameColumnWidth = value;
			OnPropertyChanged(nameof(IDColumnWidth));
		}
	}

	public ObservableCollection<PlayerInfo> Players { get; } = new();

	public PlayerInfo SelectedPlayerInfo
	{
		get => selectedPlayerInfo;
		set
		{
			selectedPlayerInfo = value;
			OnPropertyChanged(nameof(SelectedPlayerInfo));
			SelectedPlayerInfoChanged?.Invoke(this, selectedPlayerInfo);
		}
	}
	public DispatcherTimer PlayerUpdate { get; }

	public void AddWeakHandlerToTimer(EventHandler<EventArgs> handler)
	{
		WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(PlayerUpdate, nameof(DispatcherTimer.Tick), handler);
	}

	public PlayersListViewViewModel(DispatcherTimer timer = null)
	{
		if (timer == null)
		{
			PlayerUpdate = new();
			PlayerUpdate.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.PlayersListUpdateInterval);
			AddWeakHandlerToTimer(PlayerUpdate_Tick);
			PlayerUpdate.Start();
		}
		else
		{
			PlayerUpdate = timer;
			AddWeakHandlerToTimer(PlayerUpdate_Tick);
		}
	}
	private void PlayerUpdate_Tick(object sender, EventArgs args)
	{
		UpdatePlayersList();
	}

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

	/// <summary>
	/// Stops the timer on this object being collected, preventing leaks.
	/// </summary>
	~PlayersListViewViewModel()
	{
		PlayerUpdate?.Stop();
	}
}
