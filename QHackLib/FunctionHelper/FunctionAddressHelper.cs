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
		private static extern int InitCL(int pid, string moduleName);

		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern unsafe bool SearchFunctionByName(int instance, string fullName, void** addr, UInt32 times);

		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern int GetNumberFunction(int instance);

		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern void GetFunction(int instance, int index, StringBuilder name, out int address);

		public Dictionary<string, int> FunctionsAddress
		{
			get;
		}

		private int Instance = 0;

		private FunctionAddressHelper()
		{
			FunctionsAddress = new Dictionary<string, int>();
		}




		public static FunctionAddressHelper Initialize(int pid, string moduleName)
		{
			FunctionAddressHelper fah = new FunctionAddressHelper();
			fah.Instance = InitCL(pid, moduleName);
			StringBuilder sb = new StringBuilder(500);
			int addr = 0;
			int num = GetNumberFunction(fah.Instance);
			for (int i = 0; i < num; i++)
			{
				GetFunction(fah.Instance, i, sb, out addr);
				string s = sb.ToString();
				if (fah.FunctionsAddress.ContainsKey(s))
				{
					int kk = 1;
					while (true)
					{
						string ss = s + " * " + kk;
						if (!fah.FunctionsAddress.ContainsKey(ss))
						{
							fah.FunctionsAddress.Add(ss, addr);
							break;
						}
						kk++;
					}
				}
				else
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
