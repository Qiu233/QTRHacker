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
using QTRHacker.ViewModels.PlayerEditor;

namespace QTRHacker.Views.PlayerEditor
{
	/// <summary>
	/// PlayerEditorWindow.xaml 的交互逻辑
	/// </summary>
	public partial class PlayerEditorWindow : MWindow
	{
		public DispatcherTimer ItemUpdateTimer { get; }
		private PlayerEditorWindowViewModel ViewModel => DataContext as PlayerEditorWindowViewModel;
		public PlayerEditorWindow()
		{
			InitializeComponent();

			ItemUpdateTimer = new();
			ItemUpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.ItemUpdateInterval);
			ItemUpdateTimer.Tick += (s, e) =>
			{
				InvEditor.UpdateAll();
				PiggyEditor.UpdateAll();
			};
			ItemUpdateTimer.Start();
		}
	}
}
