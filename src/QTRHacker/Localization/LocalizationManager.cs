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
		private readonly Dictionary<string, LocSet> Locs = new();
		private readonly Dictionary<string, string> Current = new();
		public event EventHandler<CultureChangedEventArgs> CultureChanged;
		public string CultureName
		{
			get => cultureName; 
		}
		private LocalizationManager(string initialCulture = "en-US")
		{
			SetCulture(initialCulture);
		}

		public string GetValue(string key)
		{
			if (Current.TryGetValue(key, out string s))
				return s;
			return key;
		}

		private LocSet GetLocSet(string culture)
		{
			if (Locs.TryGetValue(culture, out LocSet s))
				return s;
			return Locs[culture] = LocSet.LoadFromRes(culture);
		}

		/// <summary>
		/// This method only trys to get all possible key-value pairs and cache them.<br/>
		/// Note, the cached ones won't get delected, only get overrided.
		/// </summary>
		/// <param name="culture"></param>
		public void SetCulture(string culture)
		{
			cultureName = culture;
			var set = GetLocSet(culture);
			foreach (var key in set.Keys)
				Current[key] = set[key];
			CultureChanged?.Invoke(this, new CultureChangedEventArgs(culture));
		}
		private static LocalizationManager _Instance;
		private string cultureName;

		public static LocalizationManager Instance => _Instance ??= new LocalizationManager();

		public static void RegisterCultureChanged(ILocalizationProvider provider)
		{
			WeakEventManager<LocalizationManager, CultureChangedEventArgs>.AddHandler(
				Instance, nameof(CultureChanged), provider.OnCultureChanged);
		}
	}
}
