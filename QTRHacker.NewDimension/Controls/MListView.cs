using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Controls
{
	public class MListView : ListView
	{
		private readonly Brush ColumnBackBrush;
		private readonly Brush ColumnTextBrush;
		private readonly Brush SubItemTextBrush;

		public MListView()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.EnableNotifyMessage, true);
			ForeColor = Color.White;
			OwnerDraw = true;
			MultiSelect = false;
			FullRowSelect = true;
			View = View.Details;
			BackColor = Color.FromArgb(60, 60, 60);
			BorderStyle = BorderStyle.FixedSingle;
			ColumnBackBrush = new SolidBrush(Color.FromArgb(60, 60, 60));
			ColumnTextBrush = new SolidBrush(Color.White);
			SubItemTextBrush = new SolidBrush(Color.White);
		}

		protected override void OnNotifyMessage(Message m)
		{
			if (m.Msg != 0x14)
				base.OnNotifyMessage(m);
		}
		protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
		{
			e.Cancel = true;
			e.NewWidth = Columns[e.ColumnIndex].Width;
		}
		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			base.OnDrawColumnHeader(e);
			Rectangle r = new Rectangle(Point.Empty, new Size(0, e.Bounds.Height - 1));
			Point p = new Point(0, 4);
			for (int i = 0; i < Columns.Count; i++)
			{
				r.Width = Columns[i].Width;
				var ts = e.Graphics.MeasureString(Columns[i].Text, Font);
				p.X = r.X + (r.Width / 2 - (int)(ts.Width / 2)) - 1;
				p.Y = r.Y + (r.Height / 2 - (int)(ts.Height / 2));
				e.Graphics.FillRectangle(ColumnBackBrush, r);
				e.Graphics.DrawLine(Pens.Gray, r.Right - 1, r.Top + 3, r.Right - 1, r.Bottom - 6);
				e.Graphics.DrawString(Columns[i].Text, Font, ColumnTextBrush, p);
				r.X += r.Width;
			}
		}
		protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
		{
			base.OnDrawSubItem(e);
			if (e.Item.Selected)
				e.Graphics.FillRectangle(Brushes.Gray, e.Bounds);
			var ts = e.Graphics.MeasureString(e.SubItem.Text, Font);
			using (Brush b = new SolidBrush(e.SubItem.ForeColor))
				e.Graphics.DrawString(e.SubItem.Text, Font, b,
					e.Bounds.X + e.Bounds.Width / 2 - ts.Width / 2 - 1,
					e.Bounds.Y + e.Bounds.Height / 2 - ts.Height / 2);

		}
	}
}
