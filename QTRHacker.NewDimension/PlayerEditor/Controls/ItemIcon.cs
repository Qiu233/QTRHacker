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
	public class ItemIcon : PictureBox
	{
		public int Number, ID;
		public bool Selected = false;
		private int lastID;
		private ToolTip Tip;
		private GameContext Context;
		public static Image TMLIconImage;
		public ItemSlots Slots
		{
			get;
		}
		static ItemIcon()
		{
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.TMLIcon.png"))
				TMLIconImage = Image.FromStream(st);
		}
		public ItemIcon(GameContext Context, ItemSlots slots, int num, int id)
		{
			this.Context = Context;
			Slots = slots;
			Number = num;
			ID = id;
			Tip = new ToolTip();
		}
		/// <summary>
		/// 更新的代码写在Paint里面，原因是每500ms都会进行一次更新，就不分开写了
		/// 需要注意的是这个更新是需要手动调用的，比如执行Refresh
		/// </summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			var item = Slots[ID];
			int nowID = item.Type;
			if (lastID != nowID)
			{
				if (GameResLoader.ItemImages.Images.ContainsKey(nowID.ToString()))
				{
					if (Image != null)
						Image.Dispose();
					Image = (Image)GameResLoader.ItemImages.Images[nowID.ToString()].Clone();
					//Tip.SetToolTip(this, GameResLoader.IDToItem[nowID]);
				}
				else
				{
					Image = TMLIconImage;
					//Tip.SetToolTip(this, "");
				}
				lastID = nowID;
			}
			base.OnPaint(pe);
			pe.Graphics.DrawString(item.Stack.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), 10, 35);
			if (Selected)
			{
				pe.Graphics.DrawRectangle(new Pen(Color.BlueViolet, 3), 1, 1, pe.ClipRectangle.Width - 3, pe.ClipRectangle.Height - 3);
			}
		}
	}
}
