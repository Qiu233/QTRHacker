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
using System.Windows.Forms;
using WinFormsGraphicsDevice;

namespace QTRHacker.NewDimension.XNAControls
{
	public sealed class TreeView : GraphicsDeviceControl
	{
		public enum TreeAnchor
		{
			Up, Down, Right, Left
		}

		public TreeAnchor TAnchor
		{
			get;
			set;
		}
		public static readonly Color LineColor = new Color(160f / 255, 160f / 255, 160f / 255);
		public Point OriginToWorld
		{
			get;
			set;
		}
		public float Zoom
		{
			get;
			set;
		}
		public Texture2D SlotBackgroudFramework
		{
			get;
			private set;
		}
		public Texture2D NumbersTexture
		{
			get;
			private set;
		}
		public Texture2D Pixel1x1
		{
			get;
			private set;
		}
		public bool Initialized { get; private set; }
		public SpriteBatch Batch
		{
			get;
			set;
		}
		public List<ItemTreeNode> NodesFrom
		{
			get;
			set;
		}
		public List<ItemTreeNode> NodesTo
		{
			get;
			set;
		}
		public TreeNode Root
		{
			get;
			set;
		}

		public Vector2 MouseWorld
		{
			get
			{
				var p = PointToClient(MousePosition);
				return ControlToWorld(new Point(p.X, p.Y));
			}
		}


		public Vector2 ControlToWorld(Point v)
		{
			return new Vector2((int)(v.X * Zoom + OriginToWorld.X), (int)(v.Y * Zoom + OriginToWorld.Y));
		}

		public TreeView()
		{
			NodesFrom = new List<ItemTreeNode>();
			NodesTo = new List<ItemTreeNode>();
			OriginToWorld = new Point(0, 0);
			Zoom = 1f;
		}

		protected override void Draw()
		{
			GraphicsDevice.Clear(new Color(100f / 255, 100f / 255, 100f / 255));
			Batch.Begin();
			DrawLines();
			foreach (var n in NodesFrom)
				n.Draw(Batch);
			foreach (var n in NodesTo)
				n.Draw(Batch);
			Root.Draw(Batch);
			Batch.End();
		}

		private int GetDrawLength(int raw)
		{
			return (int)(raw * Zoom);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			/*if (IsDraging) return;
			int v = e.Delta;
			if (v < 0 && Zoom < 1.2f)
				Zoom += 0.05f;
			else if (v > 0 && Zoom > 0.6f)
				Zoom -= 0.05f;
			Invalidate();*/
		}

		private bool IsDraging = false;
		private Point RawOriginToWorld;
		private System.Drawing.Point DragStart;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			IsDraging = true;
			RawOriginToWorld = OriginToWorld;
			DragStart = e.Location;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			IsDraging = false;
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (IsDraging)
			{
				int dX = (int)((e.Location.X - DragStart.X) * Zoom);
				int dY = (int)((e.Location.Y - DragStart.Y) * Zoom);

				var p = new Point(RawOriginToWorld.X - dX, RawOriginToWorld.Y - dY);

				/*if (p.X < 0)
					p.X = 0;
				if (p.Y < 0)
					p.Y = 0;
				if (p.X + Width * Zoom > Width)
					p.X = Width - (int)(Width * Zoom);
				if (p.Y + Height * Zoom > Height)
					p.Y = Height - (int)(Height * Zoom);*/

				OriginToWorld = p;
				Invalidate();
			}
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			var mousePos = PointToClient(MousePosition);

		}

