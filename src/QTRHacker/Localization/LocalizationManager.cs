using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.Localization
{
	public sealed class CultureChangedEventArgs : EventArgs
	{
		public string Name { get; }

		public CultureChangedEventArgs(string name)
		{
			Name = name;
		}
	}
	public sealed class LocalizationManager
	{
		private readonly Dictionary<string, string> CurrentHack = new();
		private readonly Dictionary<string, string> CurrentGame = new();
		public event EventHandler<CultureChangedEventArgs> CultureChanged;
		public string CultureName
		{
			get => cultureName;
		}
		private LocalizationManager(string initialCulture = "en")
		{
			SetCulture(initialCulture);
		}

		public string GetValue(string key, LocalizationType type = LocalizationType.Hack)
		{
			if (type == LocalizationType.Hack && CurrentHack.TryGetValue(key, out string s1))
				return s1;
			else if (type == LocalizationType.Game && CurrentGame.TryGetValue(key, out string s2))
				return s2;
			return key;
		}

		private static void ApplySet(Dictionary<string, string> dic, LocSet set)
		{
			foreach (var key in set.Keys)
				dic[key] = set[key];
		}

		/// <summary>
		/// This method only trys to get all possible key-value pairs and cache them.<br/>
		/// Note, the cached ones won't get delected, only get overrided.
		/// </summary>
		/// <param name="culture"></param>
		public void SetCulture(string culture)
		{
			cultureName = culture;
			ApplySet(CurrentHack, LocSet.LoadFromRes(culture));
			ApplySet(CurrentGame, LocSet.LoadFromGame(culture));
			CultureChanged?.Invoke(this, new CultureChangedEventArgs(culture));
		}
		private static LocalizationManager _Instance;
		private string cultureName;

		public static LocalizationManager Instance => _Instance ??= new LocalizationManager();

		/// <summary>
		/// This method will dispatch the handler after registering it.
		/// </summary>
		/// <param name="provider"></param>
		public static void RegisterLocalizationProvider(ILocalizationProvider provider)
		{
			WeakEventManager<LocalizationManager, CultureChangedEventArgs>.AddHandler(
				Instance, nameof(CultureChanged), provider.OnCultureChanged);
			provider.OnCultureChanged(Instance, new CultureChangedEventArgs(Instance.CultureName));
		}
	}
}
