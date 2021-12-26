using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MProgressBar : ProgressBar
	{
		public MProgressBar()
		{
			SetStyle(ControlStyles.UserPaint, true);
			UpdateStyles();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			SolidBrush brush = new SolidBrush(Color.FromArgb(150, 0, 255, 0));
			Rectangle bounds = new Rectangle(2, 2,
				((int)(Width * (((double)base.Value) / ((double)base.Maximum)))) - 4,
				Height - 4);
			e.Graphics.FillRectangle(brush, bounds);
		}
	}
}
