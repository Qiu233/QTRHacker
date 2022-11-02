using System.Windows;
using System.Windows.Media;

namespace QTRHacker.EventManagers;

public class RenderingEventManager : WeakEventManager
{
	public static void AddListener(IWeakEventListener listener) => CurrentManager.ProtectedAddListener(null, listener);

	public static void RemoveListener(IWeakEventListener listener) => CurrentManager.ProtectedRemoveListener(null, listener);

	private static RenderingEventManager CurrentManager
	{
		get
		{
			var managerType = typeof(RenderingEventManager);
			var manager = (RenderingEventManager)GetCurrentManager(managerType);
			if (manager == null)
			{
				manager = new RenderingEventManager();
				SetCurrentManager(managerType, manager);
			}
			return manager;
		}
	}

	protected sealed override void StartListening(object source) => CompositionTarget.Rendering += OnCompositionTargetRendering;

	protected sealed override void StopListening(object source) => CompositionTarget.Rendering -= OnCompositionTargetRendering;

	void OnCompositionTargetRendering(object sender, EventArgs e) => DeliverEvent(null, e);
}
