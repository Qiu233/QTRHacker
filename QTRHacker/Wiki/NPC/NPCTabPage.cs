using Newtonsoft.Json.Linq;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Controls;
using QTRHacker.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Wiki.NPC
{
	public class NPCTabPage : TabPage
	{
		private static readonly object _lock = new();
		public readonly static Color NPCColor = Color.FromArgb(240, 120, 120);
		public ListView NPCListView;
		private readonly MTabControl InfoTabs;
		private readonly NPCInfoSubPage NPCInfoPage;
		private readonly NPCSearcherSubPage SearcherPage;
		public static JArray /*NPCName_en, NPCName_cn, */NPCInfo;
		private string KeyWord = "";
		public bool Updating
		{
			get;
			private set;
		}
		public NPCTabPage()
		{
			if (NPCInfo == null)
			{
				using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Game.WikiRes.zip");
				using ZipArchive z = new(s);
				/*using (var u = new StreamReader(z.GetEntry("NPCName_en.json").Open()))
					NPCName_en = JArray.Parse(u.ReadToEnd());
				using (var u = new StreamReader(z.GetEntry("NPCName_cn.json").Open()))
					NPCName_cn = JArray.Parse(u.ReadToEnd());*/
				using (var u = new StreamReader(z.GetEntry("NPCInfo.json").Open()))
					NPCInfo = JArray.Parse(u.ReadToEnd());
				GC.Collect();
			}
			BackColor = Color.LightGray;
			BorderStyle = BorderStyle.None;

			NPCListView = new ListView();
			NPCListView.Bounds = new Rectangle(5, 5, 450, 440);
			NPCListView.FullRowSelect = true;
			NPCListView.MultiSelect = false;
			NPCListView.HideSelection = false;
			NPCListView.View = View.Details;
			NPCListView.Columns.Add(HackContext.CurrentLanguage["Index"], 50);
			NPCListView.Columns.Add(HackContext.CurrentLanguage["EnglishName"], 180);
			NPCListView.Columns.Add(HackContext.CurrentLanguage["ChineseName"], 180);

			NPCListView.MouseDoubleClick += (s, e) =>
			{
				int id = Convert.ToInt32(NPCListView.SelectedItems[0].Text.ToString());
				var pos = HackContext.GameContext.MyPlayer.Position;
				Functions.GameObjects.Terraria.NPC.NewNPC(HackContext.GameContext, (int)pos.X, (int)pos.Y, id);
			};

			ContextMenuStrip strip = NPCListView.ContextMenuStrip = new ContextMenuStrip();
			strip.Items.Add(HackContext.CurrentLanguage["AddToNPCOne"]).Click += (s, e) =>
			{
				int id = Convert.ToInt32(NPCListView.SelectedItems[0].Text.ToString());
				var pos = HackContext.GameContext.MyPlayer.Position;
				Functions.GameObjects.Terraria.NPC.NewNPC(HackContext.GameContext, (int)pos.X, (int)pos.Y + 10, id);
			};
			strip.Items.Add(HackContext.CurrentLanguage["AddToNPCSpecify"]).Click += (s, e) =>
			{
				int id = Convert.ToInt32(NPCListView.SelectedItems[0].Text.ToString());
				var player = HackContext.GameContext.MyPlayer;
				MForm AddNPCMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = HackContext.CurrentLanguage["AddNPC"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 92)
				};

				Label NumberTip = new Label()
				{
					Text = $"{HackContext.CurrentLanguage["Number"]}:",
					Location = new Point(0, 0),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter,
				};
				AddNPCMForm.MainPanel.Controls.Add(NumberTip);

				TextBox NumberBox = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = "1",
					Location = new Point(85, 0),
					Size = new Size(95, 20)
				};
				NumberBox.KeyPress += (s1, e1) => e1.Handled = e1.Handled || (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-');
				AddNPCMForm.MainPanel.Controls.Add(NumberBox);


				Label CoorXTip = new Label()
				{
					Text = "X:",
					Location = new Point(0, 20),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				AddNPCMForm.MainPanel.Controls.Add(CoorXTip);

				TextBox CoorXBox = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = ((int)player.Position.X).ToString(),
					Location = new Point(85, 20),
					Size = new Size(95, 20)
				};
				CoorXBox.KeyPress += (s1, e1) => e1.Handled = e1.Handled || (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-');
				AddNPCMForm.MainPanel.Controls.Add(CoorXBox);

				Label CoorYTip = new Label()
				{
					Text = "Y:",
					Location = new Point(0, 40),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				AddNPCMForm.MainPanel.Controls.Add(CoorYTip);

				TextBox CoorYBox = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = ((int)player.Position.Y).ToString(),
					Location = new Point(85, 40),
					Size = new Size(95, 20)
				};
				CoorYBox.KeyPress += (s1, e1) => e1.Handled = e1.Handled || (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-');
				AddNPCMForm.MainPanel.Controls.Add(CoorYBox);

				Button ConfirmButton = new Button();
				ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 60);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					int count = int.Parse(NumberBox.Text);
					for (int i = 0; i < count; i++)
						Functions.GameObjects.Terraria.NPC.NewNPC(HackContext.GameContext, int.Parse(CoorXBox.Text), int.Parse(CoorYBox.Text), id);
					AddNPCMForm.Dispose();
				};
				AddNPCMForm.MainPanel.Controls.Add(ConfirmButton);
				AddNPCMForm.ShowDialog(this);
			};


			NPCListView.SelectedIndexChanged += ItemListView_SelectedIndexChanged;

			NPCInfoPage = new NPCInfoSubPage();
			SearcherPage = new NPCSearcherSubPage();
			SearcherPage.TownNPCCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.BossCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.FriendlyCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.OthersCheckBox.CheckedChanged += Filter_CheckedChanged;

			SearcherPage.KeyWordTextBox.KeyDown += (s, e) =>
			{
				if (e.KeyCode == Keys.Enter)
				{
					e.Handled = true;
					KeyWord = SearcherPage.KeyWordTextBox.Text;
					RefreshNPCs();
				}
			};

			SearcherPage.SearchButton.Click += (s, e) =>
			{
				KeyWord = SearcherPage.KeyWordTextBox.Text;
				RefreshNPCs();
			};

			SearcherPage.ResetButton.Click += (s, e) =>
			{
				KeyWord = "";
				SearcherPage.KeyWordTextBox.Text = "";
				RefreshNPCs();
			};

			InfoTabs = new MTabControl();
			InfoTabs.TColor = NPCColor;
			InfoTabs.Bounds = new Rectangle(460, 5, 270, 440);
			InfoTabs.Controls.Add(NPCInfoPage);
			InfoTabs.Controls.Add(SearcherPage);

			Controls.Add(NPCListView);
			Controls.Add(InfoTabs);
		}

		private void Filter_CheckedChanged(object sender, EventArgs e)
		{
			RefreshNPCs();
		}

		private bool Filter(JToken j)
		{
			List<bool> b = new List<bool>();
			b.Add(j["TownNPC"].ToObject<bool>());
			b.Add(j["Boss"].ToObject<bool>());
			b.Add(j["Friendly"].ToObject<bool>());
			bool r = false;
			r |= (SearcherPage.TownNPCCheckBox.Checked && b[0]);
			r |= (SearcherPage.BossCheckBox.Checked && b[1]);
			r |= (SearcherPage.FriendlyCheckBox.Checked && b[2]);
			if (b.TrueForAll(t => !t) && SearcherPage.OthersCheckBox.Checked)
				return true;
			return r;
		}

		private void ItemListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (NPCListView.SelectedIndices.Count > 0)
			{
				int i = Convert.ToInt32(NPCListView.SelectedItems[0].Text.ToString());
				NPCInfoPage.SetData(i);
			}
			else
			{
				NPCInfoPage.SetData(0);
			}
			NPCListView.Focus();
		}


		public void RefreshNPCs()
		{
			lock (_lock)
			{
				Updating = true;
				NPCListView.BeginUpdate();
				NPCListView.Items.Clear();
				/*for (int i = 0; i < NPCInfo.Count; i++)
				{
					var npc = NPCName_en[i];
					if (npc["Type"].ToString() == "0" || !Filter(NPCInfo[i])) continue;
					bool flag = false;
					flag |= npc["Type"].ToString().ToLower().Contains(KeyWord.ToLower());
					flag |= NPCName_cn[i]["Name"].ToString().ToLower().Contains(KeyWord.ToLower());
					flag |= npc["Name"].ToString().ToLower().Contains(KeyWord.ToLower());
					if (flag)
					{
						ListViewItem lvi = new ListViewItem(npc["Type"].ToString());
						lvi.Name = npc["Type"].ToString();
						lvi.SubItems.Add(npc["Name"].ToString());
						lvi.SubItems.Add(NPCName_cn[i]["Name"].ToString());
						NPCListView.Items.Add(lvi);
					}
				}*/
				NPCListView.EndUpdate();
				Updating = false;
			}
		}
	}
}
