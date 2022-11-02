using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Controls;
using QTRHacker.Localization;
using QTRHacker.ViewModels.Common;
using QTRHacker.ViewModels.Common.PropertyEditor;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.Views.Common;
using QTRHacker.Views.PlayerEditor;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QTRHacker.ViewModels.PagePanels;

public class PlayersPageViewModel : PagePanelViewModel
{
	private readonly List<PetInfo> Pets = new();
	private readonly List<MountInfo> Mounts = new();
	private string infoBoxName;
	private int infoBoxMaxLife;
	private int infoBoxMaxMana;
	private float infoBoxCoordinateX;
	private float infoBoxCoordinateY;

	public RelayCommand EditPlayerCommand { get; }
	public RelayCommand EditPlayerPropertyCommand { get; }
	public RelayCommand TPToPlayerCommand { get; }
	public RelayCommand AddBuffCommand { get; }
	public RelayCommand SetPetCommand { get; }
	public RelayCommand SetMountCommand { get; }

	public PlayersListViewViewModel PlayersListViewViewModel { get; } = new();

	private bool GetIsPlayerSelected(object o) => PlayersListViewViewModel.SelectedPlayerInfo is not null;

	public string InfoBoxName
	{
		get => infoBoxName;
		set
		{
			infoBoxName = value;
			OnPropertyChanged(nameof(InfoBoxName));
		}
	}

	public int? InfoBoxMaxLife
	{
		get => infoBoxMaxLife == 0 ? null : infoBoxMaxLife;
		set
		{
			infoBoxMaxLife = value ?? 0;
			OnPropertyChanged(nameof(InfoBoxMaxLife));
		}
	}

	public int? InfoBoxMaxMana
	{
		get => infoBoxMaxMana == 0 ? null : infoBoxMaxMana;
		set
		{
			infoBoxMaxMana = value ?? 0;
			OnPropertyChanged(nameof(InfoBoxMaxMana));
		}
	}

	public float? InfoBoxCoordinateX
	{
		get => infoBoxCoordinateX == 0 ? null : infoBoxCoordinateX;
		set
		{
			infoBoxCoordinateX = value ?? 0;
			OnPropertyChanged(nameof(InfoBoxCoordinateX));
		}
	}

	public float? InfoBoxCoordinateY
	{
		get => infoBoxCoordinateY == 0 ? null : infoBoxCoordinateY;
		set
		{
			infoBoxCoordinateY = value ?? 0;
			OnPropertyChanged(nameof(InfoBoxCoordinateY));
		}
	}

	private bool ShowWindow_GetMount(out int type)
	{
		MWindow window = new();
		window.SizeToContent = SizeToContent.WidthAndHeight;
		window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		window.Title = "Mount";
		window.MinimizeBox = false;
		Grid grid = new();
		window.Content = grid;

		grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
		grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
		grid.ColumnDefinitions.Add(new ColumnDefinition());
		grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

		Label tip = new();
		tip.Foreground = new SolidColorBrush(Colors.White);
		tip.Content = $"{LocalizationManager.Instance.GetValue("UI.Type")}:";
		grid.Children.Add(tip);
		Grid.SetColumn(tip, 0);

		ComboBox box = new();
		box.Width = 160;
		box.VerticalContentAlignment = VerticalAlignment.Center;
		box.Background = new SolidColorBrush(Color.FromArgb(20, 200, 200, 200));
		box.Foreground = new SolidColorBrush(Color.FromRgb(20, 20, 20));
		grid.Children.Add(box);
		Grid.SetColumn(box, 1);

		box.ItemsSource = Mounts;
		box.DisplayMemberPath = "Name";

		Button btn = new();
		btn.Foreground = new SolidColorBrush(Colors.White);
		btn.Padding = new Thickness(2);
		btn.Content = LocalizationManager.Instance.GetValue("UI.Confirm");
		grid.Children.Add(btn);
		Grid.SetColumn(btn, 2);

		btn.Click += (s, e) =>
		{
			window.DialogResult = true;
			window.Close();
		};

		var result = window.ShowDialog();
		type = 0;
		if (box.SelectedItem is MountInfo mi)
			type = mi.BuffType;
		return result == true;
	}

