using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.Advanced.AimBot
{
	public class AimBotWindowViewModel : ViewModelBase
	{
		private readonly PlayersListViewViewModel playersListViewViewModel;

		public enum AimBotMode : int
		{
			Disabled = 0,
			NearestNPC = 1,
			NearestPlayer = 2,
			TargetedPlayer = 3
		}

		public PlayersListViewViewModel PlayersListViewViewModel => playersListViewViewModel;

		public AimBotMode Mode
		{
			get => (AimBotMode)HackGlobal.GameContext.Patches.AimBot_Mode;
			set
			{
				if (Enum.GetValues<AimBotMode>().Contains(value))
					HackGlobal.GameContext.Patches.AimBot_Mode = (int)value;
				else
					HackGlobal.GameContext.Patches.AimBot_Mode = (int)AimBotMode.Disabled;
				OnPropertyChanged(nameof(Mode));
			}
		}

		public bool Disabled
		{
			get => Mode == AimBotMode.Disabled;
			set
			{
				if (value)
					Mode = AimBotMode.Disabled;
			}
		}

		public bool NearestNPC
		{
			get => Mode == AimBotMode.NearestNPC;
			set
			{
				if (value)
					Mode = AimBotMode.NearestNPC;
			}
		}

		public bool NearestPlayer
		{
			get => Mode == AimBotMode.NearestPlayer;
			set
			{
				if (value)
					Mode = AimBotMode.NearestPlayer;
			}
		}

		public bool TargetedPlayer
		{
			get => Mode == AimBotMode.TargetedPlayer;
			set
			{
				if (value)
					Mode = AimBotMode.TargetedPlayer;
			}
		}

		public int TargetedPlayerIndex
		{
			get => HackGlobal.GameContext.Patches.AimBot_TargetedPlayerIndex;
			set
			{
				if (value < 0 || value >= HackGlobal.GameContext.Players.Length)
					return;
				HackGlobal.GameContext.Patches.AimBot_TargetedPlayerIndex = value;
				OnPropertyChanged(nameof(TargetedPlayerIndex));
			}
		}


#pragma warning disable CA1822 // 将成员标记为 static
		public bool HostileNPCsOnly
		{
			get => HackGlobal.GameContext.Patches.AimBot_HostileNPCsOnly;
			set => HackGlobal.GameContext.Patches.AimBot_HostileNPCsOnly = value;
		}
		public bool HostilePlayersOnly
		{
			get => HackGlobal.GameContext.Patches.AimBot_HostilePlayersOnly;
			set => HackGlobal.GameContext.Patches.AimBot_HostilePlayersOnly = value;
		}
		public float MaxDistance_NPC
		{
			get => HackGlobal.GameContext.Patches.AimBot_MaxDistance_NPC;
			set => HackGlobal.GameContext.Patches.AimBot_MaxDistance_NPC = value;
		}
		public float MaxDistance_Player
		{
			get => HackGlobal.GameContext.Patches.AimBot_MaxDistance_Player;
			set => HackGlobal.GameContext.Patches.AimBot_MaxDistance_Player = value;
		}
#pragma warning restore CA1822 // 将成员标记为 static

		public bool UpdateTextBoxes
		{
			get;
			set;
		} = true;

		public DispatcherTimer UpdateTimer { get; }

		public void Update()
		{
			OnPropertyChanged(nameof(Mode));
			OnPropertyChanged(nameof(TargetedPlayerIndex));
			OnPropertyChanged(nameof(HostileNPCsOnly));
			OnPropertyChanged(nameof(HostilePlayersOnly));
			if (UpdateTextBoxes)
			{
				UpdateTextBoxes = false;
				OnPropertyChanged(nameof(MaxDistance_NPC));
				OnPropertyChanged(nameof(MaxDistance_Player));
			}
			int index = TargetedPlayerIndex;
			var players = PlayersListViewViewModel.Players.Where(t => t.ID == index).ToList();
			if (players.Count == 1 && PlayersListViewViewModel.SelectedPlayerInfo?.ID != index)
			{
				PlayersListViewViewModel.SelectedPlayerInfo = players[0];
			}
		}

		public AimBotWindowViewModel()
		{
			UpdateTimer = new();
			UpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.SchesUpdateInterval);
			playersListViewViewModel = new PlayersListViewViewModel(UpdateTimer);
			PlayersListViewViewModel.SelectedPlayerInfoChanged += (s) =>
			{
				if (s is null)
				{
					HackGlobal.GameContext.Patches.AimBot_TargetedPlayerIndex = -1;
					return;
				}
				HackGlobal.GameContext.Patches.AimBot_TargetedPlayerIndex = s.ID;
			};
			HackGlobal.GameContext.Patches.Init();
			PropertyChanged += AimBotWindowViewModel_PropertyChanged;
			WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(UpdateTimer, nameof(DispatcherTimer.Tick), UpdateTimer_Tick);
			UpdateTimer.Start();
		}

		~AimBotWindowViewModel()
		{
			UpdateTimer?.Stop();
		}

		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			Update();
		}

		private void AimBotWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Mode))
			{
				OnPropertyChanged(nameof(Disabled));
				OnPropertyChanged(nameof(NearestNPC));
				OnPropertyChanged(nameof(NearestPlayer));
				OnPropertyChanged(nameof(TargetedPlayer));
			}
		}
	}
}
