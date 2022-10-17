namespace QTRHacker.ViewModels.PlayerEditor;

public abstract class PlayerEditorTabViewModelBase : ViewModelBase
{
	private bool updating;
	private string header;

	public bool Updating
	{
		get => updating;
		set
		{
			updating = value;
			OnPropertyChanged(nameof(Updating));
		}
	}

	public string Header
	{
		get => header;
		set
		{
			header = value;
			OnPropertyChanged(nameof(Header));
		}
	}
}
