using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Controls;
using QTRHacker.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
#pragma warning disable CS1690

namespace QTRHacker.PlayerEditor
{
	public partial class PlayerEditorForm : Form
	{
		private readonly MTabControl Tabs;
		public static readonly Color ButtonNormalColor = Color.Transparent;
		public static readonly Color ButtonHoverColor = Color.FromArgb(70, 70, 80);
		private Point Drag_MousePos;
		public PlayerEditorForm(Player TargetPlayer, bool Editable)
		{
			InitializeComponent();
			Text = TargetPlayer.Name.GetString() + (Editable ? "" : $" ({HackContext.CurrentLanguage["NonEditable"]})");
			BackColor = Color.FromArgb(45, 45, 48);

			var CloseButton = new PictureBox();
			CloseButton.MouseEnter += (s, e) => CloseButton.BackColor = ButtonHoverColor;
			CloseButton.MouseLeave += (s, e) => CloseButton.BackColor = ButtonNormalColor;
			CloseButton.Click += (s, e) => Dispose();
			CloseButton.Bounds = new Rectangle(Width - 32, -1, 32, 32);
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Image.close.png"))
				CloseButton.Image = Image.FromStream(st);

			Controls.Add(CloseButton);

			Tabs = new MTabControl
			{
				BColor = Color.FromArgb(70, 70, 70),
				TColor = Color.FromArgb(90, 90, 90),
				Bounds = new Rectangle(0, 31, 1005, 360),
				ForeColor = GlobalColors.TipForeColor
			};
			Tabs.Controls.Add(new PlayerEditor(HackContext.GameContext, this, TargetPlayer, Editable) { BackColor = Tabs.BColor });
			Tabs.Controls.Add(new InvEditor(HackContext.GameContext, this, TargetPlayer, Editable) { BackColor = Tabs.BColor });
			Tabs.Controls.Add(new ArmorEditor(HackContext.GameContext, this, TargetPlayer, Editable) { BackColor = Tabs.BColor });
			Tabs.Controls.Add(new PiggyBankEditor(HackContext.GameContext, this, TargetPlayer, Editable) { BackColor = Tabs.BColor });
			Tabs.Controls.Add(new SafeEditor(HackContext.GameContext, this, TargetPlayer, Editable) { BackColor = Tabs.BColor });
			Tabs.Controls.Add(new VoidVaultEditor(HackContext.GameContext, this, TargetPlayer, Editable) { BackColor = Tabs.BColor });
			Tabs.Controls.Add(new ForgeEditor(HackContext.GameContext, this, TargetPlayer, Editable) { BackColor = Tabs.BColor });
			Controls.Add(Tabs);
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
				Top = MousePosition.Y - Drag_MousePos.Y;
				Left = MousePosition.X - Drag_MousePos.X;
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawString(Text, new Font("Arial", 16f), Brushes.White, new Point(5, 3));
		}

	}
}
