using System.ComponentModel;
using System.Windows;

namespace QTRHacker.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string name)
	{
		Application.Current.Dispatcher.Invoke(
			() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)));
	}
}
