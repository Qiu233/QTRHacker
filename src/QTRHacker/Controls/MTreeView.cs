using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MTreeView : TreeView
	{
		private Brush SelectedBrush, HotBrush;
		public MTreeView()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			UpdateStyles();
			BackColor = Color.FromArgb(37, 37, 38);
			BorderStyle = BorderStyle.FixedSingle;
			DrawMode = TreeViewDrawMode.OwnerDrawAll;
			ShowLines = false;
			CheckBoxes = false;
			FullRowSelect = true;
			HotTracking = true;
			ShowPlusMinus = false;
			SelectedBrush = new SolidBrush(Color.FromArgb(64, 64, 64));
			HotBrush = new SolidBrush(Color.FromArgb(80, 80, 80));
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0014)
				return;
			base.WndProc(ref m);
		}
		protected override void OnDrawNode(DrawTreeNodeEventArgs e)
		{
			if (e.Node == null || !e.Node.IsVisible) return;
			if ((e.State & TreeNodeStates.Selected) != 0)
				e.Graphics.FillRectangle(SelectedBrush, 0, e.Bounds.Y, Width, e.Bounds.Height);
			else if ((e.State & TreeNodeStates.Hot) != 0 && e.Node.Text != "")
				e.Graphics.FillRectangle(HotBrush, 0, e.Bounds.Y, Width, e.Bounds.Height);
			else
				using (Brush bb = new SolidBrush(BackColor))
					e.Graphics.FillRectangle(bb, 0, e.Bounds.Y, Width, e.Bounds.Height);
			e.Graphics.DrawString(e.Node.Text, Font, Brushes.White, e.Bounds.X + 4 + e.Node.Level * 20, e.Bounds.Y + 1);
		}
	}
}
