using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	public class InlineHook
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct InlineHookInfo
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string Name;
			public bool TimesLimit;
			public UInt32 Times;
		}
		private Context Context;
		public InlineHook(Context ctx)
		{
			Context = ctx;
		}
		public void Inject(string hookName, bool timesLimit, UInt32 times)
		{

		}
	}
}
