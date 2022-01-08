using QHackLib;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PagePanels
{/*
	public class PagePanel_AimBot : PagePanel
	{
		public MListView PlayerListView;
		private bool _locked;

		public bool Locked
		{
			get => _locked;
			set
			{
				_locked = value;
				if (_locked)
				{
					PlayerListView.Enabled = false;
					LockPlayerButton.Enabled = false;
					UnLockPlayerButton.Enabled = true;
					return;
				}
				PlayerListView.Enabled = true;
				LockPlayerButton.Enabled = true;
				UnLockPlayerButton.Enabled = false;
			}
		}
		public int TargetIndex
		{
			get;
			set;
		}
		private Button LockPlayerButton, UnLockPlayerButton;
		private readonly Timer UpdatePlayerTimer;
		public PagePanel_AimBot(int Width, int Height) : base(Width, Height)
		{
			PlayerListView = new MListView();
			PlayerListView.Bounds = new Rectangle(3, 3, 210, 362);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Index"], 40);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Name"], 70);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Life"], 50);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Mana"], 50);
			this.Controls.Add(PlayerListView);

			LockPlayerButton = new Button();
			LockPlayerButton.FlatStyle = FlatStyle.Flat;
			LockPlayerButton.Text = HackContext.CurrentLanguage["Lock"];
			LockPlayerButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			LockPlayerButton.Bounds = new Rectangle(215, 3, 80, 30);
			LockPlayerButton.Click += (s, e) =>
			{
				TargetIndex = Convert.ToInt32(PlayerListView.SelectedItems[0].Text);
				Locked = true;
				var helper = HackContext.GameContext.HContext.GetAddressHelper("TRInjections.dll");
				int tiAddr = helper.GetStaticFieldAddress("TRInjections.AimBot", "TargetIndex");
				int enAddr = helper.GetStaticFieldAddress("TRInjections.AimBot", "Enabled");
				int target = TargetIndex;
				bool enabled = true;
				NativeFunctions.WriteProcessMemory(HackContext.GameContext.HContext.Handle, tiAddr, ref target, 4, 0);
				NativeFunctions.WriteProcessMemory(HackContext.GameContext.HContext.Handle, enAddr, ref enabled, 1, 0);
			};
			Controls.Add(LockPlayerButton);

			UnLockPlayerButton = new Button();
			UnLockPlayerButton.Enabled = false;
			UnLockPlayerButton.FlatStyle = FlatStyle.Flat;
			UnLockPlayerButton.Text = HackContext.CurrentLanguage["Unlock"];
			UnLockPlayerButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			UnLockPlayerButton.Bounds = new Rectangle(215, 33, 80, 30);
			UnLockPlayerButton.Click += (s, e) =>
			{
				Locked = false;
				var helper = HackContext.GameContext.HContext.GetAddressHelper("TRInjections.dll");
				int enAddr = helper.GetStaticFieldAddress("TRInjections.AimBot", "Enabled");
				bool enabled = false;
				NativeFunctions.WriteProcessMemory(HackContext.GameContext.HContext.Handle, enAddr, ref enabled, 1, 0);
			};
			Controls.Add(UnLockPlayerButton);
			
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
						t.SubItems.Add(p.Life.ToString());
						t.SubItems.Add(p.Mana.ToString());
						if (i == HackContext.GameContext.MyPlayerIndex)
							name.ForeColor = Color.DeepPink;
					}
					else//更新数据
					{
						var t = ps[0];
						t.SubItems[1].Text = p.Name;
						t.SubItems[2].Text = p.Life.ToString();
						t.SubItems[3].Text = p.Mana.ToString();
					}
				}
				else//不存在
				{
					if (ps.Length > 0)//但是列表里存在
						ps[0].Remove();//移除
				}
			}


			var helper = HackContext.GameContext.HContext.GetAddressHelper("TRInjections.dll");
			int enAddr = helper.GetStaticFieldAddress("TRInjections.AimBot", "Enabled");
			bool enabled = false;
			NativeFunctions.ReadProcessMemory(HackContext.GameContext.HContext.Handle, enAddr, ref enabled, 1, 0);
			Locked = enabled;
		}
	}*/
}
