using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class Projectile : GameObject
	{
		public Projectile(GameContext context, int bAddr) : base(context, bAddr)
		{

		}
		public static int NewProjectile(GameContext Context, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0f, float ai1 = 0f)
		{
			int ret = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Projectile::NewProjectile"),
				ret,
				true,
				Type, Damage, Y, X, SpeedY, SpeedX, KnockBack, Owner, ai0, ai1);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, ret, ref ret, 4, 0);
			NativeFunctions.VirtualFreeEx(Context.HContext.Handle, ret, 0);
			return ret;
		}
	}
}
