using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor.Controls
{
	public class SlotsPanel<T> : Panel where T : SlotsLayout
	{
		private class ItemIcon : PictureBox
		{
			public bool Selected
			{
				get;
				set;
			}
			public int Index
			{
				get;
			}
			public int Type
			{
				get;
				set;
			}
			public int Stack
			{
				get;
				set;
			}
			private static readonly Image TMLIconImage;
			private int lastType;
			static ItemIcon()
			{
				using Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Image.TMLIcon.png");
				TMLIconImage = Image.FromStream(st);
			}
			public ItemIcon(int index)
			{
				Index = index;
				SizeMode = PictureBoxSizeMode.CenterImage;
			}

			protected override void OnPaint(PaintEventArgs pe)
			{
				if (lastType != Type)
				{
					Image = GameResLoader.ItemImages.Images.ContainsKey(Type.ToString())
						? GameResLoader.ItemImages.Images[Type.ToString()]
						: TMLIconImage;
					lastType = Type;
				}
				base.OnPaint(pe);
				pe.Graphics.DrawString(Stack.ToString(), new Font("Arial", 10), new SolidBrush(GlobalColors.TipForeColor), 5, 33);
				if (Selected)
					pe.Graphics.DrawRectangle(new Pen(Color.BlueViolet, 3), 1, 1, pe.ClipRectangle.Width - 3, pe.ClipRectangle.Height - 3);
			}
		}
		public T SlotsLayout
		{
			get;
		}
		private List<ItemIcon> ItemSlots
		{
			get; set;
		}
		public event Action<Item> OnItemSlotSelected = (o) => { };
		public event Action<object, Item, MouseEventArgs> OnItemClick = (s, o, e) => { };
		public const int SlotsWidth = 50;
		public const int SlotsGap = 5;
		private int SelectedIconIndex
		{
			get;
			set;
		}
		public Item SelectedItem => SlotsLayout[SelectedIconIndex];
		public SlotsPanel(int number)
		{
			SlotsLayout = Activator.CreateInstance<T>();
			Size = new Size(10 * (SlotsWidth + SlotsGap), 300);
			Location = new Point(5, 5);
			ItemSlots = new List<ItemIcon>(number);
			for (int i = 0; i < number; i++)
			{
				ItemIcon icon = new ItemIcon(i)
				{
					Size = new Size(SlotsWidth, SlotsWidth),
					Location = SlotsLayout.Position(i),
					BackColor = Color.FromArgb(90, 90, 90),
					SizeMode = PictureBoxSizeMode.CenterImage,
					Selected = false
				};
				icon.MouseClick += (s, e) =>
				{
					SelectItem((s as ItemIcon).Index);
					OnItemClick(s, SelectedItem, e);
				};
				Controls.Add(icon);
				ItemSlots.Add(icon);
			}
			if (number > 0)
			{
				SelectedIconIndex = 0;
				ItemSlots[0].Selected = true;
			}
		}
		private void UpdateSlots()
		{
			foreach (var slot in ItemSlots)
			{
				UpdateSlot(slot);
			}
		}
		private void UpdateSlot(ItemIcon slot)
		{
			var item = SlotsLayout[slot.Index];
			slot.Type = item.Type;
			slot.Stack = item.Stack;
		}
		public override void Refresh()
		{
			base.Refresh();
			UpdateSlots();
		}
		public void SelectItem(int index)
		{
			ItemIcon icon = ItemSlots[index];
			foreach (var slot in ItemSlots)
				slot.Selected = false;
			icon.Selected = true;
			SelectedIconIndex = index;
			OnItemSlotSelected(SelectedItem);
			Refresh();
		}
	}
}
