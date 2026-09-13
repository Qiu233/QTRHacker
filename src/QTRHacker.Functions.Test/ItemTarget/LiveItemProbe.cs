using System.Runtime.CompilerServices;
using System.Threading;

namespace VehItemTarget;

// Loaded into an already running Terraria by the existing assembly loader.
// Owns a private item; nothing is inserted into a player or world inventory.
public sealed class LiveItemProbe
{
	public static Terraria.Item Scratch;
	public static int Calls;
	public static int Initialize(string unused)
	{
		new LiveItemProbe();
		return 1;
	}
	public LiveItemProbe()
	{
		Reset();
		Release(false); // Prepare the cleanup method too.
	}
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Reset()
	{
		Scratch = new Terraria.Item();
		Exercise(1); // JIT both the wrapper and SetDefaults before publishing it.
		Calls = 0;
	}
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Exercise(int type)
	{
		Scratch.SetDefaults(type);
		Interlocked.Increment(ref Calls);
	}
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Release(bool release)
	{
		if (release) Scratch = null;
	}
}
