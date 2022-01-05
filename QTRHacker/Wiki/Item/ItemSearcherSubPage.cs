using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Wiki.Item
{
	public class ItemSearcherSubPage : TabPage
	{
		public CheckBox BlockCheckBox, WallCheckBox, QuestItemCheckBox, HeadCheckBox, BodyCheckBox, LegCheckBox, AccessoryCheckBox, MeleeCheckBox, RangedCheckBox, MagicCheckBox, SummonCheckBox, BuffCheckBox, ConsumableCheckBox, OthersCheckBox;

		public TextBox KeyWordTextBox;
		public Button ReverseButton, SearchButton, ResetButton;

		public ItemSearcherSubPage() : base(HackContext.CurrentLanguage["Search"])
		{
			BackColor = ItemsTabPage.GlobalBack;
			GroupBox filterGroupBox = new GroupBox();
			filterGroupBox.Text = HackContext.CurrentLanguage["Filter"];
			filterGroupBox.Bounds = new Rectangle(5, 10, 255, 125);

			BlockCheckBox = new CheckBox();
			BlockCheckBox.ForeColor = Color.Black;
			BlockCheckBox.Text = HackContext.CurrentLanguage["Blocks"];
			BlockCheckBox.Checked = true;
			BlockCheckBox.Bounds = new Rectangle(25, 20, 80, 20);

			WallCheckBox = new CheckBox();
			WallCheckBox.ForeColor = Color.Black;
			WallCheckBox.Text = HackContext.CurrentLanguage["Walls"];
			WallCheckBox.Checked = true;
			WallCheckBox.Bounds = new Rectangle(105, 20, 80, 20);

			QuestItemCheckBox = new CheckBox();
			QuestItemCheckBox.ForeColor = Color.Black;
			QuestItemCheckBox.Text = HackContext.CurrentLanguage["Quest"];
			QuestItemCheckBox.Checked = true;
			QuestItemCheckBox.Bounds = new Rectangle(185, 20, 80, 20);

			HeadCheckBox = new CheckBox();
			HeadCheckBox.ForeColor = Color.Black;
			HeadCheckBox.Text = HackContext.CurrentLanguage["Head"];
			HeadCheckBox.Checked = true;
			HeadCheckBox.Bounds = new Rectangle(25, 40, 80, 20);

			BodyCheckBox = new CheckBox();
			BodyCheckBox.ForeColor = Color.Black;
			BodyCheckBox.Text = HackContext.CurrentLanguage["Body"];
			BodyCheckBox.Checked = true;
			BodyCheckBox.Bounds = new Rectangle(105, 40, 80, 20);

			LegCheckBox = new CheckBox();
			LegCheckBox.ForeColor = Color.Black;
			LegCheckBox.Text = HackContext.CurrentLanguage["Leg"];
			LegCheckBox.Checked = true;
			LegCheckBox.Bounds = new Rectangle(185, 40, 80, 20);

			MeleeCheckBox = new CheckBox();
			MeleeCheckBox.ForeColor = Color.Black;
			MeleeCheckBox.Text = HackContext.CurrentLanguage["Melee"];
			MeleeCheckBox.Checked = true;
			MeleeCheckBox.Bounds = new Rectangle(25, 60, 80, 20);

			RangedCheckBox = new CheckBox();
			RangedCheckBox.ForeColor = Color.Black;
			RangedCheckBox.Text = HackContext.CurrentLanguage["Ranged"];
			RangedCheckBox.Checked = true;
			RangedCheckBox.Bounds = new Rectangle(105, 60, 80, 20);

			MagicCheckBox = new CheckBox();
			MagicCheckBox.ForeColor = Color.Black;
			MagicCheckBox.Text = HackContext.CurrentLanguage["Magic"];
			MagicCheckBox.Checked = true;
			MagicCheckBox.Bounds = new Rectangle(185, 60, 80, 20);

			SummonCheckBox = new CheckBox();
			SummonCheckBox.ForeColor = Color.Black;
			SummonCheckBox.Text = HackContext.CurrentLanguage["Summon"];
			SummonCheckBox.Checked = true;
			SummonCheckBox.Bounds = new Rectangle(25, 80, 80, 20);

			BuffCheckBox = new CheckBox();
			BuffCheckBox.ForeColor = Color.Black;
			BuffCheckBox.Text = HackContext.CurrentLanguage["Buff"];
			BuffCheckBox.Checked = true;
			BuffCheckBox.Bounds = new Rectangle(105, 80, 80, 20);

			ConsumableCheckBox = new CheckBox();
			ConsumableCheckBox.ForeColor = Color.Black;
			ConsumableCheckBox.Text = HackContext.CurrentLanguage["Consumable"];
			ConsumableCheckBox.Checked = true;
			ConsumableCheckBox.Bounds = new Rectangle(185, 80, 80, 20);

			AccessoryCheckBox = new CheckBox();
			AccessoryCheckBox.ForeColor = Color.Black;
			AccessoryCheckBox.Text = HackContext.CurrentLanguage["Accessory"];
			AccessoryCheckBox.Checked = true;
			AccessoryCheckBox.Bounds = new Rectangle(25, 100, 80, 20);

			OthersCheckBox = new CheckBox();
			OthersCheckBox.ForeColor = Color.Black;
			OthersCheckBox.Text = HackContext.CurrentLanguage["Others"];
			OthersCheckBox.Checked = true;
			OthersCheckBox.Bounds = new Rectangle(105, 100, 80, 20);

			filterGroupBox.Controls.Add(BlockCheckBox);
			filterGroupBox.Controls.Add(WallCheckBox);
			filterGroupBox.Controls.Add(QuestItemCheckBox);
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
			tipSearch.ForeColor = Color.Black;
			tipSearch.Text = HackContext.CurrentLanguage["KeyWord"] + ":";
			tipSearch.Bounds = new Rectangle(15, 153, 50, 20);

			KeyWordTextBox = new TextBox();
			KeyWordTextBox.BackColor = ItemsTabPage.GlobalBack;
			KeyWordTextBox.ForeColor = Color.Black;
			KeyWordTextBox.ImeMode = ImeMode.OnHalf;
			KeyWordTextBox.Bounds = new Rectangle(65, 150, 175, 20);

			ReverseButton = new Button();
			ReverseButton.ForeColor = Color.Black;
			ReverseButton.Text = HackContext.CurrentLanguage["ReverseCheck"];
			ReverseButton.Bounds = new Rectangle(50, 180, 60, 30);

			SearchButton = new Button();
			SearchButton.ForeColor = Color.Black;
			SearchButton.Text = HackContext.CurrentLanguage["Search"];
			SearchButton.Bounds = new Rectangle(110, 180, 60, 30);

			ResetButton = new Button();
			ResetButton.ForeColor = Color.Black;
			ResetButton.Text = HackContext.CurrentLanguage["Reset"];
			ResetButton.Bounds = new Rectangle(170, 180, 60, 30);

			Controls.Add(filterGroupBox);
			Controls.Add(tipSearch);
			Controls.Add(ReverseButton);
			Controls.Add(KeyWordTextBox);
			Controls.Add(SearchButton);
			Controls.Add(ResetButton);
		}
	}
}
