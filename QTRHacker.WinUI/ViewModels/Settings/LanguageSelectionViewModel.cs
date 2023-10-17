using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace QTRHacker.ViewModels.Settings;

public class LanguageSelectionViewModel : ObservableObject
{
	public ObservableCollection<CultureInfo> Cultures { get; } = new();

	private string? selectedCultureName;
	public string? SelectedCultureName
	{
		get => selectedCultureName;
		set
		{
			SetProperty(ref selectedCultureName, value);
			if (value is null)
				return;
			ApplicationLanguages.PrimaryLanguageOverride = value;
			_ = LocalizationManager.Instance.SetCulture(value);
		}
	}
	public LanguageSelectionViewModel()
	{
		var languages = ApplicationLanguages.ManifestLanguages;
		var supported = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(t => languages.Contains(t.Name));
		foreach (CultureInfo culture in supported)
		{
			Cultures.Add(culture);
		}
		selectedCultureName = ApplicationLanguages.PrimaryLanguageOverride;
	}
}
