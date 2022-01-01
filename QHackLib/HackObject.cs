using QHackCLR.Common;
using QHackLib;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public static class HackObjectExtension
	{
		public static T GetValue<T>(this HackObject obj) where T : unmanaged
		{
			ClrType type = obj.InternalEntity.Type;
			if (type.IsObjectReference)
				throw new HackObject.HackObjectTypeException("Can't get value from ref types.", type.Name);
			return obj.Context.DataAccess.Read<T>(obj.BaseAddress);
		}
		public unsafe static void SetValue<T>(this HackObject obj, T value) where T : unmanaged
		{
			ClrType type = obj.InternalEntity.Type;
			if (type.IsObjectReference)
				throw new HackObject.HackObjectTypeException("Can't set value to ref types.", type.Name);
			obj.Context.DataAccess.Write(obj.BaseAddress, value);
		}
	}
	public class HackObject : DynamicObject, IEquatable<HackObject>
	{
		public QHackContext Context { get; }
		public AddressableTypedEntity InternalEntity { get; }

		public nuint BaseAddress => InternalEntity.Address;
		public ClrType ClrType => InternalEntity.Type;

		public HackObject(QHackContext context, AddressableTypedEntity clrObject)
		{
			Context = context;
			InternalEntity = clrObject;
		}

		public int GetArrayRank()
		{
			if (InternalEntity is not ClrObject iobj || !iobj.IsArray)
				throw new HackObjectTypeException($"Not an array.", InternalEntity.Type.Name);
			return iobj.Type.Rank;
		}

		public int GetArrayLength()
		{
			if (InternalEntity is not ClrObject iobj)
				throw new HackObjectTypeException($"Not a ref object.", InternalEntity.Type.Name);
			return iobj.GetLength();
		}

		public int GetArrayLength(int i)
		{
			if (InternalEntity is not ClrObject iobj || !iobj.IsArray)
				throw new HackObjectTypeException($"Not an array.", InternalEntity.Type.Name);
			if (iobj.Type.Rank < i)
				throw new HackObjectInvalidArgsException($"Not an {i} dimension array.");
			return iobj.GetLength(i);
		}

		public HackObject InternalGetIndex(object[] indexes)
		{
			if (InternalEntity is not ClrObject obj || !obj.IsArray)
				throw new HackObjectTypeException($"Not an array.", InternalEntity.Type.Name);
			if (!indexes.ToList().TrueForAll(t => t is int))
				throw new HackObjectInvalidArgsException("Invalid indexes, accepts only int[].");
			int[] _indexes = indexes.Select(t => (int)t).ToArray();
			uint size = obj.Type.ComponentSize;
			int rank = obj.Type.Rank;
			if (rank != _indexes.Length)
				throw new HackObjectInvalidArgsException($"Invalid indexes, rank not equal. Expected: {rank}");
			return new HackObject(Context, obj.GetArrayElement(_indexes));
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			result = InternalGetIndex(indexes);
			return true;
		}

		public void InternalSetIndex(object[] indexes, object value)
		{
			if (InternalEntity is not ClrObject iobj || !iobj.IsArray)
				throw new HackObjectTypeException($"Not an array.", InternalEntity.Type.Name);
			if (!indexes.ToList().TrueForAll(t => t is int))
				throw new HackObjectInvalidArgsException("Invalid indexes, accepts only int[].");
			Type valueType = value.GetType();
			int[] _indexes = indexes.Select(t => (int)t).ToArray();
			ClrType iobjType = iobj.Type;
			ClrType componentType = iobjType.ComponentType;
			int rank = iobjType.Rank;
			if (iobjType.Rank != _indexes.Length)
				throw new HackObjectInvalidArgsException($"Invalid indexes, rank not equal. Expected: {rank}");
			if (value is ClrObject obj)
			{
				if (obj.Type != componentType)
					throw new HackObjectTypeException($"Not the same ref type as {componentType.Name}.", obj.Type.Name);
				Context.DataAccess.Write(iobj.GetArrayElementAddress(_indexes), (int)obj.Address);
			}
			else if (value is ClrValue val)
			{
				Context.DataAccess.WriteBytes(iobj.GetArrayElementAddress(_indexes), Context.DataAccess.ReadBytes(val.Address, iobjType.ComponentSize));
			}
			else if (valueType.IsValueType)
			{
				Context.DataAccess.Write(iobj.GetArrayElementAddress(_indexes), value);
			}
			else
			{
				throw new HackObjectTypeException($"Value of ref type cannot be set to a object.", valueType.FullName);
			}
		}

		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			InternalSetIndex(indexes, value);
			return true;
		}

		private ClrInstanceField SearchFieldRecursively(ClrType type, string name)
		{
			var field = type.GetInstanceFieldByName(name);
			if (field is not null)
				return field;
			if (type.BaseType is null)
				return null;
			return SearchFieldRecursively(type.BaseType, name);
		}

		public HackObject InternalGetMember(string name)
		{
			ClrInstanceField field = SearchFieldRecursively(InternalEntity.Type, name);
			if (field is null)
				throw new ArgumentException("No such field", nameof(name));
			return new HackObject(Context, field.GetValue(InternalEntity));
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = InternalGetMember(binder.Name);
			return true;
		}

		public unsafe void InternalSetMember(string name, object value)
		{
			Type valueType = value.GetType();
			ClrInstanceField field = InternalEntity.Type.GetInstanceFieldByName(name);
			if (value is AddressableTypedEntity entity && entity.Type != field.Type)
				throw new HackObjectTypeException($"Not the same type as {field.Type.Name}.", entity.Type.Name);
			if (value is ClrObject obj)
			{
				if (obj.Type != field.Type)
					throw new HackObjectTypeException($"Not the same ref type as {field.Type.Name}.", obj.Type.Name);
				Context.DataAccess.Write(field.GetAddress(InternalEntity), obj.Address);
			}
			else if (value is ClrValue val)
			{
				Context.DataAccess.WriteBytes(field.GetAddress(InternalEntity),
					Context.DataAccess.ReadBytes(val.Address, field.Type.BaseSize - 2 * (uint)sizeof(nuint)));
			}
			else if (valueType.IsValueType)
			{
				Context.DataAccess.Write(field.GetAddress(InternalEntity), value);
			}
			else
			{
				throw new HackObjectTypeException($"Value of ref type cannot be set to a object.", valueType.FullName);
			}
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			InternalSetMember(binder.Name, value);
			return true;
		}


		public HackMethodCall GetMethodCall(string sig) => new HackMethod(Context, InternalEntity.Type.MethodsInVTable.First(t => t.Signature == sig)).Call(InternalEntity.Address);
		public HackMethodCall GetMethodCall(Func<ClrMethod, bool> filter) => new HackMethod(Context, InternalEntity.Type.MethodsInVTable.First(t => filter(t))).Call(InternalEntity.Address);

		public object InternalConvert(Type type)
		{
			if (!type.IsValueType)
				throw new HackObjectConvertException(type);
			return Context.DataAccess.Read(type, BaseAddress);
		}

		public T InternalConvert<T>() where T : unmanaged => Context.DataAccess.Read<T>(BaseAddress);
		public bool Equals(HackObject other) => InternalEntity.Equals(other?.InternalEntity);
		public override bool Equals(object obj) => Equals(obj as HackObject);
		public override int GetHashCode() => InternalEntity.GetHashCode();

		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			result = InternalConvert(binder.Type);
			return true;
		}


		public static bool operator ==(HackObject a, HackObject b)
		{
			if (a == null)
				return b == null;
			return a.Equals(b);
		}
		public static bool operator !=(HackObject a, HackObject b) => !(a == b);

		internal abstract class HackObjectException : Exception
		{
			public HackObjectException(string msg) : base(msg) { }
		}
		internal class HackObjectTypeException : HackObjectException
		{
			public HackObjectTypeException(string msg, string type) : base($"Type: {type}. {msg}") { }
		}
		internal class HackObjectConvertException : HackObjectException
		{
			public HackObjectConvertException(Type type) : base($"Cannot convert a hack object to ref type: {type}") { }
		}
		internal class HackObjectInvalidArgsException : HackObjectException
		{
			public HackObjectInvalidArgsException(string msg) : base(msg) { }
		}
	}
}
