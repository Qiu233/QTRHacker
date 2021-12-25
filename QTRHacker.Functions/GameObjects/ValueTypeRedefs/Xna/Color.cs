using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.ValueTypeRedefs.Xna
{
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct Color : IEquatable<Color>
	{
		[FieldOffset(0)]
		public byte A;
		[FieldOffset(1)]
		public byte B;
		[FieldOffset(2)]
		public byte G;
		[FieldOffset(3)]
		public byte R;

		[FieldOffset(0)]
		public uint PackedValue;

		public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{{R:{0} G:{1} B:{2} A:{3}}}", R, G, B, A);
		public override int GetHashCode() => PackedValue.GetHashCode();
		public override bool Equals(object obj) => obj is Color color && Equals(color);
		public bool Equals(Color other) => PackedValue.Equals(other.PackedValue);
		public static bool operator ==(Color a, Color b) => a.Equals(b);
		public static bool operator !=(Color a, Color b) => !a.Equals(b);
	}
}
