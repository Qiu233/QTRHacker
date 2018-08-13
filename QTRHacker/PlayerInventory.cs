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
