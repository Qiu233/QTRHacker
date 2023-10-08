using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QTRHacker.Controls;
using QTRHacker.Models;
using QTRHacker.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Views.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page
{
	public MainPageViewModel ViewModel => (MainPageViewModel)DataContext;
	public MainPage()
	{
		this.InitializeComponent();
		this.DataContext = new MainPageViewModel();
	}

	private void Expander_Expanding(Expander sender, ExpanderExpandingEventArgs args)
	{
		ViewModel.LoadProcesses().ContinueWith(t => ProcessSearchBox.Focus(FocusState.Programmatic), TaskContinuationOptions.ExecuteSynchronously);
	}

	private async Task Attach(int pid)
	{
		AttachExpander.IsExpanded = false;
		AttachExpander.IsHitTestVisible = false;
		Cross.Visibility = Visibility.Collapsed;
		SpinnerDotCircle.Visibility = Visibility.Visible;

		await ViewModel.AttachTo(pid);

		Cross.Visibility = Visibility.Visible;
		SpinnerDotCircle.Visibility = Visibility.Collapsed;
		AttachExpander.IsHitTestVisible = true;
	}

	private async void ProcessesListView_ItemClick(object sender, ItemClickEventArgs e)
	{
		var item = (ProcessItem)e.ClickedItem;
		await Attach(item.Pid);
	}
	[DllImport("User32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetCursorPos(out Point32 lpPoint);
	[DllImport("User32.dll")]
	private static extern nuint WindowFromPoint(int x, int y);
	[DllImport("User32.dll")]
	private static extern void GetWindowThreadProcessId(nuint hwnd, out int ID);

	private async void Cross_CrossReleased(object sender, CrossReleasedEventArgs e)
	{
		GetCursorPos(out Point32 p);
		nuint hwnd = WindowFromPoint(p.X, p.Y);
		GetWindowThreadProcessId(hwnd, out int pid);
		await Attach(pid);
	}
}
