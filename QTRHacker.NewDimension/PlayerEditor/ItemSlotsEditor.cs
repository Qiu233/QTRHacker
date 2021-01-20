using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.PlayerEditor.Controls;
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
	public abstract class ItemSlotsEditor : TabPage
	{
		public GameContext Context
		{
			get;
		}
		public Form ParentForm
		{
			get;
		}
		public Player TargetPlayer
		{
			get;
		}
		public bool Editable
		{
			get;
		}
		public ItemSlots TargetItemSlots
		{
			get;
		}
		public ItemPropertiesPanel ItemPropertiesPanel
		{
			get;
		}
		public SlotsPanel SlotsPanel
		{
			get;
		}
		protected Timer Timer
		{
			get;
		}
		protected int Selected = 0, LastSelectedID = 0;

		protected int Clip_ItemType;
		protected int Clip_ItemStack;
		protected byte Clip_ItemPrefix;
		public ItemSlotsEditor(GameContext Context, Form ParentForm, Player TargetPlayer, ItemSlots TargetItemSlots, bool Editable, int Count)
		{
			this.Context = Context;
			this.ParentForm = ParentForm;
			this.TargetPlayer = TargetPlayer;
			this.TargetItemSlots = TargetItemSlots;
			this.Editable = Editable;


			ItemPropertiesPanel = new ItemPropertiesPanel();
			ItemPropertiesPanel.Location = new Point(10 * (SlotsPanel.SlotsWidth + SlotsPanel.SlotsGap) + 15, 10);
			this.Controls.Add(ItemPropertiesPanel);


			ContextMenuStrip cms = new ContextMenuStrip();
			cms.Items.Add(MainForm.CurrentLanguage["Copy"]);
			cms.Items.Add(MainForm.CurrentLanguage["Paste"]);
			cms.ItemClicked += (sender, e) =>
			{
				var item = TargetItemSlots[Selected];
				if (e.ClickedItem.Text == MainForm.CurrentLanguage["Copy"])
				{
					Clip_ItemType = item.Type;
					Clip_ItemStack = item.Stack;
					Clip_ItemPrefix = item.Prefix;
					RefreshSelected();
				}
				else if (e.ClickedItem.Text == MainForm.CurrentLanguage["Paste"])
				{
					if (Clip_ItemType != 0)
					{
						item.SetDefaultsAndPrefix(Clip_ItemType, Clip_ItemPrefix);
						item.Stack = Clip_ItemStack;
					}
					RefreshSelected();
				}
			};

			SlotsPanel = new SlotsPanel(Context, TargetItemSlots, Count);
			SlotsPanel.OnItemSlotClick += (sender, e) =>
			{
				foreach (var s in SlotsPanel.ItemSlots)
					s.Selected = false;
				sender.Selected = true;
				SlotsPanel.Refresh();
				Selected = sender.Number;
				InitItemData(Selected);
				if (e.Button == MouseButtons.Right && Editable)
					cms.Show(sender, e.Location.X, e.Location.Y);
			};
			this.Controls.Add(SlotsPanel);


			SlotsPanel.ItemSlots[0].Selected = true;
			InitItemData(0);

			Timer = new Timer()
			{
				Interval = 500
			};
			Timer.Tick += (sender, e) =>
			{
				if (Enabled)
				{
					SlotsPanel.Refresh();
					Item item = TargetItemSlots[Selected];
					if (LastSelectedID != item.Type)
					{
						InitItemData(Selected);
						LastSelectedID = item.Type;
					}
				}
			};
			Timer.Start();
		}

		public static byte GetPrefixFromIndex(int id)
		{
			return Convert.ToByte(GameResLoader.PrefixToID[GameResLoader.Prefixes[id]]);
		}

		public int GetIndexFromPrefix(byte id)
		{
			var a = GameResLoader.PrefixToID.Where(t => t.Value == id);
			if (a.Count() == 0)
				return 0;
			return GameResLoader.Prefixes.ToList().IndexOf(a.ElementAt(0).Key);
		}

		public virtual void InitItemData(int slot)
		{
			Item item = TargetItemSlots[slot];
			Type t = typeof(Item);
			foreach (DictionaryEntry de in ItemPropertiesPanel.Hack)
			{
				object[] args = new object[1];
				args[0] = slot;
				var pi = t.GetProperty((string)de.Key);
				if (pi == null)
					return;
				((TextBox)de.Value).Text = Convert.ToString(pi.GetValue(item));

			}
			ItemPropertiesPanel.SelectedPrefix = GetIndexFromPrefix(item.Prefix);
			ItemPropertiesPanel.AutoReuse = item.AutoReuse ? CheckState.Checked : CheckState.Unchecked;
			ItemPropertiesPanel.Equippable = item.Accessory ? CheckState.Checked : CheckState.Unchecked;
		}
		public virtual void ApplyItemData(int slot)
		{
			Item item = TargetItemSlots[slot];
			Type t = typeof(Item);
			foreach (DictionaryEntry de in ItemPropertiesPanel.Hack)
			{
				object[] args = new object[1];
				args[0] = slot;
				var pi = t.GetProperty((string)de.Key);
				if (pi == null)
					return;
				if (pi.PropertyType == typeof(long) || pi.PropertyType == typeof(ulong))
					pi.SetValue(item, Convert.ToInt64(((TextBox)de.Value).Text));
				else if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(uint))
					pi.SetValue(item, Convert.ToInt32(((TextBox)de.Value).Text));
				else if (pi.PropertyType == typeof(short) || pi.PropertyType == typeof(ushort))
					pi.SetValue(item, Convert.ToInt16(((TextBox)de.Value).Text));
				else if (pi.PropertyType == typeof(float))
					pi.SetValue(item, Convert.ToSingle(((TextBox)de.Value).Text));
				else if (pi.PropertyType == typeof(double))
					pi.SetValue(item, Convert.ToDouble(((TextBox)de.Value).Text));
				else if (pi.PropertyType == typeof(bool))
					pi.SetValue(item, Convert.ToBoolean(((TextBox)de.Value).Text));
				else if (pi.PropertyType == typeof(byte))
					pi.SetValue(item, Convert.ToByte(((TextBox)de.Value).Text));

			}
			item.Prefix = GetPrefixFromIndex(ItemPropertiesPanel.SelectedPrefix);
			item.AutoReuse = ItemPropertiesPanel.AutoReuse == CheckState.Checked;
			item.Accessory = ItemPropertiesPanel.Equippable == CheckState.Checked;
		}
		public virtual void RefreshSelected()
		{
			SlotsPanel.ItemSlots[Selected].Refresh();
		}
	}
}
