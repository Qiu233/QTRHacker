using QHackLib;
using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.Terraria
{
	public class Projectile : Entity
	{
		public Projectile(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}

		public void NewProjectile(nuint? SpawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0f, float ai1 = 0f)
		{
			Context.RunByHookOnUpdate(
					new HackMethod(Context.HContext,
					Context.GameModuleHelper.GetClrMethodBySignature("Terraria.Projectile", 
					"Terraria.Projectile.NewProjectile(Terraria.DataStructures.IProjectileSource, Single, Single, Single, Single, Int32, Int32, Single, Int32, Single, Single)"))
				.Call(null)
				.Call(true, null, null, new object[] { SpawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1 }));
		}
	}
}
