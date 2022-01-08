using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.XNA
{
	public class GDITextFactory : IDisposable
	{
		public readonly Dictionary<char, (Texture2D, Rectangle)> Cache = new();
		private const int StoreSize = 30;
		private readonly System.Drawing.Bitmap BMP;
		private readonly System.Drawing.Graphics Graphics;
		private readonly System.Drawing.Font Font;
		private int ScanX = 0, ScanY = 0;
		private const int Size = 1024;
		public readonly List<Texture2D> Textures = new();
		private readonly GraphicsDevice GraphicsDevice;


		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Graphics.Dispose();
			BMP.Dispose();
			Font.Dispose();
			foreach (var t in Textures)
				t.Dispose();
		}

		public GDITextFactory(GraphicsDevice device, string fontName)
		{
			GraphicsDevice = device;
			Font = new System.Drawing.Font(fontName, StoreSize - 10);
			Textures.Add(new Texture2D(device, Size, Size));
			BMP = new System.Drawing.Bitmap(Size, Size);
			Graphics = System.Drawing.Graphics.FromImage(BMP);
		}


		private unsafe (Texture2D, Rectangle) GetChar(char c)
		{
			if (Cache.TryGetValue(c, out (Texture2D, Rectangle) v))
				return v;
			string s = c.ToString();
			var fontRawSize = Graphics.MeasureString(s, Font, System.Drawing.PointF.Empty, System.Drawing.StringFormat.GenericTypographic);
			int fontWidth = (int)Math.Ceiling(fontRawSize.Width);
			int fontHeight = (int)Math.Ceiling(fontRawSize.Height);
			var texture = Textures.Last();
			if (ScanX + fontWidth > Size) // new line
			{
				if (ScanY + StoreSize > Size)
				{
					texture = new Texture2D(GraphicsDevice, Size, Size);
					Textures.Add(texture);
					ScanX = 0;
					ScanY = 0;
				}
				else
				{
					ScanX = 0;
					ScanY += StoreSize;
				}
			}
			var drawRect = new System.Drawing.Rectangle(ScanX, ScanY, fontWidth, fontHeight);
			Graphics.DrawString(s, Font, System.Drawing.Brushes.White, drawRect, System.Drawing.StringFormat.GenericTypographic);
			Graphics.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);

			using var bmp = BMP.Clone(drawRect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			byte[] targetData = new byte[fontHeight * fontWidth * sizeof(Color)];
			Marshal.Copy(data.Scan0, targetData, 0, targetData.Length);
			bmp.UnlockBits(data);
			var dst = new Rectangle(ScanX, ScanY, fontWidth, fontHeight);
			texture.SetData(0, dst, targetData, 0, targetData.Length);
			ScanX += fontWidth;
			return Cache[c] = (texture, dst);
		}
		public void DrawString(SpriteBatch batch, string s, Vector2 dest, Color color, int size)
		{
			var pos = dest;
			float scale = size / Font.Size;
			foreach (var c in s)
			{
				var font = GetChar(c);
				var rect = font.Item2;
				Rectangle d = new Rectangle((int)Math.Round(pos.X), (int)Math.Round(pos.Y), (int)Math.Round(rect.Width * scale), (int)Math.Round(rect.Height * scale));
				batch.Draw(font.Item1, d, rect, color);
				pos.X += d.Width;
			}
		}
	}
}
