using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using QTRHacker.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using WinUIEx;

namespace QTRHacker;

public sealed partial class MainWindow : WindowEx
{
	public MainWindow()
	{
		this.InitializeComponent();
		this.ExtendsContentIntoTitleBar = true;
		this.SetTitleBar(TitleBar);
		this.AppWindow.TitleBar.IconShowOptions = Microsoft.UI.Windowing.IconShowOptions.HideIconAndSystemMenu;
		this.Closed += MainWindow_Closed;
	}

	private void MainWindow_Closed(object sender, WindowEventArgs args)
	{
		Application.Current.Exit();
	}

	private void NavigationView_Loaded(object sender, RoutedEventArgs e)
	{
		if (sender is not NavigationView v)
			return;
		v.SelectedItem = v.MenuItems.LastOrDefault();
	}

	private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
	{
		if (args.IsSettingsSelected == true)
		{
			return;
		}
		if (args.SelectedItemContainer.Tag is not string typeName)
			return;
		Type? newPage = Type.GetType(typeName);
		if (newPage is null)
			return;
		MainFrame.Navigate(newPage, null, args.RecommendedNavigationTransitionInfo);
	}
}
