using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace QTRHacker.Patches
{
	public class WorldPainter
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct STile
		{
			public ushort Type;
			public ushort Wall;
			public byte Liquid;
			public short STileHeader;
			public byte BTileHeader;
			public byte BTileHeader2;
			public byte BTileHeader3;
			public short FrameX;
			public short FrameY;

			public bool Active()
			{
				return (STileHeader & 0x20) == 0x20;
			}
			public int WallFrameX()
			{
				return (BTileHeader2 & 0xF) * 36;
			}
			public int WallFrameY()
			{
				return (BTileHeader3 & 7) * 36;
			}
		}
		public static bool BrushActive, EyeDropperActive;
		private static bool Brushing, Dropping;
		private static bool Loading;
		private unsafe static IntPtr Buffer;
		private static Vector2 BeginPos, EndPos;
		private static Vector2 BrushBeginPos;
		private static STile[,] ClipBoard;
		private static readonly Texture2D magicPixel;

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool VirtualFree(
			IntPtr lpAddress,
			IntPtr dwSize,
			uint dwFreeType = 0x00008000);

		static WorldPainter()
		{
			HooksDef.DoUpdateHook.Pre += DoUpdateHook_Pre;
			Boot.OnGameDraw += Boot_OnGameDraw;

			magicPixel = new Texture2D(Main.instance.GraphicsDevice, 1, 1);
			magicPixel.SetData(new Color[] { new Color(255, 255, 255) });
		}

		private static Color ProcessColor(Color newColor, float R, float G, float B, float A)
		{
			newColor.R = (byte)(newColor.R * R);
			newColor.G = (byte)(newColor.G * G);
			newColor.B = (byte)(newColor.B * B);
			newColor.A = (byte)(newColor.A * A);
			return newColor;
		}

		public static void DrawPreview(SpriteBatch sb, Vector2 position)
		{
			int width = ClipBoard.GetLength(0);
			int height = ClipBoard.GetLength(1);


			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					STile tile = GetClipboard(x, y);
					if (tile.Wall > 0)
					{
						Vector2 pos = position + new Vector2(x * 16, y * 16);
						Texture2D wallTexture = Terraria.GameContent.TextureAssets.Wall[tile.Wall].Value;
						sb.Draw(wallTexture, pos, new Rectangle(tile.WallFrameX(), tile.WallFrameY(), 32, 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
			}

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					STile tile = GetClipboard(x, y);
					if (tile.Active())
					{
						if (Terraria.GameContent.TextureAssets.Tile[tile.Type] == null || !Terraria.GameContent.TextureAssets.Tile[tile.Type].IsLoaded)
							Main.instance.LoadTiles(tile.Type);
						Texture2D texture = Terraria.GameContent.TextureAssets.Tile[tile.Type].Value;
						Color color = Color.White;
						color.A = 160;
						Rectangle? value = new Rectangle(tile.FrameX, tile.FrameY, 16, 16);
						Vector2 pos = position + new Vector2(x * 16, y * 16);
						sb.Draw(texture, pos, value, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		private static void Boot_OnGameDraw(SpriteBatch batch)
		{
			if (Main.gameMenu || Main.playerInventory)
				return;
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
				Color color = ProcessColor(Color.White, r, g, b, a);

				batch.Draw(magicPixel, upperLeftScreen, value, color * scale, 0f, Vector2.Zero, 16f * brushSize, SpriteEffects.None, 0f);
				b = 0.3f;
				g = 0.95f;
				scale = (a = 1f);
				color = ProcessColor(Color.White, r, g, b, a);
				batch.Draw(magicPixel,
					upperLeftScreen + Vector2.UnitX * -2f, value, color * scale, 0f, Vector2.Zero, new Vector2(2f, 16f * brushSize.Y), SpriteEffects.None, 0f);
				batch.Draw(magicPixel,
					upperLeftScreen + Vector2.UnitX * 16f * brushSize.X, value, color * scale, 0f, Vector2.Zero, new Vector2(2f, 16f * brushSize.Y), SpriteEffects.None, 0f);
				batch.Draw(magicPixel,
					upperLeftScreen + Vector2.UnitY * -2f, value, color * scale, 0f, Vector2.Zero, new Vector2(16f * brushSize.X, 2f), SpriteEffects.None, 0f);
				batch.Draw(magicPixel,
					upperLeftScreen + Vector2.UnitY * 16f * brushSize.Y, value, color * scale, 0f, Vector2.Zero, new Vector2(16f * brushSize.X, 2f), SpriteEffects.None, 0f);
			}
			else if (BrushActive && InsideScreen())
			{
				int brushWidth = ClipBoard.GetLength(0);
				int brushHeight = ClipBoard.GetLength(1);
				Vector2 Size = new Vector2(brushWidth, brushHeight);
				Point point = (Main.MouseWorld + new Vector2((brushWidth + 1) % 2, (brushHeight + 1) % 2) * 8).ToTileCoordinates();
				point.X -= brushWidth / 2;
				point.Y -= brushHeight / 2;
				Vector2 dPos = point.ToWorldCoordinates(0, 0) - Main.screenPosition;

				if (!(Mouse.GetState().LeftButton == ButtonState.Pressed)) DrawPreview(batch, dPos);
				Rectangle value = new Rectangle(0, 0, 1, 1);
				float r = 1f;
				if (!(Mouse.GetState().LeftButton == ButtonState.Pressed)) r = .25f;
				float g = 0.9f;
				float b = 0.1f;
				float a = 1f;
				float scale = 0.6f;
				Color color = ProcessColor(Color.White, r, g, b, a);
				batch.Draw(magicPixel, dPos, value, color * scale, 0f, Vector2.Zero, 16f * Size, SpriteEffects.None, 0f);
				b = 0.3f;
				g = 0.95f;
				scale = (a = 1f);
				color = ProcessColor(Color.White, r, g, b, a);
				batch.Draw(magicPixel, dPos + Vector2.UnitX * -2f, value, color * scale, 0f, Vector2.Zero, new Vector2(2f, 16f * Size.Y), SpriteEffects.None, 0f);
				batch.Draw(magicPixel, dPos + Vector2.UnitX * 16f * Size.X, value, color * scale, 0f, Vector2.Zero, new Vector2(2f, 16f * Size.Y), SpriteEffects.None, 0f);
				batch.Draw(magicPixel, dPos + Vector2.UnitY * -2f, value, color * scale, 0f, Vector2.Zero, new Vector2(16f * Size.X, 2f), SpriteEffects.None, 0f);
				batch.Draw(magicPixel, dPos + Vector2.UnitY * 16f * Size.Y, value, color * scale, 0f, Vector2.Zero, new Vector2(16f * Size.X, 2f), SpriteEffects.None, 0f);
			}
		}
		private static bool InsideScreen()
		{
			return Main.mouseX > 0 && Main.mouseY > 0 && Main.mouseX < Main.screenWidth && Main.mouseY < Main.screenHeight;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static STile GetClipboard(int x, int y) => ClipBoard[x, y];

		private static bool LastLeftPressed = false;
		private static bool LastRightPressed = false;
		private static bool LastFocus = false;
		private static void DoUpdateHook_Pre()
		{
			if (Loading)
			{
				unsafe
				{
					IntPtr ptr = Buffer;
					int width = *(int*)ptr;
					int height = *(int*)(ptr + 4);

					STile[,] tiles = new STile[width, height];
					for (int x = 0; x < width; x++)
					{
						for (int y = 0; y < height; y++)
						{
							tiles[x, y] = *((STile*)(ptr + 8) + x * height + y);
						}
					}
					ClipBoard = tiles;
				}
				VirtualFree(Buffer, IntPtr.Zero);
				Loading = false;
			}
			if (Main.gameMenu || Main.playerInventory)
			{
				Brushing = false;
				BrushActive = false;
				return;
			}
			Player player = Main.LocalPlayer;
			bool inside = InsideScreen();
			bool leftDown = Mouse.GetState().LeftButton == ButtonState.Pressed && !LastLeftPressed && LastFocus && inside;
			bool leftUp = Mouse.GetState().LeftButton != ButtonState.Pressed && LastLeftPressed && LastFocus && inside;
			bool rightUp = Mouse.GetState().RightButton != ButtonState.Pressed && LastRightPressed && LastFocus && inside;
			LastLeftPressed = Mouse.GetState().LeftButton == ButtonState.Pressed;
			LastRightPressed = Mouse.GetState().RightButton == ButtonState.Pressed;
			LastFocus = Main.hasFocus;
			if (rightUp)
			{
				Dropping = false;
				Brushing = false;
				EyeDropperActive = false;
				BrushActive = false;
			}
			if (EyeDropperActive)
			{
				player.cursorItemIconID = ItemID.EmptyDropper;
				player.cursorItemIconEnabled = true;
			}
			if (BrushActive)
			{
				player.cursorItemIconID = ItemID.Paintbrush;
				player.cursorItemIconEnabled = true;
			}
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
						ClipBoard = null;
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
						ClipBoard = new STile[maxX - minX, maxY - minY];
						for (int x = minX; x < maxX; x++)
						{
							for (int y = minY; y < maxY; y++)
							{
								if (!WorldGen.InWorld(x, y))
									continue;
								Tile from = Framing.GetTileSafely(x, y);
								ClipBoard[x - minX, y - minY] = new STile
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
							$"    Totally [C/FF9933:{ClipBoard.Length}] blocks ([C/FF9933:{ClipBoard.GetLength(0)}] X [C/FF9933:{ClipBoard.GetLength(1)}])"
							, false, Color.White);
					}
				}
			}
			else if (BrushActive && ClipBoard != null)
			{
				int bWidth = ClipBoard.GetLength(0);
				int bHeight = ClipBoard.GetLength(1);
				Point Point = (Main.MouseWorld + new Vector2((bWidth + 1) % 2, (bHeight + 1) % 2) * 8).ToTileCoordinates();
				Point.X -= bWidth / 2;
				Point.Y -= bHeight / 2;
				if (leftDown)
				{
					player.mouseInterface = true;
					BrushBeginPos = Point.ToVector2();
					Brushing = true;
				}
				else if (leftUp)
				{
					player.mouseInterface = false;
					Brushing = false;
				}
				if (Brushing)
				{
					for (int x = 0; x < bWidth; x++)
					{
						for (int y = 0; y < bHeight; y++)
						{
							if (WorldGen.InWorld(x + Point.X, y + Point.Y))
							{
								Tile target = Framing.GetTileSafely(x + Point.X, y + Point.Y);
								int cycledX = ((x + Point.X - (int)BrushBeginPos.X) % bWidth + bWidth) % bWidth;
								int cycledY = ((y + Point.Y - (int)BrushBeginPos.Y) % bHeight + bHeight) % bHeight;

								STile tile = GetClipboard(cycledX, cycledY);
								target.type = tile.Type;
								target.wall = tile.Wall;
								target.liquid = tile.Liquid;
								target.sTileHeader = tile.STileHeader;
								target.bTileHeader = tile.BTileHeader;
								target.bTileHeader2 = tile.BTileHeader2;
								target.bTileHeader3 = tile.BTileHeader3;
								target.frameX = tile.FrameX;
								target.frameY = tile.FrameY;
							}
						}
					}

					for (int x = 0; x < bWidth; x++)
					{
						for (int y = 0; y < bHeight; y++)
						{
							if (WorldGen.InWorld(x + Point.X, y + Point.Y))
							{
								WorldGen.SquareTileFrame(x + Point.X, y + Point.Y, true);
							}
						}
					}
					if (Main.netMode == 1)
					{
						NetMessage.SendTileSquare(-1, Point.X + bWidth / 2, Point.Y + bHeight / 2, Math.Max(bWidth, bHeight));
					}
				}
			}
		}
	}
}
