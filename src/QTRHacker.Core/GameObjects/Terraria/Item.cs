using QHackLib;
using QHackLib.Assemble;
using QHackLib.Memory;
using QTRHacker.Core.GameObjects.ValueTypeRedefs.Xna;

namespace QTRHacker.Core.GameObjects.Terraria;

/// <summary>
/// Wrapper for Terraria.Item
/// </summary>
public partial class Item : Entity
{
	public Item(GameContext ctx, HackObject obj) : base(ctx, obj)
	{
	}

	public void SetDefaults(int type)
	{
		Context.RunByHookUpdate(TypedInternalObject.GetMethodCall("Terraria.Item.SetDefaults(Int32, Terraria.GameContent.Items.ItemVariant)")
			.Call(true, null, null, new object[] { type, (nuint)0 }));
	}

	public void SetPrefix(int prefix)
	{
		Context.RunByHookUpdate(TypedInternalObject.GetMethodCall("Terraria.Item.Prefix(Int32)")
			.Call(true, null, null, new object[] { prefix }));
	}

	/// <summary>
	/// Calling this is much more effective than calling the two functions separately.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="prefix"></param>
	public void SetDefaultsAndPrefix(int type, int prefix)
	{
		Context.RunByHookUpdate(AssemblySnippet.FromCode(
			new AssemblyCode[] {
				Instruction.Create("push ecx"),
				Instruction.Create("push edx"),
				TypedInternalObject.GetMethodCall("Terraria.Item.SetDefaults(Int32, Terraria.GameContent.Items.ItemVariant)").Call(false, null, null, new object[] { type, (nuint)0 }),
				TypedInternalObject.GetMethodCall("Terraria.Item.Prefix(Int32)").Call(false, null, null, new object[] { prefix }),
				Instruction.Create("pop edx"),
				Instruction.Create("pop ecx")
			}));
	}


	public static int NewItem(GameContext Context, int X, int Y, int Width, int Height, int Type, int Stack = 1,
		bool noBroadcast = false, int pfix = 0, bool noGrabDelay = false)
	{
		using MemoryAllocation ret = new(Context.HContext);

		Context.RunByHookUpdate(
			new HackMethod(Context.HContext,
				Context.GameModuleHelper.GetClrMethodBySignature("Terraria.Item",
				"Terraria.Item.NewItem(Terraria.DataStructures.IEntitySource, Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2, Int32, Int32, Boolean, Int32, Boolean)"))
			.Call(null)
			.Call(true, null, ret.AllocationBase, new object[] { 0, new Vector2(X, Y), new Vector2(Width, Height), Type, Stack, noBroadcast, pfix, noGrabDelay }));

		return Context.HContext.DataAccess.Read<int>(ret.AllocationBase);
	}
}
