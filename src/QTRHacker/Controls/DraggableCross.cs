using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class DraggableCross : Control
	{
		/*public static readonly Image CrossImage;
		static DraggableCross()
		{
			var bmp = new Bitmap(50, 50, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			unsafe
			{
				var data = bmp.LockBits(new Rectangle(0, 0, 50, 50), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				IntPtr baseAddr = data.Scan0;
				for (int i = 0; i < 50; i++)
				{
					uint* ptrA = (uint*)(baseAddr + (data.Stride * 24) + (i * 4));
					uint* ptrB = (uint*)(baseAddr + (data.Stride * 25) + (i * 4));
					*ptrA = 0xFF000000;
					*ptrB = 0xFF000000;
				}
				for (int i = 0; i < 50; i++)
				{
					ulong* ptr = (ulong*)(baseAddr + (data.Stride * i) + (24 * 4));
					*ptr = 0xFF000000_FF000000;
				}
				bmp.UnlockBits(data);
			}
			CrossImage = bmp;
		}*/
		private bool Dragginng = false;
		public event Action<Point> OnCrossRelease;
		public DraggableCross(int size = 25)
		{
			Size = new Size(size, size);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var g = e.Graphics;
			if (!Dragginng)
			{
				using Pen p = new(Color.FromArgb(255, 180, 180, 180));
				g.DrawLine(p, new Point(Width / 2, 0), new Point(Width / 2, Height - 1));
				g.DrawLine(p, new Point(Width / 2 + 1, 0), new Point(Width / 2 + 1, Height - 1));
				g.DrawLine(p, new Point(0, Height / 2), new Point(Width - 1, Height / 2));
				g.DrawLine(p, new Point(0, Height / 2 + 1), new Point(Width - 1, Height / 2 + 1));
			}
			else
			{
				using Pen p = new(Color.FromArgb(255, 40, 40, 40));
				g.DrawLine(p, new Point(Width / 2, 0), new Point(Width / 2, Height - 1));
				g.DrawLine(p, new Point(Width / 2 + 1, 0), new Point(Width / 2 + 1, Height - 1));
				g.DrawLine(p, new Point(0, Height / 2), new Point(Width - 1, Height / 2));
				g.DrawLine(p, new Point(0, Height / 2 + 1), new Point(Width - 1, Height / 2 + 1));
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Invalidate();
			Capture = true;
			Dragginng = true;
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Invalidate();
			if (e.Button == MouseButtons.Left && Dragginng)
			{
				Capture = false;
				Dragginng = false;
				OnCrossRelease?.Invoke(e.Location);
			}
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			Cursor = Cursors.Cross;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!Dragginng)
				Cursor = Cursors.Default;
		}
	}
}
