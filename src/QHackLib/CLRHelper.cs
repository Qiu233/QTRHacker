using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public unsafe class CLRHelper
	{
		public QHackContext Context { get; }
		public ClrModule Module { get; }
		public string ModuleName => Module.Name;
		public nuint this[string typeName, string FunctionName]
		{
			get => GetFunctionAddress(typeName, FunctionName);
		}
		/*public ILToNativeMap this[string typeName, string FunctionName, int ILOffset]
		{
			get => GetFunctionInstruction(typeName, FunctionName, ILOffset);
		}*/
		internal CLRHelper(QHackContext ctx, ClrModule module)
		{
			Module = module;
			Context = ctx;
		}
		public ClrType GetClrType(string typeName) => Module.GetTypeByName(typeName);

		public ClrMethod GetClrMethod(string typeName, string methodName)
		{
			ClrMethod[] methods = GetClrType(typeName).MethodsInVTable.Where(t => t.Name == methodName).ToArray();
			return methods[0];
		}

		public ClrMethod GetClrMethod(string typeName, Predicate<ClrMethod> filter) => GetClrType(typeName).MethodsInVTable.First(t => filter(t));

		public nuint GetFunctionAddress(string typeName, string FunctionName) => GetClrMethod(typeName, FunctionName).NativeCode;
		public nuint GetFunctionAddress(string typeName, Predicate<ClrMethod> filter) => GetClrMethod(typeName, t => filter(t)).NativeCode;

		//public ILToNativeMap GetFunctionInstruction(string typeName, string FunctionName, int ILOffset) => GetClrType(typeName).MethodsInVTable.First(t => t.Name == FunctionName).ILOffsetMap.First(t => t.ILOffset == ILOffset);

		public ClrMethod GetClrMethodBySignature(string typeName, string signature) => GetClrMethod(typeName, m => m.Signature == signature);

		public nuint GetStaticFieldAddress(string typeName, string fieldName) => GetClrType(typeName).GetStaticFieldByName(fieldName).GetAddress();

		public uint GetInstanceFieldOffset(string typeName, string fieldName) => GetClrType(typeName).GetInstanceFieldByName(fieldName).Offset;

		public T GetStaticFieldValue<T>(string typeName, string fieldName) where T : unmanaged => Context.DataAccess.Read<T>(GetClrType(typeName).GetStaticFieldByName(fieldName).GetAddress());
		public void SetStaticFieldValue<T>(string typeName, string fieldName, T value) where T : unmanaged => Context.DataAccess.Write(GetClrType(typeName).GetStaticFieldByName(fieldName).GetAddress(), value);

		public HackObject GetStaticHackObject(string typeName, string fieldName)
		{
			var field = GetClrType(typeName).GetStaticFieldByName(fieldName);
			if (field.Type.IsPrimitive)
				throw new ArgumentException("Primitive static field cannot be cast to HackObject.", nameof(fieldName));
			return new HackObject(Context, field.Type, field.GetRawValue<nuint>());
		}

		public void SetStaticHackObject(string typeName, string fieldName, HackObject o)
		{
			var field = GetClrType(typeName).GetStaticFieldByName(fieldName);
			if (field.Type.IsPrimitive)
				throw new ArgumentException("Primitive static field cannot be cast to HackObject.", nameof(fieldName));
			Context.DataAccess.Write(field.GetAddress(), o.BaseAddress);
		}
	}
}
