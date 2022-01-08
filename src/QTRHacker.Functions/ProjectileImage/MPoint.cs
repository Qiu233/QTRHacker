using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.ProjectileImage
{
	public struct MPoint
	{
		public int X
		{
			get; set;
		}
		public int Y
		{
			get; set;
		}
		public MPoint(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
		}
		public static MPoint operator +(MPoint a, MPoint b)
		{
			return new MPoint(a.X + b.X, a.Y + b.Y);
		}
		public static explicit operator MPointF(MPoint f)
		{
			return new MPointF(f.X, f.Y);
		}
	}
}
