using QTRHacker.Scripts;
using System.Collections.ObjectModel;

namespace QTRHacker.ViewModels.PagePanels;

public class FunctionsBoxViewModel : ViewModelBase
{
	public ObservableCollection<BaseFunction> Functions { get; } = new();
}
