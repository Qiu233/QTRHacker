using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.WorldGen")]
	public class WorldGen : GameObject
	{
		public WorldGen(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
		public static void SquareTileFrame(GameContext Context, int i, int j, bool resetFrame = true)
		{
			AssemblySnippet snippet = AssemblySnippet.FromClrCall(
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.WorldGen", "SquareTileFrame"),
				null,
				true,
				i, j, true);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), true);
		}
	}
}
