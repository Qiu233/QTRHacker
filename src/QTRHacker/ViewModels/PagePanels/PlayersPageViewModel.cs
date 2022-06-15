using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Controls;
using QTRHacker.Core;
using QTRHacker.Localization;
using QTRHacker.ViewModels.Common;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.PagePanels
{
	public class PlayersPageViewModel : PagePanelViewModel
	{

		private readonly RelayCommand editPlayerCommand;
		private readonly RelayCommand tpToPlayerCommand;
		private readonly RelayCommand addBuffCommand;
		private readonly RelayCommand setPetCommand;
		private readonly RelayCommand setMountCommand;

		private readonly List<PetInfo> Pets = new();
		private readonly List<MountInfo> Mounts = new();


		public RelayCommand EditPlayerCommand => editPlayerCommand;
		public RelayCommand TPToPlayerCommand => tpToPlayerCommand;
		public RelayCommand AddBuffCommand => addBuffCommand;
		public RelayCommand SetPetCommand => setPetCommand;
		public RelayCommand SetMountCommand => setMountCommand;

		public PlayersListViewViewModel PlayersListViewViewModel { get; } = new();

		private bool GetIsPlayerSelected(object o) => PlayersListViewViewModel.SelectedPlayerInfo is not null;

		public DispatcherTimer PlayerUpdate { get; }

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

		public PlayersPageViewModel()
		{
			PlayersListViewViewModel.SelectedPlayerInfoChanged += (s) =>
			{
				EditPlayerCommand.TriggerCanExecuteChanged();
				TPToPlayerCommand.TriggerCanExecuteChanged();
				AddBuffCommand.TriggerCanExecuteChanged();
				SetPetCommand.TriggerCanExecuteChanged();
				setMountCommand.TriggerCanExecuteChanged();
			};
			editPlayerCommand = new RelayCommand(GetIsPlayerSelected, (o) =>
			{
				var player = HackGlobal.GameContext.Players[PlayersListViewViewModel.SelectedPlayerInfo.ID];
				if (!player.Active)
					return;
				PlayerEditorWindow window = new();
				window.DataContext = new PlayerEditorWindowViewModel(player);
				window.Show();
			});
			tpToPlayerCommand = new(GetIsPlayerSelected, (o) =>
			{
				var player = HackGlobal.GameContext.Players[PlayersListViewViewModel.SelectedPlayerInfo.ID];
				if (!player.Active)
					return;
				HackGlobal.GameContext.MyPlayer.Position = player.Position;
			});
			addBuffCommand = new(GetIsPlayerSelected, (o) =>
			{
				if (!ShowWindow_GetBuff(out string type, out string time))
					return;
				if (!int.TryParse(type, out int type_i) || !int.TryParse(time, out int time_i))
					return;
				HackGlobal.GameContext.MyPlayer.AddBuff(type_i, time_i);
			});
			setPetCommand = new(GetIsPlayerSelected, (o) =>
			{
				LoadPets();
				if (ShowWindow_GetPet(out int type))
					HackGlobal.GameContext.MyPlayer.AddBuff(type, 3600);
			});
			setMountCommand = new(GetIsPlayerSelected, (o) =>
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

}
