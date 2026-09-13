using QTRHacker.Localization;

namespace QTRHacker.ViewModels.Wiki;

public abstract class CategoryFilterViewModel : ViewModelBase, ILocalizationProvider
{
	private readonly string localizationKey;
	private bool isSelected = true;
	private string hint;

	public bool IsSelected
	{
		get => isSelected;
		set
		{
			if (SetProperty(ref isSelected, value))
				SelectedChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	public string Hint => hint;

	public event EventHandler SelectedChanged;

	protected CategoryFilterViewModel(string localizationKey)
	{
		this.localizationKey = localizationKey;
		LocalizationManager.RegisterLocalizationProvider(this);
	}

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		SetProperty(ref hint, LocalizationManager.Instance.GetValue(localizationKey), nameof(Hint));
	}
}
