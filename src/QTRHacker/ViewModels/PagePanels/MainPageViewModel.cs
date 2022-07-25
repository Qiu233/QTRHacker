using QHackLib;
using QTRHacker.Commands;
using QTRHacker.Core.GameObjects;
using QTRHacker.Localization;
using QTRHacker.ViewModels.Common;
using QTRHacker.ViewModels.Common.PropertyEditor;
using QTRHacker.Views.Common;
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
		private string playersArrayAddress;
		private string npcsArrayAddress;
		private string myPlayerAddress;
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

		public string PlayersArrayAddress
		{
			get => playersArrayAddress;
			set
			{
				playersArrayAddress = value;
				OnPropertyChanged(nameof(PlayersArrayAddress));
			}
		}

		public string NPCsArrayAddress
		{
			get => npcsArrayAddress;
			set
			{
				npcsArrayAddress = value;
				OnPropertyChanged(nameof(NPCsArrayAddress));
			}
		}

		public string MyPlayerAddress
		{
			get => myPlayerAddress;
			set
			{
				myPlayerAddress = value;
				OnPropertyChanged(nameof(MyPlayerAddress));
			}
		}

		public HackCommand EditPlayersCommand { get; }
		public HackCommand EditMyPlayerCommand { get; }
		public HackCommand EditNPCsCommand { get; }

		private static bool InternalInit(int pid)
		{
			var process = Process.GetProcessById(pid);
			HackGlobal.Logging.Log($"pid = {pid}, name = {process.ProcessName}");
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

		public void UpdateAddrs()
		{
			PlayersArrayAddress = HackGlobal.GameContext.Players.BaseAddress.ToHexAddr();
			NPCsArrayAddress = HackGlobal.GameContext.NPC.BaseAddress.ToHexAddr();
			MyPlayerAddress = HackGlobal.GameContext.MyPlayer.BaseAddress.ToHexAddr();
		}

		public async void TryInitGame()
		{
			var ps = Process.GetProcesses().Where(t =>
			{
				if (!string.Equals(t.ProcessName, "dotnet", StringComparison.OrdinalIgnoreCase))
					return false;
				using QHackContext ctx = QHackContext.Create(t.Id);
				return ctx.CLRHelpers.Where(t => string.Equals(t.Key.Name, "tModLoader", StringComparison.OrdinalIgnoreCase)).Any();
			}).ToArray();
			if (ps.Length == 0)
			{
				MessageBox.Show("Please be sure that you have launched tModLoader");
				return;
			}

			CrossVisibility = Visibility.Collapsed;
			SpinnerVisibility = Visibility.Visible;
			if (await Task.Run(() => InternalInit(ps[0].Id)))
			{
				UpdateAddrs();
				var tasks = ActionsAfterAttachedToGame.Select(t => new Task(t)).ToList();
				tasks.ForEach(t => t.Start());
				await Task.WhenAll(tasks);
			}
			SpinnerVisibility = Visibility.Collapsed;
			CrossVisibility = Visibility.Visible;
		}

		public async void InitGame(Point p)
		{
			nuint hwnd = WindowFromPoint(new Point32((int)p.X, (int)p.Y));
			GetWindowThreadProcessId(hwnd, out int pid);
			CrossVisibility = Visibility.Collapsed;
			SpinnerVisibility = Visibility.Visible;
			if (await Task.Run(() => InternalInit(pid)))
			{
				UpdateAddrs();
				var tasks = ActionsAfterAttachedToGame.Select(t => new Task(t)).ToList();
				tasks.ForEach(t => t.Start());
				await Task.WhenAll(tasks);
			}
			SpinnerVisibility = Visibility.Collapsed;
			CrossVisibility = Visibility.Visible;
		}


		public MainPageViewModel()
		{
			EditPlayersCommand = new HackCommand(o =>
			{
				PropertyEditorWindow window = new();
				window.DataContext = new PropertyEditorWindowViewModel();
				window.ViewModel.Roots.Add(PropertyBase.New(HackGlobal.GameContext.Players.TypedInternalObject, "Players"));
				window.Show();
			});
			EditMyPlayerCommand = new HackCommand(o =>
			{
				PropertyEditorWindow window = new();
				window.DataContext = new PropertyEditorWindowViewModel();
				window.ViewModel.Roots.Add(PropertyBase.New(HackGlobal.GameContext.MyPlayer.TypedInternalObject, "MyPlayer"));
				window.Show();
			});
			EditNPCsCommand = new HackCommand(o =>
			{
				PropertyEditorWindow window = new();
				window.DataContext = new PropertyEditorWindowViewModel();
				window.ViewModel.Roots.Add(PropertyBase.New(HackGlobal.GameContext.NPC.TypedInternalObject, "NPCs"));
				window.Show();
			});
		}


		[StructLayout(LayoutKind.Sequential)]
		private record struct Point32(int X, int Y);

		[DllImport("User32.dll")]
		private static extern nuint WindowFromPoint(Point32 point);
		[DllImport("User32.dll")]
		private static extern void GetWindowThreadProcessId(nuint hwnd, out int ID);
	}
}
