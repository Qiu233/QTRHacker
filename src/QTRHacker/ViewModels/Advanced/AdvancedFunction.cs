using QTRHacker.Commands;
using QTRHacker.Localization;
using System.Windows.Input;

namespace QTRHacker.ViewModels.Advanced;

public abstract class AdvancedFunction : ViewModelBase, ILocalizationProvider
{
	private string name;
	private readonly HackCommand runCommand;

	public string Name
	{
		get => name;
		set
		{
			name = value;
			OnPropertyChanged(nameof(Name));
		}
	}

	public ICommand RunCommand => runCommand;

	public abstract void Run();

	public abstract void ApplyLocalization(string culture);

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		ApplyLocalization(args.Name);
	}

	public AdvancedFunction()
	{
		LocalizationManager.RegisterLocalizationProvider(this);
		runCommand = new HackCommand(o => Run());
	}
}
