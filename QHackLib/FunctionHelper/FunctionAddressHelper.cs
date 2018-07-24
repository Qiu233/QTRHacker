using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	public class FunctionAddressHelper
	{
		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern void InitCL(int pid, bool dotnet, string moduleName);

		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern unsafe bool SearchFunctionByName(string fullName, void** addr, UInt32 times);

		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern int GetNumberFunction();

		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern void GetFunction(int index, StringBuilder name, out int address);

		public Dictionary<string, int> FunctionsAddress
		{
			get;
		}

		private FunctionAddressHelper()
		{
			FunctionsAddress = new Dictionary<string, int>();
		}


		public static FunctionAddressHelper Initialize(int pid, string moduleName)
		{
			FunctionAddressHelper fah = new FunctionAddressHelper();
			InitCL(pid, true, moduleName);
			StringBuilder sb = new StringBuilder(500);
			int addr = 0;
			int num = GetNumberFunction();
			for (int i = 0; i < num; i++)
			{
				GetFunction(i, sb, out addr);
				string s = sb.ToString();
				if (!fah.FunctionsAddress.ContainsKey(s))
					fah.FunctionsAddress.Add(s, addr);
			}
			return fah;
		}

		public int GetFunctionAddress(string fullName)
		{
			return FunctionsAddress[fullName];
		}

	}
}
