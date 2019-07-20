using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.NPC")]
	public class NPC : Entity
	{
		public const int MAXNUMBER = 201;


		public NPC(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
		public static void NewNPC(GameContext Context, int x, int y, int type, int start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int target = 255)
		{

			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.NPC", "NewNPC"),
				null,
				true,
				x, y, type, start, ai0, ai1, ai2, ai3, target);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
		}

		public void AddBuff(int type, int time, bool quiet = false)
		{

			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.NPC", "AddBuff"),
				null,
				true,
				BaseAddress, type, time, quiet);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
		}
	}
}
