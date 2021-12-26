using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using QHackLib;
using QTRHacker.Functions;
using QTRHacker.Functions.ProjectileImage;
using QTRHacker.Functions.ProjectileImage.RainbowImage;
using QTRHacker.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker
{
	public class HackContext
	{
		public const string PATH_CONTENT = ".\\Content";
		public const string PATH_CONFIGS = ".\\Content\\Configs";
		public const string PATH_INVS = ".\\Content\\Invs";
		public const string PATH_PROJS = ".\\Content\\Projs";
		public const string PATH_SCRIPTS = ".\\Content\\Scripts";
		public const string PATH_SCHES = ".\\Content\\Sches";
		public const string PATH_CHATTEMPLATES = ".\\Content\\ChatTemplates";
		public const string PATH_RAINBOWFONTS = ".\\Content\\RainbowFonts";

		public static readonly string[] PATHS = new string[] { PATH_CONTENT, PATH_INVS, PATH_CONFIGS, PATH_PROJS, PATH_SCRIPTS, PATH_SCHES, PATH_CHATTEMPLATES, PATH_RAINBOWFONTS };

		public static Languages.Language CurrentLanguage
		{
			get;
			private set;
		}
		public static Dictionary<char, ProjImage> Characters
		{
			get;
			private set;
		}
		public static Dictionary<string, Config> Configs
		{
			get;
			set;
		}
		public static GameContext GameContext
		{
			get;
			set;
		}
		public static ScriptRuntime QHScriptRuntime
		{
			get;
			private set;
		}
		public static ScriptEngine QHScriptEngine
		{
			get;
			private set;
		}
		public static void LoadConfigs()
		{
			if (Configs == null)
				Configs = new Dictionary<string, Config>();
			Configs.Clear();
			var ts = Assembly.GetExecutingAssembly().
				DefinedTypes.Where(
				t => t.Namespace == "QTRHacker.Configs" &&//in configs
				t.IsSubclassOf(typeof(Config)));//inheriting Config
			ts.ToList().ForEach(t =>
			{
				Configs[t.Name] = LoadConfig(t.Name, t);
			});
		}
		public static void SaveConfigs()
		{
			foreach (var name in Configs.Keys)
			{
				string file = Path.Combine(PATH_CONFIGS, $"{name}.json");
				File.WriteAllText(
					file,
					JsonConvert.SerializeObject(Configs[name], Formatting.Indented));
			}
		}
		private static Config LoadConfig(string name, Type t)
		{
			string file = Path.Combine(PATH_CONFIGS, $"{name}.json");
			Config value = null;
			if (File.Exists(file))
				value = JsonConvert.DeserializeObject(File.ReadAllText(file), t) as Config;
			else
				value = Activator.CreateInstance(t) as Config;
			File.WriteAllText(
				file,
				JsonConvert.SerializeObject(value, Formatting.Indented));
			return value;
		}


		public static ScriptScope CreateScriptScope(ScriptEngine engine)
		{
			ScriptScope s = engine.CreateScope();
			s.SetVariable("game_context", GameContext);
			engine.Execute("import clr\n" +
				"clr.AddReference('QHackLib')\n" +
				"clr.AddReference('QTRHacker')\n" +
				"clr.AddReference('QTRHacker.Functions')\n" +
				"from QHackLib import *\n" +
				"from QTRHacker import *\n" +
				"from QTRHacker.Functions import *", s);
			return s;
		}
		public static void CreateDirectoriesAndFiles()
		{
			foreach (var d in PATHS)
			{
				if (!Directory.Exists(d))
					Directory.CreateDirectory(d);
			}
		}
		private static void InitScriptRuntime()
		{
			QHScriptRuntime = Python.CreateRuntime();
			QHScriptEngine = QHScriptRuntime.GetEngine("Python");
			var paths = QHScriptEngine.GetSearchPaths();
			paths.Add(Path.GetFullPath(HackContext.PATH_SCRIPTS));
			QHScriptEngine.SetSearchPaths(paths);
		}
		private static void InitLanguage()
		{
			if ((Configs["CFG_QTRHacker"] as CFG_QTRHacker).IsCN)
				CurrentLanguage = Languages.Language.GetLanguage("zh-CN");
			else
				CurrentLanguage = Languages.Language.GetLanguage("en");
		}
		public static void Initialize()
		{
			CreateDirectoriesAndFiles();
			LoadConfigs();
			InitLanguage();
			InitScriptRuntime();
			LoadRainbowFonts();
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
	}
}
