using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.NetMessage")]
	public class NetMessage : GameObject
	{
		public NetMessage(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
		public static void SendData(GameContext Context, int msgType, int remoteClient = -1, int ignoreClient = -1,
			int text = 0, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f,
			int number5 = 0, int number6 = 0, int number7 = 0)
		{
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.AddressHelper.GetFunctionAddress("Terraria.NetMessage", "SendData"),
				null,
				true,
				msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
		}
		public static void SendWater(GameContext Context, int x, int y)
		{
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.AddressHelper.GetFunctionAddress("Terraria.NetMessage", "sendWater"),
				null,
				true,
				x, y);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
		}
	}
}
