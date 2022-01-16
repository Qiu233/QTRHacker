using QTRHacker.Res;
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
		private readonly ComboBox PrefixComboBox;
		private readonly CheckBox AutoReuseCheckBox, EquippableCheckBox;
		public int SelectedPrefix
		{
			get => PrefixComboBox.SelectedIndex;
			set => PrefixComboBox.SelectedIndex = value;
		}

		public bool AutoReuse
		{
			get => AutoReuseCheckBox.CheckState == CheckState.Checked;
			set => AutoReuseCheckBox.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
		}

		public bool Equippable
		{
			get => EquippableCheckBox.CheckState == CheckState.Checked;
			set => EquippableCheckBox.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
		}

		public ItemPropertiesPanel()
		{
			Size = new Size(250, 360);

			Hack = new Hashtable();
			AddTextBox(HackContext.CurrentLanguage["Type"], "Type", null);
			AddTextBox(HackContext.CurrentLanguage["Damage"], "Damage", null);
			AddTextBox(HackContext.CurrentLanguage["Stack"], "Stack", null);
			AddTextBox(HackContext.CurrentLanguage["KnockBack"], "KnockBack", null, true);
			AddTextBox(HackContext.CurrentLanguage["Crit"], "Crit", null);
			AddTextBox(HackContext.CurrentLanguage["BuffType"], "BuffType", null);
			AddTextBox(HackContext.CurrentLanguage["BuffTime"], "BuffTime", null);
			AddTextBox(HackContext.CurrentLanguage["HealMana"], "HealMana", null);
			AddTextBox(HackContext.CurrentLanguage["HealLife"], "HealLife", null);
			AddTextBox(HackContext.CurrentLanguage["UseTime"], "UseTime", null);
			AddTextBox(HackContext.CurrentLanguage["UseAnimation"], "UseAnimation", null);
			AddTextBox(HackContext.CurrentLanguage["Scale"], "Scale", null, true);
			AddTextBox(HackContext.CurrentLanguage["Defense"], "Defense", null);
			AddTextBox(HackContext.CurrentLanguage["ShootSpeed"], "ShootSpeed", null, true);
			AddTextBox(HackContext.CurrentLanguage["Shoot"], "Shoot", null);
			AddTextBox(HackContext.CurrentLanguage["Pick"], "Pick", null);
			AddTextBox(HackContext.CurrentLanguage["Axe"], "Axe", null);
			AddTextBox(HackContext.CurrentLanguage["Hammer"], "Hammer", null);
			AddTextBox(HackContext.CurrentLanguage["TileBoost"], "TileBoost", null);
			AddTextBox(HackContext.CurrentLanguage["CreateTile"], "CreateTile", null);
			AddTextBox(HackContext.CurrentLanguage["PlaceStyle"], "PlaceStyle", null);
			AddTextBox(HackContext.CurrentLanguage["FishingPole"], "FishingPole", null);
			AddTextBox(HackContext.CurrentLanguage["Bait"], "Bait", null);


			PrefixComboBox = AddComboBox(HackContext.CurrentLanguage["Prefix"], GameResLoader.Prefixes);

			AutoReuseCheckBox = new CheckBox()
			{
				Text = HackContext.CurrentLanguage["AutoReuse"],
				Size = new Size(130, 20),
				Location = new Point(0, 245),
				ForeColor = GlobalColors.TipForeColor
			};
			Controls.Add(AutoReuseCheckBox);

			EquippableCheckBox = new CheckBox()
			{
				Text = HackContext.CurrentLanguage["Accessory"],
				Size = new Size(130, 20),
				Location = new Point(135, 245),
				ForeColor = GlobalColors.TipForeColor
			};
			Controls.Add(EquippableCheckBox);
		}
		private TextBox AddTextBox(string tipstr, string hack, EventHandler handler, bool f = false)
		{
			int a = ControlID % 2, b = (int)Math.Floor((double)ControlID / 2);
			Label tip = new Label
			{
				Text = tipstr,
				Location = new Point(130 * a, 20 * b),
				ForeColor = GlobalColors.TipForeColor,
				Size = new Size(60, 20),
				Font = new Font("Arial", 9)
			};
			TextBox val = new TextBox
			{
				Size = new Size(60, 20),
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.FromArgb(120, 120, 120),
				Location = new Point(60 + 130 * a, 20 * b),
				Multiline = true,
				MaxLength = 7,
				Font = new Font("Arial", 8)
			};
			val.KeyDown += delegate (object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Enter)
					e.SuppressKeyPress = true;
			};
			val.KeyPress += delegate (object sender, KeyPressEventArgs e)
			{
				if (!char.IsNumber(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != '-')
					e.Handled = true;
				if (e.KeyChar == '.' && f)
					e.Handled = false;
			};
			if (val != null)
				val.TextChanged += handler;
			Controls.Add(tip);
			Controls.Add(val);
			if (hack != "")
				Hack.Add(hack, val);
			ControlID++;
			return val;
		}


		private ComboBox AddComboBox(string tipstr, string[] src)
		{
			int a = ControlID % 2, b = (int)Math.Floor((double)ControlID / 2);
			Label tip = new Label
			{
				Text = tipstr,
				Location = new Point(130 * a, 20 * b),
				Size = new Size(60, 20),
				Font = new Font("Arial", 9),
				ForeColor = GlobalColors.TipForeColor
			};
			ComboBox box = new ComboBox
			{
				Size = new Size(60, 20),
				Location = new Point(60 + 130 * a, 20 * b),
				DropDownStyle = ComboBoxStyle.DropDownList,
				DropDownHeight = 150
			};
			foreach (var o in src)
			{
				string[] t = o.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
				string v = t[0];
				box.Items.Add(v);
			}
			Controls.Add(tip);
			Controls.Add(box);
			ControlID++;
			return box;
		}
	}
}
