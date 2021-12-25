using QHackCLR.Dac.Utils;
using System;

namespace QHackCLR.Clr
{
	public abstract class ClrEntity : IClrHandled
	{
		/// <summary>
		/// Represents the handle to an entity in target process.<br/>
		/// i.e. MethodTable for Type, FieldDesc for Field, ImageBase for Module.<br/>
		/// WARNING: Platform Dependent
		/// </summary>
		public nuint ClrHandle { get; }
		protected ClrEntity(nuint handle)
		{
			ClrHandle = handle;
		}

		public override string ToString()
		{
			return string.Format($"{{0}}(0x{{1:X{IntPtr.Size * 2}}})", GetType().Name, ClrHandle);
		}


		public virtual bool Equals(IClrHandled other)
		{
			if (other is null)
				return false;
			return ClrHandle == other.ClrHandle;
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as IClrHandled);
		}
		public override int GetHashCode()
		{
			return (int)ClrHandle;
		}
		public static bool operator ==(ClrEntity a, ClrEntity b)
		{
			if (a is null)
				return b is null;
			return a.Equals(b);
		}
		public static bool operator !=(ClrEntity a, ClrEntity b)
		{
			return !(a == b);
		}
	}
}
