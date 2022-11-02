namespace QTRHacker.Localization;

public interface ILocalizationProvider
{
	void OnCultureChanged(object sender, CultureChangedEventArgs args);
}
