using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Wiki.NPC
{
	public class NPCSearcherSubPage : TabPage
	{
		public CheckBox TownNPCCheckBox, BossCheckBox, FriendlyCheckBox, OthersCheckBox;
		public TextBox KeyWordTextBox;
		public Button SearchButton, ResetButton;

		public NPCSearcherSubPage() : base(MainForm.CurrentLanguage["Search"])
		{
			GroupBox filterGroupBox = new GroupBox();
			filterGroupBox.Text = MainForm.CurrentLanguage["Filter"];
			filterGroupBox.Bounds = new Rectangle(5, 10, 255, 55);

			TownNPCCheckBox = new CheckBox();
			TownNPCCheckBox.Text = MainForm.CurrentLanguage["TownNPC"];
			TownNPCCheckBox.Checked = true;
			TownNPCCheckBox.Bounds = new Rectangle(5, 20, 50, 20);

			BossCheckBox = new CheckBox();
			BossCheckBox.Text = "Boss";
			BossCheckBox.Checked = true;
			BossCheckBox.Bounds = new Rectangle(70, 20, 50, 20);

			FriendlyCheckBox = new CheckBox();
			FriendlyCheckBox.Text = MainForm.CurrentLanguage["Friendly"];
			FriendlyCheckBox.Checked = true;
			FriendlyCheckBox.Bounds = new Rectangle(135, 20, 50, 20);

			OthersCheckBox = new CheckBox();
			OthersCheckBox.Text = MainForm.CurrentLanguage["Others"];
			OthersCheckBox.Checked = true;
			OthersCheckBox.Bounds = new Rectangle(200, 20, 50, 20);



			filterGroupBox.Controls.Add(TownNPCCheckBox);
			filterGroupBox.Controls.Add(BossCheckBox);
			filterGroupBox.Controls.Add(FriendlyCheckBox);
			filterGroupBox.Controls.Add(OthersCheckBox);

			Label tipSearch = new Label();
			tipSearch.Text = MainForm.CurrentLanguage["KeyWord"] + ":";
			tipSearch.Bounds = new Rectangle(15, 83, 50, 20);

			KeyWordTextBox = new TextBox();
			KeyWordTextBox.ImeMode = ImeMode.OnHalf;
			KeyWordTextBox.Bounds = new Rectangle(65, 80, 175, 20);

			SearchButton = new Button();
			SearchButton.Text = MainForm.CurrentLanguage["Search"];
			SearchButton.Bounds = new Rectangle(70, 110, 60, 20);

			ResetButton = new Button();
			ResetButton.Text = MainForm.CurrentLanguage["Reset"];
			ResetButton.Bounds = new Rectangle(130, 110, 60, 20);

			Controls.Add(filterGroupBox);
			Controls.Add(tipSearch);
			Controls.Add(KeyWordTextBox);
			Controls.Add(SearchButton);
			Controls.Add(ResetButton);
		}
	}
}
