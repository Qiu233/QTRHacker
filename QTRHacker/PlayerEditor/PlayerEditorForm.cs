using QTRHacker.Functions;
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
	public class ItemIcon : PictureBox
	{
		public int Number, ID;
		public bool Selected = false;
		private int lastID;
		private ToolTip Tip;
		private GameContext Context;
		public ItemSlots Slots
		{
			get;
		}
		public ItemIcon(GameContext Context, ItemSlots slots, int num,int id)
		{
			this.Context = Context;
			Slots = slots;
			Number = num;
			ID = id;
			Tip = new ToolTip();
		}
		protected override void OnPaint(PaintEventArgs pe)
		{
			var item = Slots[ID];
			int nowID = item.Type;
			if (lastID != nowID)
			{
				var img = MainForm.item_images.Images["Item_" + nowID];
				if (img != null)
				{
					Image newImg = (Image)img.Clone();
					this.Image = newImg;
					Tip.SetToolTip(this, MainForm.resource.Items.First(i => i.id == nowID).name);
				}
				else
				{
					Tip.SetToolTip(this, "");
				}
				this.lastID = nowID;
			}
			base.OnPaint(pe);
			pe.Graphics.DrawString(item.Stack.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), 10, 35);
			if (Selected)
			{
				pe.Graphics.DrawRectangle(new Pen(Color.BlueViolet, 3), 1, 1, pe.ClipRectangle.Width - 3, pe.ClipRectangle.Height - 3);
			}
		}
	}
	public class AltItemIcon : PictureBox
	{
		public int ID = 0, Stack = 0;
		public byte Prefix = 0;

	}
	public partial class PlayerEditorForm : Form
	{
		private MTabControl Tabs;
		public PlayerEditorForm()
		{
			InitializeComponent();
			Tabs = new MTabControl();
			Tabs.bColor = Color.FromArgb(175, 215, 230);
			Tabs.tColor = Color.CadetBlue;
			Tabs.Bounds = new Rectangle(0, 0, 1005, 360);
			Tabs.Controls.Add(new PlayerEditor(MainForm.Context, this) { BackColor = Color.FromArgb(205, 255, 230) });
			Tabs.Controls.Add(new InvEditor(MainForm.Context, this) { BackColor = Color.FromArgb(205, 255, 230) });
			Tabs.Controls.Add(new ArmorEditor(MainForm.Context, this) { BackColor = Color.FromArgb(205, 255, 230) });
			this.Controls.Add(Tabs);
		}


	}
}
