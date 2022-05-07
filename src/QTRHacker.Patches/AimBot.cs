using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;

namespace QTRHacker.Patches
{
	internal class AimBot
	{
		private enum AimBotMode : int
		{
			Disabled = 0,
			NearestNPC = 1,
			NearestPlayer = 2,
			TargetedPlayer = 3
		}
#pragma warning disable IDE0044 // 添加只读修饰符
		private static AimBotMode Mode = AimBotMode.Disabled;
		private static int TargetedPlayerIndex = -1;
#pragma warning restore IDE0044 // 添加只读修饰符
		public static float MaxDistance_NPC = 9600;
		public static bool HostileNPCsOnly = true;

		public static float MaxDistance_Player = 9600;
		public static bool HostilePlayersOnly = true;
		static AimBot()
		{
			HooksDef.DoUpdateHook.Pre += DoUpdateHook_Pre;
		}

		private static Entity GetTarget()
		{
			switch (Mode)
			{
				case AimBotMode.NearestNPC: return FindNPC();
				case AimBotMode.NearestPlayer: return FindPlayer();
				case AimBotMode.TargetedPlayer:
					{
						if (TargetedPlayerIndex < 0
							|| TargetedPlayerIndex >= Main.player.Length
							|| TargetedPlayerIndex == Main.myPlayer)
							return null;
						var player = Main.player[TargetedPlayerIndex];
						if (!player.active)
							return null;
						return player;
					}
				default: return null;
			}
		}

		private static void DoUpdateHook_Pre()
		{
			if (Mode == AimBotMode.Disabled)
				return;
			if (!Main.hasFocus)
				return;
			Entity p = GetTarget();
			if (p is null)
				return;
			var player = Main.LocalPlayer;
			var bulletStart = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
			var item = player.HeldItem;

			int projToShoot = item.shoot;
			float speed = item.shootSpeed;
			bool canShoot = true;
			int damage = item.damage;
			float knockBack = item.knockBack;
			Main.LocalPlayer.PickAmmo(item, ref projToShoot, ref speed, ref canShoot, ref damage, ref knockBack, out int _, true);
			Projectile proj = new Projectile();
			proj.SetDefaults(projToShoot);
			float bulletV = speed * (proj.extraUpdates + 1);
			if (bulletV == 0)
				return;
			var targetOffset = Calculate(p.Center, bulletStart, p.velocity, bulletV);
			var playerToTargetDst = p.Center + targetOffset - bulletStart;
			var mousePos = bulletStart - Main.screenPosition + 128 * Vector2.Normalize(playerToTargetDst);
			PlayerInput.MouseX = (int)Math.Round(mousePos.X);
			PlayerInput.MouseY = (int)Math.Round(mousePos.Y);
		}

		private static Vector2 Calculate(Vector2 targetPos, Vector2 playerPos, Vector2 targetV, float bulletV)
		{
			Vector2 d = playerPos - targetPos;
			if (targetV.LengthSquared() == 0 || d.LengthSquared() == 0)
				return new Vector2(0, 0);
			double k = targetV.Length() / bulletV;
			double cos_alpha = Vector2.Dot(targetV, d) / (targetV.Length() * d.Length());
			double a = (Math.Pow(k, 2) - 1) * Math.Pow(bulletV, 2);
			double b = -2 * d.Length() * k * bulletV * cos_alpha;
			double c = Math.Pow(d.Length(), 2);
			double delta = Math.Pow(b, 2) - 4 * a * c;
			float t = (float)((-b - Math.Sqrt(delta)) / (2 * a));
			return t * targetV;
		}

		private static T Find<T>(IEnumerable<T> ts, Predicate<T> condition = null) where T : Entity
		{
			T entity = null;
			Vector2 v = new Vector2(float.MaxValue, float.MaxValue);
			foreach (var n in ts)
			{
				if (condition != null && !condition(n))
					continue;
				Vector2 y = Main.LocalPlayer.Center - n.Center;
				if (n.active && (v = (v.Length() > y.Length() ? y : v)) == y)
					entity = n;
			}
			return entity;
		}

		private static NPC FindNPC()
		{
			return Find(Main.npc, e =>
				Vector2.Distance(e.Center, Main.LocalPlayer.Center) <= MaxDistance_NPC
				&& (!e.friendly || !HostileNPCsOnly));
		}

		private static Player FindPlayer()
		{
			return Find(Main.player, e =>
				e != Main.LocalPlayer
				&& Vector2.Distance(e.Center, Main.LocalPlayer.Center) <= MaxDistance_Player
				&& (e.InOpposingTeam(Main.LocalPlayer) || !HostilePlayersOnly));
		}
	}
}
