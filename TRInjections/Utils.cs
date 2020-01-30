using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TRInjections
{
	public class Utils
	{
		public static void RevealMap()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
				for (int j = 0; j < Main.maxTilesY; j++)
					Main.Map.UpdateLighting(i, j, 255);
			Main.refreshMap = true;
		}

		public static void RightClickTPCheck()
		{
			if (!Main.mapFullscreen|| !Main.mouseRightRelease|| !Main.mouseRight)
				return;
			Main.mouseRightRelease = false;
			Main.mapFullscreen = false;
			Main.player[Main.myPlayer].position = MouseToWorld(new Vector2(Main.mouseX, Main.mouseY)) * 16;
		}
		public static Vector2 MouseToWorld(Vector2 Mouse)
		{
			var v = new Vector2(Terraria.Main.mapFullscreenPos.X - ((Terraria.Main.screenWidth / 2f) - Mouse.X) / Terraria.Main.mapFullscreenScale, Terraria.Main.mapFullscreenPos.Y - ((Terraria.Main.screenHeight / 2f) - Mouse.Y) / Terraria.Main.mapFullscreenScale);
			v.X = v.X >= Terraria.Main.maxTilesX ? Terraria.Main.maxTilesX : v.X;
			v.Y = v.Y >= Terraria.Main.maxTilesY ? Terraria.Main.maxTilesY : v.Y;
			return v;
		}
	}
}
