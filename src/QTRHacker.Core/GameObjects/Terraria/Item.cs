using QHackLib;
using QHackLib.Assemble;
using QHackLib.Memory;
using System.Runtime.InteropServices;
using QTRHacker.Core.GameObjects.ValueTypeRedefs.Xna;

namespace QTRHacker.Core.GameObjects.Terraria;

/// <summary>
/// Wrapper for Terraria.Item
/// </summary>
public partial class Item : GameObject
{
	// Nullable<Vector2> is a 12-byte value on Terraria's x86 CLR. Boxing a
	// null Vector2? would pass a single null pointer and unbalance the stack.
	[StructLayout(LayoutKind.Sequential)]
	private struct OptionalVector2
	{
		public int HasValue;
		public Vector2 Value;

		public OptionalVector2(Vector2? value)
		{
			HasValue = value.HasValue ? 1 : 0;
			Value = value.GetValueOrDefault();
		}
	}

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
		bool noBroadcast = false, int pfix = 0, NewItemOwnership ownership = NewItemOwnership.None, Vector2? velocity = null)
	{
		using MemoryAllocation ret = new(Context.HContext);

		Context.RunByHookUpdate(
			new HackMethod(Context.HContext,
				Context.GameModuleHelper.GetClrMethodBySignature("Terraria.Item",
				"Terraria.Item.NewItem(Terraria.DataStructures.IEntitySource, Int32, Int32, Int32, Int32, Int32, Int32, Boolean, Int32, Terraria.NewItemOwnership, System.Nullable`1<Microsoft.Xna.Framework.Vector2>, NewItemModifier)"))
			.Call(null)
			.Call(true, null, ret.AllocationBase, new object[] { 0, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, (int)ownership, new OptionalVector2(velocity), 0 }));

		return Context.HContext.DataAccess.Read<int>(ret.AllocationBase);
	}

	public static void RequestNewItem(GameContext context, Vector2 center, int type, int stack = 1,
		int prefix = 0, NewItemOwnership ownership = NewItemOwnership.None, Vector2? velocity = null)
	{
		context.RunByHookUpdate(new HackMethod(context.HContext,
			context.GameModuleHelper.GetClrMethodBySignature("Terraria.Item",
				"Terraria.Item.RequestNewItem(Terraria.DataStructures.IEntitySource, Microsoft.Xna.Framework.Vector2, Int32, Int32, Int32, Terraria.NewItemOwnership, System.Nullable`1<Microsoft.Xna.Framework.Vector2>, NewItemModifier)"))
			.Call(null)
			.Call(true, null, null, new object[] { 0, center, type, stack, prefix, (int)ownership, new OptionalVector2(velocity), 0 }));
	}
}
