using QTRHacker.Localization;
namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemCategoryFilter : ViewModelBase, ILocalizationProvider
{
	private bool isChecked = true;
	private string hint;

	public bool IsSelected
	{
		get => isChecked;
		set
		{
			isChecked = value;
			OnPropertyChanged(nameof(IsSelected));
			SelectedChanged?.Invoke(this, EventArgs.Empty);
		}
	}
	public event EventHandler SelectedChanged;
	public string Hint => hint;
	public ItemCategory Category { get; }

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		hint = LocalizationManager.Instance.GetValue($"UI.ItemCategories.{Category}", LocalizationType.Hack);
		OnPropertyChanged(nameof(Hint));
	}

	public ItemCategoryFilter(ItemCategory category)
	{
		Category = category;
		LocalizationManager.RegisterLocalizationProvider(this);
	}
}
