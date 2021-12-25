using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public unsafe sealed class ClrValue : AddressableTypedEntity
	{
		public override ClrType Type { get; }
		public ClrValue(ClrType type, nuint address) : base(type.ClrObjectHelper, address)
		{
			Type = type;
		}

		public override AddressableTypedEntity GetFieldValue(string name)
		{
			ClrInstanceField field = Type.EnumerateInstanceFields().FirstOrDefault(t => t.Name == name) ??
				throw new ArgumentException("No such field", nameof(name));
			return field.GetValue(Address);
		}

		/// <summary>
		/// You can get value from this entity as <typeparamref name="T"/> 
		/// only when sizeof(<typeparamref name="T"/>) is less than size of this Type.<br/>
		/// Otherwise an exception will be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public unsafe T GetValue<T>() where T : unmanaged
		{
			if (Type.UserSize > Marshal.SizeOf<T>())
				throw new InvalidOperationException("Size exceeded.");
			return Read<T>(0);
		}

		/// <summary>
		/// Reads bytes with unchecked size.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public byte[] ReadBytes(uint size) => DataAccess.ReadBytes(Address, size);

		/// <summary>
		/// Reads bytes with default size.
		/// </summary>
		/// <returns></returns>
		public byte[] ReadBytes() => DataAccess.ReadBytes(Address, (uint)(Type.BaseSize - sizeof(nuint)));
	}
}
