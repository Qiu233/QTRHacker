using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public int GetFunctionAddress(string TypeName, string FunctionName)
		{
			return (int)Module.GetTypeByName(TypeName).Methods.First(t => t.Name == FunctionName).NativeCode;
		}
		public int GetFunctionAddress(string TypeName, Func<ClrMethod, bool> filter)
		{
			return (int)Module.GetTypeByName(TypeName).Methods.First(t => filter(t)).NativeCode;
		}
		public ILToNativeMap GetFunctionInstruction(string TypeName, string FunctionName, int ILOffset)
		{
			return Module.GetTypeByName(TypeName).Methods.First(t => t.Name == FunctionName).ILOffsetMap.First(t => t.ILOffset == ILOffset);
		}
		public int GetStaticFieldAddress(string TypeName, string FieldName)
		{
			return (int)Module.GetTypeByName(TypeName).GetStaticFieldByName(FieldName).GetAddress(Module.AppDomains[0]);
		}
		public int GetFieldOffset(string TypeName, string FieldName)
		{
			return Module.GetTypeByName(TypeName).Fields.First(t => t.Name == FieldName).Offset + 4;//to get true offset must +4
		}
	}
}
