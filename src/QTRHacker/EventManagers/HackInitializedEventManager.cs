using System.Windows;

namespace QTRHacker.EventManagers;

public class HackInitializedEventManager : WeakEventManager
{
	public static void AddListener(IWeakEventListener listener) => CurrentManager.ProtectedAddListener(null, listener);

	public static void RemoveListener(IWeakEventListener listener) => CurrentManager.ProtectedRemoveListener(null, listener);

	private static HackInitializedEventManager CurrentManager
	{
		get
		{
			var managerType = typeof(HackInitializedEventManager);
			var manager = (HackInitializedEventManager)GetCurrentManager(managerType);
			if (manager == null)
			{
				manager = new HackInitializedEventManager();
				SetCurrentManager(managerType, manager);
			}
			return manager;
		}
	}

	protected sealed override void StartListening(object source) => HackGlobal.Initialized += OnCompositionTargetRendering;

	protected sealed override void StopListening(object source) => HackGlobal.Initialized -= OnCompositionTargetRendering;

	void OnCompositionTargetRendering(object sender, EventArgs e) => DeliverEvent(null, e);
}