	private bool ShowWindow_GetPet(out int type)
	{
		MWindow window = new();
		window.SizeToContent = SizeToContent.WidthAndHeight;
		window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		window.Title = "Pet";
		window.MinimizeBox = false;
		Grid grid = new();
		window.Content = grid;

		grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
		grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
		grid.ColumnDefinitions.Add(new ColumnDefinition());
		grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

		Label tip = new();
		tip.Foreground = new SolidColorBrush(Colors.White);
		tip.Content = $"{LocalizationManager.Instance.GetValue("UI.Type")}:";
		grid.Children.Add(tip);
		Grid.SetColumn(tip, 0);

		ComboBox box = new();
		box.Width = 160;
		box.VerticalContentAlignment = VerticalAlignment.Center;
		box.Background = new SolidColorBrush(Color.FromArgb(20, 200, 200, 200));
		box.Foreground = new SolidColorBrush(Color.FromRgb(20, 20, 20));
		grid.Children.Add(box);
		Grid.SetColumn(box, 1);

		box.ItemsSource = Pets;
		box.DisplayMemberPath = "Name";

		Button btn = new();
		btn.Foreground = new SolidColorBrush(Colors.White);
		btn.Padding = new Thickness(2);
		btn.Content = LocalizationManager.Instance.GetValue("UI.Confirm");
		grid.Children.Add(btn);
		Grid.SetColumn(btn, 2);

		btn.Click += (s, e) =>
		{
			window.DialogResult = true;
			window.Close();
		};

		var result = window.ShowDialog();
		type = 0;
		if (box.SelectedItem is PetInfo pi)
			type = pi.Type;
		return result == true;
	}

	private static bool ShowWindow_GetBuff(out string type, out string time)
	{
		MWindow window = new();
		window.SizeToContent = SizeToContent.WidthAndHeight;
		window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		window.Title = "Buff";
		window.MinimizeBox = false;
		Grid grid = new();
		window.Content = grid;

		grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
		grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
		grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
		grid.ColumnDefinitions.Add(new ColumnDefinition());
		grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

		Label tip1 = new();
		tip1.Foreground = new SolidColorBrush(Colors.White);
		tip1.Content = $"{LocalizationManager.Instance.GetValue("UI.Type")}:";
		grid.Children.Add(tip1);
		Grid.SetColumn(tip1, 0);

		TextBox box1 = new();
		box1.Text = "5";
		box1.Width = 160;
		box1.VerticalContentAlignment = VerticalAlignment.Center;
		box1.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
		box1.Foreground = new SolidColorBrush(Colors.White);
		grid.Children.Add(box1);
		Grid.SetColumn(box1, 1);

		Label tip2 = new();
		tip2.Foreground = new SolidColorBrush(Colors.White);
		tip2.Content = $"{LocalizationManager.Instance.GetValue("UI.Time")}:";
		grid.Children.Add(tip2);
		Grid.SetColumn(tip2, 0);
		Grid.SetRow(tip2, 1);

		TextBox box2 = new();
		box2.Text = "3600";
		box2.Width = 160;
		box2.VerticalContentAlignment = VerticalAlignment.Center;
		box2.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
		box2.Foreground = new SolidColorBrush(Colors.White);
		grid.Children.Add(box2);
		Grid.SetColumn(box2, 1);
		Grid.SetRow(box2, 1);

		Button btn = new();
		btn.Foreground = new SolidColorBrush(Colors.White);
		btn.Padding = new Thickness(2);
		btn.Content = LocalizationManager.Instance.GetValue("UI.Confirm");
		grid.Children.Add(btn);
		Grid.SetColumn(btn, 2);
		Grid.SetRowSpan(btn, 2);

		btn.Click += (s, e) =>
		{
			window.DialogResult = true;
			window.Close();
		};

		var result = window.ShowDialog();
		(type, time) = (box1.Text, box2.Text);
		return result == true;
	}

	private void LoadPets()
	{
		if (Pets.Any())
			return;
		var vanityPets = new Core.GameObjects.GameObjectArrayV<bool>(HackGlobal.GameContext,
			HackGlobal.GameContext.GameModuleHelper.GetStaticHackObject("Terraria.Main", "vanityPet")).GetAllElements();
		for (int i = 0; i < vanityPets.Length; i++)
		{
			if (vanityPets[i])
				Pets.Add(new PetInfo(i, WikiResLoader.BuffKeys[i]));
		}
	}

