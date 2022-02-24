using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PagePanels
{
	public class PagePanelViewModel : ViewModelBase
	{
		private bool isSelected;
		private bool isEnabled;

		public bool IsSelected
		{
			get => isSelected;
			set
			{
				isSelected = value;
				OnPropertyChanged(nameof(IsSelected));
			}
		}

		public bool IsEnabled
		{
			get => isEnabled;
			set
			{
				isEnabled = value;
				OnPropertyChanged(nameof(IsEnabled));
			}
		}
	}
}
