using QHackLib;

using QTRHacker.Core.GameObjects.ValueTypeRedefs.Xna;

namespace QTRHacker.Core.GameObjects.Terraria;

public class Projectile : Entity
{
	public Projectile(GameContext ctx, HackObject obj) : base(ctx, obj)
	{
	}

	public static void NewProjectile(GameContext ctx, nuint? SpawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0f, float ai1 = 0f, float ai2=0f)
	{
		ctx.RunByHookUpdate(NewProjectileCode(ctx, SpawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2));
	}

	public static QHackLib.Assemble.AssemblyCode NewProjectileCode(GameContext ctx, nuint? SpawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0f, float ai1 = 0f, float ai2=0f)
	{
		return new HackMethod(ctx.HContext,
			ctx.GameModuleHelper.GetClrMethodBySignature("Terraria.Projectile",
			"Terraria.Projectile.NewProjectile(Terraria.DataStructures.IEntitySource, Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2, Int32, Int32, Single, Int32, Single, Single, Single, Terraria.NewProjectileModifier)"))
		.Call(null)
		.Call(true, null, null, new object[] { SpawnSource, new Vector2(X, Y), new Vector2(SpeedX, SpeedY), Type, Damage, KnockBack, Owner, ai0, ai1, ai2, (nuint)0 });
	}
}
