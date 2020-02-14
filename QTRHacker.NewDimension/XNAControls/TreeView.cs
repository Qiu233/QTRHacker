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
		public const int WorldLength = 1000;
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
		public SpriteBatch Batch
		{
			get;
			set;
		}
		public List<TreeNode> Roots
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
			Roots = new List<TreeNode>();
			OriginToWorld = new Point(0, 0);
			Zoom = 1f;
		}

		protected override void Draw()
		{
			RenderTarget2D rt = new RenderTarget2D(GraphicsDevice, WorldLength, WorldLength);
			GraphicsDevice.SetRenderTarget(rt);
			GraphicsDevice.Clear(new Color(100f / 255, 100f / 255, 100f / 255));
			Batch.Begin();
			DrawLines();
			foreach (var root in Roots)
				root.Draw(Batch);
			Batch.End();


			GraphicsDevice.SetRenderTarget(null);
			Batch.Begin();
			Batch.Draw(rt, new Rectangle(0, 0, Width, Height), new Rectangle(OriginToWorld.X, OriginToWorld.Y, GetDrawLength(Width), GetDrawLength(Height)), Color.White);
			Batch.End();
			rt.Dispose();
		}

		private int GetDrawLength(int raw)
		{
			return (int)(raw * Zoom);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (IsDraging) return;
			int v = e.Delta;
			if (v < 0 && Zoom < 1.2f)
				Zoom += 0.05f;
			else if (v > 0 && Zoom > 0.6f)
				Zoom -= 0.05f;
			Invalidate();
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

				if (p.X < 0)
					p.X = 0;
				if (p.Y < 0)
					p.Y = 0;
				if (p.X + Width * Zoom > WorldLength)
					p.X = WorldLength - (int)(Width * Zoom);
				if (p.Y + Height * Zoom > WorldLength)
					p.Y = WorldLength - (int)(Height * Zoom);

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
						color[i * 32 + j] = new Color(132f / 255, 101f / 255, 219f / 255, 1f);//132,101,219
				SlotBackgroudFramework.SetData(color);
			}
			foreach (var root in Roots)
				root.Initialize();
			Application.Idle += Application_Idle;
		}

		private void DisposeNodes(TreeNode root)
		{
			if (root == null) return;
			foreach (var n in root.SubNodes)
				n?.Dispose();
			root.Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			foreach (var r in Roots)
				DisposeNodes(r);
			Application.Idle -= Application_Idle;
		}

		public void DrawLines()
		{
			foreach (var r in Roots)
				DrawNodeLines(r);
		}

		public void DrawNodeLines(TreeNode node)
		{
			foreach (var n in node.SubNodes)
			{
				switch (n.Anchor)
				{
					case TreeNode.TreeNodeAnchor.Down:
						{
							Vector2 start = n.Location + new Vector2(n.Width / 2, 0);
							Vector2 end = start - new Vector2(0, 13);
							DrawLine(start, end, LineColor, 2);
						}
						break;
					case TreeNode.TreeNodeAnchor.Up:
						{
							Vector2 start = n.Location + new Vector2(n.Width / 2, n.Height);
							Vector2 end = start + new Vector2(0, 13);
							DrawLine(start, end, LineColor, 2);
						}
						break;
					case TreeNode.TreeNodeAnchor.Left:
						{
							Vector2 start = n.Location + new Vector2(n.Width, n.Height / 2);
							Vector2 end = start + new Vector2(13, 0);
							DrawLine(start, end, LineColor, 2);
						}
						break;
					case TreeNode.TreeNodeAnchor.Right:
						{
							Vector2 start = n.Location + new Vector2(0, n.Height / 2);
							Vector2 end = start - new Vector2(13, 0);
							DrawLine(start, end, LineColor, 2);
						}
						break;
					default:
						break;
				}
			}
			if (!node.HasSubNodes)
				return;
			foreach (var n in node.SubNodes)
			{
				DrawNodeLines(n);
			}

			var head = node.SubNodes.First();
			var tail = node.SubNodes.Last();
			switch (head.Anchor)
			{
				case TreeNode.TreeNodeAnchor.Down:
					{
						Vector2 b = head.Location + new Vector2(head.Width / 2, -15);
						Vector2 e = tail.Location + new Vector2(tail.Width / 2, -15);
						DrawLine(b, e, LineColor, 2);
						Vector2 center = (b + e) / 2;
						Vector2 centerEnd = center - new Vector2(0, 11);
						DrawLine(center, centerEnd, LineColor, 2);
					}
					break;
				case TreeNode.TreeNodeAnchor.Up:
					{
						Vector2 b = head.Location + new Vector2(head.Width / 2 - 2, head.Height + 15);
						Vector2 e = tail.Location + new Vector2(tail.Width / 2 - 2, head.Height + 15);
						DrawLine(b, e, LineColor, 2);
						Vector2 center = (b + e) / 2 + new Vector2(2, 0);
						Vector2 centerEnd = center + new Vector2(0, 11);
						DrawLine(center, centerEnd, LineColor, 2);
					}
					break;
				case TreeNode.TreeNodeAnchor.Left:
					{
						Vector2 b = head.Location + new Vector2(head.Width + 15, head.Height / 2);
						Vector2 e = tail.Location + new Vector2(tail.Width + 15, head.Height / 2);
						DrawLine(b, e, LineColor, 2);
						Vector2 center = (b + e) / 2;
						Vector2 centerEnd = center + new Vector2(11, 0);
						DrawLine(center, centerEnd, LineColor, 2);
					}
					break;
				case TreeNode.TreeNodeAnchor.Right:
					{
						Vector2 b = head.Location + new Vector2(-15, head.Height / 2 - 2);
						Vector2 e = tail.Location + new Vector2(-15, head.Height / 2 - 2);
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
			List<List<TreeNode>> nodes = new List<List<TreeNode>>();

			int depth = Roots.Max(n => n.Depth);
			nodes.Add(new List<TreeNode>());
			nodes[0].AddRange(Roots);

			for (int i = 1; i < depth; i++)
			{
				var list = new List<TreeNode>();
				nodes.Add(list);
				foreach (var n in nodes[i - 1])
					list.AddRange(n.SubNodes);
			}

			for (int i = depth - 1; i >= 0; i--)
			{
				var layer = nodes[i];
				//
				for (int j = 0; j < layer.Count; j++)
				{
					var n = layer[j];
					if (n.HasSubNodes)
					{
						var head = n.SubNodes.First();
						var tail = n.SubNodes.Last();
						switch (head.Anchor)
						{
							case TreeNode.TreeNodeAnchor.Down:
								n.Location = new Vector2((head.X + tail.X + tail.Width) / 2 - n.Width / 2, (head.Y + tail.Y + tail.Height) / 2 - n.Height / 2 - 60);
								break;
							case TreeNode.TreeNodeAnchor.Up:
								n.Location = new Vector2((head.X + tail.X + tail.Width) / 2 - n.Width / 2, (head.Y + tail.Y + tail.Height) / 2 - n.Height / 2 + 60);
								break;
							case TreeNode.TreeNodeAnchor.Left:
								n.Location = new Vector2((head.X + tail.X + tail.Width) / 2 - n.Width / 2 + 60, (head.Y + tail.Y + tail.Height) / 2 - n.Height / 2);
								break;
							case TreeNode.TreeNodeAnchor.Right:
								n.Location = new Vector2((head.X + tail.X + tail.Width) / 2 - n.Width / 2 - 60, (head.Y + tail.Y + tail.Height) / 2 - n.Height / 2);
								break;
							default:
								break;
						}
					}
					else if (j > 0)
					{
						var pre = layer[j - 1];
						switch (n.Anchor)
						{
							case TreeNode.TreeNodeAnchor.Down:
							case TreeNode.TreeNodeAnchor.Up:
								n.Location = pre.Location + new Vector2(pre.Width + 20, 0);
								break;
							case TreeNode.TreeNodeAnchor.Left:
							case TreeNode.TreeNodeAnchor.Right:
								n.Location = pre.Location + new Vector2(0, pre.Height + 20);
								break;
							default:
								break;
						}
					}
				}

				//
				for (int j = 1; j < layer.Count; j++)
				{
					var last = layer[j - 1];
					var cur = layer[j];
					switch (cur.Anchor)
					{
						case TreeNode.TreeNodeAnchor.Up:
						case TreeNode.TreeNodeAnchor.Down:
							{
								var d = 20 - (cur.X - (last.X + last.Width));
								if (d > 0)
									cur.Move((int)d, 0);
							}
							break;
						case TreeNode.TreeNodeAnchor.Left:
						case TreeNode.TreeNodeAnchor.Right:
							{
								var d = 20 - (cur.Y - (last.Y + last.Height));
								if (d > 0)
									cur.Move(0, (int)d);
							}
							break;
						default:
							break;
					}
				}
			}
		}



		private void Application_Idle(object sender, EventArgs e)
		{
			Draw();
		}
	}
}
