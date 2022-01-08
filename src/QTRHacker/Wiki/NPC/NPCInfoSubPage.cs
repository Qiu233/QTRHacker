using QTRHacker.Controls;
using QTRHacker.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Wiki.NPC
{
	public class NPCInfoSubPage : TabPage
	{
		private readonly NPCView NPCView;
		private readonly InfoView NPCViewInfoView, NPCNameInfoView, NPCTypeInfoView, NPCAIStyleInfoView;
		private readonly InfoView NPCWidthInfoView, NPCHeightInfoView;
		private readonly InfoView NPCDamageInfoView, NPCDefenseInfoView;
		private readonly InfoView NPCLifeMaxInfoView, NPCKnockbackResistInfoView;
		private readonly InfoView NPCTownNPCInfoView, NPCBossInfoView, NPCFriendlyInfoView;
		public NPCInfoSubPage() : base(HackContext.CurrentLanguage["NPCInfo"])
		{
			BackColor = NPCTabPage.GlobalBack;

			NPCView = new NPCView();
			NPCView.BackColor = NPCTabPage.GlobalBack;
			NPCViewInfoView = new InfoView(NPCView, InfoView.TipDock.Top);
			NPCViewInfoView.Bounds = new Rectangle(5, 5, 80, 80);
			NPCViewInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCViewInfoView.Text = HackContext.CurrentLanguage["Icon"];
			Controls.Add(NPCViewInfoView);

			NPCNameInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			NPCNameInfoView.Text = HackContext.CurrentLanguage["Name"];
			NPCNameInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCNameInfoView.Bounds = new Rectangle(0, 0, 170, 20);

			NPCTypeInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			NPCTypeInfoView.Text = HackContext.CurrentLanguage["Type"];
			NPCTypeInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCTypeInfoView.Bounds = new Rectangle(0, 20, 170, 20);

			NPCAIStyleInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			NPCAIStyleInfoView.Text = HackContext.CurrentLanguage["AIStyle"];
			NPCAIStyleInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCAIStyleInfoView.Bounds = new Rectangle(0, 40, 170, 20);

			InfoView NPCDetailInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel ItemDetailInfoViewContent = (NPCDetailInfoView.View as Panel);
			ItemDetailInfoViewContent.Controls.Add(NPCNameInfoView);
			ItemDetailInfoViewContent.Controls.Add(NPCTypeInfoView);
			ItemDetailInfoViewContent.Controls.Add(NPCAIStyleInfoView);
			NPCDetailInfoView.Text = HackContext.CurrentLanguage["Details"];
			NPCDetailInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCDetailInfoView.Bounds = new Rectangle(90, 5, 170, 80);
			Controls.Add(NPCDetailInfoView);



			NPCWidthInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCWidthInfoView.Text = HackContext.CurrentLanguage["Width"];
			NPCWidthInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCWidthInfoView.Bounds = new Rectangle(0, 0, 255, 20);

			NPCHeightInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCHeightInfoView.Text = HackContext.CurrentLanguage["Height"];
			NPCHeightInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCHeightInfoView.Bounds = new Rectangle(0, 20, 255, 20);

			NPCDamageInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCDamageInfoView.Text = HackContext.CurrentLanguage["Damage"];
			NPCDamageInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCDamageInfoView.Bounds = new Rectangle(0, 40, 255, 20);

			NPCDefenseInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCDefenseInfoView.Text = HackContext.CurrentLanguage["Defense"];
			NPCDefenseInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCDefenseInfoView.Bounds = new Rectangle(0, 60, 255, 20);

			NPCLifeMaxInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCLifeMaxInfoView.Text = HackContext.CurrentLanguage["LifeMax"];
			NPCLifeMaxInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCLifeMaxInfoView.Bounds = new Rectangle(0, 80, 255, 20);

			NPCKnockbackResistInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCKnockbackResistInfoView.Text = HackContext.CurrentLanguage["KnockbackResist"];
			NPCKnockbackResistInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCKnockbackResistInfoView.Bounds = new Rectangle(0, 100, 255, 20);

			InfoView NPCPropInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel NPCDetailtInfoViewContent = (NPCPropInfoView.View as Panel);
			NPCDetailtInfoViewContent.Controls.Add(NPCWidthInfoView);
			NPCDetailtInfoViewContent.Controls.Add(NPCHeightInfoView);
			NPCDetailtInfoViewContent.Controls.Add(NPCDamageInfoView);
			NPCDetailtInfoViewContent.Controls.Add(NPCDefenseInfoView);
			NPCDetailtInfoViewContent.Controls.Add(NPCLifeMaxInfoView);
			NPCDetailtInfoViewContent.Controls.Add(NPCKnockbackResistInfoView);
			NPCPropInfoView.Text = HackContext.CurrentLanguage["Properties"];
			NPCPropInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCPropInfoView.Bounds = new Rectangle(5, 105, 255, 140);
			Controls.Add(NPCPropInfoView);

			NPCTownNPCInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCTownNPCInfoView.Text = HackContext.CurrentLanguage["TownNPC"];
			NPCTownNPCInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCTownNPCInfoView.Bounds = new Rectangle(0, 0, 255, 20);

			NPCBossInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCBossInfoView.Text = "Boss";
			NPCBossInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCBossInfoView.Bounds = new Rectangle(0, 20, 255, 20);

			NPCFriendlyInfoView = new InfoView(new TextBox() { BorderStyle = BorderStyle.FixedSingle, BackColor = NPCTabPage.GlobalBack, ForeColor = Color.Black, TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCFriendlyInfoView.Text = HackContext.CurrentLanguage["Friendly"];
			NPCFriendlyInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCFriendlyInfoView.Bounds = new Rectangle(0, 40, 255, 20);

			InfoView NPCCategoryInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel NPCCategoryInfoViewContent = (NPCCategoryInfoView.View as Panel);
			NPCCategoryInfoViewContent.Controls.Add(NPCTownNPCInfoView);
			NPCCategoryInfoViewContent.Controls.Add(NPCBossInfoView);
			NPCCategoryInfoViewContent.Controls.Add(NPCFriendlyInfoView);
			NPCCategoryInfoView.Text = HackContext.CurrentLanguage["Category"];
			NPCCategoryInfoView.TipLabel.BackColor = NPCTabPage.ThemeColor;
			NPCCategoryInfoView.Bounds = new Rectangle(5, 260, 255, 80);
			Controls.Add(NPCCategoryInfoView);
		}
		public void SetData(int Type)
		{
			NPCView.NPCType = Type;
			(NPCNameInfoView.View as TextBox).Text = HackContext.GameLocLoader_en.GetNPCName(NPCTabPage.NPCIDToS[Type]);
			(NPCTypeInfoView.View as TextBox).Text = Type.ToString();
			(NPCAIStyleInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].AiStyle.ToString();
			(NPCWidthInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].Width.ToString();
			(NPCHeightInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].Height.ToString();
			(NPCDamageInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].DefDamage.ToString();
			(NPCDefenseInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].DefDefense.ToString();
			(NPCLifeMaxInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].LifeMax.ToString();
			(NPCKnockbackResistInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].KnockBackResist.ToString();

			(NPCTownNPCInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].TownNPC ? HackContext.CurrentLanguage["Yes"] : HackContext.CurrentLanguage["No"];
			(NPCBossInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].Boss ? HackContext.CurrentLanguage["Yes"] : HackContext.CurrentLanguage["No"];
			(NPCFriendlyInfoView.View as TextBox).Text = NPCTabPage.NPCDatum[Type].Friendly ? HackContext.CurrentLanguage["Yes"] : HackContext.CurrentLanguage["No"];

		}
	}
}
