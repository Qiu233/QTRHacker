/*
 * 由SharpDevelop创建。
 * 用户： lopi2
 * 日期: 2017/1/24
 * 时间: 17:22
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Terraria_Hacker
{
	/// <summary>
	/// Description of PlayerDetail_Buff.
	/// </summary>
	public partial class PlayerDetail_Buff : Form
	{
		private int id;
		public PlayerDetail_Buff(int id, string name)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			
			this.Text = name;
			this.id = id;
			InitializeComponent();
			ListView l = new ListView();
			l.Location = new Point(0, 0);
			l.Size = new Size(700, 80);
			l.View = View.Details;
			l.MultiSelect = false;
			l.FullRowSelect = true;
			this.Controls.Add(l);


			l.BeginUpdate();
			/*for (int i = 0; i < HackFunctions.BUFF_MAX_COUNT; i++)
			{
				l.Columns.Add(i.ToString());
			}
			foreach (ColumnHeader item in l.Columns)
			{
				item.Width = 150;
			}
			l.Items.Add("head", GetBuffNameFromId(HackFunctions.getPlayerBuffType(id, 0)) + "/" + HackFunctions.getPlayerBuffTime(id, 0) + "tick", 0);
			for (int j = 1; j < HackFunctions.BUFF_MAX_COUNT; j++)
			{
				int slot = j;
				int buffid = HackFunctions.getPlayerBuffType(id, slot);
				int bufftype = HackFunctions.getPlayerBuffTime(id, slot);
				l.Items["head"].SubItems.Add(GetBuffNameFromId(buffid) + "/" + bufftype + "tick");
			}*/
			l.EndUpdate();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		private static string GetBuffNameFromId(int id)
		{
			foreach (var it in MainForm.resource.Buffs)
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
