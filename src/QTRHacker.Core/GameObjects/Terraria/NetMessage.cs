using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.GameObjects.Terraria
{
	public class NetMessage : GameObject
	{
		public NetMessage(GameContext context, HackObject obj) : base(context, obj)
		{
		}
		public static void SendData(GameContext Context, int msgType, int remoteClient = -1, int ignoreClient = -1,
			object text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f,
			int number5 = 0, int number6 = 0, int number7 = 0)
		{
			Context.RunByHookUpdate(
					new HackMethod(Context.HContext,
					Context.GameModuleHelper.GetClrMethodBySignature("Terraria.NetMessage",
					"Terraria.NetMessage.SendData(Int32, Int32, Int32, Terraria.Localization.NetworkText, Int32, Single, Single, Single, Int32, Int32, Int32)"))
				.Call(null)
				.Call(true, null, null, new object[] { msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7 }));
		}
		public static void SendWater(GameContext Context, int x, int y)
		{
			Context.RunByHookUpdate(
					new HackMethod(Context.HContext,
					Context.GameModuleHelper.GetClrMethodBySignature("Terraria.NetMessage",
					"Terraria.NetMessage.sendWater(Int32, Int32)"))
				.Call(null)
				.Call(true, null, null, new object[] { x, y }));
		}
	}
}
