using System;

namespace QHackCLR.Dac.Utils
{
	public readonly struct CLRDATA_ADDRESS : IEquatable<CLRDATA_ADDRESS>
	{
		public ulong Value { get; }
		public static implicit operator ulong(CLRDATA_ADDRESS cda) => cda.Value;
		public static implicit operator nuint(CLRDATA_ADDRESS cda) => (nuint)cda.Value;
		public static implicit operator CLRDATA_ADDRESS(nuint value) => new(value);
		public static implicit operator CLRDATA_ADDRESS(ulong value) => new(value);
		public static implicit operator CLRDATA_ADDRESS(nint value) => new((nuint)value);
		public CLRDATA_ADDRESS(ulong value) => Value = value;


		public static bool operator ==(CLRDATA_ADDRESS a, CLRDATA_ADDRESS b) => a.Value == b.Value;
		public static bool operator !=(CLRDATA_ADDRESS a, CLRDATA_ADDRESS b) => !(a == b);

		public override bool Equals(object obj) => Equals((CLRDATA_ADDRESS)obj);
		public bool Equals(CLRDATA_ADDRESS other) => this == other;
		public override int GetHashCode() => (int)Value;
		public override string ToString()
		{
			return $"0x{Value:X16}";
		}
	}
}
