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
using QTRHacker.Localization;
using QTRHacker.Views.Wiki.Items.SubPages;
using QTRHacker.ViewModels.Wiki.Items;
using QTRHacker.ViewModels.Wiki;

namespace QTRHacker.Views.Wiki.Items;

public sealed partial class WikiItemsPage : Page
{
	private readonly LocalizationItem TypeLI = new LocalizationItem("Wiki.Items.Properties.Type");
	private readonly LocalizationItem NameLI = new LocalizationItem("Wiki.Items.Properties.Name");
	private readonly LocalizationItem CategoryLI = new LocalizationItem("Wiki.Items.Properties.Category");

	public WikiItemsPageViewModel ViewModel => (WikiItemsPageViewModel)DataContext;

	public WikiItemsPage()
	{
		this.InitializeComponent();
	}

	private void NavigationView_Loaded(object sender, RoutedEventArgs e)
	{
		if (sender is not NavigationView v)
			return;
		v.SelectedItem = v.MenuItems.FirstOrDefault();
	}

	private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
	{
		if (args.SelectedItemContainer.Tag is null)
			return;
		var tag = args.SelectedItemContainer.Tag as string;
		switch (tag)
		{
			case "InfoSubPage":
				MainFrame.Navigate(typeof(InfoSubPage));
				break;
		}
	}

	private void MainFrame_Navigated(object sender, NavigationEventArgs e)
	{
	}
}
