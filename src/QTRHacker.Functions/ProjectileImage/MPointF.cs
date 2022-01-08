using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.ProjectileImage
{
	public struct MPointF
	{
		public float X
		{
			get; set;
		}
		public float Y
		{
			get; set;
		}
		public float LengthSquared => X * X + Y * Y;
		public float Length => (float)Math.Sqrt(LengthSquared);
		public MPointF(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public static MPointF operator +(MPointF a, MPointF b) => new(a.X + b.X, a.Y + b.Y);
		public static MPointF operator -(MPointF a, MPointF b) => new(a.X - b.X, a.Y - b.Y);
		public static MPointF operator *(MPointF a, float factor) => new(a.X * factor, a.Y * factor);
		public static MPointF operator /(MPointF a, float factor) => new(a.X / factor, a.Y / factor);
		public static explicit operator MPoint(MPointF f) => new((int)f.X, (int)f.Y);
		public static explicit operator GameObjects.ValueTypeRedefs.Xna.Vector2(MPointF f) => new(f.X, f.Y);
		public static explicit operator MPointF(GameObjects.ValueTypeRedefs.Xna.Vector2 f) => new(f.X, f.Y);
	}
}
