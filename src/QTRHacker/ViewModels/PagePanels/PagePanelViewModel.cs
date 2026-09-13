using System.Windows;
using QTRHacker.EventManagers;

namespace QTRHacker.ViewModels.PagePanels;

public class PagePanelViewModel : ViewModelBase, IWeakEventListener
{
	private bool isSelected;
	private bool isEnabled = true;

	public bool IsSelected
	{
		get => isSelected;
		set => SetProperty(ref isSelected, value);
	}

	public bool IsEnabled
	{
		get => isEnabled;
		set => SetProperty(ref isEnabled, value);
	}

	public void RegisterHackInitEvent()
	{
		IsEnabled = false;
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
