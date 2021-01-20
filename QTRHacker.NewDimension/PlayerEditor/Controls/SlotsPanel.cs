using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PlayerEditor.Controls
{
	public class SlotsPanel : Panel
	{
		public ItemIcon[] ItemSlots
		{
			get; set;
		}
		public event Action<ItemIcon, MouseEventArgs> OnItemSlotClick = (o, e) => { };
		public const int SlotsWidth = 50;
		public const int SlotsGap = 5;
		public SlotsPanel(GameContext Context, ItemSlots Slots, int Count)
		{
			this.Size = new Size(10 * (SlotsWidth + SlotsGap), 300);
			this.Location = new Point(5, 5);

			ItemSlots = new ItemIcon[Count];

			for (int i = 0; i < ItemSlots.Length; i++)
			{
				int row = (int)Math.Floor((double)(i / 10));
				int off = i % 10;
				ItemSlots[i] = new ItemIcon(Context, Slots, i, i)
				{
					Size = new Size(SlotsWidth, SlotsWidth),
					Location = new Point(off * (SlotsWidth + SlotsGap), row * (SlotsWidth + SlotsGap)),


					BackColor = Color.FromArgb(90, 90, 90),
					SizeMode = PictureBoxSizeMode.CenterImage,
					Selected = false
				};
				ItemSlots[i].Click += (s, e) =>
				{
					OnItemSlotClick(s as ItemIcon, e as MouseEventArgs);
				};
				this.Controls.Add(ItemSlots[i]);
			}
		}
	}
}
