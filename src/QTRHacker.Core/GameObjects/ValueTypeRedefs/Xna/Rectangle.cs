using System.Globalization;
using System.Runtime.InteropServices;

namespace QTRHacker.Core.GameObjects.ValueTypeRedefs.Xna;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct Rectangle
{
	public int X;
	public int Y;
	public int Width;
	public int Height;

	public int Left => X;
	public int Right => X + Width;
	public int Top => Y;
	public int Bottom => Y + Height;

	public Point Location
	{
		get => new Point(X, Y);
		set
		{
			X = value.X;
			Y = value.Y;
		}
	}

	public Point Center => new Point(X + Width / 2, Y + Height / 2);

	public static readonly Rectangle Empty = default;

	public bool IsEmpty => Width == 0 && Height == 0 && X == 0 && Y == 0;

	public Rectangle(int x, int y, int width, int height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x000401E4 File Offset: 0x0003F5E4
	public void Offset(Point amount)
	{
		X += amount.X;
		Y += amount.Y;
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x0004021C File Offset: 0x0003F61C
	public void Offset(int offsetX, int offsetY)
	{
		X += offsetX;
		Y += offsetY;
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x00040248 File Offset: 0x0003F648
	public void Inflate(int horizontalAmount, int verticalAmount)
	{
		X -= horizontalAmount;
		Y -= verticalAmount;
		Width += horizontalAmount * 2;
		Height += verticalAmount * 2;
	}

	public bool Contains(int x, int y) => X <= x && x < X + Width && Y <= y && y < Y + Height;
	public bool Contains(Point value) => X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
	public bool Contains(Rectangle value) => X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y && value.Y + value.Height <= Y + Height;
	public bool Intersects(Rectangle value) => value.X < X + Width && X < value.X + value.Width && value.Y < Y + Height && Y < value.Y + value.Height;

	public static Rectangle Intersect(Rectangle value1, Rectangle value2)
	{
		int num = value1.X + value1.Width;
		int num2 = value2.X + value2.Width;
		int num3 = value1.Y + value1.Height;
		int num4 = value2.Y + value2.Height;
		int num5 = value1.X > value2.X ? value1.X : value2.X;
		int num6 = value1.Y > value2.Y ? value1.Y : value2.Y;
		int num7 = num < num2 ? num : num2;
		int num8 = num3 < num4 ? num3 : num4;
		Rectangle result;
		if (num7 > num5 && num8 > num6)
		{
			result.X = num5;
			result.Y = num6;
			result.Width = num7 - num5;
			result.Height = num8 - num6;
		}
		else
		{
			result.X = 0;
			result.Y = 0;
			result.Width = 0;
			result.Height = 0;
		}
		return result;
	}

	public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
	{
		int num = value1.X + value1.Width;
		int num2 = value2.X + value2.Width;
		int num3 = value1.Y + value1.Height;
		int num4 = value2.Y + value2.Height;
		int num5 = value1.X > value2.X ? value1.X : value2.X;
		int num6 = value1.Y > value2.Y ? value1.Y : value2.Y;
		int num7 = num < num2 ? num : num2;
		int num8 = num3 < num4 ? num3 : num4;
		if (num7 > num5 && num8 > num6)
		{
			result.X = num5;
			result.Y = num6;
			result.Width = num7 - num5;
			result.Height = num8 - num6;
			return;
		}
		result.X = 0;
		result.Y = 0;
		result.Width = 0;
		result.Height = 0;
	}

	public static Rectangle Union(Rectangle value1, Rectangle value2)
	{
		int num = value1.X + value1.Width;
		int num2 = value2.X + value2.Width;
		int num3 = value1.Y + value1.Height;
		int num4 = value2.Y + value2.Height;
		int num5 = value1.X < value2.X ? value1.X : value2.X;
		int num6 = value1.Y < value2.Y ? value1.Y : value2.Y;
		int num7 = num > num2 ? num : num2;
		int num8 = num3 > num4 ? num3 : num4;
		Rectangle result;
		result.X = num5;
		result.Y = num6;
		result.Width = num7 - num5;
		result.Height = num8 - num6;
		return result;
	}

	public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
	{
		int num = value1.X + value1.Width;
		int num2 = value2.X + value2.Width;
		int num3 = value1.Y + value1.Height;
		int num4 = value2.Y + value2.Height;
		int num5 = value1.X < value2.X ? value1.X : value2.X;
		int num6 = value1.Y < value2.Y ? value1.Y : value2.Y;
		int num7 = num > num2 ? num : num2;
		int num8 = num3 > num4 ? num3 : num4;
		result.X = num5;
		result.Y = num6;
		result.Width = num7 - num5;
		result.Height = num8 - num6;
	}

	public bool Equals(Rectangle other) => X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;

	public override bool Equals(object obj)
	{
		bool result = false;
		if (obj is Rectangle rectangle)
			result = Equals(rectangle);
		return result;
	}

	public override string ToString()
	{
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		return string.Format(currentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3}}}", new object[]
		{
			X.ToString(currentCulture),
			Y.ToString(currentCulture),
			Width.ToString(currentCulture),
			Height.ToString(currentCulture)
		});
	}

	public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() + Width.GetHashCode() + Height.GetHashCode();
	public static bool operator ==(Rectangle a, Rectangle b) => a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
	public static bool operator !=(Rectangle a, Rectangle b) => a.X != b.X || a.Y != b.Y || a.Width != b.Width || a.Height != b.Height;
}
