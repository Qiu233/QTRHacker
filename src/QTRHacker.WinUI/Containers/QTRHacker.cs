using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using QTRHacker.Helpers;
using QTRHacker.ViewModels;
using QTRHacker.ViewModels.Pages;
using QTRHacker.ViewModels.Settings;
using StrongInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Containers;


[Register<QTRHackerViewModel>(Scope.SingleInstance)]
[Register<MainPageViewModel>(Scope.SingleInstance)]
[Register<PlayersPageViewModel>(Scope.SingleInstance)]
[Register<SettingsPageViewModel>(Scope.SingleInstance)]
[Register<LanguageSelectionViewModel>(Scope.SingleInstance)]
public partial class QTRHacker :
	IContainer<QTRHackerViewModel>,
	IContainer<LanguageSelectionViewModel>
{
	private readonly Owned<QTRHackerViewModel> owned;
	[Instance] public DispatcherQueueTimer UpdateTimer { get; }
	public MainWindow MainWindow { get; private set; }
	[Factory]
	private MainWindow GetWindow()
	{
		return this.MainWindow;
	}
	public QTRHacker()
	{
		UpdateTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
		UpdateTimer.Interval = TimeSpan.FromMilliseconds(500); // TODO: configurable
		owned = this.Resolve<QTRHackerViewModel>();
		MainWindow window = new(owned.Value);
		this.MainWindow = window;
		this.MainWindow.SetStartupLocationAtCenter();
		window.AppWindow.Destroying += AppWindow_Destroying;
		UpdateTimer.Start();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>A task completes on window's Loaded event.</returns>
	public async Task Show()
	{
		MainWindow.Activate();
		while (MainWindow.Content.XamlRoot is null)
			await Task.Delay(100);
	}

	private void AppWindow_Destroying(Microsoft.UI.Windowing.AppWindow sender, object args)
	{
		UpdateTimer.Stop();
		owned.Dispose();
		Dispose();
	}
}
