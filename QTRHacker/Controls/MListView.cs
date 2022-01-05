using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MListView : ListView
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int wndproc);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		public const int GWL_STYLE = -16;
		public const int WS_DISABLED = 0x8000000;

		private bool _enabled;

		public new bool Enabled
		{
			get => _enabled;
			set
			{
				_enabled = value;
				if (_enabled)
					SetWindowLong(Handle, GWL_STYLE, (~WS_DISABLED) & GetWindowLong(Handle, GWL_STYLE));
				else
					SetWindowLong(Handle, GWL_STYLE, WS_DISABLED + GetWindowLong(Handle, GWL_STYLE));
			}
		}

		public Color ColumnBackColor { get; set; }
		public Color ColumnTextColor { get; set; }

		public MListView()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.EnableNotifyMessage, true);
			ForeColor = Color.White;
			OwnerDraw = true;
			MultiSelect = false;
			FullRowSelect = true;
			View = View.Details;
			ColumnBackColor = BackColor = Color.FromArgb(60, 60, 60);
			ColumnTextColor = Color.White;
			BorderStyle = BorderStyle.FixedSingle;
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
			using var columnBackBrush = new SolidBrush(ColumnBackColor);
			using var columnTextBrush = new SolidBrush(ColumnTextColor);
			//e.Graphics.FillRectangle(ColumnBackBrush, new Rectangle(0, 0, Width, e.Bounds.Height));
			Rectangle r = new Rectangle(Point.Empty, new Size(0, e.Bounds.Height - 1));
			Point p = new Point(0, 4);
			for (int i = 0; i < Columns.Count; i++)
			{
				r.Width = Columns[i].Width;
				var ts = e.Graphics.MeasureString(Columns[i].Text, Font);
				p.X = r.X + (r.Width / 2 - (int)(ts.Width / 2)) - 1;
				p.Y = r.Y + (r.Height / 2 - (int)(ts.Height / 2));
				e.Graphics.FillRectangle(columnBackBrush, r);
				e.Graphics.DrawLine(Pens.Gray, r.Right - 1, r.Top + 3, r.Right - 1, r.Bottom - 6);
				e.Graphics.DrawString(Columns[i].Text, Font, columnTextBrush, p);
				r.X += r.Width;
			}
		}
		protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
		{
			base.OnDrawSubItem(e);
			if (e.Item.Selected)
				e.Graphics.FillRectangle(Brushes.Gray, e.Bounds);
			var ts = e.Graphics.MeasureString(e.SubItem.Text, Font);
			using Brush b = new SolidBrush(e.SubItem.ForeColor);
			if (ts.Width > e.Bounds.Width - 5)
			{
				string sb = e.SubItem.Text;
				while (ts.Width > e.Bounds.Width - 20)
				{
					sb = sb[0..^1];
					ts = e.Graphics.MeasureString(sb, Font);
				}
				sb += "...";
				e.Graphics.DrawString(sb, Font, b,
					e.Bounds.X + e.Bounds.Width / 2 - ts.Width / 2 - 1,
					e.Bounds.Y + e.Bounds.Height / 2 - ts.Height / 2);
			}
			else
			{
				e.Graphics.DrawString(e.SubItem.Text, Font, b,
					e.Bounds.X + e.Bounds.Width / 2 - ts.Width / 2 - 1,
					e.Bounds.Y + e.Bounds.Height / 2 - ts.Height / 2);
			}
		}
	}
}
