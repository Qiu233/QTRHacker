using QTRHacker.NewDimension.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PlayerEditor
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
			AddTextBox("类型", "Type", null);
			AddTextBox("攻击力", "Damage", null);
			AddTextBox("数量", "Stack", null);
			AddTextBox("击退", "KnockBack", null, true);
			AddTextBox("暴击", "Crit", null);
			AddTextBox("Buff类型", "BuffType", null);
			AddTextBox("Buff时间", "BuffTime", null);
			AddTextBox("回蓝", "HealMana", null);
			AddTextBox("回血", "HealLife", null);
			AddTextBox("挥动间隔", "UseTime", null);
			AddTextBox("挥动时间", "UseAnimation", null);
			AddTextBox("缩放", "Scale", null, true);
			AddTextBox("防御", "Defense", null);
			AddTextBox("弹幕速度", "ShootSpeed", null, true);
			AddTextBox("弹幕", "Shoot", null);
			AddTextBox("挖掘力", "Pick", null);
			AddTextBox("砍伐力", "Axe", null);
			AddTextBox("锤击力", "Hammer", null);
			AddTextBox("挖掘距离", "TileBoost", null);
			AddTextBox("放置ID", "CreateTile", null);
			AddTextBox("特殊值", "PlaceStyle", null);
			AddTextBox("鱼竿", "FishingPole", null);
			AddTextBox("鱼饵", "Bait", null);


			PrefixComboBox = AddComboBox("前缀", GameResLoader.Prefixes);
			PrefixComboBox.BackColor = Color.FromArgb(120, 120, 120);

			AutoReuseCheckBox = new CheckBox()
			{
				Text = "按住鼠标连续使用",
				Size = new Size(130, 20),
				Location = new Point(0, 245)
			};
			this.Controls.Add(AutoReuseCheckBox);

			EquippableCheckBox = new CheckBox()
			{
				Text = "可装备",
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
			val.BorderStyle = BorderStyle.FixedSingle;
			val.BackColor = Color.FromArgb(120, 120, 120);
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
