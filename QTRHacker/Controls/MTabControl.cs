using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MTabControl : TabControl
	{
		public Color HeaderBackColor
		{
			get; set;
		}
		public Color HeaderSelectedBackColor
		{
			get; set;
		}
		public MTabControl()
		{
			HeaderBackColor = Color.FromArgb(200, 200, 200);
			HeaderSelectedBackColor = Color.FromArgb(150, 150, 150);
			SetStyle(ControlStyles.UserPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.SupportsTransparentBackColor, true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using Brush bBrush = new SolidBrush(HeaderBackColor);
			using Brush tBrush = new SolidBrush(HeaderSelectedBackColor);
			if (TabCount > 0)
			{
				using Brush b = new SolidBrush(ForeColor);
				e.Graphics.FillRectangle(bBrush, new RectangleF(0, 0, Width, Height));
				for (int i = 0; i < TabCount; i++)
				{
					Rectangle bounds = GetTabRect(i);
					if (SelectedIndex == i)
						e.Graphics.FillRectangle(tBrush, bounds);
					SizeF textSize = TextRenderer.MeasureText(TabPages[i].Text, Font);
					PointF textPoint = new PointF(bounds.X + bounds.Width / 2 - textSize.Width / 2, bounds.Y + bounds.Height / 2 - textSize.Height / 2);

					e.Graphics.DrawString(TabPages[i].Text, Font, b, textPoint);
				}
				int height = GetTabRect(0).Height + 1;
				e.Graphics.DrawLine(new Pen(HeaderSelectedBackColor, 3), new Point(0, height), new Point(Width, height));
			}
			else
			{
				base.OnPaint(e);
				e.Graphics.FillRectangle(bBrush, new RectangleF(0, 0, Width, Height));
			}
		}
	}
}
