using Newtonsoft.Json.Linq;
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
	public class ItemsTabPage : TabPage
	{
		private static readonly object _lock = new object();
		private const int VALUE_P = 1000000, VALUE_G = 10000, VALUE_S = 100, VALUE_C = 1;
		private readonly Color ItemsColor = Color.FromArgb(160, 160, 200);
		public ListView ItemListView;
		private TabControl InfoTabs;
		private ItemInfoSubPage ItemInfoPage;
		private AccInfoSubPage AccInfoPage;
		private SearcherSubPage SearcherPage;
		public static JArray Items, Items_cn;
		public static JArray Recipes;
		public static JArray ItemDescriptions;
		private string KeyWord = "";
		public bool Updating
		{
			get;
			private set;
		}
		public ItemsTabPage()
		{
			if (Items == null)
			{
				using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Game.WikiRes.zip"))
				{
					using (ZipArchive z = new ZipArchive(s))
					{
						using (var u = new StreamReader(z.GetEntry("ItemInfo.json").Open()))
							Items = JArray.Parse(u.ReadToEnd());
						using (var u = new StreamReader(z.GetEntry("ItemName_cn.json").Open()))
							Items_cn = JArray.Parse(u.ReadToEnd());
						using (var u = new StreamReader(z.GetEntry("RecipeInfo.json").Open()))
							Recipes = JArray.Parse(u.ReadToEnd());
						using (var u = new StreamReader(z.GetEntry("ItemDescriptions.json").Open()))
							ItemDescriptions = JArray.Parse(u.ReadToEnd());
					}
					GC.Collect();
				}
			}
			this.BackColor = Color.LightGray;
			this.BorderStyle = BorderStyle.None;

			ItemListView = new ListView();
			ItemListView.Bounds = new Rectangle(5, 5, 450, 440);
			ItemListView.FullRowSelect = true;
			ItemListView.MultiSelect = false;
			ItemListView.HideSelection = false;
			ItemListView.View = View.Details;
			ItemListView.Columns.Add(MainForm.CurrentLanguage["Index"], 50);
			ItemListView.Columns.Add(MainForm.CurrentLanguage["Rare"], 50);
			ItemListView.Columns.Add(MainForm.CurrentLanguage["EnglishName"], 125);
			ItemListView.Columns.Add(MainForm.CurrentLanguage["ChineseName"], 125);
			ItemListView.Columns.Add(MainForm.CurrentLanguage["Type"], 70);

			ItemListView.MouseDoubleClick += (s, e) =>
			{
				int id = Convert.ToInt32(ItemListView.SelectedItems[0].Text.ToString());
				var player = HackContext.GameContext.MyPlayer;
				int num = Functions.GameObjects.Item.NewItem(HackContext.GameContext, player.X, player.Y, 0, 0, id, Items[id]["maxStack"].ToObject<int>(), false, 0, true);
				Functions.GameObjects.NetMessage.SendData(HackContext.GameContext, 21, -1, -1, 0, num, 0, 0, 0, 0, 0, 0);

			};
			ContextMenuStrip strip = ItemListView.ContextMenuStrip = new ContextMenuStrip();
			strip.Items.Add(MainForm.CurrentLanguage["AddToInvMax"]).Click += (s, e) =>
			{
				int id = Convert.ToInt32(ItemListView.SelectedItems[0].Text.ToString());
				var player = HackContext.GameContext.MyPlayer;
				int num = Functions.GameObjects.Item.NewItem(HackContext.GameContext, player.X, player.Y, 0, 0, id, Items[id]["maxStack"].ToObject<int>(), false, 0, true);
				Functions.GameObjects.NetMessage.SendData(HackContext.GameContext, 21, -1, -1, 0, num, 0, 0, 0, 0, 0, 0);
			};
			strip.Items.Add(MainForm.CurrentLanguage["AddToInvOne"]).Click += (s, e) =>
			{
				int id = Convert.ToInt32(ItemListView.SelectedItems[0].Text.ToString());
				var player = HackContext.GameContext.MyPlayer;
				int num = Functions.GameObjects.Item.NewItem(HackContext.GameContext, player.X, player.Y, 0, 0, id, 1, false, 0, true);
				Functions.GameObjects.NetMessage.SendData(HackContext.GameContext, 21, -1, -1, 0, num, 0, 0, 0, 0, 0, 0);
			};
			strip.Items.Add(MainForm.CurrentLanguage["ShowRecipeTree"]).Click += (s, e) =>
			{
				int id = Convert.ToInt32(ItemListView.SelectedItems[0].Text.ToString());
				RecipeTreeForm.ShowTree(id);
			};
			ItemListView.SelectedIndexChanged += ItemListView_SelectedIndexChanged;

			ItemInfoPage = new ItemInfoSubPage();
			ItemInfoPage.OnRequireItemDoubleClick += RequireItems_MouseDoubleClick;
			ItemInfoPage.OnRecipeToItemDoubleClick += RecipeToItems_MouseDoubleClick;

			AccInfoPage = new AccInfoSubPage();


			SearcherPage = new SearcherSubPage();
			SearcherPage.BlockCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.WallCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.HeadCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.BodyCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.LegCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.AccessoryCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.MeleeCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.RangedCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.MagicCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.SummonCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.BuffCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.ConsumableCheckBox.CheckedChanged += Filter_CheckedChanged;
			SearcherPage.OthersCheckBox.CheckedChanged += Filter_CheckedChanged;

			SearcherPage.KeyWordTextBox.KeyDown += (s, e) =>
			{
				if (e.KeyCode == Keys.Enter)
				{
					e.Handled = true;
					KeyWord = SearcherPage.KeyWordTextBox.Text;
					RefreshItems();
				}
			};

			SearcherPage.SearchButton.Click += (s, e) =>
			{
				KeyWord = SearcherPage.KeyWordTextBox.Text;
				RefreshItems();
			};

			SearcherPage.ResetButton.Click += (s, e) =>
			{
				KeyWord = "";
				SearcherPage.KeyWordTextBox.Text = "";
				RefreshItems();
			};

			InfoTabs = new MTabControl();
			InfoTabs.Bounds = new Rectangle(460, 5, 270, 440);
			InfoTabs.Controls.Add(ItemInfoPage);
			InfoTabs.Controls.Add(AccInfoPage);
			InfoTabs.Controls.Add(SearcherPage);

			Controls.Add(ItemListView);
			Controls.Add(InfoTabs);
		}

		private void Filter_CheckedChanged(object sender, EventArgs e)
		{
			RefreshItems();
		}

		private void RecipeToItems_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var a = (sender as ListBox);
			if (a.SelectedItem != null)
			{
				var v = (a.SelectedItem.ToString());
				var c = v.IndexOf("[");
				var d = v.IndexOf("]");
				var b = ItemListView.Items[v.Substring(c + 1, d - c - 1)];
				if (b != null)
				{
					ItemListView.EnsureVisible(b.Index);
					b.Selected = true;
				}
			}
		}

		private void RequireItems_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var a = (sender as ListBox);
			if (a.SelectedItem != null)
			{
				var v = (a.SelectedItem.ToString());
				var c = v.IndexOf("[");
				var d = v.IndexOf("]");
				var b = ItemListView.Items[v.Substring(c + 1, d - c - 1)];
				if (b != null)
				{
					ItemListView.EnsureVisible(b.Index);
					b.Selected = true;
				}
			}
		}

		public static string GetValueString(int value)
		{
			int p = value / VALUE_P;
			int a = value % VALUE_P;
			int g = a / VALUE_G;
			a = a % VALUE_G;
			int s = a / VALUE_S;
			a = a % VALUE_S;
			int c = a / VALUE_C;
			a = a % VALUE_C;
			return p + MainForm.CurrentLanguage["Platinum"] + " " + g +
				MainForm.CurrentLanguage["Gold"] + " " + s +
				MainForm.CurrentLanguage["Silver"] + " " + c +
				MainForm.CurrentLanguage["Copper"] + "";
		}

		private void ItemListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ItemListView.SelectedIndices.Count > 0)
			{
				int i = Convert.ToInt32(ItemListView.SelectedItems[0].Text.ToString());

				ItemInfoPage.SetData(i);
				AccInfoPage.SetData(i);
			}
			else
			{
				ItemInfoPage.ResetData();
				AccInfoPage.ResetData();
			}
		}

		private bool Filter(JToken j)
		{
			List<bool> b = new List<bool>();
			b.Add(j["createTile"].ToObject<int>() != -1);
			b.Add(j["createWall"].ToObject<int>() != -1);
			b.Add(j["headSlot"].ToObject<int>() != -1);
			b.Add(j["bodySlot"].ToObject<int>() != -1);
			b.Add(j["legSlot"].ToObject<int>() != -1);
			b.Add(j["accessory"].ToObject<bool>());
			b.Add(j["melee"].ToObject<bool>());
			b.Add((j["ranged"].ToObject<bool>()));
			b.Add(j["magic"].ToObject<bool>());
			b.Add((j["summon"].ToObject<bool>() || j["sentry"].ToObject<bool>()));
			b.Add(j["buffType"].ToObject<int>() != 0);
			b.Add(j["consumable"].ToObject<bool>());
			bool r = false;
			r |= (SearcherPage.BlockCheckBox.Checked && b[0]);
			r |= (SearcherPage.WallCheckBox.Checked && b[1]);
			r |= (SearcherPage.HeadCheckBox.Checked && b[2]);
			r |= (SearcherPage.BodyCheckBox.Checked && b[3]);
			r |= (SearcherPage.LegCheckBox.Checked && b[4]);
			r |= (SearcherPage.AccessoryCheckBox.Checked && b[5]);
			r |= (SearcherPage.MeleeCheckBox.Checked && b[6]);
			r |= (SearcherPage.RangedCheckBox.Checked && b[7]);
			r |= (SearcherPage.MagicCheckBox.Checked && b[8]);
			r |= (SearcherPage.SummonCheckBox.Checked && b[9]);
			r |= (SearcherPage.BuffCheckBox.Checked && b[10]);
			r |= (SearcherPage.ConsumableCheckBox.Checked && b[11]);
			if (b.TrueForAll(t => !t) && SearcherPage.OthersCheckBox.Checked)
				return true;
			return r;
		}

		private string GetItemType(JToken j)
		{
			List<bool> b = new List<bool>();
			if (j["createTile"].ToObject<int>() != -1) return MainForm.CurrentLanguage["Blocks"];
			if (j["createWall"].ToObject<int>() != -1) return MainForm.CurrentLanguage["Walls"];
			if (j["headSlot"].ToObject<int>() != -1) return MainForm.CurrentLanguage["Head"];
			if (j["bodySlot"].ToObject<int>() != -1) return MainForm.CurrentLanguage["Body"];
			if (j["legSlot"].ToObject<int>() != -1) return MainForm.CurrentLanguage["Leg"];
			if (j["accessory"].ToObject<bool>()) return MainForm.CurrentLanguage["Accessory"];
			if (j["melee"].ToObject<bool>()) return MainForm.CurrentLanguage["Melee"];
			if (j["ranged"].ToObject<bool>()) return MainForm.CurrentLanguage["Ranged"];
			if (j["magic"].ToObject<bool>()) return MainForm.CurrentLanguage["Magic"];
			if ((j["summon"].ToObject<bool>() || j["sentry"].ToObject<bool>())) return MainForm.CurrentLanguage["Summon"];
			if (j["buffType"].ToObject<int>() != 0) return MainForm.CurrentLanguage["Buff"];
			if (j["consumable"].ToObject<bool>()) return MainForm.CurrentLanguage["Consumable"];
			return "无";
		}

		public void RefreshItems()
		{
			lock (_lock)
			{
				Updating = true;
				ItemListView.BeginUpdate();
				ItemListView.Items.Clear();
				for (int i = 0; i < Items.Count; i++)
				{
					var itm = Items[i];
					if (itm["type"].ToString() == "0" || !Filter(itm)) continue;
					bool flag = false;
					flag |= itm["type"].ToString().ToLower().Contains(KeyWord.ToLower());
					flag |= Items_cn[i].ToString().ToLower().Contains(KeyWord.ToLower());
					flag |= itm["Name"].ToString().ToLower().Contains(KeyWord.ToLower());
					flag |= itm["shoot"].ToString().ToLower().Contains(KeyWord.ToLower());
					flag |= itm["createTile"].ToString().ToLower().Contains(KeyWord.ToLower());
					flag |= itm["createWall"].ToString().ToLower().Contains(KeyWord.ToLower());
					if (flag)
					{
						ListViewItem lvi = new ListViewItem(itm["type"].ToString());
						lvi.Name = itm["type"].ToString();
						lvi.SubItems.Add(itm["rare"].ToString());
						lvi.SubItems.Add(itm["Name"].ToString());
						lvi.SubItems.Add(Items_cn[i].ToString());
						lvi.SubItems.Add(GetItemType(itm));
						ItemListView.Items.Add(lvi);
					}
				}
				ItemListView.EndUpdate();
				Updating = false;
			}
		}
	}
}
