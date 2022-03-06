using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using STile = QTRHacker.Core.PatchesManager.STile;
using Microsoft.Xna.Framework.Graphics;
using QTRHacker.EventManagers;

namespace QTRHacker.Views.Advanced.Schematics
{
	/// <summary>
	/// TileView.xaml 的交互逻辑
	/// </summary>
	public partial class TileView : UserControl, IWeakEventListener
	{
		private SpriteBatch Batch;
		private Texture2D Pixel;
		private readonly Dictionary<int, Texture2D> TileTextures = new();
		private readonly Dictionary<int, Texture2D> WallTextures = new();

		public STile[,] TilesData
		{
			get => (STile[,])GetValue(TilesDataProperty);
			set => SetValue(TilesDataProperty, value);
		}
		public static readonly DependencyProperty TilesDataProperty =
			DependencyProperty.Register(nameof(TilesData), typeof(STile[,]), typeof(TileView));

		public TileView()
		{
			InitializeComponent();
			XnaControl.Draw += XnaControl_Draw;
			XnaControl.LoadContent += XnaControl_LoadContent;
			XnaControl.Initialize += XnaControl_Initialize;
		}

		public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(RenderingEventManager))
			{
				XnaControl.Render();
				return true;
			}
			return false;
		}

		private Texture2D GetTileTexture(int tile)
		{
			if (TileTextures.TryGetValue(tile, out Texture2D t))
				return t;
			return TileTextures[tile] = XnaControl.ContentManager.Load<Texture2D>($"Images/Tiles_{tile}");
		}
		private Texture2D GetWallTexture(int wall)
		{
			if (WallTextures.TryGetValue(wall, out Texture2D t))
				return t;
			return WallTextures[wall] = XnaControl.ContentManager.Load<Texture2D>($"Images/Wall_{wall}");
		}

		private void XnaControl_LoadContent(Microsoft.Xna.Framework.Content.ContentManager obj)
		{
			XnaControl.ContentManager.RootDirectory = HackGlobal.GameContext.GameContentDir;
			Pixel = new Texture2D(XnaControl.GraphicsDevice, 1, 1);
			Pixel.SetData(new Color[] { new Color(255, 255, 255) });
		}

		private void XnaControl_Initialize()
		{
			Batch = new SpriteBatch(XnaControl.GraphicsDevice);
			RenderingEventManager.AddListener(this);
		}

		private void DrawBG(int step)
		{
			int width = (int)(ActualWidth / step + 1), height = (int)(ActualHeight / step + 1);
			for (int i = 0; i < width; i += 2)
			{
				for (int j = 0; j < height; j += 2)
				{
					Color light = new(0xA0, 0xA0, 0xA0);
					Color dark = new(0x50, 0x50, 0x50);
					Batch.Draw(Pixel, new Rectangle(i * step, j * step, step, step), dark);
					Batch.Draw(Pixel, new Rectangle(i * step, (j + 1) * step, step, step), light);
					Batch.Draw(Pixel, new Rectangle((i + 1) * step, j * step, step, step), light);
					Batch.Draw(Pixel, new Rectangle((i + 1) * step, (j + 1) * step, step, step), dark);
				}
			}
		}

		private void DrawBorder(Vector2 pos, int width, int height, float scale)
		{
			float w = width * 16 * scale;
			float h = height * 16 * scale;
			Color color = Color.White;
			Batch.Draw(Pixel, new Rectangle((int)pos.X, (int)pos.Y, (int)w, 1), color);
			Batch.Draw(Pixel, new Rectangle((int)pos.X, (int)pos.Y, 1, (int)h), color);
			Batch.Draw(Pixel, new Rectangle((int)pos.X, (int)(pos.Y + h), (int)w, 1), color);
			Batch.Draw(Pixel, new Rectangle((int)(pos.X + w), (int)pos.Y, 1, (int)h), color);
		}

		private void DrawTiles()
		{
			if (TilesData == null)
				return;
			int width = TilesData.GetLength(0);
			int height = TilesData.GetLength(1);
			Vector2 targetView = new((float)ActualWidth, (float)ActualHeight);
			Vector2 targetSize = new Vector2(width, height) * 16;
			float scale = (float)Math.Min(ActualWidth / targetSize.X, ActualHeight / targetSize.Y);
			scale = scale > 1.5f ? 1.5f : scale;
			DrawBG((int)Math.Round(16 * scale));
			Vector2 position = (targetView - targetSize * scale) / 2;
			DrawBorder(position, width, height, scale);
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

		private void XnaControl_Draw()
		{
			XnaControl.GraphicsDevice.Clear(new Color(0xFF, 0xFF, 0xFF));
			Batch.Begin();
			DrawTiles();
			Batch.End();
		}
	}
}
