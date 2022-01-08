using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Controls;
using QTRHacker.PlayerEditor.Controls;
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
	public abstract class ItemSlotsEditor<T> : TabPage where T : SlotsLayout
	{
		private struct ItemInfo
		{
			public int Type;
			public int Stack;
			public byte Prefix;

			public ItemInfo(int type, int stack, byte prefix)
			{
				Type = type;
				Stack = stack;
				Prefix = prefix;
			}
		}
		private ItemInfo _Clipboard;
		public GameContext Context
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
		public ItemPropertiesPanel ItemPropertiesPanel
		{
			get;
		}
		public SlotsPanel<T> SlotsPanel
		{
			get;
		}
		public Item SelectedItem => SlotsPanel.SelectedItem;
		protected Timer Timer
		{
			get;
		}
		private int LastSelectedID
		{
			get;
			set;
		}

		protected readonly Button ButtonConfirm, ButtonRefresh, ButtonInitItem;
		protected readonly MButtonStrip ButtonStrip;
		public ItemSlotsEditor(GameContext Context, Player TargetPlayer, string Title, bool Editable, int Count)
		{
			this.Context = Context;
			this.TargetPlayer = TargetPlayer;
			this.Editable = Editable;

			Text = Title;

			LastSelectedID = -0xFF;

			ItemPropertiesPanel = new ItemPropertiesPanel();
			ItemPropertiesPanel.Location = new Point(565, 10);
			Controls.Add(ItemPropertiesPanel);

			ButtonStrip = new MButtonStrip(80, 30)
			{
				Bounds = new Rectangle(820, 10, 80, 300)
			};
			Controls.Add(ButtonStrip);

			ButtonConfirm = ButtonStrip.AddButton(HackContext.CurrentLanguage["Confirm"]);
			ButtonConfirm.Enabled = Editable;
			ButtonConfirm.Click += (sender, e) =>
			{
				OnConfirm();
			};

			ButtonRefresh = ButtonStrip.AddButton(HackContext.CurrentLanguage["Refresh"]);
			ButtonRefresh.Click += (sender, e) =>
			{
				OnRefresh();
			};

			ButtonInitItem = ButtonStrip.AddButton(HackContext.CurrentLanguage["Init"]);
			ButtonInitItem.Enabled = Editable;
			ButtonInitItem.Click += (sender, e) =>
			{
				OnInitItem();
			};


			SlotsPanel = new SlotsPanel<T>(Count);
			SlotsPanel.OnItemSlotSelected += (item) =>
			{
				FetchItemData(item);
			};
			SlotsPanel.OnItemClick += (sender, item, e) =>
			{
				if (e.Button == MouseButtons.Right)
				{
					ContextMenuStrip cms = new ContextMenuStrip();
					cms.Items.Add(HackContext.CurrentLanguage["Copy"]).Click += (s, e1) =>
					{
						_Clipboard.Type = item.Type;
						_Clipboard.Stack = item.Stack;
						_Clipboard.Prefix = item.Prefix;
					};
					cms.Items.Add(HackContext.CurrentLanguage["Paste"]).Click += (s, e1) =>
					{
						if (_Clipboard.Type == 0 || _Clipboard.Stack == 0)
							return;
						item.SetDefaultsAndPrefix(_Clipboard.Type, _Clipboard.Prefix);
						item.Stack = _Clipboard.Stack;
					};
					cms.Show(sender as Control, e.Location);
				}
			};
			Controls.Add(SlotsPanel);

			Timer = new Timer()
			{
				Interval = 500
			};
			Timer.Tick += (sender, e) =>
			{
				if (Enabled)
				{
					SlotsPanel.Refresh();
					Item item = SelectedItem;
					if (LastSelectedID != item.Type)
					{
						FetchItemData(item);
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
			if (!a.Any())
				return 0;
			return GameResLoader.Prefixes.ToList().IndexOf(a.ElementAt(0).Key);
		}

		public virtual void FetchItemData(Item item)
		{
			Type t = typeof(Item);
			foreach (DictionaryEntry de in ItemPropertiesPanel.Hack)
			{
				var pi = t.GetProperty((string)de.Key);
				if (pi == null)
					return;
				((TextBox)de.Value).Text = Convert.ToString(pi.GetValue(item));
			}
			ItemPropertiesPanel.SelectedPrefix = GetIndexFromPrefix(item.Prefix);
			ItemPropertiesPanel.AutoReuse = item.AutoReuse;
			ItemPropertiesPanel.Equippable = item.Accessory;
		}
		public virtual void ApplyItemData(Item item)
		{
			Type t = typeof(Item);
			foreach (DictionaryEntry de in ItemPropertiesPanel.Hack)
			{
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
			item.AutoReuse = ItemPropertiesPanel.AutoReuse;
			item.Accessory = ItemPropertiesPanel.Equippable;
		}
		public virtual void OnRefresh()
		{
			FetchItemData(SelectedItem);
		}
		public virtual void OnConfirm()
		{
			Item item = SelectedItem;
			ApplyItemData(item);
			FetchItemData(item);
		}
		public virtual void OnInitItem()
		{
			Item item = SelectedItem;
			item.SetDefaults(Convert.ToInt32(((TextBox)ItemPropertiesPanel.Hack["Type"]).Text));
			item.SetPrefix(GetPrefixFromIndex(ItemPropertiesPanel.SelectedPrefix));
			int stack = Convert.ToInt32(((TextBox)ItemPropertiesPanel.Hack["Stack"]).Text);
			item.Stack = stack == 0 ? 1 : stack;
			FetchItemData(item);
		}
	}
}
