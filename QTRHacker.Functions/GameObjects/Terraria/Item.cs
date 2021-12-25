using QHackLib;
using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.Terraria
{
	/// <summary>
	/// Wrapper for Terraria.Item
	/// </summary>
	public partial class Item : Entity
	{
		public Item(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}

		public void SetDefaults(int type) =>
			new ActionOnManagedThread(Context, TypedInternalObject.GetMethodCall("Terraria.Item.SetDefaults(Int32)").Call(true, null, null, type))
			.Execute()
			.WaitToDispose()
			.Wait();
	}
}
