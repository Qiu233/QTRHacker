using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Helpers;

public static class WindowHelper
{
	public static void SetStartupLocationAtCenter(this Window window)
	{
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
	}
}
