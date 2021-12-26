using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public abstract class AddressableTypedEntity : IEquatable<AddressableTypedEntity>
	{
		public abstract ClrType Type { get; }
		public abstract nuint OffsetBase { get; }
		public nuint Address { get; }
		public readonly IClrObjectHelper ObjectHelper;

		public DataAccess DataAccess => ObjectHelper.DataAccess;

		protected AddressableTypedEntity(IClrObjectHelper helper, nuint address)
		{
			ObjectHelper = helper;
			Address = address;
		}

		public T Read<T>(uint offset) where T : unmanaged => DataAccess.Read<T>(Address + offset);
		public void Write<T>(uint offset, T value) where T : unmanaged => DataAccess.Write(Address + offset, value);

		public unsafe AddressableTypedEntity GetFieldValue(string name)
		{
			ClrInstanceField field = Type.EnumerateInstanceFields().FirstOrDefault(t => t.Name == name) ??
				throw new ArgumentException("No such field", nameof(name));
			return field.GetValue(this);
		}
		public T GetFieldValue<T>(string name) where T : AddressableTypedEntity => GetFieldValue(name) as T;

		public bool Equals(AddressableTypedEntity other) => this == other;
		public override bool Equals(object obj) => Equals(obj as AddressableTypedEntity);
		public override int GetHashCode() => (int)Address;
		public static bool operator !=(AddressableTypedEntity a, AddressableTypedEntity b) => !(a == b);
		public static bool operator ==(AddressableTypedEntity a, AddressableTypedEntity b)
		{
			if (b is null)
				return a is null;
			return a.Equals(b);
		}
	}
}
