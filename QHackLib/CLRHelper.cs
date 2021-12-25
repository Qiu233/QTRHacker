using QHackCLR.Clr;
using QHackLib.QHackCLR.Clr.Structs;
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

		public ClrMethod GetClrMethod(string typeName, Func<ClrMethod, bool> filter) => GetClrType(typeName).MethodsInVTable.First(t => filter(t));

		public nuint GetFunctionAddress(string typeName, string FunctionName) => GetClrMethod(typeName, FunctionName).NativeCode;
		public nuint GetFunctionAddress(string typeName, Func<ClrMethod, bool> filter) => GetClrMethod(typeName, t => filter(t)).NativeCode;

		//public ILToNativeMap GetFunctionInstruction(string typeName, string FunctionName, int ILOffset) => GetClrType(typeName).MethodsInVTable.First(t => t.Name == FunctionName).ILOffsetMap.First(t => t.ILOffset == ILOffset);

		public ClrMethod GetClrMethodBySignature(string typeName, string signature) => GetClrMethod(typeName, m => m.Signature == signature);

		public nuint GetStaticFieldAddress(string typeName, string fieldName) => GetClrType(typeName).GetStaticFieldByName(fieldName).GetAddress();

		public uint GetFieldOffset(string typeName, string fieldName) => GetClrType(typeName).GetInstanceFieldByName(fieldName).Offset + 4;//+4 to get true offset

		public T GetStaticFieldValue<T>(string typeName, string fieldName) where T : unmanaged => Context.DataAccess.Read<T>(GetClrType(typeName).GetStaticFieldByName(fieldName).GetAddress());
		public void SetStaticFieldValue<T>(string typeName, string fieldName, T value) where T : unmanaged => Context.DataAccess.Write(GetClrType(typeName).GetStaticFieldByName(fieldName).GetAddress(), value);

		public T GetInstanceFieldValue<T>(string typeName, string fieldName, nuint obj) where T : unmanaged => Context.DataAccess.Read<T>(GetClrType(typeName).GetInstanceFieldByName(fieldName).GetAddress(obj));

		public void SetInstanceFieldValue<T>(string typeName, string fieldName, nuint obj, T value) where T : unmanaged => Context.DataAccess.Write<T>(GetClrType(typeName).GetInstanceFieldByName(fieldName).GetAddress(obj), value);

		public HackObject GetStaticHackObject(string typeName, string fieldName)
			=> new(Context, GetClrType(typeName).GetStaticFieldByName(fieldName).GetValue());

		public T GetStaticHackObjectValue<T>(string typeName, string fieldName) where T : unmanaged
			=> GetClrType(typeName).GetStaticFieldByName(fieldName).GetRawValue<T>();

		public void SetStaticHackObject<T>(string typeName, string fieldName, T value) where T : HackObject
		{
			ClrStaticField field = GetClrType(typeName).GetStaticFieldByName(fieldName);
			nuint addr = field.GetAddress();
			if (field.Type.IsPrimitive)
				Context.DataAccess.WriteBytes(addr, Context.DataAccess.ReadBytes(value.BaseAddress, (uint)(value.ClrType.BaseSize - sizeof(nuint) * 2)));
			else
				Context.DataAccess.Write(addr, value.BaseAddress);
		}

		public void SetStaticHackObjectValue<T>(string typeName, string fieldName, T value) where T : unmanaged
			=> Context.DataAccess.Write(GetClrType(typeName).GetStaticFieldByName(fieldName).GetAddress(), value);


	}
}
