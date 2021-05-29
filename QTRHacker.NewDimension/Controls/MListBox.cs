using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Controls
{
	public class MListBox : ListBox
	{
		public MListBox()
		{
			UpdateStyles();
			DrawMode = DrawMode.OwnerDrawFixed;
			BackColor = Color.FromArgb(40, 40, 40);
			BorderStyle = BorderStyle.None;
		}
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			if (e.Index != -1)
			{
				if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(120, 120, 120)), e.Bounds);
				else
					e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
				e.Graphics.DrawString((string)Items[e.Index], e.Font, new SolidBrush(Color.White), e.Bounds, new StringFormat() { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap });
			}
		}
	}
}
