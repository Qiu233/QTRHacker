using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Res;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsGraphicsDevice;
using STile = QTRHacker.Functions.PatchesManager.STile;

namespace QTRHacker.Tile
{
	public class TileView : GraphicsDeviceControl
	{
		private readonly Dictionary<int, Texture2D> TileTextures = new Dictionary<int, Texture2D>();
		private readonly Dictionary<int, Texture2D> WallTextures = new Dictionary<int, Texture2D>();
		private Texture2D Pixel;
		private SpriteBatch Batch;
		private STile[,] TilesData;
		public TileView()
		{

		}

		public void SetData(STile[,] data)
		{
			lock (this)
			{
				TilesData = data.Clone() as STile[,];
				Invalidate();
			}
		}

		private Texture2D GetTileTexture(int tile)
		{
			if (TileTextures.TryGetValue(tile, out Texture2D t))
				return t;
			Texture2D texture = null;
			if (GameResLoader.TileImageData.TryGetValue($"Tiles_{tile}", out byte[] value))
			{
				using var s = new MemoryStream(value);
				texture = Texture2D.FromStream(GraphicsDevice, s);
			}
			return TileTextures[tile] = texture;
		}
		private Texture2D GetWallTexture(int wall)
		{
			if (WallTextures.TryGetValue(wall, out Texture2D t))
				return t;
			Texture2D texture = null;
			if (GameResLoader.WallImageData.TryGetValue($"Wall_{wall}", out byte[] value))
			{
				using var s = new MemoryStream(value);
				texture = Texture2D.FromStream(GraphicsDevice, s);
			}
			return WallTextures[wall] = texture;
		}

		private void DrawTiles()
		{
			if (TilesData == null)
				return;
			int width = TilesData.GetLength(0);
			int height = TilesData.GetLength(1);
			Vector2 targetView = new Vector2(Width, Height);
			Vector2 targetSize = new Vector2(width, height) * 16;
			float scale = Math.Min(Width / targetSize.X, Height / targetSize.Y);
			scale = scale > 1.5f ? 1.5f : scale;
			DrawBG((int)Math.Round(16 * scale));
			Vector2 position = (targetView - targetSize * scale) / 2;
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					STile tile = TilesData[x, y];
					Vector2 pos = position + new Vector2(x - 0.5f, y - 0.5f) * 16 * scale;
					if (tile.Wall > 0)
					{
						Texture2D wallTexture = GetWallTexture(tile.Wall);
						Batch.Draw(wallTexture, pos, new Rectangle(tile.WallFrameX(), tile.WallFrameY(), 32, 32), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
					}
				}
			}
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					STile tile = TilesData[x, y];
					Vector2 pos = position + new Vector2(x, y) * 16 * scale;
					if (tile.Active())
					{
						Texture2D tileTexture = GetTileTexture(tile.Type);
						Batch.Draw(tileTexture, pos, new Rectangle(tile.FrameX, tile.FrameY, 16, 16), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
					}
				}
			}
		}

		private void DrawBG(int step)
		{
			int width = Width / step + 1, height = Height / step + 1;
			for (int i = 0; i < width; i += 2)
			{
				for (int j = 0; j < height; j += 2)
				{
					Batch.Draw(Pixel, new Rectangle(i * step, j * step, step, step), new Color(191, 191, 191));
					Batch.Draw(Pixel, new Rectangle(i * step, (j + 1) * step, step, step), Color.White);
					Batch.Draw(Pixel, new Rectangle((i + 1) * step, j * step, step, step), Color.White);
					Batch.Draw(Pixel, new Rectangle((i + 1) * step, (j + 1) * step, step, step), new Color(191, 191, 191));
				}
			}
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				Pixel?.Dispose();
				foreach (var t in TileTextures.Values)
					t?.Dispose();
			}
		}

		protected override void Draw()
		{
			lock (this)
			{
				GraphicsDevice.Clear(new Color(BackColor.R, BackColor.G, BackColor.B));
				Batch.Begin();
				DrawTiles();
				Batch.End();
			}
		}

		protected override void Initialize()
		{
			Batch = new SpriteBatch(GraphicsDevice);
			Pixel = new Texture2D(GraphicsDevice, 1, 1);
			Pixel.SetData(new Color[] { new Color(255, 255, 255) });
		}

	}
}
