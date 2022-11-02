using QTRHacker.Commands;
using QTRHacker.Localization;

namespace QTRHacker.ViewModels.Advanced;

public abstract class AdvancedFunction : ViewModelBase, ILocalizationProvider
{
	private string name;
	private readonly RelayCommand runCommand;

	public string Name
	{
		get => name;
		set
		{
			name = value;
			OnPropertyChanged(nameof(Name));
		}
	}

	public RelayCommand RunCommand => runCommand;

	public abstract void Run();

	public abstract void ApplyLocalization(string culture);

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		ApplyLocalization(args.Name);
	}

	public AdvancedFunction()
	{
		LocalizationManager.RegisterLocalizationProvider(this);
		runCommand = new RelayCommand(o => true, o => Run());
	}
}
