using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Terraria_Hacker
{
	public class MTabControl : TabControl
	{
		private Color bColor = Color.FromArgb(200, 200, 200);
		private Color tColor = Color.FromArgb(160, 160, 200);
		public MTabControl()
		{
			SetStyle(ControlStyles.UserPaint | 
				ControlStyles.OptimizedDoubleBuffer | 
				ControlStyles.AllPaintingInWmPaint | 
				ControlStyles.ResizeRedraw | 
				ControlStyles.SupportsTransparentBackColor, true);
			UpdateStyles();
		}

		protected override void OnPaint(PaintEventArgs e)
		{

			if (TabCount > 0)
			{
				e.Graphics.FillRectangle(new SolidBrush(bColor), new RectangleF(0, 0, Width, Height));
				for (int i = 0; i < TabCount; i++)
				{
					Rectangle bounds = GetTabRect(i);
					if (SelectedIndex == i) e.Graphics.FillRectangle(new SolidBrush(tColor), GetTabRect(i));
					SizeF textSize = TextRenderer.MeasureText(TabPages[i].Text, Font);
					PointF textPoint = new PointF(bounds.X + bounds.Width / 2 - textSize.Width / 2, bounds.Y + bounds.Height / 2 - textSize.Height / 2);

					GetTabRect(i);
					e.Graphics.DrawString(TabPages[i].Text, Font, Brushes.Black, textPoint);
				}
				e.Graphics.DrawLine(new Pen(tColor, 3), new Point(0, 23), new Point(Width, 23));
			}
			else
			{
				base.OnPaint(e);
				e.Graphics.FillRectangle(new SolidBrush(Parent.BackColor), new RectangleF(0, 0, Width, Height));
			}
		}
	}
}
