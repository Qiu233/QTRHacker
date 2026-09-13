using System.Windows;
using System.Windows.Input;

namespace QTRHacker.Commands;

public class RelayCommand : ICommand
{
	private readonly Predicate<object> _canExecute;
	private readonly Action<object> _execute;

	public RelayCommand(Action<object> execute) : this(null, execute)
	{
	}

	public RelayCommand(Predicate<object> canExecute, Action<object> execute)
	{
		_canExecute = canExecute;
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
	}

	public event EventHandler CanExecuteChanged;

	public void TriggerCanExecuteChanged()
	{
		var dispatcher = Application.Current?.Dispatcher;
		if (dispatcher == null || dispatcher.CheckAccess())
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		else
			dispatcher.Invoke(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
	}

	public bool CanExecute(object parameter)
	{
		return _canExecute?.Invoke(parameter) ?? true;
	}

	public void Execute(object parameter)
	{
		_execute(parameter);
	}
}
