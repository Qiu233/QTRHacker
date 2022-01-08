using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public abstract class HackEntity : DynamicObject, IEquatable<HackEntity>
	{
		public QHackContext Context { get; }
		public ClrType Type { get; }
		public nuint BaseAddress { get; }

		public abstract nuint OffsetBase { get; }

		protected HackEntity(QHackContext ctx, ClrType type, nuint baseAddress)
		{
			Context = ctx;
			Type = type;
			BaseAddress = baseAddress;
		}
		protected static ClrInstanceField SearchFieldRecursively(ClrType type, string name)
		{
			var field = type.GetInstanceFieldByName(name);
			if (field is not null)
				return field;
			if (type.BaseType is null)
				return null;
			return SearchFieldRecursively(type.BaseType, name);
		}

		public unsafe HackEntity InternalGetMember(string name)
		{
			ClrInstanceField field = SearchFieldRecursively(Type, name);
			if (field is null)
				throw new ArgumentException($"No such field: {name} in ", nameof(name));

			nuint addr = field.GetAddress(OffsetBase);
			if (field.Type.IsObjectReference)
				return new HackObject(Context, field.Type, Context.DataAccess.Read<nuint>(addr));
			return new HackValue(Context, field.Type, addr);
		}

		public unsafe void InternalSetMember(string name, object value)
		{
			ClrInstanceField field = SearchFieldRecursively(Type, name);
			if (field is null)
				throw new ArgumentException($"No such field: {name} in ", nameof(name));

			Type valueType = value.GetType();
			nuint addr = field.GetAddress(OffsetBase);
			if (value is ClrObject obj)
				Context.DataAccess.Write(addr, obj.Address);
			else if (value is ClrValue val)
				Context.DataAccess.WriteBytes(addr,
					Context.DataAccess.ReadBytes(val.Address, field.Type.BaseSize - 2 * (uint)sizeof(nuint)));
			else if (valueType.IsValueType)
				Context.DataAccess.Write(addr, value);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = InternalGetMember(binder.Name);
			return true;
		}

		public bool Equals(HackEntity other)
		{
			if (Type == other?.Type)
				return BaseAddress == other?.BaseAddress;
			return false;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			InternalSetMember(binder.Name, value);
			return true;
		}

		public override bool Equals(object obj) => Equals(obj as HackEntity);
		public override int GetHashCode() => (int)BaseAddress;

		public static bool operator ==(HackEntity a, HackEntity b)
		{
			if (a == null)
				return b == null;
			return a.Equals(b);
		}
		public static bool operator !=(HackEntity a, HackEntity b) => !(a == b);

	}
}
