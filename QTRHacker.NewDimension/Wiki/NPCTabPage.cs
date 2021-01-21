using Newtonsoft.Json.Linq;
using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.Res;
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

namespace QTRHacker.NewDimension.Wiki
{
	public class NPCTabPage : TabPage
	{
		private static readonly object _lock = new object();
		public readonly static Color NPCColor = Color.FromArgb(200, 100, 100);
		public ListView NPCListView;
		private MTabControl InfoTabs;
		private NPCInfoSubPage NPCInfoPage;
		private NPCSearcherSubPage SearcherPage;
		public static JArray NPCName_en, NPCName_cn, NPCInfo;
		private string KeyWord = "";
		public bool Updating
		{
			get;
			private set;
		}
		public NPCTabPage()
		{
			if (NPCName_en == null)
			{
				using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Game.WikiRes.zip"))
				{
					using (ZipArchive z = new ZipArchive(s))
					{
						using (var u = new StreamReader(z.GetEntry("NPCName_en.json").Open()))
							NPCName_en = JArray.Parse(u.ReadToEnd());
						using (var u = new StreamReader(z.GetEntry("NPCName_cn.json").Open()))
							NPCName_cn = JArray.Parse(u.ReadToEnd());
						using (var u = new StreamReader(z.GetEntry("NPCInfo.json").Open()))
							NPCInfo = JArray.Parse(u.ReadToEnd());
					}
					GC.Collect();
				}
			}
			this.BackColor = Color.LightGray;
			this.BorderStyle = BorderStyle.None;

			NPCListView = new ListView();
			NPCListView.Bounds = new Rectangle(5, 5, 450, 440);
			NPCListView.FullRowSelect = true;
			NPCListView.MultiSelect = false;
			NPCListView.HideSelection = false;
			NPCListView.View = View.Details;
			NPCListView.Columns.Add(MainForm.CurrentLanguage["Index"], 50);
			NPCListView.Columns.Add(MainForm.CurrentLanguage["EnglishName"], 180);
			NPCListView.Columns.Add(MainForm.CurrentLanguage["ChineseName"], 180);

			NPCListView.MouseDoubleClick += (s, e) =>
			{
				int id = Convert.ToInt32(NPCListView.SelectedItems[0].Text.ToString());
				var player = HackContext.GameContext.MyPlayer;
				NPC.NewNPC(HackContext.GameContext, (int)player.X, (int)player.Y, id);
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
		}


		public void RefreshNPCs()
		{
			lock (_lock)
			{
				Updating = true;
				NPCListView.BeginUpdate();
				NPCListView.Items.Clear();
				for (int i = 0; i < NPCName_en.Count; i++)
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
				}
				NPCListView.EndUpdate();
				Updating = false;
			}
		}
	}
}
