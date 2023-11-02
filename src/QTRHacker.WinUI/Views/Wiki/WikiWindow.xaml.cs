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
using QTRHacker.Views.Wiki.Items;
using QTRHacker.ViewModels.Wiki;
using WinUIEx;
using Microsoft.UI.Xaml.Media.Animation;

namespace QTRHacker.Views.Wiki;

public sealed partial class WikiWindow : WindowEx
{
	public WikiViewModel ViewModel { get; }
	public WikiWindow(WikiViewModel vm)
	{
		this.InitializeComponent();
		this.ExtendsContentIntoTitleBar = true;
		this.SetTitleBar(TitleBar);
		this.AppWindow.TitleBar.IconShowOptions = Microsoft.UI.Windowing.IconShowOptions.HideIconAndSystemMenu;
		this.ViewModel = vm;
	}

	private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
	{
		if (args.SelectedItemContainer.Tag is null)
			return;
		var tag = args.SelectedItemContainer.Tag as string;
		switch (tag)
		{
			case "Items":
				var binding = new Binding() { Source = this, Path = new PropertyPath($"ViewModel.WikiItemsPageViewModel") };
				MainFrame.Navigate(typeof(WikiItemsPage), binding, new SlideNavigationTransitionInfo()
				{
					Effect = SlideNavigationTransitionEffect.FromLeft
				});
				break;
		}
	}

	private void MainFrame_Navigated(object sender, NavigationEventArgs e)
	{
		if (e.Content is not FrameworkElement fe)
			return;
		if (e.Parameter is not BindingBase binding)
			return;
		fe.SetBinding(FrameworkElement.DataContextProperty, binding);
	}

	private void NavigationView_Loaded(object sender, RoutedEventArgs e)
	{
		if (sender is not NavigationView v)
			return;
		v.SelectedItem = v.MenuItems.FirstOrDefault();
	}
}
