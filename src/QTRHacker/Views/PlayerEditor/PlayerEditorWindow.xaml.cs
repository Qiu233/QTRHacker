using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QTRHacker.Functions.GameObjects.Terraria;
using System.Windows.Threading;
using QTRHacker.Controls;

namespace QTRHacker.Views.PlayerEditor
{
	/// <summary>
	/// PlayerEditorWindow.xaml 的交互逻辑
	/// </summary>
	public partial class PlayerEditorWindow : MWindow
	{
		public Player Player { get; }
		public DispatcherTimer ItemUpdateTimer { get; }
		public PlayerEditorWindow(Player player)
		{
			Player = player;
			InitializeComponent();
			Dispatcher.Invoke(() => Title = Player.Name);

			ItemUpdateTimer = new();
			ItemUpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.ItemUpdateInterval);
			ItemUpdateTimer.Tick += (s, e) =>
			{
				InvEditor.UpdateAll();
				PiggyEditor.UpdateAll();
			};
			ItemUpdateTimer.Start();
		}

		private void OnItemDataFetching(object sender, OnItemDataUpdatingEventArgs e)
		{
			if (sender == InvEditor)
			{
				e.Item = Player.Inventory[e.Index];
			}
			else if (sender == PiggyEditor)
			{
				e.Item = Player.Bank.Item[e.Index];
			}
		}
	}
}
