using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.XNAControls
{
	public abstract class TreeNode : IDisposable
	{

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
		
		
		public event Action<object, EventArgs> MouseHover = (s, e) => { };
		

		public bool IsHovering
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
		}
		/// <summary>
		/// Init subnodes
		/// </summary>
		public virtual void Initialize()
		{
		}

		public abstract void Dispose();

		public TreeNode(TreeView view)
		{
			TreeView = view;
		}
	}
}
