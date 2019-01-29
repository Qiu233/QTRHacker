using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Controls
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
			SolidBrush brush = new SolidBrush(Color.Coral);
			Rectangle bounds = new Rectangle(0, 0, base.Width, base.Height);
			bounds.Height -= 4;
			bounds.Width = ((int)(bounds.Width * (((double)base.Value) / ((double)base.Maximum)))) - 4;
			e.Graphics.FillRectangle(brush, bounds);
		}
	}
}
