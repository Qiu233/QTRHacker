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
using QTRHacker.Core.GameObjects.Terraria;
using System.Windows.Threading;
using QTRHacker.Controls;
using QTRHacker.ViewModels.PlayerEditor;
using System.ComponentModel;

namespace QTRHacker.Views.PlayerEditor
{
	/// <summary>
	/// PlayerEditorWindow.xaml 的交互逻辑
	/// </summary>
	public partial class PlayerEditorWindow : MWindow
	{
		public PlayerEditorWindowViewModel ViewModel => DataContext as PlayerEditorWindowViewModel;
		public PlayerEditorWindow()
		{
			InitializeComponent();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			ViewModel.UpdateTimer.Stop();//I found no cleaner way to dispose the timer or the whole viewmodel
		}
	}
}
