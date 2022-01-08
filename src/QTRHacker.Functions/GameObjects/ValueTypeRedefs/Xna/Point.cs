using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.ValueTypeRedefs.Xna
{
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct Point : IEquatable<Point>
	{
		public int X;
		public int Y;

		public static readonly Point Zero = default;
		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public bool Equals(Point other) => X == other.X && Y == other.Y;

		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Point point)
			{
				result = Equals(point);
			}
			return result;
		}

		public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode();

		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1}}}", X.ToString(currentCulture), Y.ToString(currentCulture));
		}

		public static bool operator ==(Point a, Point b) => a.Equals(b);
		public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;
	}
}
