using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QTRHacker.Core;
using Windows.UI.Popups;
using QTRHacker.Localization;

namespace QTRHacker;


public static class HackGlobal
{
	public const string GameVersion = "1.4.4.9";
	private static GameContext? _GameContext;
	//private static CFG_QTRHacker _Config;
	public static GameContext? GameContext => _GameContext;
	//public static CFG_QTRHacker Config => _Config;
	public static readonly Logging Logging;

	public const string FILE_CONFIG = "./HackConfig.json";
	public const string PATH_RAINBOWFONTS = "./Content/RainbowFonts";
	private const int MAX_LOG_FILES = 10;

	static HackGlobal()
	{
		if (!Directory.Exists("./logs"))
			Directory.CreateDirectory("./logs");
		var logs = Directory.EnumerateFiles("./logs", "*.log").ToList();
		string format = "yyyy-M-dd--HH.mm.ss";
		logs.Where(t =>
		{
			try { DateTime.ParseExact(t, format, null); } catch { return true; }
			return false;
		})
			.ToList()
			.ForEach(t => { try { File.Delete(t); } catch { } logs.Remove(t); });
		logs.Sort((a, b) =>
		{
			var span = DateTime.ParseExact(a, format, null) - DateTime.ParseExact(b, format, null);
			return (int)Math.Round(span.TotalMilliseconds);
		});
		foreach (var file in logs.Take(logs.Count - MAX_LOG_FILES))
			File.Delete(file);
		Logging = Logging.New(File.Open($"./logs/{DateTime.Now.ToString(format)}.log", FileMode.Create));
	}

	public static void SaveConfig()
	{
		//if (_Config != null)
		//	File.WriteAllText(FILE_CONFIG, JsonConvert.SerializeObject(_Config, Formatting.Indented));
	}

	public static void LoadConfig()
	{
		//if (!File.Exists(FILE_CONFIG))
		//{
		//	_Config = new CFG_QTRHacker();
		//	SaveConfig();
		//}
		//_Config = JsonConvert.DeserializeObject<CFG_QTRHacker>(File.ReadAllText(FILE_CONFIG));
		//SaveConfig();
	}

	//public static Dictionary<char, ProjImage> Characters
	//{
	//	get;
	//	private set;
	//}

	//public static void LoadRainbowFonts()
	//{
	//	if (Characters == null)
	//		Characters = new Dictionary<char, ProjImage>();
	//	Characters.Clear();
	//	LoadRainbowFonts(PATH_RAINBOWFONTS, Characters);
	//}
	//private static void LoadRainbowFonts(string dir, Dictionary<char, ProjImage> characters)
	//{
	//	Directory.EnumerateFiles(dir, "*.rbfont").ToList().ForEach(t =>
	//	{
	//		CharactersLoader.LoadCharacters(characters, File.ReadAllText(t));
	//	});
	//	Directory.EnumerateDirectories(dir).ToList().ForEach(t => LoadRainbowFonts(t, characters));
	//}

	public static event EventHandler? Initialized;

	public static async Task Initialize(int pid)
	{
		_GameContext?.Dispose(); //dispose the last one context
		await Task.Run(() =>
		{
			_GameContext = GameContext.OpenGame(Process.GetProcessById(pid));
		});
		Initialized?.Invoke(null, EventArgs.Empty);
		GC.Collect();
	}
	public static bool IsActive => _GameContext != null;

	public static void AlertExceptionOccured(Exception e)
	{
		ContentDialog dialog = new()
		{
			XamlRoot = App.WindowXamlRoot,
			Title = "Critical Error",
			Content = $"{LocalizationManager.Instance.GetValue("UI.Messages.ExceptionOccured")}\nError:\n{e.Message}\n{e.StackTrace}"
		};
		dialog.CloseButtonText = "Close";
		dialog.CloseButtonClick += (s, e) =>
		{
			Application.Current.Exit();
		};
		_ = dialog.ShowAsync();
	}

}