	private void LoadMounts()
	{
		if (Mounts.Any())
			return;
		var mountDatum = new Core.GameObjects.GameObjectArray(HackGlobal.GameContext,
			HackGlobal.GameContext.GameModuleHelper.GetStaticHackObject("Terraria.Mount", "mounts"));
		int len = mountDatum.Length;
		for (int i = 0; i < len; i++)
		{
			dynamic data = mountDatum[i];
			int buffType = data.buff;
			Mounts.Add(new MountInfo(i, buffType, WikiResLoader.BuffKeys[buffType]));
		}
	}

	public void UpdateInfo()
	{
		if (PlayersListViewViewModel.SelectedPlayerInfo is not PlayerInfo info)//null check
			return;
		var player = HackGlobal.GameContext.Players[info.ID];
		InfoBoxName = player.Name;
		InfoBoxMaxLife = player.StatLifeMax;
		InfoBoxMaxMana = player.StatManaMax;
		var pos = player.Position;// here we cache the whole struct so that only one read is needed.
		InfoBoxCoordinateX = pos.X;
		InfoBoxCoordinateY = pos.Y;
	}

	public PlayersPageViewModel()
	{
		PlayersListViewViewModel.SelectedPlayerInfoChanged += (s, e) =>
		{
			UpdateInfo();
			EditPlayerCommand.TriggerCanExecuteChanged();
			EditPlayerPropertyCommand.TriggerCanExecuteChanged();
			TPToPlayerCommand.TriggerCanExecuteChanged();
			AddBuffCommand.TriggerCanExecuteChanged();
			SetPetCommand.TriggerCanExecuteChanged();
			SetMountCommand.TriggerCanExecuteChanged();
		};
		PlayersListViewViewModel.AddWeakHandlerToTimer((s, e) => UpdateInfo());
		EditPlayerCommand = new RelayCommand(GetIsPlayerSelected, (o) =>
		{
			var player = HackGlobal.GameContext.Players[PlayersListViewViewModel.SelectedPlayerInfo.ID];
			if (!player.Active)
				return;
			PlayerEditorWindow window = new();
			window.DataContext = new PlayerEditorWindowViewModel(player);
			window.Show();
		});
		EditPlayerPropertyCommand = new RelayCommand(GetIsPlayerSelected, (o) =>
		{
			var player = HackGlobal.GameContext.Players[PlayersListViewViewModel.SelectedPlayerInfo.ID];
			PropertyEditorWindow window = new();
			window.DataContext = new PropertyEditorWindowViewModel();
			window.ViewModel.Roots.Add(new PropertyComplex(player.TypedInternalObject, "Player"));
			window.Show();
		});
		TPToPlayerCommand = new(GetIsPlayerSelected, (o) =>
		{
			var player = HackGlobal.GameContext.Players[PlayersListViewViewModel.SelectedPlayerInfo.ID];
			if (!player.Active)
				return;
			HackGlobal.GameContext.MyPlayer.Position = player.Position;
		});
		AddBuffCommand = new(GetIsPlayerSelected, (o) =>
		{
			if (!ShowWindow_GetBuff(out string type, out string time))
				return;
			if (!int.TryParse(type, out int type_i) || !int.TryParse(time, out int time_i))
				return;
			HackGlobal.GameContext.MyPlayer.AddBuff(type_i, time_i);
		});
		SetPetCommand = new(GetIsPlayerSelected, (o) =>
		{
			LoadPets();
			if (ShowWindow_GetPet(out int type))
				HackGlobal.GameContext.MyPlayer.AddBuff(type, 3600);
		});
		SetMountCommand = new(GetIsPlayerSelected, (o) =>
		{
			LoadMounts();
			if (ShowWindow_GetMount(out int type))
				HackGlobal.GameContext.MyPlayer.AddBuff(type, 3600);
		});
	}
	public class PetInfo : ViewModelBase, ILocalizationProvider
	{
		public int Type { get; }
		public string Key { get; }
		public string Name => LocalizationManager.Instance.GetValue($"BuffName.{Key}", LocalizationType.Game);

		public PetInfo(int type, string key)
		{
			Type = type;
			Key = key;
		}
		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			OnPropertyChanged(nameof(Name));
		}
	}
	public class MountInfo : ViewModelBase, ILocalizationProvider
	{
		public int Type { get; }
		public int BuffType { get; }
		public string Key { get; }
		public string Name => LocalizationManager.Instance.GetValue($"BuffName.{Key}", LocalizationType.Game);

		public MountInfo(int type, int buffType, string key)
		{
			Type = type;
			BuffType = buffType;
			Key = key;
		}

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			OnPropertyChanged(nameof(Name));
		}
	}
}

