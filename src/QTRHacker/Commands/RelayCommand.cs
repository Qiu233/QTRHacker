using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QTRHacker.Commands
{
	public class RelayCommand : ICommand
	{
		private readonly Predicate<object> _canExecute;
		private readonly Action<object> _execute;

		public RelayCommand(Predicate<object> canExecute, Action<object> execute)
		{
			_canExecute = canExecute;
			_execute = execute;
		}

		public event EventHandler CanExecuteChanged;

		public void TriggerCanExecuteChanged()
		{
			Application.Current.Dispatcher.Invoke(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}
	}
}
