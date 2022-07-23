using QTRHacker.AssetLoaders;
using QTRHacker.Assets;
using QTRHacker.Core.ProjectileImage;
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

namespace QTRHacker.Views.Advanced.RainbowFonts
{
	/// <summary>
	/// Interaction logic for RainbowFontPreviewer.xaml
	/// </summary>
	public partial class RainbowFontPreviewer : UserControl
	{
		public ProjImage Image
		{
			get => (ProjImage)GetValue(ImageProperty);
			set => SetValue(ImageProperty, value);
		}

		public static readonly DependencyProperty ImageProperty =
			DependencyProperty.Register(nameof(Image), typeof(ProjImage), typeof(RainbowFontPreviewer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		public readonly ImageSource ProjTexture;

		public RainbowFontPreviewer()
		{
			using var s = Application.GetResourceStream(new Uri($"pack://application:,,,/QTRHacker;component/Assets/Misc/RainbowFonts/RainbowProj.png", UriKind.Absolute)).Stream;
			var img = new BitmapImage();
			img.BeginInit();
			img.CacheOption = BitmapCacheOption.OnLoad;
			img.StreamSource = s;
			img.EndInit();
			WriteableBitmap wbmp = new(img);
			wbmp.Lock();
			unsafe
			{
				IntPtr pBackBuffer = wbmp.BackBuffer;

				byte* pBuff = (byte*)pBackBuffer.ToPointer();
				int stride = wbmp.BackBufferStride;
				for (int x = 0; x < wbmp.Width; x++)
				{
					for (int y = 0; y < wbmp.Height; y++)
					{
						if (pBuff[4 * x + (y * stride) + 3] != 0)
							pBuff[4 * x + (y * stride) + 3] = 120;
					}
				}

			}
			ProjTexture = wbmp;
			wbmp.Unlock();
			InitializeComponent();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			Draw(drawingContext);
		}

		private void Draw(DrawingContext drawingContext)
		{
			drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(0xff, 145, 140, 145)), null, new Rect(0, 0, ActualWidth, ActualHeight));
			var penBlueViolet = new Pen(new SolidColorBrush(Colors.BlueViolet), 1);
			drawingContext.DrawLine(penBlueViolet,
				new Point(ActualWidth / 2 - 1, 0),
				new Point(ActualWidth / 2 - 1, ActualHeight));
			drawingContext.DrawLine(penBlueViolet,
				new Point(0, ActualHeight / 2 - 1),
				new Point(ActualWidth, ActualHeight / 2 - 1));

			if (Image == null)
				return;
			foreach (var p in Image.Projs)
			{
				if (p.ProjType != 251)
					continue;
				var pos = new Point(p.Location.X + ActualWidth / 2, p.Location.Y + ActualHeight / 2);
				DrawPoint(drawingContext, pos, new Vector(p.Speed.X, p.Speed.Y));
			}
		}

		private void DrawPointRaw(DrawingContext drawingContext, Point pos, Vector direction)
		{
			drawingContext.PushTransform(new RotateTransform((Math.PI / 2 + Math.Atan2(direction.Y, direction.X)) / Math.PI * 180, pos.X, pos.Y));
			drawingContext.DrawImage(ProjTexture, new Rect(pos.X - 20, pos.Y - 20, 40, 40));
			drawingContext.Pop();
			//Batch.Draw(ProjTexture, pos, null, new Color(255, 255, 255, 120), (float)Math.PI / 2 + (float)Math.Atan2(direction.Y, direction.X), new Vector2(16, 16), 1.25f, SpriteEffects.None, 0);
		}

		private void DrawPoint(DrawingContext drawingContext, Point pos, Vector direction)
		{
			var unit = direction / direction.Length;
			DrawPointRaw(drawingContext, pos - unit * 13, direction);
		}
	}
}
