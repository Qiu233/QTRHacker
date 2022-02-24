using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels.PagePanels
{
	public class MainPageViewModel : PagePanelViewModel
	{
		private readonly BackgroundWorker attachedToGameWorker = new();
		private Visibility crossVisibility;
		private Visibility spinnerVisibility = Visibility.Collapsed;

		public BackgroundWorker AttachedToGameWorker => attachedToGameWorker;

		public Visibility CrossVisibility
		{
			get => crossVisibility;
			set
			{
				crossVisibility = value;
				OnPropertyChanged(nameof(CrossVisibility));
			}
		}
		public Visibility SpinnerVisibility
		{
			get => spinnerVisibility;
			set
			{
				spinnerVisibility = value;
				OnPropertyChanged(nameof(SpinnerVisibility));
			}
		}

		public async void InitGame(Point p)
		{
			CrossVisibility = Visibility.Collapsed;
			SpinnerVisibility = Visibility.Visible;
			if (await Task.Run(() =>
			{
				nuint hwnd = WindowFromPoint((int)p.X, (int)p.Y);
				GetWindowThreadProcessId(hwnd, out int pid);
				var process = Process.GetProcessById(pid);
				HackGlobal.Logging.Log($"Cross released at {p}, pid = {pid}, name = {process.ProcessName}");
				if (pid == 0)
				{
					HackGlobal.Logging.Error($"Attaching failed due to pid = 0");
					MessageBox.Show("Failed to fetch pid (got 0)", "Error");
					return false;
				}
				else if (pid == Environment.ProcessId)
				{
					HackGlobal.Logging.Error($"Attaching failed due to self attaching");
					MessageBox.Show("Please drag the cross to Terraria's window.", "Error");
					return false;
				}
				try
				{
					HackGlobal.Initialize(pid);
				}
				catch (Exception ex)
				{
					string msg = $"Attaching failed due to an exception:\n{ex.Message}\n{ex.StackTrace}";
					HackGlobal.Logging.Error(msg);
					MessageBox.Show(msg, "Error");
					return false;
				}
				HackGlobal.Logging.Log("Successfully attached to game");
				return true;
			}))
				AttachedToGameWorker.RunWorkerAsync(p);
			else
				PostAttaching();
		}

		private void PostAttaching()
		{
			SpinnerVisibility = Visibility.Collapsed;
			CrossVisibility = Visibility.Visible;
		}


		public MainPageViewModel()
		{
			AttachedToGameWorker.RunWorkerCompleted += (s, e) =>
			{
				PostAttaching();
			};

		}


		[DllImport("User32.dll")]
		private static extern nuint WindowFromPoint(int x, int y);
		[DllImport("User32.dll")]
		private static extern void GetWindowThreadProcessId(nuint hwnd, out int ID);
	}
}
