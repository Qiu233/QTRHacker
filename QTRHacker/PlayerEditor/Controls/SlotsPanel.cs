using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Res;
using QTRHacker.XNA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsGraphicsDevice;

namespace QTRHacker.PlayerEditor.Controls
{
	public class SlotsPanel<T> : GraphicsDeviceControl where T : SlotsLayout
	{
		public event Action<Item> OnItemSlotSelected = (o) => { };
		public event Action<object, Item, MouseEventArgs> OnItemClick = (s, o, e) => { };
		public const int ItemMargin = 10;
		private int SelectedIndex
		{
			get;
			set;
		}
		public T SLayout { get; }
		public int SlotIndices { get; }
		public Item SelectedItem => SLayout[SelectedIndex];
		public SlotsPanel(int number)
		{
			SlotIndices = number;
			SLayout = Activator.CreateInstance<T>();
			Size = new Size(10 * (SlotsLayout.SlotWidth + SlotsLayout.SlotGap), 300);
			Location = new Point(5, 5);
			SelectedIndex = 0;
		}
		private SpriteBatch Batch;
		private Dictionary<int, Texture2D> ItemTextureCache
		{
			get;
		} = new Dictionary<int, Texture2D>();
		private Texture2D BorderTexture;
		private Texture2D BackTexture;
		private GDITextFactory GDITextFactory;

		protected override void Initialize()
		{
			Batch = new SpriteBatch(GraphicsDevice);
			GDITextFactory = new GDITextFactory(GraphicsDevice, "Callibri");

			BackTexture = new Texture2D(GraphicsDevice, SlotsLayout.SlotWidth, SlotsLayout.SlotWidth);
			Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[SlotsLayout.SlotWidth * SlotsLayout.SlotWidth];
			Array.Fill(data, new Microsoft.Xna.Framework.Color(90, 90, 90));
			BackTexture.SetData(data);

			BorderTexture = new Texture2D(GraphicsDevice, SlotsLayout.SlotWidth, SlotsLayout.SlotWidth);
			var borderColor = new Microsoft.Xna.Framework.Color(255, 255, 255);
			data = new Microsoft.Xna.Framework.Color[SlotsLayout.SlotWidth * SlotsLayout.SlotWidth];
			for (int i = 0; i < SlotsLayout.SlotWidth; i++)
			{
				//top
				data[i] = borderColor;
				data[SlotsLayout.SlotWidth + i] = borderColor;
				//bottom
				data[(SlotsLayout.SlotWidth - 2) * SlotsLayout.SlotWidth + i] = borderColor;
				data[(SlotsLayout.SlotWidth - 1) * SlotsLayout.SlotWidth + i] = borderColor;
				//left
				data[i * SlotsLayout.SlotWidth] = borderColor;
				data[i * SlotsLayout.SlotWidth + 1] = borderColor;
				//right
				data[(i + 1) * SlotsLayout.SlotWidth - 2] = borderColor;
				data[(i + 1) * SlotsLayout.SlotWidth - 1] = borderColor;
			}
			BorderTexture.SetData(data);
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!disposing)
				return;
			GDITextFactory?.Dispose();
			BackTexture?.Dispose();
			BorderTexture?.Dispose();
		}

		private Texture2D GetItemTexture(int itemType)
		{
			if (ItemTextureCache.TryGetValue(itemType, out Texture2D t))
				return t;
			Texture2D res = null;
			if (GameResLoader.ItemImageData.TryGetValue($"Item_{itemType}", out byte[] value))
			{
				using var s = new MemoryStream(value);
				res = Texture2D.FromStream(GraphicsDevice, s);
			}
			return ItemTextureCache[itemType] = res;
		}

		private void DrawSlotItem(int index)
		{
			var loc = SLayout.GetPosition(index);
			Item item = SLayout[index];
			Texture2D t2d = GetItemTexture(item.Type);
			float size = SlotsLayout.SlotWidth - ItemMargin * 2;
			float scale = Math.Min(size / t2d.Width, size / t2d.Height);

			var pos = new Microsoft.Xna.Framework.Vector2(loc.X + ItemMargin + (size - t2d.Width * scale) / 2, loc.Y + ItemMargin + (size - t2d.Height * scale) / 2);
			var itemColor = item.Color;
			Microsoft.Xna.Framework.Color color = default;
			if (itemColor.A == 0)
				color = Microsoft.Xna.Framework.Color.White;
			else
				color.PackedValue = itemColor.PackedValue;
			Batch.Draw(t2d, pos, null, color, 0, Microsoft.Xna.Framework.Vector2.Zero, scale, SpriteEffects.None, 0);
		}

		private void DrawSlotBack(int index)
		{
			var loc = SLayout.GetPosition(index);
			Batch.Draw(BackTexture, new Microsoft.Xna.Framework.Vector2(loc.X, loc.Y), Microsoft.Xna.Framework.Color.White);
		}

		private void DrawSlotBorder(int index)
		{
			var loc = SLayout.GetPosition(index);
			Batch.Draw(BorderTexture, new Microsoft.Xna.Framework.Vector2(loc.X, loc.Y), new Microsoft.Xna.Framework.Color(200, 150, 100));
		}

		private void DrawItemStack(int index)
		{
			var loc = SLayout.GetPosition(index);
			int stack = SLayout[index].Stack;
			if (stack == 0 || stack == 1)
				return;
			GDITextFactory.DrawString(Batch, stack.ToString(), new Microsoft.Xna.Framework.Vector2(loc.X + 3, loc.Y + SlotsLayout.SlotWidth - 18), Microsoft.Xna.Framework.Color.White, 10);
		}

		protected override void Draw()
		{
			GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(BackColor.R, BackColor.G, BackColor.B));
			Batch.Begin();
			for (int i = 0; i < SlotIndices; i++)
			{
				DrawSlotBack(i);
				DrawSlotItem(i);
				DrawItemStack(i);
			}
			DrawSlotBorder(SelectedIndex);
			Batch.End();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Draw();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				for (int i = 0; i < SlotIndices; i++)
				{
					Rectangle r = new Rectangle(SLayout.GetPosition(i), new Size(SlotsLayout.SlotWidth, SlotsLayout.SlotWidth));
					if (r.Contains(e.Location))
					{
						SelectedIndex = i;
						break;
					}
				}
			}
			Invalidate();
		}
	}
}
