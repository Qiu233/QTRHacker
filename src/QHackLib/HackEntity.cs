using QHackCLR.Common;
using QHackCLR.Entities;
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
		public CLRType Type { get; }
		public nuint BaseAddress { get; }

		public abstract nuint OffsetBase { get; }

		protected HackEntity(QHackContext ctx, CLRType type, nuint baseAddress)
		{
			Context = ctx;
			Type = type;
			BaseAddress = baseAddress;
		}
		protected static CLRInstanceField SearchFieldRecursively(CLRType type, string name)
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
			CLRInstanceField field = SearchFieldRecursively(Type, name) ?? throw new ArgumentException($"No such field: {name} in ", nameof(name));
			nuint addr = field.GetAddress(OffsetBase);
			if (field.Type.IsObjectReference)
				return new HackObject(Context, field.Type, Context.DataAccess.ReadValue<nuint>(addr));
			return new HackValue(Context, field.Type, addr);
		}

		public unsafe void InternalSetMember(string name, object value)
		{
			CLRInstanceField field = SearchFieldRecursively(Type, name);
			if (field is null)
				throw new ArgumentException($"No such field: {name} in ", nameof(name));

			Type valueType = value.GetType();
			nuint addr = field.GetAddress(OffsetBase);
			if (value is HackObject hobj)
			{
				if (field.Type == hobj.Type)
					Context.DataAccess.WriteValue(addr, hobj.BaseAddress);
			}
			else if (value is HackValue hval)
			{
				if (field.Type == hval.Type)
					Context.DataAccess.WriteBytes(addr, 
						Context.DataAccess.ReadBytes(hval.OffsetBase, (int)hval.Type.DataSize));
			}
			else if (value is CLRObject obj)
				Context.DataAccess.WriteValue(addr, obj.Address);
			else if (value is CLRValue val)
				Context.DataAccess.WriteBytes(addr,
					Context.DataAccess.ReadBytes(val.Address, (int)field.Type.DataSize));
			else if (valueType.IsValueType)
				Context.DataAccess.WriteObject(addr, value);
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
