using QHackLib;
using QHackLib.Memory;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Scripts;
using QTRHacker.Core;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using static QTRHacker.Scripts.ScriptHelper;
using System;
using System.Windows;
using System.Threading.Tasks;
using QTRHacker.Controls;
using System.Windows.Controls;
using System.Windows.Media;
using QTRHacker;
using QTRHacker.Localization;
using QTRHacker.Views.Common;
using QTRHacker.ViewModels.Common;
using QTRHacker.ViewModels;
using QTRHacker.Core.GameObjects.Terraria;
using System.Windows.Data;
using System.IO;

public static PlayerInfo SelectPlayer(string title)
{
	PlayerInfo selected = null;
	Application.Current.Dispatcher.Invoke(new Action(() =>
	{
		MWindow window = new();
		window.SizeToContent = SizeToContent.Height;
		window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		window.Title = title;
		window.MinimizeBox = false;
		window.Width = 260;
		Grid grid = new();
		window.Content = grid;

		grid.RowDefinitions.Add(new RowDefinition());
		grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
		grid.ColumnDefinitions.Add(new ColumnDefinition());
		grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });


		PlayersListView playersList = new();
		playersList.DataContext = new PlayersListViewViewModel();
		playersList.BorderThickness = new Thickness(1);
		playersList.BorderBrush = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
		playersList.MinWidth = 200;
		playersList.Height = 160;
		grid.Children.Add(playersList);
		Grid.SetRow(playersList, 0);
		Grid.SetRowSpan(playersList, 2);

		Button btn = new();
		btn.Foreground = new SolidColorBrush(Colors.White);
		btn.Padding = new Thickness(5, 7, 5, 7);
		btn.Content = LocalizationManager.Instance.GetValue("UI.Confirm");
		grid.Children.Add(btn);
		Grid.SetRow(btn, 1);
		Grid.SetColumn(btn, 1);

		btn.Click += (s, e) =>
		{
			window.DialogResult = true;
			window.Close();
		};

		if (window.ShowDialog() == true)
		{
			selected = playersList.ViewModel.SelectedPlayerInfo;
		}
	}));
	return selected;
}

public static void CopyInventory(PlayerInfo player)
{
	if (player is null)
		return;
	MemoryStream ms = new();
	HackGlobal.GameContext.Players[player.ID].SaveInventory(ms);
	ms.Position = 0;
	HackGlobal.GameContext.MyPlayer.LoadInventory(ms);
}

public static void CopyAppearance(PlayerInfo player)
{
	if (player is null)
		return;
	var myPlayer = HackGlobal.GameContext.MyPlayer;
	var target = HackGlobal.GameContext.Players[player.ID];
	myPlayer.SkinVariant = target.SkinVariant;
	myPlayer.Hair = target.Hair;
	myPlayer.HairDye = target.HairDye;
	myPlayer.HairColor = target.HairColor;
	myPlayer.HairDyeColor = target.HairDyeColor;

	myPlayer.EyeColor = target.EyeColor;
	myPlayer.PantsColor = target.PantsColor;
	myPlayer.ShirtColor = target.ShirtColor;
	myPlayer.ShoeColor = target.ShoeColor;
	myPlayer.SkinColor = target.SkinColor;
	myPlayer.UnderShirtColor = target.UnderShirtColor;
	NetMessage.SendData(HackGlobal.GameContext, 4, -1, -1, 0, HackGlobal.GameContext.MyPlayerIndex, 0, 0, 0, 0, 0, 0);
}

public class CopyPlayersInventory : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "复制玩家背包";
				break;
			case "en":
			default:
				Name = "Copy player's inventory";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		CopyInventory(SelectPlayer(Name));
	}
	public override void Disable(GameContext ctx)
	{
		throw new NotImplementedException();
	}
}

public class CopyPlayersAppearance : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "复制玩家外观";
				break;
			case "en":
			default:
				Name = "Copy player's appearance";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		CopyAppearance(SelectPlayer(Name));
	}
	public override void Disable(GameContext ctx)
	{
		throw new NotImplementedException();
	}
}

public class CopyPlayersInventoryAndAppearance : BaseFunction
{
	public override bool CanDisable => false;
	public override void ApplyLocalization(string culture)
	{
		switch (culture)
		{
			case "zh":
				Name = "复制玩家背包和外观";
				break;
			case "en":
			default:
				Name = "Copy player's inventory and appearance";
				break;
		}
	}
	public override void Enable(GameContext ctx)
	{
		var player = SelectPlayer(Name);
		CopyInventory(player);
		CopyAppearance(player);
	}
	public override void Disable(GameContext ctx)
	{
		throw new NotImplementedException();
	}
}

/*public class Inventory : FunctionCategory
{
	public override string Category => "Inventory";
	public Inventory()
	{
		this["zh"] = "背包";
		this["en"] = "Inv";

		Add<CopyPlayersInventory>();
		Add<CopyPlayersAppearance>();
		Add<CopyPlayersInventoryAndAppearance>();
	}
}

return new Inventory();*/