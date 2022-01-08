using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MListBox : ListBox
	{
		public Color SelectedColor { get; set; }= Color.FromArgb(120, 120, 120);
		public MListBox()
		{
			UpdateStyles();
			DrawMode = DrawMode.OwnerDrawFixed;
			BackColor = Color.FromArgb(60, 60, 60);
			BorderStyle = BorderStyle.None;
		}
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			if (e.Index != -1)
			{
				Color color = BackColor;
				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					color = SelectedColor;
				var rect = e.Bounds;
				rect.Height += 3;
				using SolidBrush brush = new SolidBrush(color);
				e.Graphics.FillRectangle(brush, rect);

				using SolidBrush fbrush = new SolidBrush(ForeColor);
				rect = e.Bounds;
				//rect.Y -= 3;
				rect.Height += 3;
				e.Graphics.DrawString((string)Items[e.Index], Font, fbrush, rect, new StringFormat() { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap });
			}
		}
	}
}
