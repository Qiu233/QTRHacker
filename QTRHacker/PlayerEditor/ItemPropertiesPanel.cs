using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor
{
	public class ItemPropertiesPanel : Panel
	{
		public Hashtable Hack
		{
			get;
		}
		private int ControlID = 0;
		private ComboBox PrefixComboBox;
		private CheckBox AutoReuseCheckBox, EquippableCheckBox;
		public int SelectedPrefix
		{
			get => PrefixComboBox.SelectedIndex;
			set => PrefixComboBox.SelectedIndex = value;
		}

		public CheckState AutoReuse
		{
			get => AutoReuseCheckBox.CheckState;
			set => AutoReuseCheckBox.CheckState = value;
		}

		public CheckState Equippable
		{
			get => EquippableCheckBox.CheckState;
			set => EquippableCheckBox.CheckState = value;
		}

		public ItemPropertiesPanel()
		{
			Hack = new Hashtable();
			AddTextBox(Lang.itemID, "Type", null);
			AddTextBox(Lang.damage, "Damage", null);
			AddTextBox(Lang.number, "Stack", null);
			AddTextBox(Lang.knockBack, "KnockBack", null, true);
			AddTextBox(Lang.crit, "Crit", null);
			AddTextBox(Lang.buff, "BuffType", null);
			AddTextBox(Lang.buffTime, "BuffTime", null);
			AddTextBox(Lang.healMana, "HealMana", null);
			AddTextBox(Lang.healLife, "HealLife", null);
			AddTextBox(Lang.useCD, "UseTime", null);
			AddTextBox(Lang.waveCD, "UseAnimation", null);
			AddTextBox(Lang.scale, "Scale", null, true);
			AddTextBox(Lang.defense, "Defense", null);
			AddTextBox(Lang.projSpeed, "ShootSpeed", null, true);
			AddTextBox(Lang.projID, "Shoot", null);
			AddTextBox(Lang.pick, "Pick", null);
			AddTextBox(Lang.axe, "Axe", null);
			AddTextBox(Lang.hammer, "Hammer", null);
			AddTextBox(Lang.digRange, "TileBoost", null);
			AddTextBox(Lang.tileID, "CreateTile", null);
			AddTextBox(Lang.placeStyle, "PlaceStyle", null);
			AddTextBox(Lang.fishingPower, "FishingPole", null);
			AddTextBox(Lang.baitPower, "Bait", null);


			PrefixComboBox = AddComboBox(Lang.prefix, MainForm.resource.Prefix);

			AutoReuseCheckBox = new CheckBox()
			{
				Text = Lang.autoReuse,
				Size = new Size(130, 20),
				Location = new Point(0, 245)
			};
			this.Controls.Add(AutoReuseCheckBox);

			EquippableCheckBox = new CheckBox()
			{
				Text = Lang.equippable,
				Size = new Size(130, 20),
				Location = new Point(135, 245)
			};
			this.Controls.Add(EquippableCheckBox);
		}
		private TextBox AddTextBox(string tipstr, string hack, EventHandler handler, bool f = false)
		{
			int a = ControlID % 2, b = (int)Math.Floor((double)ControlID / 2);
			Label tip = new Label();
			TextBox val = new TextBox();
			tip.Text = tipstr;
			tip.Location = new Point(130 * a, 20 * b);
			tip.Size = new Size(60, 20);
			tip.Font = new Font("Arial", 9);
			val.Size = new Size(60, 20);
			val.Location = new Point(60 + 130 * a, 20 * b);
			val.Multiline = true;
			val.MaxLength = 7;
			val.Font = new Font("Arial", 8);
			val.KeyDown += delegate (object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Enter)
				{
					e.SuppressKeyPress = true;
				}
			};
			val.KeyPress += delegate (object sender, KeyPressEventArgs e)
			{
				if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
				{
					e.Handled = true;
				}
				if (e.KeyChar == '.' && f)
					e.Handled = false;
			};
			if (val != null)
				val.TextChanged += handler;
			this.Controls.Add(tip);
			this.Controls.Add(val);
			if (hack != "")
				Hack.Add(hack, val);
			ControlID++;
			return val;
		}


		private ComboBox AddComboBox(string tipstr, string[] src)
		{
			int a = ControlID % 2, b = (int)Math.Floor((double)ControlID / 2);
			Label tip = new Label();
			ComboBox box = new ComboBox();
			tip.Text = tipstr;
			tip.Location = new Point(130 * a, 20 * b);
			tip.Size = new Size(60, 20);
			tip.Font = new Font("Arial", 9);
			box.Size = new Size(60, 20);
			box.Location = new Point(60 + 130 * a, 20 * b);
			box.DropDownStyle = ComboBoxStyle.DropDownList;
			box.DropDownHeight = 150;
			foreach (var o in src)
			{
				string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
				string v = t[0];
				box.Items.Add(v);
			}
			this.Controls.Add(tip);
			this.Controls.Add(box);
			ControlID++;
			return box;
		}
	}
}
