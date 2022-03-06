using QTRHacker.EventManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels.PagePanels
{
	public class PagePanelViewModel : ViewModelBase, IWeakEventListener
	{
		private bool isSelected;
		private bool isEnabled = true;

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

		public PagePanelViewModel()
		{
		}

		public void RegisterHackInitEvent()
		{
			isEnabled = false;
			HackInitializedEventManager.AddListener(this);
		}

		public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(HackInitializedEventManager))
			{
				IsEnabled = true;
				return true;
			}
			return false;
		}
	}
}
