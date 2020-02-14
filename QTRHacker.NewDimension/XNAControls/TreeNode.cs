using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.XNAControls
{
	public abstract class TreeNode : IDisposable
	{
		public enum TreeNodeAnchor
		{
			Down, Up, Left, Right
		}

		public TreeNodeAnchor Anchor
		{
			get;
			set;
		}

		/// <summary>
		/// null for no parent
		/// </summary>
		public virtual TreeNode Parent
		{
			get;
			set;
		}


		protected TreeView TreeView
		{
			get;
		}


		public virtual List<TreeNode> SubNodes
		{
			get;
		}
		public abstract Vector2 Size
		{
			get;
		}
		public Vector2 Location
		{
			get;
			set;
		}
		public float X
		{
			get => Location.X;
		}
		public float Y
		{
			get => Location.Y;
		}
		public float Width
		{
			get => Size.X;
		}
		public float Height
		{
			get => Size.Y;
		}


		public Vector2 AbsLocation
		{
			get
			{
				var pos = TreeView.PointToClient(Control.MousePosition);
				return new Vector2(pos.X * TreeView.Zoom + TreeView.OriginToWorld.X, pos.Y * TreeView.Zoom + TreeView.OriginToWorld.Y);
			}
		}

		public int Depth
		{
			get
			{
				int d = 1;
				foreach (var n in SubNodes)
					if (n != null)
						d = Math.Max(d, n.Depth + 1);
				return d;
			}
		}

		public bool HasSubNodes
		{
			get => SubNodes.Count != 0;
		}

		public event Action<object, EventArgs> MouseHover = (s, e) => { };


		public void MoveTo(int x, int y)
		{
			MoveTo(new Vector2(x, y));
		}

		public void MoveTo(Vector2 v)
		{
			Move(v - Location);
		}

		public void Move(int dX, int dY)
		{
			Move(new Vector2(dX, dY));
		}

		public void Move(Vector2 dV)
		{
			foreach (var n in SubNodes)
				n.Move(dV);
			Location += dV;
		}

		public virtual void AddSubNode(TreeNode node)
		{
			SubNodes.Add(node);
			node.Parent = this;
		}

		private bool IsHovering
		{
			get
			{
				var absPos = AbsLocation;
				return new Rectangle((int)Location.X, (int)Location.Y, (int)Size.X, (int)Size.Y).Contains((int)absPos.X, (int)absPos.Y);
			}
		}

		/// <summary>
		/// Draw subnodes
		/// </summary>
		/// <param name="batch"></param>
		public virtual void Draw(SpriteBatch batch)
		{
			foreach (var item in SubNodes)
				item.Draw(batch);
		}
		/// <summary>
		/// Init subnodes
		/// </summary>
		public virtual void Initialize()
		{
			foreach (var item in SubNodes)
				item.Initialize();
		}

		public abstract void Dispose();

		public TreeNode(TreeView view)
		{
			SubNodes = new List<TreeNode>();
			TreeView = view;
		}
	}
}
