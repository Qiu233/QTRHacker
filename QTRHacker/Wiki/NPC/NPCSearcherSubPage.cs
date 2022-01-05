using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Wiki.NPC
{
	public class NPCSearcherSubPage : TabPage
	{
		public CheckBox TownNPCCheckBox, BossCheckBox, FriendlyCheckBox, OthersCheckBox;
		public TextBox KeyWordTextBox;
		public Button SearchButton, ResetButton;

		public NPCSearcherSubPage() : base(HackContext.CurrentLanguage["Search"])
		{
			BackColor = NPCTabPage.GlobalBack;
			GroupBox filterGroupBox = new GroupBox();
			filterGroupBox.Text = HackContext.CurrentLanguage["Filter"];
			filterGroupBox.Bounds = new Rectangle(5, 10, 255, 75);

			TownNPCCheckBox = new CheckBox();
			TownNPCCheckBox.ForeColor = Color.Black;
			TownNPCCheckBox.Text = HackContext.CurrentLanguage["TownNPC"];
			TownNPCCheckBox.Checked = true;
			TownNPCCheckBox.Bounds = new Rectangle(45, 20, 100, 20);

			BossCheckBox = new CheckBox();
			BossCheckBox.ForeColor = Color.Black;
			BossCheckBox.Text = "Boss";
			BossCheckBox.Checked = true;
			BossCheckBox.Bounds = new Rectangle(145, 20, 100, 20);

			FriendlyCheckBox = new CheckBox();
			FriendlyCheckBox.ForeColor = Color.Black;
			FriendlyCheckBox.Text = HackContext.CurrentLanguage["Friendly"];
			FriendlyCheckBox.Checked = true;
			FriendlyCheckBox.Bounds = new Rectangle(45, 40, 100, 20);

			OthersCheckBox = new CheckBox();
			OthersCheckBox.ForeColor = Color.Black;
			OthersCheckBox.Text = HackContext.CurrentLanguage["Others"];
			OthersCheckBox.Checked = true;
			OthersCheckBox.Bounds = new Rectangle(145, 40, 100, 20);



			filterGroupBox.Controls.Add(TownNPCCheckBox);
			filterGroupBox.Controls.Add(BossCheckBox);
			filterGroupBox.Controls.Add(FriendlyCheckBox);
			filterGroupBox.Controls.Add(OthersCheckBox);

			Label tipSearch = new Label();
			tipSearch.ForeColor = Color.Black;
			tipSearch.Text = HackContext.CurrentLanguage["KeyWord"] + ":";
			tipSearch.Bounds = new Rectangle(15, 103, 50, 20);

			KeyWordTextBox = new TextBox();
			KeyWordTextBox.ForeColor = Color.Black;
			KeyWordTextBox.ImeMode = ImeMode.OnHalf;
			KeyWordTextBox.Bounds = new Rectangle(65, 100, 175, 30);

			SearchButton = new Button();
			SearchButton.ForeColor = Color.Black;
			SearchButton.Text = HackContext.CurrentLanguage["Search"];
			SearchButton.Bounds = new Rectangle(70, 130, 60, 30);

			ResetButton = new Button();
			ResetButton.ForeColor = Color.Black;
			ResetButton.Text = HackContext.CurrentLanguage["Reset"];
			ResetButton.Bounds = new Rectangle(130, 130, 60, 30);

			Controls.Add(filterGroupBox);
			Controls.Add(tipSearch);
			Controls.Add(KeyWordTextBox);
			Controls.Add(SearchButton);
			Controls.Add(ResetButton);
		}
	}
}
