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

namespace QTRHacker
{
	public static class HackGlobal
	{
		private static GameContext _GameContext;
		private static CFG_QTRHacker _Config;
		public static GameContext GameContext => _GameContext;
		public static CFG_QTRHacker Config => _Config;
		private const string FILE_CONFIG = "./HackConfig.json";

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
