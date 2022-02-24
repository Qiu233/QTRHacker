using Microsoft.CodeAnalysis.CSharp.Scripting;
using Newtonsoft.Json;
using QTRHacker.Configs;
using QTRHacker.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace QTRHacker
{
	internal static class HackGlobal
	{
		private static GameContext _GameContext;
		private static CFG_QTRHacker _Config;
		public static GameContext GameContext => _GameContext;
		public static CFG_QTRHacker Config => _Config;
		public static readonly Logging Logging;

		private const string FILE_CONFIG = "./HackConfig.json";
		private const int MAX_LOG_FILES = 10;

		static HackGlobal()
		{
			if (!Directory.Exists("./logs"))
				Directory.CreateDirectory("./logs");
			var logs = Directory.EnumerateFiles("./logs", "*.log").ToArray();
			for (int i = 0; i < logs.Length - MAX_LOG_FILES; i++)
				File.Delete(logs[i]);
			Logging = Logging.New(File.Open($"./logs/{DateTime.Now:yyyy-dd-M--HH.mm.ss}.log", FileMode.Create));
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

		public static void Initialize(int pid)
		{
			_GameContext = GameContext.OpenGame(Process.GetProcessById(pid));
		}
		public static bool IsActive => _GameContext != null;
	}
}
