using QHackLib;

namespace QTRHacker.Core.GameObjects.Terraria;

public partial class NPC : Entity
{
	public const int MaxNPCTypes = 670;
	public NPC(GameContext context, HackObject obj) : base(context, obj)
	{
	}


	public static void NewNPC(GameContext Context, int x, int y, int type, int start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int target = 255)
	{
		Context.RunByHookUpdate(
			new HackMethod(Context.HContext,
				Context.GameModuleHelper.GetClrMethodBySignature("Terraria.NPC",
				"Terraria.NPC.NewNPC(Terraria.DataStructures.IEntitySource, Int32, Int32, Int32, Int32, Single, Single, Single, Single, Int32)"))
			.Call(null)
			.Call(true, null, null, new object[] { 0, x, y, type, start, ai0, ai1, ai2, ai3, target }));
	}

	public void AddBuff(int type, int time, bool quiet = false)
	{
		Context.RunByHookUpdate(TypedInternalObject.GetMethodCall("Terraria.NPC.AddBuff(Int32, Int32, Boolean)")
			.Call(true, null, null, new object[] { type, time, quiet }));
	}
}
