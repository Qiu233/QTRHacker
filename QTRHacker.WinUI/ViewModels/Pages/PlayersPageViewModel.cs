using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using QTRHacker.Containers.PlayerEditor;
using QTRHacker.Core.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Pages;

public partial class PlayerInfo : ObservableObject, IEquatable<PlayerInfo>
{
	public int ID { get; init; }
	public string Name { get; init; }
	[ObservableProperty] private int maxLife;
	[ObservableProperty] private int life;
	[ObservableProperty] private float x;
	[ObservableProperty] private float y;

	[ObservableProperty] private bool updating;

	/// <summary>
	/// Indicates whether data are being loaded from game.
	/// If is false, then it's safe to apply data from view to game.
	/// This won't work in concurrent context.
	/// </summary>
	private bool forwarding = false;

	public PlayerInfo(int id, string name)
	{
		ID = id;
		Name = name;
		PropertyChanged += PlayerInfo_PropertyChanged;
	}

	private void PlayerInfo_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (forwarding || HackGlobal.GameContext is null)
			return;
		var player = HackGlobal.GameContext.Players[ID];
		switch (e.PropertyName)
		{
			case nameof(MaxLife):
				player.StatLifeMax = MaxLife;
				break;
			case nameof(Life):
				player.StatLife = Life;
				break;
			case nameof(X):
			case nameof(Y):
				player.Position = new Core.GameObjects.ValueTypeRedefs.Xna.Vector2(X, Y);
				break;
		}
	}

	public async Task Update()
	{
		if (HackGlobal.GameContext is null)
			return;
		forwarding = true;
		(MaxLife, Life, X, Y) = await Task.Run(() =>
		{
			var p = HackGlobal.GameContext.Players[ID];
			return (p.StatLifeMax, p.StatLife, p.Position.X, p.Position.Y);
		});
		forwarding = false;
	}


	public static bool operator ==(PlayerInfo a, PlayerInfo b) => a.ID == b.ID && a.Name == b.Name;
	public static bool operator !=(PlayerInfo a, PlayerInfo b) => !(a == b);
	public override int GetHashCode() => HashCode.Combine(ID, Name);

	public override bool Equals(object? obj)
	{
		if (obj is not PlayerInfo pi)
			return false;
		return this == pi;
	}

	bool IEquatable<PlayerInfo>.Equals(PlayerInfo? other)
	{
		if (other is null) return false;
		return Equals(other);
	}
}

public partial class PlayersPageViewModel : PageViewModel
{
	public ObservableCollection<PlayerInfo> Players { get; } = new();
	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(EditInventoryCommand))]
	private PlayerInfo? selectedPlayer;

	public PlayersPageViewModel(DispatcherQueueTimer updateTimer)
	{
		updateTimer.Tick += UpdateTimer_Tick;
		this.PropertyChanged += PlayersPageViewModel_PropertyChanged;
	}

	private void PlayersPageViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(SelectedPlayer))
		{
			foreach (var player in Players)
				player.Updating = false;
			if (SelectedPlayer is not null)
				SelectedPlayer.Updating = true;
		}
	}

	private async void UpdateTimer_Tick(DispatcherQueueTimer sender, object args)
	{
		await UpdatePlayersList();
		await UpdateStatus();
	}
	[RelayCommand]
	public async Task UpdateStatus()
	{
		if (HackGlobal.GameContext is null)
			return;
		foreach (var p in Players)
		{
			if (!p.Updating)
				continue;
			await p.Update();
		}
	}
	[RelayCommand]
	public async Task UpdatePlayersList()
	{
		if (!HackGlobal.IsActive)
			return;
		var players = HackGlobal.GameContext!.Players;
		var currentPlayers = Players.OrderBy(t => t);
		var active = await Task.Run(() =>
		{
			return players
				.Select((Player, Index) => new { Player, Index })
				.Where(t => t.Player.Active)
				.Select(t => new PlayerInfo(t.Index, t.Player.Name))
				.OrderBy(t => t);
		});
		if (active.SequenceEqual(currentPlayers))
			return;

		currentPlayers.Except(active).ToList().ForEach(t => Players.Remove(t));
		active.Except(currentPlayers).ToList().ForEach(t => Players.Add(t));
	}

	public bool PlayerSelected => SelectedPlayer is not null;

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
