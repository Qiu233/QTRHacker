using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.PlayerEditor.Controls;
using QTRHacker.NewDimension.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PlayerEditor
{
	public class InvEditor : ItemSlotsEditor
	{
		public InvEditor(GameContext Context, Form ParentForm, Player TargetPlayer, bool Editable) : base(Context, ParentForm, TargetPlayer, TargetPlayer.Inventory, Editable, Player.ITEM_MAX_COUNT - 9)
		{
			Text = MainForm.CurrentLanguage["Inventory"];


			Button SaveInv = new Button();
			SaveInv.Click += (sender, e) =>
			{
				SaveFileDialog sfd = new SaveFileDialog()
				{
					Filter = "inv files (*.inv)|*.inv",
				};
				if (!Directory.Exists("./invs"))
					Directory.CreateDirectory("./invs");
				sfd.InitialDirectory = Path.GetFullPath("./invs");
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					SaveInventory(sfd.FileName);
					SlotsPanel.Refresh();
				}
			};
			SaveInv.FlatStyle = FlatStyle.Flat;
			SaveInv.Text = MainForm.CurrentLanguage["Save"];
			SaveInv.Size = new Size(80, 30);
			SaveInv.Location = new Point(260, 60);
			ItemPropertiesPanel.Controls.Add(SaveInv);

			Button LoadInv = new Button();
			LoadInv.Enabled = Editable;
			LoadInv.Click += (sender, e) =>
			{
				OpenFileDialog ofd = new OpenFileDialog()
				{
					Filter = "inv files (*.inv)|*.inv"
				};
				if (!Directory.Exists("./invs"))
					Directory.CreateDirectory("./invs");
				ofd.InitialDirectory = Path.GetFullPath("./invs");
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					LoadInventory(ofd.FileName);
					SlotsPanel.Refresh();
					InitItemData(Selected);
				}
			};
			LoadInv.FlatStyle = FlatStyle.Flat;
			LoadInv.Text = MainForm.CurrentLanguage["Load"];
			LoadInv.Size = new Size(80, 30);
			LoadInv.Location = new Point(260, 90);
			ItemPropertiesPanel.Controls.Add(LoadInv);

			Button SaveInvPItem = new Button();
			SaveInvPItem.Enabled = Editable;
			SaveInvPItem.Click += (sender, e) =>
			{
				SaveFileDialog sfd = new SaveFileDialog()
				{
					Filter = "inv files (*.invp)|*.invp"
				};
				if (!Directory.Exists("./invs"))
					Directory.CreateDirectory("./invs");
				sfd.InitialDirectory = Path.GetFullPath("./invs");
				if (sfd.ShowDialog() == DialogResult.OK)
				{


					File.WriteAllText(sfd.FileName, TargetPlayer.SerializeInventoryWithProperties());
					SlotsPanel.Refresh();

				}
			};
			SaveInvPItem.FlatStyle = FlatStyle.Flat;
			SaveInvPItem.Text = $"{MainForm.CurrentLanguage["Save"]}(P)";
			SaveInvPItem.Size = new Size(80, 30);
			SaveInvPItem.Location = new Point(260, 120);
			ItemPropertiesPanel.Controls.Add(SaveInvPItem);

			Button LoadInvPItem = new Button();
			LoadInvPItem.Enabled = Editable;
			LoadInvPItem.Click += (sender, e) =>
			{
				OpenFileDialog ofd = new OpenFileDialog()
				{
					Filter = "inv files (*.invp)|*.invp"
				};
				if (!Directory.Exists("./invs"))
					Directory.CreateDirectory("./invs");
				ofd.InitialDirectory = Path.GetFullPath("./invs");
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					int j = 0;
					Form p = new Form();
					ProgressBar pb = new ProgressBar();
					Label tip = new Label(), percent = new Label();
					tip.Text = "Loading inventory...";
					tip.Location = new Point(0, 0);
					tip.Size = new Size(150, 30);
					tip.TextAlign = ContentAlignment.MiddleCenter;
					percent.Location = new Point(150, 0);
					percent.Size = new Size(30, 30);
					percent.TextAlign = ContentAlignment.MiddleCenter;
					System.Timers.Timer timer = new System.Timers.Timer(1);
					p.FormBorderStyle = FormBorderStyle.FixedSingle;
					p.ClientSize = new Size(300, 60);
					p.ControlBox = false;
					pb.Location = new Point(0, 30);
					pb.Size = new Size(300, 30);
					pb.Maximum = 2;
					pb.Minimum = 0;
					pb.Value = 0;
					p.Controls.Add(tip);
					p.Controls.Add(percent);
					p.Controls.Add(pb);
					timer.Elapsed += (sender1, e1) =>
					{
						pb.Value = j;
						percent.Text = pb.Value + "/" + pb.Maximum;
						if (j >= pb.Maximum) p.Dispose();
					};
					timer.Start();
					p.Show();
					p.Location = new System.Drawing.Point(ParentForm.Location.X + ParentForm.Width / 2 - p.ClientSize.Width / 2, ParentForm.Location.Y + ParentForm.Height / 2 - p.ClientSize.Height / 2);
					new System.Threading.Thread((s) =>
					{
						j++;

						TargetPlayer.DeserializeInventoryWithProperties(File.ReadAllText(ofd.FileName));
						InitItemData(Selected);
						j++;

					}
					).Start();
				}
			};
			LoadInvPItem.FlatStyle = FlatStyle.Flat;
			LoadInvPItem.Text = $"{MainForm.CurrentLanguage["Load"]}(P)";
			LoadInvPItem.Size = new Size(80, 30);
			LoadInvPItem.Location = new Point(260, 150);
			ItemPropertiesPanel.Controls.Add(LoadInvPItem);

			
			ButtonInitItem.Location = new Point(260, 180);

			SlotsPanel.ItemSlots[0].Selected = true;
			InitItemData(0);
		}
		

		public void SaveInventory(string name)
		{
			if (File.Exists(name)) File.Delete(name);
			BinaryWriter bw = new BinaryWriter(new FileStream(name, FileMode.OpenOrCreate));
			var player = TargetPlayer;
			for (int i = 0; i < Player.ITEM_MAX_COUNT; i++)
			{
				var item = player.Inventory[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.ARMOR_MAX_COUNT; i++)
			{
				var item = player.Armor[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.DYE_MAX_COUNT; i++)
			{
				var item = player.Dye[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.MISC_MAX_COUNT; i++)
			{
				var item = player.Misc[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < Player.MISCDYE_MAX_COUNT; i++)
			{
				var item = player.MiscDye[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			bw.Close();
		}
		public void LoadInventory(string name)
		{
			int j = 0;
			Form p = new Form();
			ProgressBar pb = new ProgressBar();
			Label tip = new Label(), percent = new Label();
			tip.Text = "Loading inventory...";
			tip.Location = new Point(0, 0);
			tip.Size = new Size(150, 30);
			tip.TextAlign = ContentAlignment.MiddleCenter;
			percent.Location = new Point(150, 0);
			percent.Size = new Size(30, 30);
			percent.TextAlign = ContentAlignment.MiddleCenter;
			System.Timers.Timer timer = new System.Timers.Timer(1);
			p.FormBorderStyle = FormBorderStyle.FixedSingle;
			p.ClientSize = new Size(300, 60);
			p.ControlBox = false;
			pb.Location = new Point(0, 30);
			pb.Size = new Size(300, 30);
			pb.Maximum = Player.ITEM_MAX_COUNT + Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT + Player.MISCDYE_MAX_COUNT;
			pb.Minimum = 0;
			pb.Value = 0;
			p.Controls.Add(tip);
			p.Controls.Add(percent);
			p.Controls.Add(pb);
			timer.Elapsed += (sender, e) =>
			{
				pb.Value = j;
				percent.Text = pb.Value + "/" + pb.Maximum;
				if (j >= pb.Maximum) p.Dispose();
			};
			timer.Start();
			p.Show();
			p.Location = new System.Drawing.Point(ParentForm.Location.X + ParentForm.Width / 2 - p.ClientSize.Width / 2, ParentForm.Location.Y + ParentForm.Height / 2 - p.ClientSize.Height / 2);
			new System.Threading.Thread((s) =>
			{
				var player = TargetPlayer;
				BinaryReader br = new BinaryReader(new FileStream(name, FileMode.Open));
				for (int i = 0; i < Player.ITEM_MAX_COUNT; i++)
				{
					j++;
					var item = player.Inventory[i];
					int type = br.ReadInt32();
					int stack = br.ReadInt32();
					byte prefix = br.ReadByte();
					if (type <= 0 && item.Type <= 0) continue;
					item.SetDefaultsAndPrefix(type, prefix);
					item.Stack = stack;
				}
				for (int i = 0; i < Player.ARMOR_MAX_COUNT; i++)
				{
					j++;
					var item = player.Armor[i];
					int type = br.ReadInt32();
					int stack = br.ReadInt32();
					byte prefix = br.ReadByte();
					if (type <= 0 && item.Type <= 0) continue;
					item.SetDefaultsAndPrefix(type, prefix);
					item.Stack = stack;
				}
				for (int i = 0; i < Player.DYE_MAX_COUNT; i++)
				{
					j++;
					var item = player.Dye[i];
					int type = br.ReadInt32();
					int stack = br.ReadInt32();
					byte prefix = br.ReadByte();
					if (type <= 0 && item.Type <= 0) continue;
					item.SetDefaultsAndPrefix(type, prefix);
					item.Stack = stack;
				}
				for (int i = 0; i < Player.MISC_MAX_COUNT; i++)
				{
					j++;
					var item = player.Misc[i];
					int type = br.ReadInt32();
					int stack = br.ReadInt32();
					byte prefix = br.ReadByte();
					if (type <= 0 && item.Type <= 0) continue;
					item.SetDefaultsAndPrefix(type, prefix);
					item.Stack = stack;
				}
				for (int i = 0; i < Player.MISCDYE_MAX_COUNT; i++)
				{
					j++;
					var item = player.MiscDye[i];
					int type = br.ReadInt32();
					int stack = br.ReadInt32();
					byte prefix = br.ReadByte();
					if (type <= 0 && item.Type <= 0) continue;
					item.SetDefaultsAndPrefix(type, prefix);
					item.Stack = stack;
				}
				br.Close();
			}
			).Start();
		}
	}
}
