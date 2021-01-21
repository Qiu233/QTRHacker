using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Wiki
{
	public class NPCInfoSubPage : TabPage
	{
		private NPCView NPCView;
		private InfoView NPCViewInfoView, NPCNameInfoView, NPCTypeInfoView, NPCAIStyleInfoView;
		public NPCInfoSubPage() : base(MainForm.CurrentLanguage["NPCInfo"])
		{
			NPCView = new NPCView();

			NPCViewInfoView = new InfoView(NPCView, InfoView.TipDock.Top);
			NPCViewInfoView.Bounds = new Rectangle(5, 5, 80, 80);
			NPCViewInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCViewInfoView.Text = MainForm.CurrentLanguage["Icon"];
			this.Controls.Add(NPCViewInfoView);

			NPCNameInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.TipDock.Left, false);
			NPCNameInfoView.Text = MainForm.CurrentLanguage["Name"];
			NPCNameInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCNameInfoView.Bounds = new Rectangle(0, 0, 170, 20);

			NPCTypeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.TipDock.Left, false);
			NPCTypeInfoView.Text = MainForm.CurrentLanguage["Type"];
			NPCTypeInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCTypeInfoView.Bounds = new Rectangle(0, 20, 170, 20);

			NPCAIStyleInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.TipDock.Left, false);
			NPCAIStyleInfoView.Text = MainForm.CurrentLanguage["AIStyle"];
			NPCAIStyleInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			NPCAIStyleInfoView.Bounds = new Rectangle(0, 40, 170, 20);

			InfoView ItemDetailInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel ItemDetailInfoViewContent = (ItemDetailInfoView.View as Panel);
			ItemDetailInfoViewContent.Controls.Add(NPCNameInfoView);
			ItemDetailInfoViewContent.Controls.Add(NPCTypeInfoView);
			ItemDetailInfoViewContent.Controls.Add(NPCAIStyleInfoView);
			ItemDetailInfoView.Text = MainForm.CurrentLanguage["Details"];
			ItemDetailInfoView.Tip.BackColor = NPCTabPage.NPCColor;
			ItemDetailInfoView.Bounds = new Rectangle(90, 5, 170, 80);
			this.Controls.Add(ItemDetailInfoView);
		}
		public void SetData(int Type)
		{
			NPCView.NPCType = Type; 
			(NPCNameInfoView.View as TextBox).Text = NPCTabPage.NPCInfo[Type]["Name"].ToString();
			(NPCTypeInfoView.View as TextBox).Text = NPCTabPage.NPCInfo[Type]["Type"].ToString();
			(NPCAIStyleInfoView.View as TextBox).Text = NPCTabPage.NPCInfo[Type]["AiStyle"].ToString();
		}
	}
}
