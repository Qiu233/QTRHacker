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
		public static float Dist_Mouse = 256;

		public static int TargetIndex = 0;
		public static Entity TargetEntity = null;

		static AimBot()
		{
			IMain.PreMainCalled += Select;
			IMain.PreMainCalled += Aim;
		}


		public static void Select()
		{
			if (!Main.mouseRight)
				return;
			int num1 = -1, num2 = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
				if (num1 == -1 || Main.npc[i].Hitbox.Distance(Main.MouseWorld) < Main.npc[num1].Hitbox.Distance(Main.MouseWorld))
					num1 = i;
			for (int i = 0; i < Main.maxPlayers; i++)
				if (i != Main.myPlayer)
					if (num2 == -1 || Main.player[i].Hitbox.Distance(Main.MouseWorld) < Main.player[num2].Hitbox.Distance(Main.MouseWorld))
						num2 = i;
			if (Main.npc[num1].Hitbox.Distance(Main.MouseWorld) < Main.player[num2].Hitbox.Distance(Main.MouseWorld))
				TargetEntity = Main.npc[num1];
			else
				TargetEntity = Main.player[num2];
		}

		public static void Aim()
		{

			var player = Main.LocalPlayer;
			var gun = player.inventory[Main.LocalPlayer.selectedItem];


			if (!gun.ranged)
			{
				TargetEntity = null;
				return;
			}
			if (TargetEntity == null || !TargetEntity.active)
			{
				return;
			}


			float bulletSpeed = gun.shootSpeed, knockBack = 0;
			int shoot = 0, damage = 0;
			bool canShoot = true;
			player.PickAmmo(gun, ref shoot, ref bulletSpeed, ref canShoot, ref damage, ref knockBack, true);
			bulletSpeed *= 3f;
			var Target = TargetEntity;
			if (Target != null && Target.active)
			{
				Vector2 mobV = Target.velocity;
				Vector2 a = Main.LocalPlayer.Center - Target.Center;
				Vector2 g = new Vector2(0, 0);//怪物的运动向量
				if (mobV.LengthSquared() != 0 && a.LengthSquared() != 0)
				{
					double cos_t = Vector2.Dot(mobV, a) / (mobV.Length() * a.Length());
					double d = a.Length();
					double k = mobV.Length() / bulletSpeed;

					//开始复杂的计算
					//delta  = 4k^2d^2cos_t^2-4d^2(k^2-1)
					double _delta = 4 * Math.Pow(k, 2) * Math.Pow(d, 2) * Math.Pow(cos_t, 2) - 4 * Math.Pow(d, 2) * (Math.Pow(k, 2) - 1);
					//negative b  = 2kd*cos_t
					double _nb = 2 * k * d * cos_t;
					//2a  = 2(k^2-1)
					double _2a = 2 * (Math.Pow(k, 2) - 1);
					double t = (float)((_nb - Math.Sqrt(_delta)) / _2a);//t为子弹要运动的距离，这里的正负号取负，如果取正则计算出的目标坐标在怪物身后
					double kt = k * t;//kt为怪物的运动距离
					g = Vector2.Normalize(mobV) * (float)kt;//g为怪物的运动向量
				}
				Vector2 j = Target.Center + g - Main.LocalPlayer.Center;//j为子弹的运动向量
				Vector2 y = Main.LocalPlayer.Center - Main.screenPosition + Dist_Mouse * Vector2.Normalize(j);//将其缩放并应用到玩家坐标上
				PlayerInput.MouseX = (int)Math.Round(y.X);
				PlayerInput.MouseY = (int)Math.Round(y.Y);
			}
		}
	}
}
