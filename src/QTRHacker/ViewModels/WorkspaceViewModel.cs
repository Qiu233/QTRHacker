using QTRHacker.Commands;
using System.Windows.Input;

namespace QTRHacker.ViewModels;

public abstract class WorkspaceViewModel : ViewModelBase
{
	RelayCommand _closeCommand;
	public ICommand CloseCommand
	{
		get
		{
			return _closeCommand ??= new RelayCommand(
				   param => CanClose(),
				   param => Close());
		}
	}

	public event Action RequestClose;
	public event Action<bool?> RequestSetDialogResult;

	public virtual void Close() => RequestClose?.Invoke();

	public virtual bool CanClose() => true;

	public void SetDialogResult(bool? res) => RequestSetDialogResult?.Invoke(res);
}
