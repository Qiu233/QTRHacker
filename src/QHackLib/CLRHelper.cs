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

		/// <summary>
		/// Find a method by name. Most Terraria methods are non-virtual and do not
		/// appear in the vtable, so declared metadata methods must be searched first.
		/// </summary>
		public ClrMethod GetClrMethod(string typeName, string methodName)
		{
			ClrType type = GetClrType(typeName);
			if (type != null)
			{
				ClrMethod method = FindMethod(type, t => t.Name == methodName);
				if (method != null)
					return method;
			}

			throw new IndexOutOfRangeException($"Method '{methodName}' not found in type '{typeName}'.");
		}

		public ClrMethod GetClrMethod(string typeName, Predicate<ClrMethod> filter)
		{
			ClrType type = GetClrType(typeName);
			if (type != null)
			{
				ClrMethod method = FindMethod(type, filter);
				if (method != null)
					return method;
			}

			throw new InvalidOperationException($"Method matching filter not found in type '{typeName}'.");
		}

		private static ClrMethod FindMethod(ClrType type, Predicate<ClrMethod> filter)
		{
			try
			{
				ClrMethod method = type.Methods.FirstOrDefault(t => filter(t));
				if (method != null)
					return method;
			}
			catch { }

			try
			{
				return type.MethodsInVTable.FirstOrDefault(t => filter(t));
			}
			catch
			{
				return null;
			}
		}

		public nuint GetFunctionAddress(string typeName, string FunctionName) => GetNativeCode(GetClrMethod(typeName, FunctionName));
		public nuint GetFunctionAddress(string typeName, Predicate<ClrMethod> filter) => GetNativeCode(GetClrMethod(typeName, t => filter(t)));

		//public ILToNativeMap GetFunctionInstruction(string typeName, string FunctionName, int ILOffset) => GetClrType(typeName).MethodsInVTable.First(t => t.Name == FunctionName).ILOffsetMap.First(t => t.ILOffset == ILOffset);

		public ClrMethod GetClrMethodBySignature(string typeName, string signature) => GetClrMethod(typeName, m => m.Signature == signature);

		public nuint GetNativeCode(ClrMethod method)
		{
			nuint addr = method.NativeCode;
			if (addr != 0 && addr != uint.MaxValue)
				return addr;
			return ClrMdNativeMethodResolver.Resolve(Context, method);
		}

		public nuint GetStaticFieldAddress(string typeName, string fieldName) => GetStaticFieldByName(typeName, fieldName).GetAddress();

		public uint GetInstanceFieldOffset(string typeName, string fieldName) => GetClrType(typeName).GetInstanceFieldByName(fieldName).Offset;

		public T GetStaticFieldValue<T>(string typeName, string fieldName) where T : unmanaged => Context.DataAccess.Read<T>(GetStaticFieldByName(typeName, fieldName).GetAddress());

		public void SetStaticFieldValue<T>(string typeName, string fieldName, T value) where T : unmanaged => Context.DataAccess.Write(GetStaticFieldByName(typeName, fieldName).GetAddress(), value);

		private ClrStaticField GetStaticFieldByName(string typeName, string fieldName)
		{
			var type = GetClrType(typeName);
			if (type is null)
				throw new ArgumentException($"No such type found: {typeName}");
			var field = type.GetStaticFieldByName(fieldName);
			if (field is null)
				throw new ArgumentException($"No such field found: {fieldName} of type {typeName}");
			return field;
		}

		public HackObject GetStaticHackObject(string typeName, string fieldName)
		{
			var field = GetStaticFieldByName(typeName, fieldName);
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
