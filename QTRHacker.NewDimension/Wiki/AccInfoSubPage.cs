using Newtonsoft.Json.Linq;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Wiki
{
	public class AccInfoSubPage : TabPage
	{
		private readonly Color ItemsColor = Color.FromArgb(160, 160, 200);
		public InfoView ItemIcon2InfoView, ItemPickaxeInfoView, ItemAxeInfoView, ItemHammerInfoView, ItemDamageInfoView, ItemDefenseInfoView, ItemCritInfoView, ItemUseTimeInfoView, ItemKnockbackInfoView;
		public InfoView ItemHealLifeInfoView, ItemHealManaInfoView, ItemManaConsumeInfoView, ItemBaitInfoView, ItemShootInfoView, ItemShootSpeedInfoView, ItemCreateTileInfoView, ItemBuffTypeInfoView, ItemBuffTimeInfoView, ItemUseAnimationInfoView, ItemPlaceStyleInfoView, ItemCreateWallInfoView, ItemTileBoostInfoView;
		public InfoView ItemDescription2InfoView;

		public AccInfoSubPage() : base(MainForm.CurrentLanguage["ArmorInfo"])
		{
			ItemIcon2InfoView = new InfoView(new PictureBox() { SizeMode = PictureBoxSizeMode.CenterImage }, InfoView.TipDock.Top);
			ItemIcon2InfoView.Text = MainForm.CurrentLanguage["Icon"];
			ItemIcon2InfoView.Bounds = new Rectangle(5, 5, 80, 80);
			ItemIcon2InfoView.Tip.BackColor = ItemsColor;


			ItemPickaxeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			ItemPickaxeInfoView.Text = MainForm.CurrentLanguage["Pick"];
			ItemPickaxeInfoView.Tip.BackColor = ItemsColor;
			ItemPickaxeInfoView.Bounds = new Rectangle(0, 0, 170, 20);

			ItemAxeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			ItemAxeInfoView.Text = MainForm.CurrentLanguage["Axe"];
			ItemAxeInfoView.Tip.BackColor = ItemsColor;
			ItemAxeInfoView.Bounds = new Rectangle(0, 20, 170, 20);

			ItemHammerInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			ItemHammerInfoView.Text = MainForm.CurrentLanguage["Hammer"];
			ItemHammerInfoView.Tip.BackColor = ItemsColor;
			ItemHammerInfoView.Bounds = new Rectangle(0, 40, 170, 20);

			InfoView ItemDetailInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel ItemDetailInfoViewContent = (ItemDetailInfoView.View as Panel);

			ItemDetailInfoViewContent.Controls.Add(ItemPickaxeInfoView);
			ItemDetailInfoViewContent.Controls.Add(ItemAxeInfoView);
			ItemDetailInfoViewContent.Controls.Add(ItemHammerInfoView);
			ItemDetailInfoView.Text = MainForm.CurrentLanguage["Details"];
			ItemDetailInfoView.Tip.BackColor = ItemsColor;
			ItemDetailInfoView.Bounds = new Rectangle(90, 5, 170, 80);

			ItemDamageInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemDamageInfoView.Text = MainForm.CurrentLanguage["Damage"];
			ItemDamageInfoView.Tip.BackColor = ItemsColor;
			ItemDamageInfoView.Bounds = new Rectangle(0, 0, 127, 20);

			ItemDefenseInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemDefenseInfoView.Text = MainForm.CurrentLanguage["Defense"];
			ItemDefenseInfoView.Tip.BackColor = ItemsColor;
			ItemDefenseInfoView.Bounds = new Rectangle(128, 0, 127, 20);


			ItemCritInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemCritInfoView.Text = MainForm.CurrentLanguage["Crit"];
			ItemCritInfoView.Tip.BackColor = ItemsColor;
			ItemCritInfoView.Bounds = new Rectangle(0, 20, 127, 20);

			ItemKnockbackInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemKnockbackInfoView.Text = MainForm.CurrentLanguage["KnockBack"];
			ItemKnockbackInfoView.Tip.BackColor = ItemsColor;
			ItemKnockbackInfoView.Bounds = new Rectangle(128, 20, 127, 20);


			ItemShootInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemShootInfoView.Text = MainForm.CurrentLanguage["Shoot"];
			ItemShootInfoView.Tip.BackColor = ItemsColor;
			ItemShootInfoView.Bounds = new Rectangle(0, 40, 127, 20);

			ItemShootSpeedInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemShootSpeedInfoView.Text = MainForm.CurrentLanguage["ShootSpeed"];
			ItemShootSpeedInfoView.Tip.BackColor = ItemsColor;
			ItemShootSpeedInfoView.Bounds = new Rectangle(128, 40, 127, 20);


			ItemUseTimeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemUseTimeInfoView.Text = MainForm.CurrentLanguage["UseTime"];
			ItemUseTimeInfoView.Tip.BackColor = ItemsColor;
			ItemUseTimeInfoView.Bounds = new Rectangle(0, 60, 127, 20);

			ItemUseAnimationInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemUseAnimationInfoView.Text = MainForm.CurrentLanguage["UseAnimation"];
			ItemUseAnimationInfoView.Tip.BackColor = ItemsColor;
			ItemUseAnimationInfoView.Bounds = new Rectangle(128, 60, 127, 20);


			ItemHealLifeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemHealLifeInfoView.Text = MainForm.CurrentLanguage["HealLife"];
			ItemHealLifeInfoView.Tip.BackColor = ItemsColor;
			ItemHealLifeInfoView.Bounds = new Rectangle(0, 80, 127, 20);

			ItemHealManaInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemHealManaInfoView.Text = MainForm.CurrentLanguage["HealMana"];
			ItemHealManaInfoView.Tip.BackColor = ItemsColor;
			ItemHealManaInfoView.Bounds = new Rectangle(128, 80, 127, 20);


			ItemCreateTileInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemCreateTileInfoView.Text = MainForm.CurrentLanguage["CreateTile"];
			ItemCreateTileInfoView.Tip.BackColor = ItemsColor;
			ItemCreateTileInfoView.Bounds = new Rectangle(0, 100, 127, 20);

			ItemPlaceStyleInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemPlaceStyleInfoView.Text = MainForm.CurrentLanguage["PlaceStyle"];
			ItemPlaceStyleInfoView.Tip.BackColor = ItemsColor;
			ItemPlaceStyleInfoView.Bounds = new Rectangle(128, 100, 127, 20);


			ItemCreateWallInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemCreateWallInfoView.Text = MainForm.CurrentLanguage["CreateWall"];
			ItemCreateWallInfoView.Tip.BackColor = ItemsColor;
			ItemCreateWallInfoView.Bounds = new Rectangle(0, 120, 127, 20);

			ItemTileBoostInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemTileBoostInfoView.Text = MainForm.CurrentLanguage["TileBoost"];
			ItemTileBoostInfoView.Tip.BackColor = ItemsColor;
			ItemTileBoostInfoView.Bounds = new Rectangle(128, 120, 127, 20);


			ItemBuffTypeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemBuffTypeInfoView.Text = MainForm.CurrentLanguage["Buff"];
			ItemBuffTypeInfoView.Tip.BackColor = ItemsColor;
			ItemBuffTypeInfoView.Bounds = new Rectangle(0, 140, 127, 20);

			ItemBuffTimeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemBuffTimeInfoView.Text = MainForm.CurrentLanguage["BuffTime"];
			ItemBuffTimeInfoView.Tip.BackColor = ItemsColor;
			ItemBuffTimeInfoView.Bounds = new Rectangle(128, 140, 127, 20);


			ItemManaConsumeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemManaConsumeInfoView.Text = MainForm.CurrentLanguage["ManaConsume"];
			ItemManaConsumeInfoView.Tip.BackColor = ItemsColor;
			ItemManaConsumeInfoView.Bounds = new Rectangle(0, 160, 127, 20);

			ItemBaitInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 60);
			ItemBaitInfoView.Text = MainForm.CurrentLanguage["Bait"];
			ItemBaitInfoView.Tip.BackColor = ItemsColor;
			ItemBaitInfoView.Bounds = new Rectangle(128, 160, 127, 20);


			InfoView ItemPropertiesInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
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
			ItemPropertiesInfoView.Text = MainForm.CurrentLanguage["Properties"];
			ItemPropertiesInfoView.Tip.BackColor = ItemsColor;
			ItemPropertiesInfoView.Bounds = new Rectangle(5, 105, 255, 10 * 20);



			ItemDescription2InfoView = new InfoView(new TextBox() { Multiline = true }, InfoView.TipDock.Left);
			ItemDescription2InfoView.Text = MainForm.CurrentLanguage["Description"];
			ItemDescription2InfoView.Tip.BackColor = ItemsColor;
			ItemDescription2InfoView.Bounds = new Rectangle(5, 320, 255, 80);

			Controls.Add(ItemIcon2InfoView);
			Controls.Add(ItemDetailInfoView);
			Controls.Add(ItemPropertiesInfoView);
			Controls.Add(ItemDescription2InfoView);
		}

		public void ResetData()
		{
			(ItemIcon2InfoView.View as PictureBox).Image = null;
			string desc = "";
			(ItemDescription2InfoView.View as TextBox).Text = desc;



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

		public void SetData(int index)
		{
			Image img = GameResLoader.ItemImages.Images[index.ToString()];
			(ItemIcon2InfoView.View as PictureBox).Image = img;
			//string desc = string.Concat(ItemsTabPage.Items[index]["ToolTip"]["_tooltipLines"].ToList().Select(t => t.ToString() + "\n"));
			//(ItemDescription2InfoView.View as TextBox).Text = desc;

			string desc = ItemsTabPage.ItemDescriptions[index].ToString();
			(ItemDescription2InfoView.View as TextBox).Text = desc;


			(ItemPickaxeInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["pick"].ToString();
			(ItemAxeInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["axe"].ToString();
			(ItemHammerInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["hammer"].ToString();
			(ItemDamageInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["damage"].ToString();
			(ItemDefenseInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["defense"].ToString();
			(ItemCritInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["crit"].ToString();
			(ItemKnockbackInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["knockBack"].ToString();
			(ItemShootInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["shoot"].ToString();
			(ItemShootSpeedInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["shootSpeed"].ToString();
			(ItemUseTimeInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["useTime"].ToString();
			(ItemUseAnimationInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["useAnimation"].ToString();
			(ItemHealLifeInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["healLife"].ToString();
			(ItemHealManaInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["healMana"].ToString();
			(ItemCreateTileInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["createTile"].ToString();
			(ItemPlaceStyleInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["placeStyle"].ToString();
			(ItemCreateWallInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["createWall"].ToString();
			(ItemTileBoostInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["tileBoost"].ToString();
			(ItemBuffTypeInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["buffType"].ToString();
			(ItemBuffTimeInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["buffTime"].ToString();
			(ItemManaConsumeInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["mana"].ToString();
			(ItemBaitInfoView.View as TextBox).Text = ItemsTabPage.Items[index]["bait"].ToString();
		}
	}
}
