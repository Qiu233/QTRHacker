using QTRHacker.Core;

namespace QTRHacker.Scripts;

public abstract class GameplayFeatureFunction : BaseFunction
{
	protected abstract GameplayFeature Feature { get; }
	public override bool CanDisable => true;

	public override void Enable(GameContext context)
	{
		context.Patches.SetGameplayFeature(Feature, true);
		IsEnabled = true;
	}

	public override void Disable(GameContext context)
	{
		context.Patches.SetGameplayFeature(Feature, false);
		IsEnabled = false;
	}
}
