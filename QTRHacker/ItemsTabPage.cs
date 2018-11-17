using Newtonsoft.Json.Linq;
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

namespace QTRHacker
{
	public class ItemsTabPage : TabPage
	{
		private static readonly object _lock = new object();
		private const int VALUE_P = 1000000, VALUE_G = 10000, VALUE_S = 100, VALUE_C = 1;
		private readonly Color ItemsColor = Color.FromArgb(160, 160, 200);
		public ListView ItemListView;
		private TabControl InfoTabs;
		private TabPage ItemInfoPage, AccInfoPage, SearcherPage;
		private static JArray Items, Items_cn;
		private static JArray Recipes;
		private InfoView ItemIconInfoView, ItemNameInfoView, ItemTypeInfoView, ItemRareInfoView, ItemDescriptionInfoView, ItemRecipeFromInfoView, ItemRecipeToInfoView, ItemValueInfoView;
		private InfoView ItemIcon2InfoView, ItemPickaxeInfoView, ItemAxeInfoView, ItemHammerInfoView, ItemDamageInfoView, ItemDefenseInfoView, ItemCritInfoView, ItemUseTimeInfoView, ItemKnockbackInfoView;
		private InfoView ItemHealLifeInfoView, ItemHealManaInfoView, ItemManaConsumeInfoView, ItemBaitInfoView, ItemShootInfoView, ItemShootSpeedInfoView, ItemCreateTileInfoView, ItemBuffTypeInfoView, ItemBuffTimeInfoView, ItemUseAnimationInfoView, ItemPlaceStyleInfoView, ItemCreateWallInfoView, ItemTileBoostInfoView;
		private InfoView ItemDescription2InfoView;
		private CheckBox BlockCheckBox, WallCheckBox, HeadCheckBox, BodyCheckBox, LegCheckBox, AccessoryCheckBox, MeleeCheckBox, RangedCheckBox, MagicCheckBox, SummonCheckBox, BuffCheckBox, ConsumableCheckBox, OthersCheckBox;
		private TextBox KeyWordTextBox;
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
				using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Resource.WikiRes.zip"))
				{
					ZipArchive z = new ZipArchive(s);
					using (var u = new StreamReader(z.GetEntry("ItemInfo.json").Open()))
						Items = JArray.Parse(u.ReadToEnd());
					using (var u = new StreamReader(z.GetEntry("ItemName_cn.json").Open()))
						Items_cn = JArray.Parse(u.ReadToEnd());
					using (var u = new StreamReader(z.GetEntry("RecipeInfo.json").Open()))
						Recipes = JArray.Parse(u.ReadToEnd());
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
			ItemListView.Columns.Add(Lang.ID, 50);
			ItemListView.Columns.Add(Lang.rare, 50);
			ItemListView.Columns.Add(Lang.name_en, 125);
			ItemListView.Columns.Add(Lang.name_cn, 125);
			ItemListView.Columns.Add(Lang.type, 70);

			ItemListView.MouseDoubleClick += (s, e) =>
			{
				int id = Convert.ToInt32(ItemListView.SelectedItems[0].Text.ToString());
				var player = MainForm.Context.MyPlayer;
				int num = QTRHacker.Functions.Item.NewItem(MainForm.Context, player.X, player.Y, 0, 0, id, Items[id]["maxStack"].ToObject<int>(), false, 0, true);
				QTRHacker.Functions.NetMessage.SendData(MainForm.Context, 21, -1, -1, 0, num, 0, 0, 0, 0, 0, 0);
			};
			ItemListView.SelectedIndexChanged += ItemListView_SelectedIndexChanged;

			ItemInfoPage = new TabPage(Lang.itemInfo);

			{
				ItemIconInfoView = new InfoView(new PictureBox() { SizeMode = PictureBoxSizeMode.CenterImage }, InfoView.Dock.Top);
				ItemIconInfoView.Text = Lang.icon;
				ItemIconInfoView.Bounds = new Rectangle(5, 5, 80, 80);
				ItemIconInfoView.Tip.BackColor = ItemsColor;

				ItemNameInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.Dock.Left, false);
				ItemNameInfoView.Text = Lang.name;
				ItemNameInfoView.Tip.BackColor = ItemsColor;
				ItemNameInfoView.Bounds = new Rectangle(0, 0, 170, 20);

				ItemTypeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.Dock.Left, false);
				ItemTypeInfoView.Text = Lang.type;
				ItemTypeInfoView.Tip.BackColor = ItemsColor;
				ItemTypeInfoView.Bounds = new Rectangle(0, 20, 170, 20);

				ItemRareInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.Dock.Left, false);
				ItemRareInfoView.Text = Lang.rare;
				ItemRareInfoView.Tip.BackColor = ItemsColor;
				ItemRareInfoView.Bounds = new Rectangle(0, 40, 170, 20);

				InfoView ItemDetailInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.Dock.Top);
				Panel ItemDetailInfoViewContent = (ItemDetailInfoView.View as Panel);
				ItemDetailInfoViewContent.Controls.Add(ItemNameInfoView);
				ItemDetailInfoViewContent.Controls.Add(ItemTypeInfoView);
				ItemDetailInfoViewContent.Controls.Add(ItemRareInfoView);
				ItemDetailInfoView.Text = Lang.details;
				ItemDetailInfoView.Tip.BackColor = ItemsColor;
				ItemDetailInfoView.Bounds = new Rectangle(90, 5, 170, 80);

				ItemDescriptionInfoView = new InfoView(new TextBox() { Multiline = true }, InfoView.Dock.Left);
				ItemDescriptionInfoView.Text = Lang.description;
				ItemDescriptionInfoView.Tip.BackColor = ItemsColor;
				ItemDescriptionInfoView.Bounds = new Rectangle(5, 90, 255, 80);

				ListBox requireItems = new ListBox()
				{
					BorderStyle = BorderStyle.None
				};
				requireItems.MouseDoubleClick += RequireItems_MouseDoubleClick;
				ItemRecipeFromInfoView = new InfoView(requireItems, InfoView.Dock.Top);
				ItemRecipeFromInfoView.Text = Lang.recipeFrom;
				ItemRecipeFromInfoView.Tip.BackColor = ItemsColor;
				ItemRecipeFromInfoView.Bounds = new Rectangle(5, 175, 255, 100);

				ListBox recipeToItems = new ListBox()
				{
					BorderStyle = BorderStyle.None
				};
				recipeToItems.MouseDoubleClick += RecipeToItems_MouseDoubleClick;
				ItemRecipeToInfoView = new InfoView(recipeToItems, InfoView.Dock.Top);
				ItemRecipeToInfoView.Text = Lang.recipeTo;
				ItemRecipeToInfoView.Tip.BackColor = ItemsColor;
				ItemRecipeToInfoView.Bounds = new Rectangle(5, 280, 255, 100);

				ItemValueInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left);
				ItemValueInfoView.Text = Lang.value;
				ItemValueInfoView.Tip.BackColor = ItemsColor;
				ItemValueInfoView.Bounds = new Rectangle(5, 385, 255, 20);



				ItemInfoPage.Controls.Add(ItemIconInfoView);
				ItemInfoPage.Controls.Add(ItemDetailInfoView);
				ItemInfoPage.Controls.Add(ItemDescriptionInfoView);
				ItemInfoPage.Controls.Add(ItemRecipeFromInfoView);
				ItemInfoPage.Controls.Add(ItemRecipeToInfoView);
				ItemInfoPage.Controls.Add(ItemValueInfoView);
			}

			AccInfoPage = new TabPage(Lang.equipInfo);

			{
				ItemIcon2InfoView = new InfoView(new PictureBox() { SizeMode = PictureBoxSizeMode.CenterImage }, InfoView.Dock.Top);
				ItemIcon2InfoView.Text = Lang.icon;
				ItemIcon2InfoView.Bounds = new Rectangle(5, 5, 80, 80);
				ItemIcon2InfoView.Tip.BackColor = ItemsColor;


				ItemPickaxeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false);
				ItemPickaxeInfoView.Text = Lang.pick1;
				ItemPickaxeInfoView.Tip.BackColor = ItemsColor;
				ItemPickaxeInfoView.Bounds = new Rectangle(0, 0, 170, 20);

				ItemAxeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false);
				ItemAxeInfoView.Text = Lang.axe1;
				ItemAxeInfoView.Tip.BackColor = ItemsColor;
				ItemAxeInfoView.Bounds = new Rectangle(0, 20, 170, 20);

				ItemHammerInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false);
				ItemHammerInfoView.Text = Lang.hammer1;
				ItemHammerInfoView.Tip.BackColor = ItemsColor;
				ItemHammerInfoView.Bounds = new Rectangle(0, 40, 170, 20);

				InfoView ItemDetailInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.Dock.Top);
				Panel ItemDetailInfoViewContent = (ItemDetailInfoView.View as Panel);

				ItemDetailInfoViewContent.Controls.Add(ItemPickaxeInfoView);
				ItemDetailInfoViewContent.Controls.Add(ItemAxeInfoView);
				ItemDetailInfoViewContent.Controls.Add(ItemHammerInfoView);
				ItemDetailInfoView.Text = Lang.details;
				ItemDetailInfoView.Tip.BackColor = ItemsColor;
				ItemDetailInfoView.Bounds = new Rectangle(90, 5, 170, 80);

				ItemDamageInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemDamageInfoView.Text = Lang.damage;
				ItemDamageInfoView.Tip.BackColor = ItemsColor;
				ItemDamageInfoView.Bounds = new Rectangle(0, 0, 127, 20);

				ItemDefenseInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemDefenseInfoView.Text = Lang.defense;
				ItemDefenseInfoView.Tip.BackColor = ItemsColor;
				ItemDefenseInfoView.Bounds = new Rectangle(128, 0, 127, 20);


				ItemCritInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemCritInfoView.Text = Lang.crit;
				ItemCritInfoView.Tip.BackColor = ItemsColor;
				ItemCritInfoView.Bounds = new Rectangle(0, 20, 127, 20);

				ItemKnockbackInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemKnockbackInfoView.Text = Lang.knockBack;
				ItemKnockbackInfoView.Tip.BackColor = ItemsColor;
				ItemKnockbackInfoView.Bounds = new Rectangle(128, 20, 127, 20);


				ItemShootInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemShootInfoView.Text = Lang.projID;
				ItemShootInfoView.Tip.BackColor = ItemsColor;
				ItemShootInfoView.Bounds = new Rectangle(0, 40, 127, 20);

				ItemShootSpeedInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemShootSpeedInfoView.Text = Lang.projSpeed;
				ItemShootSpeedInfoView.Tip.BackColor = ItemsColor;
				ItemShootSpeedInfoView.Bounds = new Rectangle(128, 40, 127, 20);


				ItemUseTimeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemUseTimeInfoView.Text = Lang.useCD;
				ItemUseTimeInfoView.Tip.BackColor = ItemsColor;
				ItemUseTimeInfoView.Bounds = new Rectangle(0, 60, 127, 20);

				ItemUseAnimationInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemUseAnimationInfoView.Text = Lang.waveCD;
				ItemUseAnimationInfoView.Tip.BackColor = ItemsColor;
				ItemUseAnimationInfoView.Bounds = new Rectangle(128, 60, 127, 20);


				ItemHealLifeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemHealLifeInfoView.Text = Lang.healLife;
				ItemHealLifeInfoView.Tip.BackColor = ItemsColor;
				ItemHealLifeInfoView.Bounds = new Rectangle(0, 80, 127, 20);

				ItemHealManaInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemHealManaInfoView.Text = Lang.healMana;
				ItemHealManaInfoView.Tip.BackColor = ItemsColor;
				ItemHealManaInfoView.Bounds = new Rectangle(128, 80, 127, 20);


				ItemCreateTileInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemCreateTileInfoView.Text = Lang.tileID;
				ItemCreateTileInfoView.Tip.BackColor = ItemsColor;
				ItemCreateTileInfoView.Bounds = new Rectangle(0, 100, 127, 20);

				ItemPlaceStyleInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemPlaceStyleInfoView.Text = Lang.placeStyle;
				ItemPlaceStyleInfoView.Tip.BackColor = ItemsColor;
				ItemPlaceStyleInfoView.Bounds = new Rectangle(128, 100, 127, 20);


				ItemCreateWallInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemCreateWallInfoView.Text = Lang.wall;
				ItemCreateWallInfoView.Tip.BackColor = ItemsColor;
				ItemCreateWallInfoView.Bounds = new Rectangle(0, 120, 127, 20);

				ItemTileBoostInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemTileBoostInfoView.Text = Lang.tileBoost;
				ItemTileBoostInfoView.Tip.BackColor = ItemsColor;
				ItemTileBoostInfoView.Bounds = new Rectangle(128, 120, 127, 20);


				ItemBuffTypeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemBuffTypeInfoView.Text = Lang.buff;
				ItemBuffTypeInfoView.Tip.BackColor = ItemsColor;
				ItemBuffTypeInfoView.Bounds = new Rectangle(0, 140, 127, 20);

				ItemBuffTimeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemBuffTimeInfoView.Text = Lang.buffTime;
				ItemBuffTimeInfoView.Tip.BackColor = ItemsColor;
				ItemBuffTimeInfoView.Bounds = new Rectangle(128, 140, 127, 20);


				ItemManaConsumeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemManaConsumeInfoView.Text = Lang.mana;
				ItemManaConsumeInfoView.Tip.BackColor = ItemsColor;
				ItemManaConsumeInfoView.Bounds = new Rectangle(0, 160, 127, 20);

				ItemBaitInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.Dock.Left, false, 60);
				ItemBaitInfoView.Text = Lang.bait;
				ItemBaitInfoView.Tip.BackColor = ItemsColor;
				ItemBaitInfoView.Bounds = new Rectangle(128, 160, 127, 20);


				InfoView ItemPropertiesInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.Dock.Top);
				Panel ItemPropertiesInfoViewContent = (ItemPropertiesInfoView.View as Panel);
				ItemPropertiesInfoViewContent.Controls.Add(ItemDamageInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemDefenseInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemCritInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemKnockbackInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemShootInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemShootSpeedInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemUseTimeInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemUseAnimationInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemHealLifeInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemHealManaInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemCreateTileInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemPlaceStyleInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemCreateWallInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemTileBoostInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemBuffTypeInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemBuffTimeInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemManaConsumeInfoView);
				ItemPropertiesInfoViewContent.Controls.Add(ItemBaitInfoView);
				ItemPropertiesInfoView.Text = Lang.properties;
				ItemPropertiesInfoView.Tip.BackColor = ItemsColor;
				ItemPropertiesInfoView.Bounds = new Rectangle(5, 105, 255, 10 * 20);



				ItemDescription2InfoView = new InfoView(new TextBox() { Multiline = true }, InfoView.Dock.Left);
				ItemDescription2InfoView.Text = Lang.description;
				ItemDescription2InfoView.Tip.BackColor = ItemsColor;
				ItemDescription2InfoView.Bounds = new Rectangle(5, 320, 255, 80);

				AccInfoPage.Controls.Add(ItemIcon2InfoView);
				AccInfoPage.Controls.Add(ItemDetailInfoView);
				AccInfoPage.Controls.Add(ItemPropertiesInfoView);
				AccInfoPage.Controls.Add(ItemDescription2InfoView);
			}

			SearcherPage = new TabPage(Lang.search);

			{
				GroupBox filterGroupBox = new GroupBox();
				filterGroupBox.Text = Lang.filter;
				filterGroupBox.Bounds = new Rectangle(5, 10, 255, 105);

				BlockCheckBox = new CheckBox();
				BlockCheckBox.Text = Lang.block;
				BlockCheckBox.Checked = true;
				BlockCheckBox.Bounds = new Rectangle(5, 20, 50, 20);
				BlockCheckBox.CheckedChanged += Filter_CheckedChanged;

				WallCheckBox = new CheckBox();
				WallCheckBox.Text = Lang.wallw;
				WallCheckBox.Checked = true;
				WallCheckBox.Bounds = new Rectangle(70, 20, 50, 20);
				WallCheckBox.CheckedChanged += Filter_CheckedChanged;

				HeadCheckBox = new CheckBox();
				HeadCheckBox.Text = Lang.head;
				HeadCheckBox.Checked = true;
				HeadCheckBox.Bounds = new Rectangle(5, 40, 50, 20);
				HeadCheckBox.CheckedChanged += Filter_CheckedChanged;

				BodyCheckBox = new CheckBox();
				BodyCheckBox.Text = Lang.body;
				BodyCheckBox.Checked = true;
				BodyCheckBox.Bounds = new Rectangle(70, 40, 50, 20);
				BodyCheckBox.CheckedChanged += Filter_CheckedChanged;

				LegCheckBox = new CheckBox();
				LegCheckBox.Text = Lang.leg;
				LegCheckBox.Checked = true;
				LegCheckBox.Bounds = new Rectangle(135, 40, 50, 20);
				LegCheckBox.CheckedChanged += Filter_CheckedChanged;

				AccessoryCheckBox = new CheckBox();
				AccessoryCheckBox.Text = Lang.accessory;
				AccessoryCheckBox.Checked = true;
				AccessoryCheckBox.Bounds = new Rectangle(200, 40, 50, 20);
				AccessoryCheckBox.CheckedChanged += Filter_CheckedChanged;

				MeleeCheckBox = new CheckBox();
				MeleeCheckBox.Text = Lang.melee;
				MeleeCheckBox.Checked = true;
				MeleeCheckBox.Bounds = new Rectangle(5, 60, 50, 20);
				MeleeCheckBox.CheckedChanged += Filter_CheckedChanged;

				RangedCheckBox = new CheckBox();
				RangedCheckBox.Text = Lang.ranged;
				RangedCheckBox.Checked = true;
				RangedCheckBox.Bounds = new Rectangle(70, 60, 50, 20);
				RangedCheckBox.CheckedChanged += Filter_CheckedChanged;

				MagicCheckBox = new CheckBox();
				MagicCheckBox.Text = Lang.magic;
				MagicCheckBox.Checked = true;
				MagicCheckBox.Bounds = new Rectangle(135, 60, 50, 20);
				MagicCheckBox.CheckedChanged += Filter_CheckedChanged;

				SummonCheckBox = new CheckBox();
				SummonCheckBox.Text = Lang.summon;
				SummonCheckBox.Checked = true;
				SummonCheckBox.Bounds = new Rectangle(200, 60, 50, 20);
				SummonCheckBox.CheckedChanged += Filter_CheckedChanged;

				BuffCheckBox = new CheckBox();
				BuffCheckBox.Text = Lang.buff;
				BuffCheckBox.Checked = true;
				BuffCheckBox.Bounds = new Rectangle(5, 80, 50, 20);
				BuffCheckBox.CheckedChanged += Filter_CheckedChanged;

				ConsumableCheckBox = new CheckBox();
				ConsumableCheckBox.Text = Lang.consumable;
				ConsumableCheckBox.Checked = true;
				ConsumableCheckBox.Bounds = new Rectangle(70, 80, 50, 20);
				ConsumableCheckBox.CheckedChanged += Filter_CheckedChanged;

				OthersCheckBox = new CheckBox();
				OthersCheckBox.Text = Lang.others;
				OthersCheckBox.Checked = true;
				OthersCheckBox.Bounds = new Rectangle(135, 80, 50, 20);
				OthersCheckBox.CheckedChanged += Filter_CheckedChanged;

				filterGroupBox.Controls.Add(BlockCheckBox);
				filterGroupBox.Controls.Add(WallCheckBox);
				filterGroupBox.Controls.Add(HeadCheckBox);
				filterGroupBox.Controls.Add(BodyCheckBox);
				filterGroupBox.Controls.Add(LegCheckBox);
				filterGroupBox.Controls.Add(AccessoryCheckBox);
				filterGroupBox.Controls.Add(MeleeCheckBox);
				filterGroupBox.Controls.Add(RangedCheckBox);
				filterGroupBox.Controls.Add(MagicCheckBox);
				filterGroupBox.Controls.Add(SummonCheckBox);
				filterGroupBox.Controls.Add(BuffCheckBox);
				filterGroupBox.Controls.Add(ConsumableCheckBox);
				filterGroupBox.Controls.Add(OthersCheckBox);

				Label tipSearch = new Label();
				tipSearch.Text = Lang.keyWord + ":";
				tipSearch.Bounds = new Rectangle(15, 133, 50, 20);

				KeyWordTextBox = new TextBox();
				KeyWordTextBox.Bounds = new Rectangle(65, 130, 175, 20);
				KeyWordTextBox.KeyDown += (s, e) =>
				{
					if (e.KeyCode == Keys.Enter)
					{
						e.Handled = true;
						KeyWord = KeyWordTextBox.Text;
						RefreshItems();
					}
				};

				Button searchButton = new Button();
				searchButton.Text = Lang.search;
				searchButton.Bounds = new Rectangle(70, 160, 60, 20);
				searchButton.Click += (s, e) =>
				{
					KeyWord = KeyWordTextBox.Text;
					RefreshItems();
				};

				Button resetButton = new Button();
				resetButton.Text = Lang.reset;
				resetButton.Bounds = new Rectangle(130, 160, 60, 20);
				resetButton.Click += (s, e) =>
				{
					KeyWord = "";
					KeyWordTextBox.Text = "";
					RefreshItems();
				};

				SearcherPage.Controls.Add(filterGroupBox);
				SearcherPage.Controls.Add(tipSearch);
				SearcherPage.Controls.Add(KeyWordTextBox);
				SearcherPage.Controls.Add(searchButton);
				SearcherPage.Controls.Add(resetButton);
			}

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
			var a = (ItemRecipeToInfoView.View as ListBox);
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
			var a = (ItemRecipeFromInfoView.View as ListBox);
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

		private static string GetValueString(int value)
		{
			int p = value / VALUE_P;
			int a = value % VALUE_P;
			int g = a / VALUE_G;
			a = a % VALUE_G;
			int s = a / VALUE_S;
			a = a % VALUE_S;
			int c = a / VALUE_C;
			a = a % VALUE_C;
#if ENG
			return p + "Platinum " + g + "Gold " + s + "Silver " + c + "Copper";
#else
			return p + "铂 " + g + "金 " + s + "银 " + c + "铜";
#endif
		}

		private void ItemListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ItemListView.SelectedIndices.Count > 0)
			{
				int i = Convert.ToInt32(ItemListView.SelectedItems[0].Text.ToString());
				Image img = MainForm.item_images.Images["Item_" + i];
				(ItemIconInfoView.View as PictureBox).Image = img;
				(ItemIcon2InfoView.View as PictureBox).Image = img;
				(ItemNameInfoView.View as TextBox).Text = Items[i]["Name"].ToString();
				(ItemTypeInfoView.View as TextBox).Text = Items[i]["type"].ToString();
				(ItemRareInfoView.View as TextBox).Text = Items[i]["rare"].ToString();
				string desc = string.Concat(Items[i]["ToolTip"]["_tooltipLines"].ToList().Select(t => t.ToString() + "\n"));
				(ItemDescriptionInfoView.View as TextBox).Text = desc;
				(ItemDescription2InfoView.View as TextBox).Text = desc;
				(ItemRecipeFromInfoView.View as ListBox).Items.Clear();
				var pRe = Recipes.Where(t => t["item"]["type"].ToObject<int>() == Items[i]["type"].ToObject<int>());
				if (pRe.Count() > 0)
				{
					var ritems = (pRe.ElementAt(0)["rItems"] as JArray);
					foreach (var itm in ritems)
					{
						var itemType = itm["type"].ToObject<int>();
						if (itemType != 0)
							(ItemRecipeFromInfoView.View as ListBox).Items.Add("[" + itemType + "] " + Items[itemType]["Name"].ToString() + " [" + itm["stack"].ToObject<int>() + "]");
					}
				}
				(ItemRecipeToInfoView.View as ListBox).Items.Clear();
				pRe = Recipes.Where(
					t => (t["rItems"] as JArray).Where(
						y => y["type"].ToObject<int>() == i && i != 0).Count() > 0);
				foreach (var p in pRe)
				{
					var itm = p["item"];
					(ItemRecipeToInfoView.View as ListBox).Items.Add("[" + itm["type"] + "] " + Items[Convert.ToInt32(itm["type"])]["Name"].ToString() + " [" + itm["stack"] + "]");
				}

				(ItemValueInfoView.View as TextBox).Text = GetValueString(Convert.ToInt32(Items[i]["value"].ToString()));


				(ItemPickaxeInfoView.View as TextBox).Text = Items[i]["pick"].ToString();
				(ItemAxeInfoView.View as TextBox).Text = Items[i]["axe"].ToString();
				(ItemHammerInfoView.View as TextBox).Text = Items[i]["hammer"].ToString();
				(ItemDamageInfoView.View as TextBox).Text = Items[i]["damage"].ToString();
				(ItemDefenseInfoView.View as TextBox).Text = Items[i]["defense"].ToString();
				(ItemCritInfoView.View as TextBox).Text = Items[i]["crit"].ToString();
				(ItemKnockbackInfoView.View as TextBox).Text = Items[i]["knockBack"].ToString();
				(ItemShootInfoView.View as TextBox).Text = Items[i]["shoot"].ToString();
				(ItemShootSpeedInfoView.View as TextBox).Text = Items[i]["shootSpeed"].ToString();
				(ItemUseTimeInfoView.View as TextBox).Text = Items[i]["useTime"].ToString();
				(ItemUseAnimationInfoView.View as TextBox).Text = Items[i]["useAnimation"].ToString();
				(ItemHealLifeInfoView.View as TextBox).Text = Items[i]["healLife"].ToString();
				(ItemHealManaInfoView.View as TextBox).Text = Items[i]["healMana"].ToString();
				(ItemCreateTileInfoView.View as TextBox).Text = Items[i]["createTile"].ToString();
				(ItemPlaceStyleInfoView.View as TextBox).Text = Items[i]["placeStyle"].ToString();
				(ItemCreateWallInfoView.View as TextBox).Text = Items[i]["createWall"].ToString();
				(ItemTileBoostInfoView.View as TextBox).Text = Items[i]["tileBoost"].ToString();
				(ItemBuffTypeInfoView.View as TextBox).Text = Items[i]["buffType"].ToString();
				(ItemBuffTimeInfoView.View as TextBox).Text = Items[i]["buffTime"].ToString();
				(ItemManaConsumeInfoView.View as TextBox).Text = Items[i]["mana"].ToString();
				(ItemBaitInfoView.View as TextBox).Text = Items[i]["bait"].ToString();
			}
			else
			{
				(ItemIconInfoView.View as PictureBox).Image = null;
				(ItemIcon2InfoView.View as PictureBox).Image = null;
				(ItemNameInfoView.View as TextBox).Text = "";
				(ItemTypeInfoView.View as TextBox).Text = "";
				(ItemRareInfoView.View as TextBox).Text = "";
				string desc = "";
				(ItemDescriptionInfoView.View as TextBox).Text = desc;
				(ItemDescription2InfoView.View as TextBox).Text = desc;
				(ItemRecipeFromInfoView.View as ListBox).Items.Clear();
				(ItemRecipeToInfoView.View as ListBox).Items.Clear();

				(ItemValueInfoView.View as TextBox).Text = "";


				(ItemPickaxeInfoView.View as TextBox).Text = "";
				(ItemAxeInfoView.View as TextBox).Text = "";
				(ItemHammerInfoView.View as TextBox).Text = "";
				(ItemDamageInfoView.View as TextBox).Text = "";
				(ItemDefenseInfoView.View as TextBox).Text = "";
				(ItemCritInfoView.View as TextBox).Text = "";
				(ItemKnockbackInfoView.View as TextBox).Text = "";
				(ItemShootInfoView.View as TextBox).Text = "";
				(ItemShootSpeedInfoView.View as TextBox).Text = "";
				(ItemUseTimeInfoView.View as TextBox).Text = "";
				(ItemUseAnimationInfoView.View as TextBox).Text = "";
				(ItemHealLifeInfoView.View as TextBox).Text = "";
				(ItemHealManaInfoView.View as TextBox).Text = "";
				(ItemCreateTileInfoView.View as TextBox).Text = "";
				(ItemPlaceStyleInfoView.View as TextBox).Text = "";
				(ItemCreateWallInfoView.View as TextBox).Text = "";
				(ItemTileBoostInfoView.View as TextBox).Text = "";
				(ItemBuffTypeInfoView.View as TextBox).Text = "";
				(ItemBuffTimeInfoView.View as TextBox).Text = "";
				(ItemManaConsumeInfoView.View as TextBox).Text = "";
				(ItemBaitInfoView.View as TextBox).Text = "";
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
			b.Add((j["ranged"].ToObject<bool>() || j["thrown"].ToObject<bool>()));
			b.Add(j["magic"].ToObject<bool>());
			b.Add((j["summon"].ToObject<bool>() || j["sentry"].ToObject<bool>()));
			b.Add(j["buffType"].ToObject<int>() != 0);
			b.Add(j["consumable"].ToObject<bool>());
			bool r = false;
			r |= (BlockCheckBox.Checked && b[0]);
			r |= (WallCheckBox.Checked && b[1]);
			r |= (HeadCheckBox.Checked && b[2]);
			r |= (BodyCheckBox.Checked && b[3]);
			r |= (LegCheckBox.Checked && b[4]);
			r |= (AccessoryCheckBox.Checked && b[5]);
			r |= (MeleeCheckBox.Checked && b[6]);
			r |= (RangedCheckBox.Checked && b[7]);
			r |= (MagicCheckBox.Checked && b[8]);
			r |= (SummonCheckBox.Checked && b[9]);
			r |= (BuffCheckBox.Checked && b[10]);
			r |= (ConsumableCheckBox.Checked && b[11]);
			if (b.TrueForAll(t => !t) && OthersCheckBox.Checked)
				return true;
			return r;
		}

		private string GetItemType(JToken j)
		{
			List<bool> b = new List<bool>();
			if (j["createTile"].ToObject<int>() != -1) return Lang.block;
			if (j["createWall"].ToObject<int>() != -1) return Lang.wall;
			if (j["headSlot"].ToObject<int>() != -1) return Lang.head;
			if (j["bodySlot"].ToObject<int>() != -1) return Lang.body;
			if (j["legSlot"].ToObject<int>() != -1) return Lang.leg;
			if (j["accessory"].ToObject<bool>()) return Lang.accessory;
			if (j["melee"].ToObject<bool>()) return Lang.melee;
			if ((j["ranged"].ToObject<bool>() || j["thrown"].ToObject<bool>())) return Lang.ranged;
			if (j["magic"].ToObject<bool>()) return Lang.magic;
			if ((j["summon"].ToObject<bool>() || j["sentry"].ToObject<bool>())) return Lang.summon;
			if (j["buffType"].ToObject<int>() != 0) return Lang.buff;
			if (j["consumable"].ToObject<bool>()) return Lang.consumable;
#if ENG
			return "None";
#else
			return "无";
#endif
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
