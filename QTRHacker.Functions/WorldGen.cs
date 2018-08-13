using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class WorldGen : GameObject
	{
		public WorldGen(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
		public static void SquareTileFrame(GameContext Context, int i, int j, bool resetFrame = true)
		{
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.WorldGen::SquareTileFrame"),
				null,
				true,
				i, j, true);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
		}
	}
}
