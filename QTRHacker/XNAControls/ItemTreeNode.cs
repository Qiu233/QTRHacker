using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.XNAControls
{
	public class ItemTreeNode : TreeNode
	{
		public Texture2D ContentPicture
		{
			get;
			set;
		}
		public Color BackColor
		{
			get;
			set;
		}
		public static readonly Color DefaultBackColor = new Color(132, 101, 219);
		public System.Drawing.Image Image
		{
			get;
		}
		public int Type { get; }

		public override Vector2 Size => new Vector2(32, 32);

		public override TreeNode Parent
		{
			get;
			set;
		}

		public int ItemCount
		{
			get;
		}

		public event Action<object, EventArgs> OnClick = (s, e) => { };

		public void DispatchOnClick(object o, EventArgs args)
		{
			OnClick(o, args);
		}

		public void DrawNumber(SpriteBatch batch, float x, float y, string number)
		{
			//24*31
			for (int i = 0; i < number.Length; i++)
			{
				int id = number[i] - '0';
				Rectangle src = new Rectangle(10 * id, 0, 10, 14);
				Rectangle dest = new Rectangle((int)x + i * 7, (int)y, 7, 10);
				batch.Draw(TreeView.NumbersTexture, dest, src, Color.White);
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			if (ContentPicture == null)
			{
				if (Image == null)
				{
					ContentPicture = new Texture2D(TreeView.GraphicsDevice, 1, 1);
					ContentPicture.SetData(new Color[] { Color.White });
				}
				else
				{
					MemoryStream ms = new MemoryStream();
					Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					ms.Seek(0, SeekOrigin.Begin);
					ContentPicture = Texture2D.FromStream(TreeView.GraphicsDevice, ms);
					ms.Close();
				}
			}
			var rect = new Rectangle((int)Location.X - TreeView.OriginToWorld.X, (int)Location.Y - TreeView.OriginToWorld.Y, (int)Size.X, (int)Size.Y);
			batch.Draw(TreeView.SlotBackgroudFramework, rect, BackColor);
			rect.X += 4;
			rect.Y += 4;
			rect.Width -= 8;
			rect.Height -= 8;
			batch.Draw(ContentPicture, rect, Color.White);
			if (ItemCount > 1)
				DrawNumber(batch, Location.X + Width / 2 - 15 - TreeView.OriginToWorld.X, Location.Y + Height - 12 - TreeView.OriginToWorld.Y, ItemCount.ToString());
			base.Draw(batch);
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Dispose()
		{
			ContentPicture?.Dispose();
		}

		public ItemTreeNode(TreeView view, System.Drawing.Image Image, int ItemCount, int Type, Color? BackColor = null) : base(view)
		{
			this.Image = Image;
			this.Type = Type;
			this.ItemCount = ItemCount;
			if (BackColor == null)
				this.BackColor = DefaultBackColor;
			else
				this.BackColor = BackColor.Value;
		}
	}
}
