using Microsoft.Xna.Framework;
using QTRInjectionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;

namespace TRInjections
{
	public class AimBot
	{
		public static int Dist = 100;
		public static float Dist_Mouse = 128;
		public static bool Enabled = false;

		public static int TargetIndex = 0;

		static AimBot()
		{
			IMain.PreMainCalled += Update_Pre;
		}


		public static void Update_Pre()
		{
			if (!Enabled)
				return;
			var player = Main.LocalPlayer;
			var gun = player.inventory[Main.LocalPlayer.selectedItem];
			float bulletSpeed = 0, knockBack = 0;
			int shoot = 0, damage = 0;
			bool canShoot = true;
			player.PickAmmo(gun, ref shoot, ref bulletSpeed, ref canShoot, ref damage, ref knockBack, true);
			bulletSpeed += gun.shootSpeed;
			var Target = Main.player[TargetIndex];
			if (Target != null && Target.active)
			{
				Vector2 mobV = Target.velocity;
				Vector2 a = Main.LocalPlayer.Center - Target.Center;
				Vector2 g = new Vector2(0, 0);//怪物的运动向量
				if (mobV.LengthSquared() != 0 && a.LengthSquared() != 0)
				{
					float cos_t = Vector2.Dot(mobV, a) / (mobV.Length() * a.Length());
					float d = a.Length();
					float k = mobV.Length() / bulletSpeed;

					//开始复杂的计算
					//delta  = 4k^2d^2cos_t^2-4d^2(k^2-1)
					double _delta = 4 * Math.Pow(k, 2) * Math.Pow(d, 2) * Math.Pow(cos_t, 2) - 4 * Math.Pow(d, 2) * (Math.Pow(k, 2) - 1);
					//negative b  = 2kd*cos_t
					double _nb = 2 * k * d * cos_t;
					//2a  = 2(k^2-1)
					double _2a = 2 * (Math.Pow(k, 2) - 1);
					float t = (float)((_nb - Math.Sqrt(_delta)) / _2a);//t为子弹要运动的距离，这里的正负号取负，如果取正则计算出的目标坐标在怪物身后
					float kt = k * t;//kt为怪物的运动距离
					g = Vector2.Normalize(mobV) * kt;//g为怪物的运动向量
				}
				Vector2 j = Target.Center + g - Main.LocalPlayer.Center;//j为子弹的运动向量
				Vector2 y = Main.LocalPlayer.Center - Main.screenPosition + Dist_Mouse * Vector2.Normalize(j);//将其缩放并应用到玩家坐标上
				PlayerInput.MouseX = (int)Math.Round(y.X);
				PlayerInput.MouseY = (int)Math.Round(y.Y);
			}
		}
		public static NPC FindNPC()
		{
			NPC npc = null;
			Vector2 v = new Vector2(float.MaxValue, float.MaxValue);
			float f = Dist * 16f;
			foreach (var n in Main.npc)
			{
				if (Vector2.Distance(n.Center, Main.LocalPlayer.Center) > f)
					continue;
				Vector2 y = Main.LocalPlayer.Center - n.Center;
				if (n.active && (v = (v.Length() > y.Length() ? y : v)) == y)
					npc = n;
			}
			return npc;
		}
	}
}
