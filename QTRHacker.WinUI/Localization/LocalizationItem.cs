using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace QTRHacker.Localization;

public sealed class LocalizationItem : ObservableObject, ILocalizationProvider
{
	public event EventHandler<string>? ValueChanged;
	private readonly string Key;
	private readonly LocalizationType Type;

	public string Value => LocalizationManager.Instance.GetValue(Key, Type);

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		OnPropertyChanged(nameof(Value));
		ValueChanged?.Invoke(this, Value);
	}

	public LocalizationItem(string key, LocalizationType type = LocalizationType.Hack)
	{
		Key = key;
		Type = type;
		LocalizationManager.RegisterLocalizationProvider(this);
	}
}
