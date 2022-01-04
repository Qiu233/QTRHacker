using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QTRHacker.Contrast.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace QTRHacker.Patches
{
	public class WorldPainter
	{
		public static bool BrushActive, EyeDropperActive;
		public static bool Brushing, Dropping;
		public static Vector2 BeginPos, EndPos;
		public static Vector2 BrushBeginPos;
		public static STile[,] Tiles = new STile[0, 0];

		static WorldPainter()
		{
			HooksDef.DoUpdateHook.Pre += DoUpdateHook_Pre;
			Boot.OnGameDraw += Boot_OnGameDraw;
		}

		private static Color BuffColor(Color newColor, float R, float G, float B, float A)
		{
			newColor.R = (byte)(newColor.R * R);
			newColor.G = (byte)(newColor.G * G);
			newColor.B = (byte)(newColor.B * B);
			newColor.A = (byte)(newColor.A * A);
			return newColor;
		}

		private static void Boot_OnGameDraw(SpriteBatch batch)
		{
			if (Dropping)
			{
				Vector2 upperLeft = new Vector2(Math.Min(BeginPos.X, EndPos.X), Math.Min(BeginPos.Y, EndPos.Y));
				Vector2 lowerRight = new Vector2(Math.Max(BeginPos.X, EndPos.X) + 1, Math.Max(BeginPos.Y, EndPos.Y) + 1);
				Vector2 upperLeftScreen = upperLeft * 16f - Main.screenPosition;
				Vector2 lowerRightScreen = lowerRight * 16f - Main.screenPosition;
				Vector2 brushSize = lowerRight - upperLeft;
				Rectangle value = new Rectangle(0, 0, 1, 1);
				float r = 1f;
				float g = 0.9f;
				float b = 0.1f;
				float a = 1f;
				float scale = 0.6f;
				Color color = BuffColor(Color.White, r, g, b, a);

				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, upperLeftScreen, value, color * scale, 0f, Vector2.Zero, 16f * brushSize, SpriteEffects.None, 0f);
				b = 0.3f;
				g = 0.95f;
				scale = (a = 1f);
				color = BuffColor(Color.White, r, g, b, a);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value,
					upperLeftScreen + Vector2.UnitX * -2f, value, color * scale, 0f, Vector2.Zero, new Vector2(2f, 16f * brushSize.Y), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value,
					upperLeftScreen + Vector2.UnitX * 16f * brushSize.X, value, color * scale, 0f, Vector2.Zero, new Vector2(2f, 16f * brushSize.Y), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value,
					upperLeftScreen + Vector2.UnitY * -2f, value, color * scale, 0f, Vector2.Zero, new Vector2(16f * brushSize.X, 2f), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value,
					upperLeftScreen + Vector2.UnitY * 16f * brushSize.Y, value, color * scale, 0f, Vector2.Zero, new Vector2(16f * brushSize.X, 2f), SpriteEffects.None, 0f);
			}
			if (EyeDropperActive && !Main.gameMenu && !Main.playerInventory)
			{
				Main.LocalPlayer.cursorItemIconID = ItemID.EmptyDropper;
				Main.LocalPlayer.cursorItemIconEnabled = true;
			}
		}

		private static void DoUpdateHook_Pre()
		{
			Player player = Main.LocalPlayer;

			bool leftDown = Mouse.GetState().LeftButton == ButtonState.Pressed && Main.mouseLeftRelease && Main.mouseX > 0 && Main.mouseY > 0;
			bool leftUp = Mouse.GetState().LeftButton != ButtonState.Pressed && !Main.mouseLeftRelease && Main.mouseX > 0 && Main.mouseY > 0;

			if (EyeDropperActive)
			{
				if (Dropping)
					EndPos = new Vector2(Player.tileTargetX, Player.tileTargetY);
				if (leftDown)
				{
					Main.mouseLeftRelease = false;
					player.mouseInterface = true;
					if (!Dropping)
					{
						Dropping = true;
						EndPos = BeginPos = new Vector2(Player.tileTargetX, Player.tileTargetY);
						Tiles = null;
					}
					else if (Dropping)
					{
						Dropping = false;
						EndPos = new Vector2(Player.tileTargetX, Player.tileTargetY);
						Vector2 upperLeft = new Vector2(Math.Min(BeginPos.X, EndPos.X), Math.Min(BeginPos.Y, EndPos.Y));
						Vector2 lowerRight = new Vector2(Math.Max(BeginPos.X, EndPos.X), Math.Max(BeginPos.Y, EndPos.Y));
						int minX = (int)upperLeft.X;
						int maxX = (int)lowerRight.X + 1;
						int minY = (int)upperLeft.Y;
						int maxY = (int)lowerRight.Y + 1;
						Tiles = new STile[maxX - minX, maxY - minY];
						for (int x = minX; x < maxX; x++)
						{
							for (int y = minY; y < maxY; y++)
							{
								if (!WorldGen.InWorld(x, y))
									continue;
								Tile from = Framing.GetTileSafely(x, y);
								Tiles[x - minX, y - minY] = new STile
								{
									Type = from.type,
									Wall = from.wall,
									Liquid = from.liquid,
									STileHeader = from.sTileHeader,
									BTileHeader = from.bTileHeader,
									BTileHeader2 = from.bTileHeader2,
									BTileHeader3 = from.bTileHeader3,
									FrameX = from.frameX,
									FrameY = from.frameY
								};
							}
						}
						Main.NewTextMultiline($"Area selected:\n" +
							$"    From [C/FF9933:({minX}, {minY})] to [C/FF9933:({maxX}, {maxY})]\n" +
							$"    Totally [C/FF9933:{Tiles.Length}] blocks ([C/FF9933:{Tiles.GetLength(0)}] X [C/FF9933:{Tiles.GetLength(1)}])"
							, false, Color.White);
					}
				}
			}
		}
	}
}
