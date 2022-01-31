using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Localization
{
	public sealed class LocalizationManager
	{
		private readonly Dictionary<string, LocSet> Locs = new();
		private readonly Dictionary<string, string> Current = new();
		public event EventHandler CultureChanged;
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

		public void SetCulture(string culture)
		{
			var set = GetLocSet(culture);
			foreach (var key in set.Keys)
				Current[key] = set[key];
			CultureChanged?.Invoke(this, new EventArgs());
		}
		private static LocalizationManager _Instance;
		public static LocalizationManager Instance => _Instance ??= new LocalizationManager();
	}
}
