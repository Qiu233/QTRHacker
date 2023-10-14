using Microsoft.UI.Dispatching;
using QTRHacker.ViewModels;
using QTRHacker.ViewModels.Pages;
using StrongInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Containers;


[Register<QTRHackerViewModel>]
[Register<MainPageViewModel>]
[Register<PlayersPageViewModel>]
public partial class QTRHacker : IContainer<QTRHackerViewModel>
{
	private readonly Owned<QTRHackerViewModel> owned;
	[Instance] public DispatcherQueueTimer UpdateTimer { get; }
	private QTRHacker()
	{
		UpdateTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
		UpdateTimer.Interval = TimeSpan.FromMilliseconds(500); // TODO: configurable
		owned = this.Resolve();
	}
	private MainWindow Show()
	{
		MainWindow window = new(owned.Value);
		var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
		Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
		Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
		if (appWindow is not null)
		{
			Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
			if (displayArea is not null)
			{
				var CenteredPosition = appWindow.Position;
				CenteredPosition.X = ((displayArea.WorkArea.Width - appWindow.Size.Width) / 2);
				CenteredPosition.Y = ((displayArea.WorkArea.Height - appWindow.Size.Height) / 2);
				appWindow.Move(CenteredPosition);
			}
		}
		window.Closed += Window_Closed;
		window.Activate();
		UpdateTimer.Start();

		return window;
	}

	private void Window_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
	{
		UpdateTimer.Stop();
		owned.Dispose();
		Dispose();
	}

	public static MainWindow Run()
	{
		return new QTRHacker().Show();
	}
}
