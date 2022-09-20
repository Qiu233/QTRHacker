using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.Advanced.AutoFishing
{
	public class AutoFishingWindowViewModel : ViewModelBase
	{
		public enum FishingBotMode : int
		{
			Disabled = 0,
			All = 1,
			Items = 2,
			NPCs = 3,
		}

		public FishingBotMode Mode
		{
			get => (FishingBotMode)HackGlobal.GameContext.Patches.AutoFishing_Mode;
			set
			{
				if (Enum.GetValues<FishingBotMode>().Contains(value))
					HackGlobal.GameContext.Patches.AutoFishing_Mode = (int)value;
				else
					HackGlobal.GameContext.Patches.AutoFishing_Mode = (int)FishingBotMode.Disabled;
				OnPropertyChanged(nameof(Mode));
			}
		}

		public bool Disabled
		{
			get => Mode == FishingBotMode.Disabled;
			set
			{
				if (value)
					Mode = FishingBotMode.Disabled;
			}
		}
		public bool All
		{
			get => Mode == FishingBotMode.All;
			set
			{
				if (value)
					Mode = FishingBotMode.All;
			}
		}
		public bool Items
		{
			get => Mode == FishingBotMode.Items;
			set
			{
				if (value)
					Mode = FishingBotMode.Items;
			}
		}
		public bool NPCs
		{
			get => Mode == FishingBotMode.NPCs;
			set
			{
				if (value)
					Mode = FishingBotMode.NPCs;
			}
		}
		public bool CratesOnly
		{
			get => HackGlobal.GameContext.Patches.AutoFishing_CratesOnly;
			set
			{
				HackGlobal.GameContext.Patches.AutoFishing_CratesOnly = value;
				if (value)
					HackGlobal.GameContext.Patches.AutoFishing_QuestItemsOnly = false;
				OnPropertyChanged(nameof(CratesOnly));
				OnPropertyChanged(nameof(QuestItemsOnly));
			}
		}
		public bool QuestItemsOnly
		{
			get => HackGlobal.GameContext.Patches.AutoFishing_QuestItemsOnly;
			set
			{
				HackGlobal.GameContext.Patches.AutoFishing_QuestItemsOnly = value;
				if (value)
					HackGlobal.GameContext.Patches.AutoFishing_CratesOnly = false;
				OnPropertyChanged(nameof(QuestItemsOnly));
				OnPropertyChanged(nameof(CratesOnly));
			}
		}

		public DispatcherTimer UpdateTimer { get; }

		public void Update()
		{
			OnPropertyChanged(nameof(Mode));
			OnPropertyChanged(nameof(CratesOnly));
			OnPropertyChanged(nameof(QuestItemsOnly));
		}

		public AutoFishingWindowViewModel()
		{
			UpdateTimer = new();
			UpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.SchesUpdateInterval);
			HackGlobal.GameContext.Patches.Init();
			PropertyChanged += AutoFishingWindowViewModel_PropertyChanged;
			WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(UpdateTimer, nameof(DispatcherTimer.Tick), UpdateTimer_Tick);
			UpdateTimer.Start();
		}

		~AutoFishingWindowViewModel()
		{
			UpdateTimer?.Stop();
		}
		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			Update();
		}

		private void AutoFishingWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Mode))
			{
				OnPropertyChanged(nameof(Disabled));
				OnPropertyChanged(nameof(All));
				OnPropertyChanged(nameof(Items));
				OnPropertyChanged(nameof(NPCs));
			}
		}
	}
}