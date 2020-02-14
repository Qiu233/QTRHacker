using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.NewDimension.XNAControls
{
	public class ItemTreeNode : TreeNode
	{
		public Texture2D ContentPicture
		{
			get;
			set;
		}
		public System.Drawing.Image Image
		{
			get;
		}
		public override Vector2 Size => new Vector2(32, 32);

		public override TreeNode Parent
		{
			get;
			set;
		}

		public int ItemCountFrom
		{
			get;
		}
		public int ItemCountTo
		{
			get;
		}


		public override void AddSubNode(TreeNode node)
		{
			base.AddSubNode(node);
		}

		public void DrawNumber(SpriteBatch batch, float x, float y, string number)
		{
			//24*31
			for (int i = 0; i < number.Length; i++)
			{
				int id = number[i] - '0';
				Rectangle src = new Rectangle(10 * id, 0, 10, 14);
				Rectangle dest = new Rectangle((int)x + i * 10, (int)y, 7, 10);
				batch.Draw(TreeView.NumbersTexture, dest, src, Color.White);
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			var rect = new Rectangle((int)Location.X, (int)Location.Y, (int)Size.X, (int)Size.Y);
			batch.Draw(TreeView.SlotBackgroudFramework, rect, Color.White);
			rect.X += 2;
			rect.Y += 2;
			rect.Width -= 4;
			rect.Height -= 4;
			batch.Draw(ContentPicture, rect, Color.White);
			switch (Anchor)
			{
				case TreeNodeAnchor.Down:
					if (ItemCountFrom > 1)
						DrawNumber(batch, Location.X + Width / 2 - 10, Location.Y + Height - 43, ItemCountFrom.ToString());
					if (ItemCountTo > 1)
						DrawNumber(batch, Location.X + Width / 2 - 10, Location.Y + Height + 1, ItemCountFrom.ToString());
					break;
				case TreeNodeAnchor.Up:
					break;
				case TreeNodeAnchor.Left:
					break;
				case TreeNodeAnchor.Right:
					break;
				default:
					break;
			}
			base.Draw(batch);
		}

		public override void Initialize()
		{
			MemoryStream ms = new MemoryStream();
			Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			ms.Seek(0, SeekOrigin.Begin);
			ContentPicture = Texture2D.FromStream(TreeView.GraphicsDevice, ms);
			ms.Close();
			base.Initialize();
		}

		public override void Dispose()
		{
			ContentPicture.Dispose();
		}

		public ItemTreeNode(TreeView view, System.Drawing.Image Image, int ItemCountTo, int ItemCountFrom, TreeNodeAnchor Anchor = TreeNodeAnchor.Left) : base(view)
		{
			this.Image = Image;
			this.ItemCountFrom = ItemCountFrom;
			this.ItemCountTo = ItemCountTo;
			this.Anchor = Anchor;
		}
	}
}
