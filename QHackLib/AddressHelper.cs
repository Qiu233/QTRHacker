using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public class AddressHelper
	{
		public Context Context { get; }
		public ClrModule Module { get; }
		public int this[string TypeName, string FunctionName]
		{
			get => GetFunctionAddress(TypeName, FunctionName);
		}
		internal AddressHelper(Context ctx, string subModuleName)
		{
			Context = ctx;
			Module = Context.Runtime.Modules.First(t => t.Name.EndsWith("\\" + subModuleName));
		}
		public int GetFunctionAddress(string TypeName, string FunctionName)
		{
			return (int)Module.GetTypeByName(TypeName).Methods.First(t => t.Name == FunctionName).NativeCode;
		}
		public int GetStaticFieldAddress(string TypeName, string FieldName)
		{
			return (int)Module.GetTypeByName(TypeName).GetStaticFieldByName(FieldName).GetAddress(Module.AppDomains[0]);
		}
	}
}
