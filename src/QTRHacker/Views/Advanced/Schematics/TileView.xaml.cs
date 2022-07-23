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
using QTRHacker.Assets;
using QTRHacker.EventManagers;
using static QTRHacker.Core.PatchesManager;

namespace QTRHacker.Views.Advanced.Schematics
{
	/// <summary>
	/// TileView.xaml 的交互逻辑
	/// </summary>
	public partial class TileView : UserControl
	{
		private readonly Dictionary<(int, Int32Rect), ImageSource> TileTextures = new();
		private readonly Dictionary<(int, Int32Rect), ImageSource> WallTextures = new();

		public STile[,] TilesData
		{
			get => (STile[,])GetValue(TilesDataProperty);
			set => SetValue(TilesDataProperty, value);
		}
		public static readonly DependencyProperty TilesDataProperty =
			DependencyProperty.Register(nameof(TilesData), typeof(STile[,]), typeof(TileView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		protected override void OnRender(DrawingContext drawingContext)
		{
			DrawTiles(drawingContext);
		}

		private void DrawBG(DrawingContext drawingContext, int step)
		{
			int width = (int)(ActualWidth / step + 1), height = (int)(ActualHeight / step + 1);
			for (int i = 0; i < width; i += 2)
			{
				for (int j = 0; j < height; j += 2)
				{
					Brush light = new SolidColorBrush(Color.FromArgb(0xFF, 0xA0, 0xA0, 0xA0));
					Brush dark = new SolidColorBrush(Color.FromArgb(0xFF, 0x50, 0x50, 0x50));
					drawingContext.DrawRectangle(dark, null, new Rect(i * step, j * step, step, step));
					drawingContext.DrawRectangle(light, null, new Rect(i * step, (j + 1) * step, step, step));
					drawingContext.DrawRectangle(light, null, new Rect((i + 1) * step, j * step, step, step));
					drawingContext.DrawRectangle(dark, null, new Rect((i + 1) * step, (j + 1) * step, step, step));
				}
			}
		}

		private static void DrawBorder(DrawingContext drawingContext, Point pos, int width, int height, float scale)
		{
			double w = width * 16 * scale;
			double h = height * 16 * scale;
			Brush color = new SolidColorBrush(Colors.White);
			drawingContext.DrawRectangle(color, null, new Rect(pos.X, pos.Y, w, 1));
			drawingContext.DrawRectangle(color, null, new Rect(pos.X, pos.Y, 1, h));
			drawingContext.DrawRectangle(color, null, new Rect(pos.X, (pos.Y + h), w, 1));
			drawingContext.DrawRectangle(color, null, new Rect((pos.X + w), pos.Y, 1, h));
		}

		private static Point Point_Add(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);

		private static Point Point_Sub(Point p1, Point p2) => new(p1.X - p2.X, p1.Y - p2.Y);
		private static Point Point_Mul(Point p1, double f) => new(p1.X * f, p1.Y * f);

		private void DrawTiles(DrawingContext drawingContext)
		{
			if (TilesData == null)
				return;
			int width = TilesData.GetLength(0);
			int height = TilesData.GetLength(1);
			Point targetView = new(ActualWidth, ActualHeight);
			Point targetSize = Point_Mul(new Point(width, height), 16);
			float scale = (float)Math.Min(ActualWidth / targetSize.X, ActualHeight / targetSize.Y);
			scale = scale > 1.5f ? 1.5f : scale;
			DrawBG(drawingContext, (int)Math.Round(16 * scale));
			Point position = Point_Mul((Point_Sub(targetView, Point_Mul(targetSize, scale))), 0.5);
			DrawBorder(drawingContext, position, width, height, scale);
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					STile tile = TilesData[x, y];
					if (tile.Wall > 0)
					{
						ImageSource wallTexture = GetWallTexture(tile.Wall, new Int32Rect(tile.WallFrameX(), tile.WallFrameY(), 32, 32));
						Point pos = Point_Add(position, Point_Mul(new Point(x - 0.5f, y - 0.5f), 16 * scale));
						drawingContext.DrawImage(wallTexture, new Rect(pos, new Size(32 * scale, 32 * scale)));
					}
				}
			}
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					STile tile = TilesData[x, y];
					if (tile.Active())
					{
						ImageSource tileTexture = GetTileTexture(tile.Type, new Int32Rect(tile.FrameX, tile.FrameY, 16, 16));
						Point pos = Point_Add(position, Point_Mul(new Point(x, y), 16 * scale));
						drawingContext.DrawImage(tileTexture, new Rect(pos, new Size(16 * scale, 16 * scale)));
					}
				}
			}
		}

		public TileView()
		{
			InitializeComponent();
			/*XnaControl.Draw += XnaControl_Draw;
			XnaControl.LoadContent += XnaControl_LoadContent;
			XnaControl.Initialize += XnaControl_Initialize;*/
		}

		private ImageSource GetTileTexture(int tile, Int32Rect source)
		{
			if (TileTextures.TryGetValue((tile, source), out var texture))
				return texture;
			var rawImg = GameImages.GetTileImage(tile);
			return TileTextures[(tile, source)] = new CroppedBitmap(rawImg, new Int32Rect(source.X, source.Y, source.Width, source.Height));
		}
		private ImageSource GetWallTexture(int wall, Int32Rect source)
		{
			if (WallTextures.TryGetValue((wall, source), out var texture))
				return texture;
			var rawImg = GameImages.GetWallImage(wall);
			return WallTextures[(wall, source)] = new CroppedBitmap(rawImg, new Int32Rect(source.X, source.Y, source.Width, source.Height));
		}
	}
}
