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
		private readonly Dictionary<string, string> Current = new();
		public event EventHandler<CultureChangedEventArgs> CultureChanged;
		public string CultureName
		{
			get => cultureName; 
		}
		private LocalizationManager(string initialCulture = "en")
		{
			SetCulture(initialCulture);
		}

		public string GetValue(string key)
		{
			if (Current.TryGetValue(key, out string s))
				return s;
			return key;
		}

		private void ApplySet(LocSet set)
		{
			foreach (var key in set.Keys)
				Current[key] = set[key];
		}

		/// <summary>
		/// This method only trys to get all possible key-value pairs and cache them.<br/>
		/// Note, the cached ones won't get delected, only get overrided.
		/// </summary>
		/// <param name="culture"></param>
		public void SetCulture(string culture)
		{
			cultureName = culture;
			ApplySet(LocSet.LoadFromRes(culture));
			ApplySet(LocSet.LoadFromGame(culture));
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
