using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.Terraria
{
	public partial class NPC : Entity
	{
		public NPC(GameContext context, HackObject obj) : base(context, obj)
		{
		}


		public static void NewNPC(GameContext Context, int x, int y, int type, int start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int target = 255)
		{
			Context.RunByHookOnUpdate(
				new HackMethod(Context.HContext,
					Context.GameModuleHelper.GetClrMethodBySignature("Terraria.NPC",
					"Terraria.NPC.NewNPC(Int32, Int32, Int32, Int32, Single, Single, Single, Single, Int32)"))
				.Call(null)
				.Call(true, null, null, new object[] { x, y, type, start, ai0, ai1, ai2, ai3, target }));
		}

		public void AddBuff(int type, int time, bool quiet = false)
		{
			Context.RunByHookOnUpdate(TypedInternalObject.GetMethodCall("Terraria.NPC.AddBuff(Int32, Int32, Boolean)")
				.Call(true, null, null, new object[] { type, time, quiet }));
		}
	}
}
