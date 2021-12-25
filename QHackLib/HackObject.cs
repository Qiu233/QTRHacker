using QHackCLR.Clr;
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
			int len = Marshal.SizeOf<T>();
			if (len != type.BaseSize - 8)
				throw new HackObject.HackObjectSizeNotEqualException(type.BaseSize, (uint)len);
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
				uint size = iobjType.ComponentSize;
				if (val.Type.BaseSize - 8 != size)
					throw new HackObjectSizeNotEqualException(size, val.Type.BaseSize);
				byte[] data = Context.DataAccess.ReadBytes(val.Address, size);
				Context.DataAccess.WriteBytes(iobj.GetArrayElementAddress(_indexes), data);
			}
			else if (valueType.IsValueType)
			{
				int size = Marshal.SizeOf(valueType);
				if (size != iobjType.ComponentSize)
					throw new HackObjectSizeNotEqualException(iobjType.ComponentSize, (uint)size);
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

		public HackObject InternalGetMember(string name)
		{
			return new HackObject(Context, InternalEntity.GetFieldValue(name));
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = InternalGetMember(binder.Name);
			return true;
		}

		public void InternalSetMember(string name, object value)
		{
			Type valueType = value.GetType();
			ClrInstanceField field = InternalEntity.Type.GetInstanceFieldByName(name);
			if (value is AddressableTypedEntity entity && entity.Type != field.Type)
				throw new HackObjectTypeException($"Not the same type as {field.Type.Name}.", entity.Type.Name);
			if (value is ClrObject obj)
			{
				if (obj.Type != field.Type)
					throw new HackObjectTypeException($"Not the same ref type as {field.Type.Name}.", obj.Type.Name);
				Context.DataAccess.Write(field.GetAddress(InternalEntity.Address), (int)obj.Address);
			}
			else if (value is ClrValue val)
			{
				uint size = val.Type.BaseSize - 8;
				if (size != field.Type.BaseSize - 8)
					throw new HackObjectSizeNotEqualException(field.Type.BaseSize, size);
				byte[] data = Context.DataAccess.ReadBytes(val.Address, size);
				Context.DataAccess.WriteBytes(field.GetAddress(InternalEntity.Address), data);
			}
			else if (valueType.IsValueType)//except ClrObject/ClrValueType
			{
				int size = Marshal.SizeOf(valueType);
				if (size != field.Type.BaseSize - 8)
					throw new HackObjectSizeNotEqualException(field.Type.BaseSize, (uint)size);
				Context.DataAccess.Write(field.GetAddress(InternalEntity.Address), value);
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
		internal class HackObjectSizeNotEqualException : HackObjectException
		{
			public HackObjectSizeNotEqualException(uint expected, uint got) : base($"Size not equal. expected {expected}, however, got {got}.") { }
		}
	}
}
