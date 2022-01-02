using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class InfoView : UserControl
	{
		public enum TipDock
		{
			Top, Left, Down, Right
		}
		public override string Text
		{
			get => TipLabel.Text;
			set => TipLabel.Text = value;
		}
		public Label TipLabel
		{
			get;
		}
		public Control View
		{
			get;
		}
		private readonly TipDock ViewDock;
		private readonly int TipWidth;
		public InfoView(Control view, TipDock dock, bool Border = true, int tipWidth = 40)
		{
			TipWidth = tipWidth;
			View = view;
			ViewDock = dock;
			BorderStyle = Border ? BorderStyle.FixedSingle : BorderStyle.None;
			TipLabel = new Label
			{
				BorderStyle = BorderStyle.FixedSingle,
				TextAlign = ContentAlignment.MiddleCenter
			};
			Controls.Add(TipLabel);
			Controls.Add(view);
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			switch (ViewDock)
			{
				case TipDock.Top:
					TipLabel.Bounds = new Rectangle(0, 0, Width, 20);
					View.Bounds = new Rectangle(0, 20, Width, Height - 20);
					break;
				case TipDock.Left:
					TipLabel.Bounds = new Rectangle(0, 0, TipWidth, Height);
					View.Bounds = new Rectangle(TipWidth, 0, Width - TipWidth, Height);
					break;
				case TipDock.Down:
					TipLabel.Bounds = new Rectangle(0, Height - 20, Width, 20);
					View.Bounds = new Rectangle(0, 0, Width, Height - 20);
					break;
				case TipDock.Right:
					TipLabel.Bounds = new Rectangle(Width - TipWidth, 0, TipWidth, Height);
					View.Bounds = new Rectangle(0, 0, Width - TipWidth, Height);
					break;
			}
		}
	}
}
