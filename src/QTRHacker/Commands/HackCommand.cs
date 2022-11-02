using QTRHacker.EventManagers;
using System.Windows;

namespace QTRHacker.Commands;

public class HackCommand : RelayCommand, IWeakEventListener
{
	public HackCommand(Action<object> execute) : base(CanExecuteInternal, execute)
	{
		HackInitializedEventManager.AddListener(this);
	}
	private static bool CanExecuteInternal(object o)
	{
		return HackGlobal.IsActive;
	}

	public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
	{
		if (managerType == typeof(HackInitializedEventManager))
		{
			TriggerCanExecuteChanged();
			return true;
		}
		return false;
	}
}
