/*
 * 由SharpDevelop创建。
 * 用户： lopi2
 * 日期: 2017/1/21
 * 时间: 15:28
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using QTRHacker.Functions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QTRHacker
{
	/// <summary>
	/// Description of PlayerDetail.
	/// </summary>
	public partial class PlayerInventory : Form
	{
		private int id;
		private GameContext Context;
		public PlayerInventory(GameContext Context, int id, string name)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.Context = Context;
			this.Text = name;
			this.id = id;
			InitializeComponent();

			ListView l = new ListView();
			l.Location = new Point(0, 0);
			l.Size = new Size(this.ClientSize.Width, 130);
			l.View = View.SmallIcon;
			l.MultiSelect = false;
			l.FullRowSelect = true;
			this.Controls.Add(l);

			l.BeginUpdate();

			var player = Context.Players[id];
			l.SmallImageList = MainForm.item_images;
			for (int slot = 0; slot < 50; slot++)
			{
				var item = player.Inventory[slot];
				int itemid = item.Type;
				string s = GetItemNameFromId(itemid);
				if (s.Length > 12)
					s = s.Substring(0, 9) + "...";
				ListViewItem lvi = new ListViewItem(s + "*" + item.Stack, "Item_" + itemid);

				lvi.Font = new Font("Arial", 7);
				l.Items.Add(lvi);
			}


			for (int i = 0; i < 10; i++)
			{
				l.Columns.Add(i.ToString());
			}
			foreach (ColumnHeader item in l.Columns)
			{
				item.Width = l.Size.Width / 10;
			}
			for (int i = 0; i < 5; i++)
			{
				int h = i * 10;
				var item = player.Inventory[h];
				int hid = item.Type;
				int hnum = item.Stack;
				string s = GetItemNameFromId(hid);
				l.Items.Add(h.ToString(), s + "*" + hnum, 0);
				for (int j = 1; j < 10; j++)
				{
					int slot = i * 10 + j;
					var item2 = player.Inventory[slot];
					int itemid = item2.Type;
					int itemnum = item2.Stack;
					l.Items[h.ToString()].SubItems.Add(GetItemNameFromId(itemid) + "*" + itemnum);
				}
			}
			l.EndUpdate();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		private string GetItemNameFromId(int id)
		{
			foreach (var it in MainForm.resource.Items)
			{
				if (id == it.id)
				{
					return it.name;
				}
			}
			return "NULL";
		}
	}
}
