using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using QTRHacker.Assets;
using QTRHacker.ViewModels;
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
public class NavItem
{
	public Type? PageType { get; set; }
	public string? ViewModel { get; set; }
}
public sealed partial class MainWindow : WindowEx
{
	public QTRHackerViewModel ViewModel { get; }
	public MainWindow(QTRHackerViewModel vm)
	{
		this.InitializeComponent();
		this.ExtendsContentIntoTitleBar = true;
		this.SetTitleBar(TitleBar);
		this.AppWindow.TitleBar.IconShowOptions = Microsoft.UI.Windowing.IconShowOptions.HideIconAndSystemMenu;
		this.Closed += MainWindow_Closed;

		this.ViewModel = vm;
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
		if (args.SelectedItemContainer.Tag is not NavItem nav)
			return;
		Type? newPage = nav.PageType;
		if (newPage is null)
			return;
		var binding = new Binding() { Source = this, Path = new PropertyPath($"ViewModel.{nav.ViewModel}") };
		MainFrame.Navigate(newPage, binding, args.RecommendedNavigationTransitionInfo);
	}

	private void MainFrame_Navigated(object sender, NavigationEventArgs e)
	{
		if (e.Content is not FrameworkElement fe)
			return;
		if (e.Parameter is not BindingBase binding)
			return;
		fe.SetBinding(FrameworkElement.DataContextProperty, binding);
	}
}
