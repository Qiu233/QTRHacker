using Microsoft.CodeAnalysis.CSharp.Scripting;
using Newtonsoft.Json;
using QTRHacker.Configs;
using QTRHacker.Core;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace QTRHacker
{
	public static class HackGlobal
	{
		private static GameContext _GameContext;
		private static CFG_QTRHacker _Config;
		public static GameContext GameContext => _GameContext;
		public static CFG_QTRHacker Config => _Config;
		public static readonly Logging Logging;

		private const string FILE_CONFIG = "./HackConfig.json";
		private const string PATH_RAINBOWFONTS = "./Content/RainbowFonts";
		private const int MAX_LOG_FILES = 10;

		static HackGlobal()
		{
			if (!Directory.Exists("./logs"))
				Directory.CreateDirectory("./logs");
			var logs = Directory.EnumerateFiles("./logs", "*.log").ToArray();
			for (int i = 0; i < logs.Length - MAX_LOG_FILES; i++)
				File.Delete(logs[i]);
			Logging = Logging.New(File.Open($"./logs/{DateTime.Now:yyyy-M-dd--HH.mm.ss}.log", FileMode.Create));
		}

		public static void SaveConfig()
		{
			if (_Config != null)
				File.WriteAllText(FILE_CONFIG, JsonConvert.SerializeObject(_Config, Formatting.Indented));
		}

		public static void LoadConfig()
		{
			if (!File.Exists(FILE_CONFIG))
			{
				_Config = new CFG_QTRHacker();
				SaveConfig();
			}
			_Config = JsonConvert.DeserializeObject<CFG_QTRHacker>(File.ReadAllText(FILE_CONFIG));
			SaveConfig();
		}

		public static Dictionary<char, ProjImage> Characters
		{
			get;
			private set;
		}

		public static void LoadRainbowFonts()
		{
			if (Characters == null)
				Characters = new Dictionary<char, ProjImage>();
			Characters.Clear();
			LoadRainbowFonts(PATH_RAINBOWFONTS, Characters);
		}
		private static void LoadRainbowFonts(string dir, Dictionary<char, ProjImage> characters)
		{
			Directory.EnumerateFiles(dir, "*.rbfont").ToList().ForEach(t =>
			{
				CharactersLoader.LoadCharacters(characters, File.ReadAllText(t));
			});
			Directory.EnumerateDirectories(dir).ToList().ForEach(t => LoadRainbowFonts(t, characters));
		}

		public static event EventHandler Initialized;

		public static void Initialize(int pid)
		{
			_GameContext = GameContext.OpenGame(Process.GetProcessById(pid));
			Initialized?.Invoke(null, EventArgs.Empty);
		}
		public static bool IsActive => _GameContext != null;

		private static BackgroundWorker Worker = new();

		public static void StartBackgroundWork(UIElement parent, DoWorkEventHandler work, bool suspendParent = false)
		{
			if (Worker == null)
				throw new InvalidOperationException();
			Popup popup = new();
			popup = new Popup
			{
				PlacementTarget = parent,
				Placement = PlacementMode.Center
			};
			ProgressBar bar = new();
			bar.Foreground = new SolidColorBrush(Colors.DarkGray);
			bar.IsIndeterminate = true;
			bar.Width = 300;
			bar.Height = 40;
			popup.Child = bar;
			if (suspendParent)
				MainWindow.Instance.IsEnabled = false;
			popup.IsOpen = true;
			Worker = new BackgroundWorker
			{
				WorkerReportsProgress = true
			};
			Worker.DoWork += work;
			Worker.RunWorkerCompleted += (s, e) =>
			{
				if (e.Error != null)
				{
					Logging.Exception(e.Error);
					MessageBox.Show("Exception occured when running background work, please check the log file.");
				}
				if (suspendParent)
					MainWindow.Instance.IsEnabled = true;
				popup.IsOpen = false;
			};
			Worker.RunWorkerAsync();
		}
	}
}
