using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class ImageButton : UserControl
	{
		public Image Image { get; }
		private readonly Font TextFont;
		protected static readonly Color NormalColor = Color.Transparent;
		protected static readonly Color HoverColor = Color.FromArgb(50, 50, 45);
		protected static readonly Color SelectedColor = Color.FromArgb(30, 30, 30);
		protected static readonly Color MouseDownColor = Color.FromArgb(40, 40, 37);

		public ImageButton(Image image)
		{
			Image = image;
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

			BackColor = NormalColor;
			ForeColor = Color.White;
			TextFont = new Font("Arial", 10);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			BackColor = MouseDownColor;
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			BackColor = HoverColor;
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			BackColor = HoverColor;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			BackColor = NormalColor;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawImage(Image, new Rectangle(4, 4, 22, 22));
			using Brush b = new SolidBrush(ForeColor);
			e.Graphics.DrawString(Text, TextFont, b, 30, 8);
		}
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			if (!Enabled) ForeColor = Color.FromArgb(200, 200, 200);
			else ForeColor = Color.White;
		}
	}
}
