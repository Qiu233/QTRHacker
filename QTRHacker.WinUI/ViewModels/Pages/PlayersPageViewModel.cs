using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using QTRHacker.Containers.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Pages;

public record PlayerInfo(int ID, string Name);

public partial class PlayersPageViewModel : PageViewModel
{
	public ObservableCollection<PlayerInfo> Players { get; } = new();
	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(EditInventoryCommand))]
	private PlayerInfo? selectedPlayer;

	public PlayersPageViewModel(DispatcherQueueTimer updateTimer)
	{
		updateTimer.Tick += UpdateTimer_Tick;
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

	public bool PlayerSelected => SelectedPlayer != null;

	[RelayCommand(CanExecute = nameof(PlayerSelected))]
	public void EditInventory()
	{
		if (HackGlobal.GameContext is null)
			return;
		if (SelectedPlayer is null)
			return;
		var player = HackGlobal.GameContext.Players[SelectedPlayer.ID];
		InventoryEditor.ShowFor(player);
	}
}
