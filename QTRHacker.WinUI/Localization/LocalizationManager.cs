using System.Windows;

namespace QTRHacker.Localization;

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
	public event EventHandler<CultureChangedEventArgs>? CultureChanged;
	private static LocalizationManager? _Instance;
	private string? cultureName;
	public string CultureName => cultureName!;
	private LocalizationManager(string initialCulture = "en")
	{
		SetCulture(initialCulture).Wait();
	}

	public string GetValue(string key, LocalizationType type = LocalizationType.Hack)
	{
		if (type == LocalizationType.Hack && CurrentHack.TryGetValue(key, out string? s1))
			return s1;
		else if (type == LocalizationType.Game && CurrentGame.TryGetValue(key, out string? s2))
			return s2;
		return "!" + key;
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
	public async Task SetCulture(string culture)
	{
		cultureName = culture;
		ApplySet(CurrentHack, await LocSet.LoadFromRes(culture).ConfigureAwait(false));
		ApplySet(CurrentGame, await LocSet.LoadFromGame(culture).ConfigureAwait(false));
		CultureChanged?.Invoke(this, new CultureChangedEventArgs(culture));
	}

	public static LocalizationManager Instance => _Instance ??= new LocalizationManager();

	/// <summary>
	/// This method will dispatch the handler after registering it.
	/// </summary>
	/// <param name="provider"></param>
	public static void RegisterLocalizationProvider(ILocalizationProvider provider)
	{
		WeakEventListener<ILocalizationProvider, LocalizationManager, CultureChangedEventArgs> listener = new(provider);
		void handler(object? s, CultureChangedEventArgs e) => listener.OnEvent(s as LocalizationManager, e);
		listener.OnEventAction = (i, s, e) => i.OnCultureChanged(Instance, e);
		Instance.CultureChanged += handler;
		listener.OnDetachAction = l => Instance.CultureChanged -= handler;
		provider.OnCultureChanged(Instance, new CultureChangedEventArgs(Instance.CultureName));
		// TODO:

		//WeakEventManager<LocalizationManager, CultureChangedEventArgs>.AddHandler(
		//	Instance, nameof(CultureChanged), provider.OnCultureChanged);
		//provider.OnCultureChanged(Instance, new CultureChangedEventArgs(Instance.CultureName));
	}
}
