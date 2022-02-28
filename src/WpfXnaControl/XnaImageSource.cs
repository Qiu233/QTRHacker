using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Graphics;

namespace WpfXnaControl
{
	public class XnaImageSource : IDisposable
	{
		public RenderTarget2D RenderTarget { get; }
		public WriteableBitmap WriteableBitmap { get; }
		private readonly byte[] Buffer;

		public XnaImageSource(GraphicsDevice graphics, int width, int height)
		{
			RenderTarget = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
			Buffer = new byte[width * height * 4];
			WriteableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
		}

		~XnaImageSource()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				RenderTarget.Dispose();
		}

		private unsafe void FetchData()
		{
			RenderTarget.GetData(Buffer);
			int len = Buffer.Length;
			fixed (byte* ptr = Buffer)
			{
				for (int i = 0; i < len - 2; i += 4)
				{
					byte r = ptr[i];
					ptr[i] = ptr[i + 2];
					ptr[i + 2] = r;
				}
			}
		}

		public void Commit()
		{
			FetchData();

			WriteableBitmap.Lock();
			Marshal.Copy(Buffer, 0, WriteableBitmap.BackBuffer, Buffer.Length);
			WriteableBitmap.AddDirtyRect(
				new Int32Rect(0, 0, RenderTarget.Width, RenderTarget.Height));
			WriteableBitmap.Unlock();
		}
	}
}