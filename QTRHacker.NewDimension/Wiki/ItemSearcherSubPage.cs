using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Wiki
{
	public class ItemSearcherSubPage : TabPage
	{
		public CheckBox BlockCheckBox, WallCheckBox, HeadCheckBox, BodyCheckBox, LegCheckBox, AccessoryCheckBox, MeleeCheckBox, RangedCheckBox, MagicCheckBox, SummonCheckBox, BuffCheckBox, ConsumableCheckBox, OthersCheckBox;

		public TextBox KeyWordTextBox;
		public Button SearchButton,ResetButton;

		public ItemSearcherSubPage() : base(MainForm.CurrentLanguage["Search"])
		{
			GroupBox filterGroupBox = new GroupBox();
			filterGroupBox.Text = MainForm.CurrentLanguage["Filter"];
			filterGroupBox.Bounds = new Rectangle(5, 10, 255, 105);

			BlockCheckBox = new CheckBox();
			BlockCheckBox.Text = MainForm.CurrentLanguage["Blocks"];
			BlockCheckBox.Checked = true;
			BlockCheckBox.Bounds = new Rectangle(5, 20, 50, 20);

			WallCheckBox = new CheckBox();
			WallCheckBox.Text = MainForm.CurrentLanguage["Walls"];
			WallCheckBox.Checked = true;
			WallCheckBox.Bounds = new Rectangle(70, 20, 50, 20);

			HeadCheckBox = new CheckBox();
			HeadCheckBox.Text = MainForm.CurrentLanguage["Head"];
			HeadCheckBox.Checked = true;
			HeadCheckBox.Bounds = new Rectangle(5, 40, 50, 20);

			BodyCheckBox = new CheckBox();
			BodyCheckBox.Text = MainForm.CurrentLanguage["Body"];
			BodyCheckBox.Checked = true;
			BodyCheckBox.Bounds = new Rectangle(70, 40, 50, 20);

			LegCheckBox = new CheckBox();
			LegCheckBox.Text = MainForm.CurrentLanguage["Leg"];
			LegCheckBox.Checked = true;
			LegCheckBox.Bounds = new Rectangle(135, 40, 50, 20);

			AccessoryCheckBox = new CheckBox();
			AccessoryCheckBox.Text = MainForm.CurrentLanguage["Accessory"];
			AccessoryCheckBox.Checked = true;
			AccessoryCheckBox.Bounds = new Rectangle(200, 40, 50, 20);

			MeleeCheckBox = new CheckBox();
			MeleeCheckBox.Text = MainForm.CurrentLanguage["Melee"];
			MeleeCheckBox.Checked = true;
			MeleeCheckBox.Bounds = new Rectangle(5, 60, 50, 20);

			RangedCheckBox = new CheckBox();
			RangedCheckBox.Text = MainForm.CurrentLanguage["Ranged"];
			RangedCheckBox.Checked = true;
			RangedCheckBox.Bounds = new Rectangle(70, 60, 50, 20);

			MagicCheckBox = new CheckBox();
			MagicCheckBox.Text = MainForm.CurrentLanguage["Magic"];
			MagicCheckBox.Checked = true;
			MagicCheckBox.Bounds = new Rectangle(135, 60, 50, 20);

			SummonCheckBox = new CheckBox();
			SummonCheckBox.Text = MainForm.CurrentLanguage["Summon"];
			SummonCheckBox.Checked = true;
			SummonCheckBox.Bounds = new Rectangle(200, 60, 50, 20);

			BuffCheckBox = new CheckBox();
			BuffCheckBox.Text = MainForm.CurrentLanguage["Buff"];
			BuffCheckBox.Checked = true;
			BuffCheckBox.Bounds = new Rectangle(5, 80, 50, 20);

			ConsumableCheckBox = new CheckBox();
			ConsumableCheckBox.Text = MainForm.CurrentLanguage["Consumable"];
			ConsumableCheckBox.Checked = true;
			ConsumableCheckBox.Bounds = new Rectangle(70, 80, 50, 20);

			OthersCheckBox = new CheckBox();
			OthersCheckBox.Text = MainForm.CurrentLanguage["Others"];
			OthersCheckBox.Checked = true;
			OthersCheckBox.Bounds = new Rectangle(135, 80, 50, 20);

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
			tipSearch.Text = MainForm.CurrentLanguage["KeyWord"] + ":";
			tipSearch.Bounds = new Rectangle(15, 133, 50, 20);

			KeyWordTextBox = new TextBox();
			KeyWordTextBox.ImeMode = ImeMode.OnHalf;
			KeyWordTextBox.Bounds = new Rectangle(65, 130, 175, 20);

			SearchButton = new Button();
			SearchButton.Text = MainForm.CurrentLanguage["Search"];
			SearchButton.Bounds = new Rectangle(70, 160, 60, 20);

			ResetButton = new Button();
			ResetButton.Text = MainForm.CurrentLanguage["Reset"];
			ResetButton.Bounds = new Rectangle(130, 160, 60, 20);

			Controls.Add(filterGroupBox);
			Controls.Add(tipSearch);
			Controls.Add(KeyWordTextBox);
			Controls.Add(SearchButton);
			Controls.Add(ResetButton);
		}
	}
}
