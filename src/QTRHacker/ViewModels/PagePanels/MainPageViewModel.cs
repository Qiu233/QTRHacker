using QTRHacker.Core.GameObjects;
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

		private Visibility crossVisibility;
		private Visibility spinnerVisibility = Visibility.Collapsed;

		private readonly List<Action> ActionsAfterAttachedToGame = new();

		public event Action AttachedToGame
		{
			add => ActionsAfterAttachedToGame.Add(value);
			remove => ActionsAfterAttachedToGame.Remove(value);
		}

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

		private static bool InternalInit(Point p)
		{
			nuint hwnd = WindowFromPoint((int)p.X, (int)p.Y);
			GetWindowThreadProcessId(hwnd, out int pid);
			var process = Process.GetProcessById(pid);
			HackGlobal.Logging.Log($"Cross released at ({p}), pid = {pid}, name = {process.ProcessName}");
			List<ProcessModule> modules = new();
			foreach (ProcessModule module in process.Modules)
				modules.Add(module);
			HackGlobal.Logging.Log($"Modules:\t[{string.Join(", ", modules.Select(t => t.ModuleName))}]");

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
				HackGlobal.Logging.Exception(ex);
				MessageBox.Show(msg, "Error");
				return false;
			}
			HackGlobal.Logging.Log("Successfully attached to game");
			try
			{
				var obj = HackGlobal.GameContext.GameModuleHelper.GetStaticHackObject("Terraria.Main", "versionNumber");
				HackGlobal.Logging.Log("Game Version:\t" + new GameString(HackGlobal.GameContext, obj).GetValue());
			}
			catch
			{
				HackGlobal.Logging.Error("Failed to fetch game version");
			}
			return true;
		}

		public async void InitGame(Point p)
		{
			CrossVisibility = Visibility.Collapsed;
			SpinnerVisibility = Visibility.Visible;
			if (await Task.Run(() => InternalInit(p)))
			{
				var tasks = ActionsAfterAttachedToGame.Select(t => new Task(t)).ToList();
				tasks.ForEach(t => t.Start());
				await Task.WhenAll(tasks);
			}
			SpinnerVisibility = Visibility.Collapsed;
			CrossVisibility = Visibility.Visible;
		}


		public MainPageViewModel()
		{
		}


		[DllImport("User32.dll")]
		private static extern nuint WindowFromPoint(int x, int y);
		[DllImport("User32.dll")]
		private static extern void GetWindowThreadProcessId(nuint hwnd, out int ID);
	}
}
