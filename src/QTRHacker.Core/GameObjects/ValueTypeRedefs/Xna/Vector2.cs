using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace QTRHacker.Core.GameObjects.ValueTypeRedefs.Xna;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct Vector2 : IEquatable<Vector2>
{
	[FieldOffset(0)]
	public float X;
	[FieldOffset(4)]
	public float Y;

	public static readonly Vector2 One = new Vector2(1f, 1f);
	public static readonly Vector2 Zero = default;

	public Vector2(float value)
	{
		Y = value;
		X = value;
	}
	public Vector2(float x, float y)
	{
		X = x;
		Y = y;
	}

	public float Length() => (float)Math.Sqrt(X * X + Y * Y);
	public float LengthSquared() => X * X + Y * Y;

	public static float Dot(Vector2 value1, Vector2 value2) => value1.X * value2.X + value1.Y * value2.Y;

	public void Normalize()
	{
		float num = X * X + Y * Y;
		float num2 = 1f / (float)Math.Sqrt(num);
		X *= num2;
		Y *= num2;
	}

	public static bool operator ==(Vector2 value1, Vector2 value2) => value1.X == value2.X && value1.Y == value2.Y;
	public static bool operator !=(Vector2 value1, Vector2 value2) => value1.X != value2.X || value1.Y != value2.Y;

	public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode();

	public override string ToString()
	{
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		return string.Format(currentCulture, "{{X:{0} Y:{1}}}", X.ToString(currentCulture), Y.ToString(currentCulture));
	}

	public override bool Equals(object obj)
	{
		bool result = false;
		if (obj is Vector2 vector)
			result = Equals(vector);
		return result;
	}
	public bool Equals(Vector2 other) => X == other.X && Y == other.Y;

	public static Vector2 operator +(Vector2 value1, Vector2 value2)
	{
		Vector2 result;
		result.X = value1.X + value2.X;
		result.Y = value1.Y + value2.Y;
		return result;
	}

	public static Vector2 operator /(Vector2 value1, Vector2 value2)
	{
		Vector2 result;
		result.X = value1.X / value2.X;
		result.Y = value1.Y / value2.Y;
		return result;
	}
	public static Vector2 operator /(Vector2 value1, float divider)
	{
		float num = 1f / divider;
		Vector2 result;
		result.X = value1.X * num;
		result.Y = value1.Y * num;
		return result;
	}
	public static Vector2 operator *(Vector2 value1, Vector2 value2)
	{
		Vector2 result;
		result.X = value1.X * value2.X;
		result.Y = value1.Y * value2.Y;
		return result;
	}
	public static Vector2 operator *(Vector2 value, float scaleFactor)
	{
		Vector2 result;
		result.X = value.X * scaleFactor;
		result.Y = value.Y * scaleFactor;
		return result;
	}
	public static Vector2 operator *(float scaleFactor, Vector2 value)
	{
		Vector2 result;
		result.X = value.X * scaleFactor;
		result.Y = value.Y * scaleFactor;
		return result;
	}
	public static Vector2 operator -(Vector2 value1, Vector2 value2)
	{
		Vector2 result;
		result.X = value1.X - value2.X;
		result.Y = value1.Y - value2.Y;
		return result;
	}
	public static Vector2 operator -(Vector2 value)
	{
		Vector2 result;
		result.X = -value.X;
		result.Y = -value.Y;
		return result;
	}
}
