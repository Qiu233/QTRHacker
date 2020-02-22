using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public class AddressHelper
	{
		public Context Context { get; }
		public ClrModule Module { get; }
		public string ModuleName { get => Module.Name; }
		public int this[string TypeName, string FunctionName]
		{
			get => GetFunctionAddress(TypeName, FunctionName);
		}
		public ILToNativeMap this[string TypeName, string FunctionName, int ILOffset]
		{
			get => GetFunctionInstruction(TypeName, FunctionName, ILOffset);
		}
		internal AddressHelper(Context ctx, ClrModule module)
		{
			Module = module;
			Context = ctx;
		}
		public ClrType GetClrType(string TypeName) => Module.GetTypeByName(TypeName);
		public ClrMethod GetClrMethod(string TypeName, string MethodName) => GetClrType(TypeName).Methods.First(t => t.Name == MethodName);
		public ClrMethod GetClrMethod(string TypeName, Func<ClrMethod, bool> filter) => GetClrType(TypeName).Methods.First(t => filter(t));
		public int GetFunctionAddress(string TypeName, string FunctionName) => (int)GetClrType(TypeName).Methods.First(t => t.Name == FunctionName).NativeCode;
		public int GetFunctionAddress(string TypeName, Func<ClrMethod, bool> filter) => (int)GetClrType(TypeName).Methods.First(t => filter(t)).NativeCode;
		public ILToNativeMap GetFunctionInstruction(string TypeName, string FunctionName, int ILOffset) => GetClrType(TypeName).Methods.First(t => t.Name == FunctionName).ILOffsetMap.First(t => t.ILOffset == ILOffset);
		public int GetStaticFieldAddress(string TypeName, string FieldName) => (int)GetClrType(TypeName).GetStaticFieldByName(FieldName).GetAddress(Module.AppDomains[0]);
		public int GetFieldOffset(string TypeName, string FieldName) => GetClrType(TypeName).Fields.First(t => t.Name == FieldName).Offset + 4;//to get true offset must +4

		public T GetStaticFieldValue<T>(string TypeName, string FieldName) where T : struct
		{
			var t = GetClrType(TypeName);
			int len = Marshal.SizeOf(typeof(T));
			byte[] bs = new byte[len];
			NativeFunctions.ReadProcessMemory(Context.Handle, (int)t.GetStaticFieldByName(FieldName).GetAddress(Module.AppDomains[0]), bs, len, 0);
			IntPtr ptr = Marshal.AllocHGlobal(len);
			Marshal.Copy(bs, 0, ptr, len);
			T result = Marshal.PtrToStructure<T>(ptr);
			Marshal.FreeHGlobal(ptr);
			return result;
		}
		public void SetStaticFieldValue<T>(string TypeName, string FieldName, T v) where T : struct
		{
			var t = GetClrType(TypeName);
			int len = Marshal.SizeOf(typeof(T));
			byte[] bs = new byte[len];
			IntPtr ptr = Marshal.AllocHGlobal(len);
			Marshal.StructureToPtr(v, ptr, false);
			Marshal.Copy(ptr, bs, 0, len);
			NativeFunctions.WriteProcessMemory(Context.Handle, (int)t.GetStaticFieldByName(FieldName).GetAddress(Module.AppDomains[0]), bs, len, 0);
			Marshal.FreeHGlobal(ptr);
		}

		public T GetInstanceFieldValue<T>(string TypeName, string FieldName, int obj) where T : struct
		{
			var t = GetClrType(TypeName);
			int len = Marshal.SizeOf(typeof(T));
			byte[] bs = new byte[len];
			NativeFunctions.ReadProcessMemory(Context.Handle, obj + t.GetFieldByName(FieldName).Offset + 4, bs, len, 0);
			IntPtr ptr = Marshal.AllocHGlobal(len);
			Marshal.Copy(bs, 0, ptr, len);
			T result = Marshal.PtrToStructure<T>(ptr);
			Marshal.FreeHGlobal(ptr);
			return result;
		}

		public void SetInstanceFieldValue<T>(string TypeName, string FieldName, int obj, T v) where T : struct
		{
			var t = GetClrType(TypeName);
			int len = Marshal.SizeOf(typeof(T));
			byte[] bs = new byte[len];
			IntPtr ptr = Marshal.AllocHGlobal(len);
			Marshal.StructureToPtr(v, ptr, false);
			Marshal.Copy(ptr, bs, 0, len);
			NativeFunctions.WriteProcessMemory(Context.Handle, obj + t.GetFieldByName(FieldName).Offset + 4, bs, len, 0);
			Marshal.FreeHGlobal(ptr);
		}
	}
}
