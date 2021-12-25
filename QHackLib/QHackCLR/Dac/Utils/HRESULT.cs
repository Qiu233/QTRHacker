using System;

namespace QHackCLR.Dac.Utils
{
	public struct HRESULT : IEquatable<HRESULT>
	{
		private readonly uint Value;

		public HRESULT(uint value) => Value = value;

		public bool IsOK() => Value == S_OK;
		public override int GetHashCode() => (int)Value;
		public bool Equals(HRESULT other) => this == other;
		public override bool Equals(object obj) => Equals((HRESULT)obj);

		public static bool operator ==(HRESULT a, HRESULT b) => a.Value == b.Value;
		public static bool operator !=(HRESULT a, HRESULT b) => !(a == b);
		public static implicit operator uint(HRESULT v) => v.Value;
		public static implicit operator bool(HRESULT v) => (int)(v.Value) >= 0;
		public static implicit operator HRESULT(uint v) => new(v);

		public const uint S_OK = 0;
		public const uint S_FALSE = 1;
		public const uint E_FAIL = 0x80004005u;
		public const uint E_INVALIDARG = 0x80070057u;
		public const uint E_NOTIMPL = 0x80004001u;
		public const uint E_NOINTERFACE = 0x80004002u;
	}
}
