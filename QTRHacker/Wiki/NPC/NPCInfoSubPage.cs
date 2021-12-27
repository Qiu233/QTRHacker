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
		private NPCView NPCView;
		private InfoView NPCViewInfoView, NPCNameInfoView, NPCTypeInfoView, NPCAIStyleInfoView;
		private InfoView NPCWidthInfoView, NPCHeightInfoView;
		private InfoView NPCDamageInfoView, NPCDefenseInfoView;
		private InfoView NPCLifeMaxInfoView, NPCKnockbackResistInfoView;
		private InfoView NPCTownNPCInfoView, NPCBossInfoView, NPCFriendlyInfoView;
		public NPCInfoSubPage() : base(HackContext.CurrentLanguage["NPCInfo"])
		{
			NPCView = new NPCView();

			NPCViewInfoView = new InfoView(NPCView, InfoView.TipDock.Top);
			NPCViewInfoView.Bounds = new Rectangle(5, 5, 80, 80);
			NPCViewInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCViewInfoView.Text = HackContext.CurrentLanguage["Icon"];
			this.Controls.Add(NPCViewInfoView);

			NPCNameInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			NPCNameInfoView.Text = HackContext.CurrentLanguage["Name"];
			NPCNameInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCNameInfoView.Bounds = new Rectangle(0, 0, 170, 20);

			NPCTypeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			NPCTypeInfoView.Text = HackContext.CurrentLanguage["Type"];
			NPCTypeInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCTypeInfoView.Bounds = new Rectangle(0, 20, 170, 20);

			NPCAIStyleInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false);
			NPCAIStyleInfoView.Text = HackContext.CurrentLanguage["AIStyle"];
			NPCAIStyleInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCAIStyleInfoView.Bounds = new Rectangle(0, 40, 170, 20);

			InfoView NPCDetailInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel ItemDetailInfoViewContent = (NPCDetailInfoView.View as Panel);
			ItemDetailInfoViewContent.Controls.Add(NPCNameInfoView);
			ItemDetailInfoViewContent.Controls.Add(NPCTypeInfoView);
			ItemDetailInfoViewContent.Controls.Add(NPCAIStyleInfoView);
			NPCDetailInfoView.Text = HackContext.CurrentLanguage["Details"];
			NPCDetailInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCDetailInfoView.Bounds = new Rectangle(90, 5, 170, 80);
			this.Controls.Add(NPCDetailInfoView);



			NPCWidthInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCWidthInfoView.Text = HackContext.CurrentLanguage["Width"];
			NPCWidthInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCWidthInfoView.Bounds = new Rectangle(0, 0, 255, 20);

			NPCHeightInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCHeightInfoView.Text = HackContext.CurrentLanguage["Height"];
			NPCHeightInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCHeightInfoView.Bounds = new Rectangle(0, 20, 255, 20);

			NPCDamageInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCDamageInfoView.Text = HackContext.CurrentLanguage["Damage"];
			NPCDamageInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCDamageInfoView.Bounds = new Rectangle(0, 40, 255, 20);

			NPCDefenseInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCDefenseInfoView.Text = HackContext.CurrentLanguage["Defense"];
			NPCDefenseInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCDefenseInfoView.Bounds = new Rectangle(0, 60, 255, 20);

			NPCLifeMaxInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCLifeMaxInfoView.Text = HackContext.CurrentLanguage["LifeMax"];
			NPCLifeMaxInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCLifeMaxInfoView.Bounds = new Rectangle(0, 80, 255, 20);

			NPCKnockbackResistInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCKnockbackResistInfoView.Text = HackContext.CurrentLanguage["KnockbackResist"];
			NPCKnockbackResistInfoView.Tip.BackColor = NPCTabPage.NPCColor;
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
			NPCPropInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCPropInfoView.Bounds = new Rectangle(5, 105, 255, 140);
			this.Controls.Add(NPCPropInfoView);

			NPCTownNPCInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCTownNPCInfoView.Text = HackContext.CurrentLanguage["TownNPC"];
			NPCTownNPCInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCTownNPCInfoView.Bounds = new Rectangle(0, 0, 255, 20);

			NPCBossInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCBossInfoView.Text = "Boss";
			NPCBossInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCBossInfoView.Bounds = new Rectangle(0, 20, 255, 20);

			NPCFriendlyInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left, false, 100);
			NPCFriendlyInfoView.Text = HackContext.CurrentLanguage["Friendly"];
			NPCFriendlyInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCFriendlyInfoView.Bounds = new Rectangle(0, 40, 255, 20);

			InfoView NPCCategoryInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel NPCCategoryInfoViewContent = (NPCCategoryInfoView.View as Panel);
			NPCCategoryInfoViewContent.Controls.Add(NPCTownNPCInfoView);
			NPCCategoryInfoViewContent.Controls.Add(NPCBossInfoView);
			NPCCategoryInfoViewContent.Controls.Add(NPCFriendlyInfoView);
			NPCCategoryInfoView.Text = HackContext.CurrentLanguage["Category"];
			NPCCategoryInfoView.Tip.BackColor = NPCTabPage.NPCColor;
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
