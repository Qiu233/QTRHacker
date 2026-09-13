using System.Runtime.CompilerServices;
using QTRHacker.Localization;
using QTRHacker.Themes;

namespace QTRHacker.ViewModels.PagePanels;

public sealed class SettingsPageViewModel : PagePanelViewModel
{
	public IReadOnlyList<int> RefreshIntervals { get; }

	public string Theme
	{
		get => ThemeManager.CurrentTheme;
		set
		{
			if (value is not ("Dark" or "Light") || Theme == value)
				return;
			ThemeManager.Apply(value);
			HackGlobal.Config.Theme = value;
			HackGlobal.SaveConfig();
			OnPropertyChanged();
		}
	}

	public string Language
	{
		get => HackGlobal.Config.Language;
		set
		{
			if (value is not ("system" or "en" or "zh") || Language == value)
				return;
			HackGlobal.Config.Language = value;
			HackGlobal.Config.ForceEnglish = value == "en";
			HackGlobal.SaveConfig();
			LocalizationManager.Instance.ApplyLanguage(value);
			OnPropertyChanged();
		}
	}

	public int ItemUpdateInterval
	{
		get => HackGlobal.Config.ItemUpdateInterval;
		set => SetInterval(ref HackGlobal.Config.ItemUpdateInterval, value);
	}

	public int PlayersListUpdateInterval
	{
		get => HackGlobal.Config.PlayersListUpdateInterval;
		set => SetInterval(ref HackGlobal.Config.PlayersListUpdateInterval, value);
	}

	public int SchesUpdateInterval
	{
		get => HackGlobal.Config.SchesUpdateInterval;
		set => SetInterval(ref HackGlobal.Config.SchesUpdateInterval, value);
	}

	public SettingsPageViewModel()
	{
		RefreshIntervals = new[] { 50, 100, 200, 500, 1000, 2000, 5000,
			ItemUpdateInterval, PlayersListUpdateInterval, SchesUpdateInterval }
			.Where(interval => interval > 0)
			.Distinct()
			.OrderBy(interval => interval)
			.ToArray();
	}

	private void SetInterval(ref int field, int value, [CallerMemberName] string propertyName = null)
	{
		if (value > 0 && SetProperty(ref field, value, propertyName))
			HackGlobal.SaveConfig();
	}
}
