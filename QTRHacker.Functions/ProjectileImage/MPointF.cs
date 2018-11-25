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
		public MPointF(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
		}
		public static MPointF operator +(MPointF a, MPointF b)
		{
			return new MPointF(a.X + b.X, a.Y + b.Y);
		}
		public static explicit operator MPoint(MPointF f)
		{
			return new MPoint((int)f.X, (int)f.Y);
		}
	}
}
