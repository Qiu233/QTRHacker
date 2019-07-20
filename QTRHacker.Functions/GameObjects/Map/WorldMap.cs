using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.Map
{
	[GameFieldOffsetTypeName("Terraria.Map.WorldMap")]
	public class WorldMap : GameObject
	{
		public WorldMap(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
		public bool UpdateLighting(int x, int y, byte light)
		{
			int ret = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Map.WorldMap", "UpdateLighting"),
				ret,
				true,
				BaseAddress, x, y, light);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
			bool rv = false;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, ret, ref rv, 1, 0);
			NativeFunctions.VirtualFreeEx(Context.HContext.Handle, ret, 0);
			return rv;
		}
	}
}
