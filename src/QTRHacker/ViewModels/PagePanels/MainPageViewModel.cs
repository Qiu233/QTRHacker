using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels.PagePanels
{
	public class MainPageViewModel : ViewModelBase
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

		private static bool TryAttach(int pid)
		{
			if (pid == 0)
			{
				MessageBox.Show("Failed to fetch pid(got 0)", "Error");
				return false;
			}
			else if (pid == Environment.ProcessId)
			{
				MessageBox.Show("Please drag the cross to Terraria's window.", "Error");
				return false;
			}
			HackGlobal.Initialize(pid);
			return true;
		}

		public async void InitGame(Point p)
		{
			CrossVisibility = Visibility.Collapsed;
			SpinnerVisibility = Visibility.Visible;
			if (await Task.Run(() =>
			{
				nuint hwnd = WindowFromPoint((int)p.X, (int)p.Y);
				GetWindowThreadProcessId(hwnd, out int pid);
				return TryAttach(pid);
			}))
				AttachedToGameWorker.RunWorkerAsync(p);
		}


		public MainPageViewModel()
		{
			AttachedToGameWorker.RunWorkerCompleted += (s, e) =>
			{
				SpinnerVisibility = Visibility.Collapsed;
				CrossVisibility = Visibility.Visible;
			};

		}


		[DllImport("User32.dll")]
		private static extern nuint WindowFromPoint(int x, int y);
		[DllImport("User32.dll")]
		private static extern void GetWindowThreadProcessId(nuint hwnd, out int ID);
	}
}
