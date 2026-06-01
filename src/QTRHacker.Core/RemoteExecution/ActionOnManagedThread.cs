using QHackLib.Assemble;

namespace QTRHacker.Core.RemoteExecution;

public class ActionOnManagedThread
{
	public GameContext Context { get; }
	public AssemblyCode Code { get; }
	public uint Size { get; }
	public ActionOnManagedThread(GameContext ctx, AssemblyCode code, uint size = 0x1000)
	{
		Context = ctx;
		Code = code;
		Size = size;
	}

	/// <summary>
	/// Executes the code. Returns null if the managed thread path failed and
	/// fallback to RunByHookUpdate was used. Callers should null-check before
	/// calling WaitToDispose.
	/// </summary>
	public QHackLib.FunctionHelper.RemoteThread Execute() => Context.RunOnManagedThread(Code, Size);
}
