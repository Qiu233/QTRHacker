using QTRHacker.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PagePanels
{
	public class FunctionsBoxViewModel : ViewModelBase
	{
		public ObservableCollection<BaseFunction> Functions { get; } = new();
	}
}
