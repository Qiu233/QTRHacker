using QTRHacker.Functions;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.PlayerEditor;
using QTRHacker.NewDimension.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_Player : PagePanel
	{
		public MListView PlayerListView;
		public Timer UpdatePlayerTimer;
		public Button EditPlayerInfoButton;
		public PagePanel_Player(int Width, int Height) : base(Width, Height)
		{
			EditPlayerInfoButton = new Button();
			EditPlayerInfoButton.Enabled = false;
			EditPlayerInfoButton.FlatStyle = FlatStyle.Flat;
			EditPlayerInfoButton.Text = "编辑玩家";
			EditPlayerInfoButton.ForeColor = Color.White;
			EditPlayerInfoButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			EditPlayerInfoButton.Bounds = new Rectangle(Width - 102, 335, 100, 30);
			EditPlayerInfoButton.Click += (s, e) =>
			{
				PlayerEditorForm f = new PlayerEditorForm();
				f.Show();
			};

			Controls.Add(EditPlayerInfoButton);


			PlayerListView = new MListView();
			PlayerListView.SelectedIndexChanged += (s, e) =>
			{
				var m = s as MListView;
				if (m.SelectedIndices.Count > 0)
					EditPlayerInfoButton.Enabled = true;
				else
					EditPlayerInfoButton.Enabled = false;
			};
			PlayerListView.Bounds = new Rectangle(3, 3, Width - 5, 330);
			PlayerListView.Columns.Add("序号", 40);
			PlayerListView.Columns.Add("名称", 70);
			PlayerListView.Columns.Add("X坐标", 90);
			PlayerListView.Columns.Add("Y坐标", 90);
			Controls.Add(PlayerListView);


			UpdatePlayerTimer = new Timer();
			UpdatePlayerTimer.Interval = 50;//每隔50ms进行一次玩家列表的检查和更新
			UpdatePlayerTimer.Tick += (s, e) =>
			{
				if (Visible)
					UpdatePlayerList();
			};
			UpdatePlayerTimer.Start();
		}

		public void UpdatePlayerList()
		{
			if (HackContext.GameContext == null) return;
			for (int i = 0; i < Player.MAX_PLAYER; i++)
			{
				Player p = HackContext.GameContext.Players[i];
				var ps = PlayerListView.Items.Find(i.ToString(), false);
				if (p.Active)//玩家存在
				{
					if (ps.Length == 0)//添加到列表
					{
						var t = PlayerListView.Items.Add(i.ToString(), i.ToString(), 0);
						var name = t.SubItems.Add(p.Name);
						t.SubItems.Add(p.X.ToString());
						t.SubItems.Add(p.Y.ToString());
						if (i == HackContext.GameContext.MyPlayerIndex)
							name.ForeColor = Color.DeepPink;
					}
					else//更新数据
					{
						var t = ps[0];
						t.SubItems[1].Text = p.Name;
						t.SubItems[2].Text = p.X.ToString();
						t.SubItems[3].Text = p.Y.ToString();
					}
				}
				else//不存在
				{
					if (ps.Length > 0)//但是列表里存在
						ps[0].Remove();//移除
				}
			}
		}
	}
}
