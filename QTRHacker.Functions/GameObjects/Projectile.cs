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
		public static AssemblySnippet GetSnippet_Call_NewProjectile(GameContext Context, int? ret, bool regProtection, object SpawnSource, object X, object Y, object SpeedX, object SpeedY, object Type, object Damage, object KnockBack, object Owner, object ai0, object ai1)
		{
			return AssemblySnippet.FromClrCall(
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Projectile", t => t.GetFullSignature() == "Terraria.Projectile.NewProjectile(Terraria.DataStructures.IProjectileSource, Single, Single, Single, Single, Int32, Int32, Single, Int32, Single, Single)"),
				ret,
				regProtection,
				SpawnSource, Type, X, Y, SpeedX, SpeedY, Damage, KnockBack, Owner, ai0, ai1);
		}
		public static int NewProjectile(GameContext Context, int SpawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0f, float ai1 = 0f)
		{
			int ret = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);

			AssemblySnippet snippet = GetSnippet_Call_NewProjectile(
				Context,
				ret,
				true,
				SpawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), true);
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, ret, ref ret, 4, 0);
			NativeFunctions.VirtualFreeEx(Context.HContext.Handle, ret, 0);
			return ret;
		}
	}
}
