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
	[GameFieldOffsetTypeName("Terraria.Projectile")]
	public class Projectile : GameObject
	{
		public Projectile(GameContext context, int bAddr) : base(context, bAddr)
		{

		}
		public static AssemblySnippet GetSnippet_Call_NewProjectile(GameContext Context, int? ret, bool regProtection, object X, object Y, object SpeedX, object SpeedY, object Type, object Damage, object KnockBack, object Owner, object ai0, object ai1)
		{
			return AssemblySnippet.FromDotNetCall(
				Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Projectile", "NewProjectile"),
				ret,
				regProtection,
				Type, Damage, Y, X, SpeedY, SpeedX, KnockBack, Owner, ai0, ai1);
		}
		public static int NewProjectile(GameContext Context, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0f, float ai1 = 0f)
		{
			int ret = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);

			AssemblySnippet snippet = GetSnippet_Call_NewProjectile(
				Context,
				ret,
				true,
				X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, ret, ref ret, 4, 0);
			NativeFunctions.VirtualFreeEx(Context.HContext.Handle, ret, 0);
			return ret;
		}
	}
}
