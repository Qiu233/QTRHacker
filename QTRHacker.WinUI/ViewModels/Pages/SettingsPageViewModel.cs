using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Microsoft.UI.Xaml.Media.Animation;
using QTRHacker.Localization;

namespace QTRHacker.ViewModels.Pages;

public partial class SettingsPageViewModel : PageViewModel
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
	public SettingsPageViewModel()
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
