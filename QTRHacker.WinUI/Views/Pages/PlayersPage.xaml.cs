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
using QTRHacker.Containers.PlayerEditor;
using QTRHacker.Core;

namespace QTRHacker.Views.Pages;

public sealed partial class PlayersPage : Page
{
	public PlayersPageViewModel ViewModel => (PlayersPageViewModel)DataContext;
	public PlayersPage()
	{
		this.InitializeComponent();
	}

	private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		ViewModel.EditInventoryCommand.NotifyCanExecuteChanged();
	}
}
