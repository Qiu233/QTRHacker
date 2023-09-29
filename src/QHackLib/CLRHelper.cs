﻿using QHackCLR.Common;
using QHackCLR.Entities;
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
		public CLRModule Module { get; }
		public string ModuleName => Module.Name;
		public nuint this[string typeName, string FunctionName]
		{
			get => GetFunctionAddress(typeName, FunctionName);
		}
		/*public ILToNativeMap this[string typeName, string FunctionName, int ILOffset]
		{
			get => GetFunctionInstruction(typeName, FunctionName, ILOffset);
		}*/
		internal CLRHelper(QHackContext ctx, CLRModule module)
		{
			Module = module;
			Context = ctx;
		}
		public CLRType GetClrType(string typeName) => Module.GetTypeByName(typeName);

		public CLRMethod GetClrMethod(string typeName, string methodName)
		{
			CLRMethod[] methods = GetClrType(typeName).MethodsInVTable.Where(t => t.Name == methodName).ToArray();
			return methods[0];
		}

		public CLRMethod GetClrMethod(string typeName, Predicate<CLRMethod> filter) => GetClrType(typeName).MethodsInVTable.First(t => filter(t));

		public nuint GetFunctionAddress(string typeName, string FunctionName) => GetClrMethod(typeName, FunctionName).NativeCode;
		public nuint GetFunctionAddress(string typeName, Predicate<CLRMethod> filter) => GetClrMethod(typeName, t => filter(t)).NativeCode;

		//public ILToNativeMap GetFunctionInstruction(string typeName, string FunctionName, int ILOffset) => GetClrType(typeName).MethodsInVTable.First(t => t.Name == FunctionName).ILOffsetMap.First(t => t.ILOffset == ILOffset);

		public CLRMethod GetClrMethodBySignature(string typeName, string signature) => GetClrMethod(typeName, m => m.Signature == signature);

		public nuint GetStaticFieldAddress(string typeName, string fieldName) => GetStaticFieldByName(typeName, fieldName).Address;

		public uint GetInstanceFieldOffset(string typeName, string fieldName) => GetClrType(typeName).GetInstanceFieldByName(fieldName).Offset;

		public T GetStaticFieldValue<T>(string typeName, string fieldName) where T : unmanaged => Context.DataAccess.ReadValue<T>(GetStaticFieldByName(typeName, fieldName).Address);

		public void SetStaticFieldValue<T>(string typeName, string fieldName, T value) where T : unmanaged => Context.DataAccess.WriteValue(GetStaticFieldByName(typeName, fieldName).Address, value);

		private CLRStaticField GetStaticFieldByName(string typeName, string fieldName)
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
			Context.DataAccess.WriteValue(field.Address, o.BaseAddress);
		}
	}
}