		protected override void Initialize()
		{
			Initialized = true;
			Batch = new SpriteBatch(GraphicsDevice);
			Pixel1x1 = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			Pixel1x1.SetData(new Color[] { Color.White });
			using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.Numbers.png"))
				NumbersTexture = Texture2D.FromStream(GraphicsDevice, s);

			{
				Color[] raw_color = new Color[NumbersTexture.Width * NumbersTexture.Height];
				Color[] color = new Color[NumbersTexture.Width * NumbersTexture.Height];
				NumbersTexture.GetData(raw_color);
				raw_color.CopyTo(color, 0);

				for (int i = 0; i < raw_color.Length; i++)
				{
					if (color[i] == Color.Transparent) continue;
					color[i].R = (byte)(255 - raw_color[i].R);
					color[i].G = (byte)(255 - raw_color[i].G);
					color[i].B = (byte)(255 - raw_color[i].B);
					color[i].A = raw_color[i].A;
				}
				NumbersTexture.SetData(color);
			}

			{
				SlotBackgroudFramework = new Texture2D(GraphicsDevice, 32, 32, false, SurfaceFormat.Color);
				Color[] color = new Color[32 * 32];
				for (int i = 0; i < 32; i++)
					color[i] = color[31 * 32 + i] = color[32 * i] = color[32 * i + 31] = Color.Purple;
				for (int i = 1; i < 31; i++)
					for (int j = 1; j < 31; j++)
						color[i * 32 + j] = new Color(1f, 1f, 1f, 1f);//132,101,219
				SlotBackgroudFramework.SetData(color);
			}
			Root.Initialize();
			foreach (var root in NodesFrom)
				root.Initialize();
			foreach (var root in NodesTo)
				root.Initialize();
			Application.Idle += Application_Idle;
		}


		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			foreach (var r in NodesFrom)
				r.Dispose();
			Application.Idle -= Application_Idle;
		}

		public void DrawLines()
		{
			DrawNodeLines();
		}

		public void DrawNodeLines()
		{
			{//From
				foreach (var n in NodesFrom)
				{
					switch (TAnchor)
					{
						case TreeAnchor.Up:
							{
								Vector2 start = n.Location + new Vector2(n.Width / 2, 0) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start - new Vector2(0, 13);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						case TreeAnchor.Down:
							{
								Vector2 start = n.Location + new Vector2(n.Width / 2 + 2, n.Height) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start + new Vector2(0, 13);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						case TreeAnchor.Right:
							{
								Vector2 start = n.Location + new Vector2(n.Width, n.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start + new Vector2(13, 0);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						case TreeAnchor.Left:
							{
								Vector2 start = n.Location + new Vector2(0, n.Height / 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start - new Vector2(13, 0);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						default:
							break;
					}
				}

				if (NodesFrom.Count > 0)
				{
					var head = NodesFrom.First();
					var tail = NodesFrom.Last();
					switch (TAnchor)
					{
						case TreeAnchor.Up:
							{
								Vector2 b = head.Location + new Vector2(head.Width / 2, -15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(tail.Width / 2, -15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2;
								Vector2 centerEnd = center - new Vector2(0, 11);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						case TreeAnchor.Down:
							{
								Vector2 b = head.Location + new Vector2(head.Width / 2, head.Height + 15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(tail.Width / 2, head.Height + 15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2 + new Vector2(2, 0);
								Vector2 centerEnd = center + new Vector2(0, 11);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						case TreeAnchor.Right:
							{
								Vector2 b = head.Location + new Vector2(head.Width + 15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(tail.Width + 15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2;
								Vector2 centerEnd = center + new Vector2(11, 0);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						case TreeAnchor.Left:
							{
								Vector2 b = head.Location + new Vector2(-15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(-15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2 + new Vector2(0, 2);
								Vector2 centerEnd = center - new Vector2(11, 0);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						default:
							break;
					}
				}
			}
			{//To
				foreach (var n in NodesTo)
				{
					switch (TAnchor)
					{
						case TreeAnchor.Up:
							{
								Vector2 start = n.Location + new Vector2(n.Width / 2 + 2, n.Height) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start + new Vector2(0, 13);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						case TreeAnchor.Down:
							{
								Vector2 start = n.Location + new Vector2(n.Width / 2, 0) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start - new Vector2(0, 13);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						case TreeAnchor.Right:
							{
								Vector2 start = n.Location + new Vector2(0, n.Height / 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start - new Vector2(13, 0);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						case TreeAnchor.Left:
							{
								Vector2 start = n.Location + new Vector2(n.Width, n.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 end = start + new Vector2(13, 0);
								DrawLine(start, end, LineColor, 2);
							}
							break;
						default:
							break;
					}
				}

				if (NodesTo.Count > 0)
				{
					var head = NodesTo.First();
					var tail = NodesTo.Last();
					switch (TAnchor)
					{
						case TreeAnchor.Up:
							{
								Vector2 b = head.Location + new Vector2(head.Width / 2, head.Height + 15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(tail.Width / 2, head.Height + 15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2 + new Vector2(2, 0);
								Vector2 centerEnd = center + new Vector2(0, 11);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						case TreeAnchor.Down:
							{
								Vector2 b = head.Location + new Vector2(head.Width / 2, -15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(tail.Width / 2, -15) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2;
								Vector2 centerEnd = center - new Vector2(0, 11);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						case TreeAnchor.Right:
							{
								Vector2 b = head.Location + new Vector2(-15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(-15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2 + new Vector2(0, 2);
								Vector2 centerEnd = center - new Vector2(11, 0);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						case TreeAnchor.Left:
							{
								Vector2 b = head.Location + new Vector2(head.Width + 15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								Vector2 e = tail.Location + new Vector2(tail.Width + 15, head.Height / 2 - 2) - new Vector2(OriginToWorld.X, OriginToWorld.Y);
								DrawLine(b, e, LineColor, 2);
								Vector2 center = (b + e) / 2;
								Vector2 centerEnd = center + new Vector2(11, 0);
								DrawLine(center, centerEnd, LineColor, 2);
							}
							break;
						default:
							break;
					}
				}
			}
		}

		/// <summary>
		/// from Stack Overflow
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="color"></param>
		/// <param name="width"></param>
		public void DrawLine(Vector2 start, Vector2 end, Color color, int width = 1)
		{
			Rectangle r = new Rectangle((int)start.X, (int)start.Y, (int)(end - start).Length() + width, width);
			Vector2 v = Vector2.Normalize(start - end);
			float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
			if (start.Y > end.Y)
				angle = MathHelper.TwoPi - angle;
			Batch.Draw(Pixel1x1, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
		}

		public void ArrangeTree()
		{
			if (NodesFrom.Count > 0)
			{
				switch (TAnchor)
				{
					case TreeAnchor.Up:
						NodesFrom[0].Location = Root.Location +
							new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2((NodesFrom.Count * NodesFrom[0].Width + 20 * (NodesFrom.Count - 1)) / 2, -44);
						break;
					case TreeAnchor.Down:
						NodesFrom[0].Location = Root.Location +
							new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2((NodesFrom.Count * NodesFrom[0].Width + 20 * (NodesFrom.Count - 1)) / 2, 76);
						break;
					case TreeAnchor.Right:
						NodesFrom[0].Location = Root.Location +
							new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2(76, (NodesFrom.Count * NodesFrom[0].Height + 20 * (NodesFrom.Count - 1)) / 2);
						break;
					case TreeAnchor.Left:
						NodesFrom[0].Location = Root.Location +
							new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2(-44, (NodesFrom.Count * NodesFrom[0].Height + 20 * (NodesFrom.Count - 1)) / 2);
						break;
					default:
						break;
				}
			}
			if (NodesTo.Count > 0)
			{
				switch (TAnchor)
				{
					case TreeAnchor.Up:
						NodesTo[0].Location = Root.Location +
							new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2((NodesTo.Count * NodesTo[0].Width + 20 * (NodesTo.Count - 1)) / 2, 76);
						break;
					case TreeAnchor.Down:
						NodesTo[0].Location = Root.Location +
							new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2((NodesTo.Count * NodesTo[0].Width + 20 * (NodesTo.Count - 1)) / 2, -44);
						break;
					case TreeAnchor.Right:
						NodesTo[0].Location = Root.Location + new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2(-44, (NodesTo.Count * NodesTo[0].Height + 20 * (NodesTo.Count - 1)) / 2);
						break;
					case TreeAnchor.Left:
						NodesTo[0].Location = Root.Location +
							new Vector2(Root.Width / 2, Root.Height / 2) -
							new Vector2(76, (NodesTo.Count * NodesTo[0].Height + 20 * (NodesTo.Count - 1)) / 2);
						break;
					default:
						break;
				}
			}
			for (int i = 1; i < NodesFrom.Count; i++)
			{
				var n = NodesFrom[i];
				var prev = NodesFrom[i - 1];
				switch (TAnchor)
				{
					case TreeAnchor.Up:
					case TreeAnchor.Down:
						n.Location = prev.Location + new Vector2(prev.Width + 20, 0);
						break;
					case TreeAnchor.Right:
					case TreeAnchor.Left:
						n.Location = prev.Location + new Vector2(0, prev.Height + 20);
						break;
					default:
						break;
				}
			}
			for (int i = 1; i < NodesTo.Count; i++)
			{
				var n = NodesTo[i];
				var prev = NodesTo[i - 1];
				switch (TAnchor)
				{
					case TreeAnchor.Up:
					case TreeAnchor.Down:
						n.Location = prev.Location + new Vector2(prev.Width + 20, 0);
						break;
					case TreeAnchor.Right:
					case TreeAnchor.Left:
						n.Location = prev.Location + new Vector2(0, prev.Height + 20);
						break;
					default:
						break;
				}
			}

			{//to strech
				int l1 = 0, l2 = 0;
				switch (TAnchor)
				{
					case TreeAnchor.Up:
					case TreeAnchor.Down:
						{
							l2 = 400;
							int t = Math.Max(NodesFrom.Count, NodesTo.Count);
							l1 = t * 32 + (t - 1) * 20 + 100;
						}
						break;
					case TreeAnchor.Right:
					case TreeAnchor.Left:
						{
							l1 = 600;
							int t = Math.Max(NodesFrom.Count, NodesTo.Count);
							l2 = t * 32 + (t - 1) * 20 + 100;
						}
						break;
					default:
						break;
				}

				{
					float jX1 = NodesFrom.Count > 0 ? NodesFrom.Min(u => u.Location.X) : float.MaxValue;
					float jY1 = NodesFrom.Count > 0 ? NodesFrom.Min(u => u.Location.Y) : float.MaxValue;
					float jX2 = NodesTo.Count > 0 ? NodesTo.Min(u => u.Location.X) : float.MaxValue;
					float jY2 = NodesTo.Count > 0 ? NodesTo.Min(u => u.Location.Y) : float.MaxValue;

					float jX = Math.Min(Math.Min(jX1, jX2), Root.X);
					float jY = Math.Min(Math.Min(jY1, jY2), Root.Y);

					var d = new Vector2(200, 50) - new Vector2(jX, jY);
					MoveTree(d);


				}
			}
		}

		private void MoveTree(Vector2 dV)
		{
			Root.Location += dV;
			foreach (var n in NodesFrom)
				n.Location += dV;
			foreach (var n in NodesTo)
				n.Location += dV;
		}


		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			for (int i = 0; i < NodesFrom.Count; i++)
			{
				var n = NodesFrom[i];
				if (n?.IsHovering == true)
				{
					n?.DispatchOnClick(n, e);
				}
			}
			for (int i = 0; i < NodesTo.Count; i++)
			{
				var n = NodesTo[i];
				if (n?.IsHovering == true)
				{
					n?.DispatchOnClick(n, e);
				}
			}

		}


		private void Application_Idle(object sender, EventArgs e)
		{
			Draw();
		}
	}
}
