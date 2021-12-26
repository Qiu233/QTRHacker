using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MForm : Form
	{
		private static readonly Color ButtonNormalColor = Color.Transparent;
		private static readonly Color ButtonHoverColor = Color.FromArgb(70, 70, 80);
		private static readonly Color TitleColor = Color.FromArgb(45, 45, 48);
		private PictureBox CloseButton;
		private Brush TitleBrush;
		private Point Drag_MousePos;
		public Panel MainPanel { get; }
		public MForm()
		{
			ControlBox = false;
			FormBorderStyle = FormBorderStyle.None;
			TitleBrush = new SolidBrush(TitleColor);
			CloseButton = new PictureBox();
			CloseButton.BackColor = TitleColor;
			CloseButton.MouseEnter += (s, e) => CloseButton.BackColor = ButtonHoverColor;
			CloseButton.MouseLeave += (s, e) => CloseButton.BackColor = TitleColor;
			CloseButton.Click += (s, e) => Dispose();
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Image.close.png"))
				CloseButton.Image = Image.FromStream(st);

			Controls.Add(CloseButton);

			MainPanel = new Panel();
			Controls.Add(MainPanel);

			Paint += MForm_Paint;
		}


		private void MForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(TitleBrush, 0, 0, e.ClipRectangle.Width, 32);
			e.Graphics.DrawString(Text, new Font("Arial", 16f), Brushes.White, new Point(5, 4));
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			CloseButton.Bounds = new Rectangle(Width - 32, 0, 32, 32);
			MainPanel.Bounds = new Rectangle(0, 32, Width, Height - 32);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				Drag_MousePos = e.Location;
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.Left)
			{
				this.Top = MousePosition.Y - Drag_MousePos.Y;
				this.Left = MousePosition.X - Drag_MousePos.X;
			}
		}
	}
}
