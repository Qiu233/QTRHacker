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
	public class HackObject : HackEntity
	{
		public unsafe override nuint OffsetBase => BaseAddress + (uint)sizeof(nuint);

		public HackObject(QHackContext context, ClrType type, nuint address) : base(context, type, address)
		{
		}

		public int GetArrayRank() => Type.Rank;
		public int GetArrayLength() => Type.GetLength(BaseAddress);
		public int GetArrayLength(int i) => Type.GetLength(BaseAddress, i);


		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			int[] _indexes = indexes.Select(t => (int)t).ToArray();
			result = InternalGetIndex(_indexes);
			return true;
		}

		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			int[] _indexes = indexes.Select(t => (int)t).ToArray();
			InternalSetIndex(_indexes, value);
			return true;
		}

		public HackEntity InternalGetIndex(int[] indexes)
		{
			var type = Type.ComponentType;
			nuint addr = Type.GetArrayElementAddress(BaseAddress, indexes);
			if (type.IsObjectReference)
				return new HackObject(Context, type, Context.DataAccess.Read<nuint>(addr));
			return new HackValue(Context, type, addr);
		}

		public void InternalSetIndex(int[] indexes, object value)
		{
			Type valueType = value.GetType();
			ClrType iobjType = Type;
			nuint addr = iobjType.GetArrayElementAddress(BaseAddress, indexes);
			if (value is ClrObject obj)
				Context.DataAccess.Write(addr, obj.Address);
			else if (value is ClrValue val)
				Context.DataAccess.WriteBytes(addr, Context.DataAccess.ReadBytes(val.Address, iobjType.ComponentSize));
			else if (valueType.IsValueType)
				Context.DataAccess.Write(addr, value);
		}


		public HackMethodCall GetMethodCall(string sig) => new HackMethod(Context, Type.MethodsInVTable.First(t => t.Signature == sig)).Call(BaseAddress);
		public HackMethodCall GetMethodCall(Func<ClrMethod, bool> filter) => new HackMethod(Context, Type.MethodsInVTable.First(t => filter(t))).Call(BaseAddress);

	}
}
