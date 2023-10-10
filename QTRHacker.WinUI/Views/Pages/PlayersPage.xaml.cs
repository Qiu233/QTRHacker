using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QTRHacker.ViewModels.Pages;
using WinUIEx;
using QTRHacker.Views.PlayerEditor;
using CommunityToolkit.WinUI.UI.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Views.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PlayersPage : Page
	{
		public PlayersPageViewModel ViewModel => (PlayersPageViewModel)DataContext;
		public PlayersPage()
		{
			this.InitializeComponent();
			DataContext = new PlayersPageViewModel();
			UniformGrid g;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			InventoryEditorWindow window = new();
			window.Show();
		}
	}
}
