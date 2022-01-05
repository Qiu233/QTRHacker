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
		private const int TITLE_BAR_HEIGHT = 32;
		private static readonly Color ButtonHoverColor = Color.FromArgb(70, 70, 80);
		private readonly PictureBox CloseButton, MinButton;
		private Point drag_MousePos;
		public Panel MainPanel { get; }

		public Color TitleBarColor
		{
			get => base.BackColor;
			set => base.BackColor = value;
		}
		public MForm()
		{
			ControlBox = false;
			FormBorderStyle = FormBorderStyle.None;

			TitleBarColor = Color.FromArgb(45, 45, 48);

			CloseButton = new PictureBox();
			CloseButton.BackColor = TitleBarColor;
			CloseButton.MouseEnter += (s, e) => CloseButton.BackColor = ButtonHoverColor;
			CloseButton.MouseLeave += (s, e) => CloseButton.BackColor = TitleBarColor;
			CloseButton.Click += (s, e) => Dispose();

			MinButton = new PictureBox();
			MinButton.BackColor = TitleBarColor;
			MinButton.Click += (s, e) => WindowState = FormWindowState.Minimized;
			MinButton.MouseEnter += (s, e) => MinButton.BackColor = ButtonHoverColor;
			MinButton.MouseLeave += (s, e) => MinButton.BackColor = TitleBarColor;

			MainPanel = new Panel();
			Controls.Add(MainPanel);
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Image.close.png"))
				CloseButton.Image = Image.FromStream(st);
			Controls.Add(CloseButton);
			if (MinimizeBox)
			{
				using Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Image.min.png");
				MinButton.Image = Image.FromStream(st);
				Controls.Add(MinButton);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using var titleBrush = new SolidBrush(TitleBarColor);
			e.Graphics.FillRectangle(titleBrush, 0, 0, e.ClipRectangle.Width, 32);
			e.Graphics.DrawString(Text, new Font("Segoe UI", 15f), Brushes.White, new Point(3, 0));
		}
		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			if (MinButton != null)
				MinButton.Bounds = new Rectangle(Width - TITLE_BAR_HEIGHT * 2, -1, TITLE_BAR_HEIGHT, TITLE_BAR_HEIGHT);
			if (CloseButton != null)
				CloseButton.Bounds = new Rectangle(Width - TITLE_BAR_HEIGHT, 0, TITLE_BAR_HEIGHT, TITLE_BAR_HEIGHT);
			if (MainPanel != null)
				MainPanel.Bounds = new Rectangle(0, TITLE_BAR_HEIGHT, Width, Height - TITLE_BAR_HEIGHT);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				drag_MousePos = e.Location;
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.Left)
			{
				Top = MousePosition.Y - drag_MousePos.Y;
				Left = MousePosition.X - drag_MousePos.X;
			}
		}
	}
}
