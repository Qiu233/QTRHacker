using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	/// <summary>
	/// 必须是非公开，静态的方法
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class CustomFunctionAttribute : Attribute
	{

		public CustomFunctionAttribute(int size = 1024) { }
	}
	public abstract class CustomFunctions
	{
		public Dictionary<string, int> Functions
		{
			get;
		}
		public Func<AssemblyCode> FunctionDelegate;
		public Context Context
		{
			get;
		}
		private void CreateFunctions(List<MethodInfo> fs, byte[] hash)
		{
			int i = NativeFunctions.VirtualAllocEx(Context.Handle, 0, 1024,
				NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);
			NativeFunctions.WriteProcessMemory(Context.Handle, i, hash, hash.Length, 0);
			i += hash.Length;
			int len = fs.Count;
			NativeFunctions.WriteProcessMemory(Context.Handle, i, ref len, 4, 0);
			i += 4;
			foreach (var s in fs)
			{
				int addr = NativeFunctions.VirtualAllocEx(Context.Handle, 0,
					(int)s.CustomAttributes.First(v => v.AttributeType == typeof(CustomFunctionAttribute)).ConstructorArguments[0].Value,
					NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);
				AssemblyCode code = (AssemblyCode)s.Invoke(null, null);
				var bCode = code.GetByteCode(addr);
				NativeFunctions.WriteProcessMemory(Context.Handle, addr, bCode, bCode.Length, 0);
				NativeFunctions.WriteProcessMemory(Context.Handle, i, ref addr, 4, 0);
				i += 4;
				Functions[s.Name] = addr;
			}
		}
		private void ReadFunctions(List<MethodInfo> fs, int tableAddr)
		{
			int i = tableAddr;
			int len = 0;
			NativeFunctions.ReadProcessMemory(Context.Handle, i, ref len, 4, 0);
			i += 4;
			foreach (var s in fs)
			{
				int addr = 0;
				NativeFunctions.ReadProcessMemory(Context.Handle, i, ref addr, 4, 0);
				Functions[s.Name] = addr;

				i += 4;
			}
		}
		public CustomFunctions(Context ctx)
		{
			Functions = new Dictionary<string, int>();
			var fs = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(
				f => f.CustomAttributes.Where(
					v => v.AttributeType == typeof(CustomFunctionAttribute)).Count() > 0).ToList();
			fs.Sort((a, b) => a.Name.CompareTo(b.Name));
			HashAlgorithm md5 = HashAlgorithm.Create("SHA256");
			byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(string.Concat(fs)));
			int addr = Utilities.AobscanHelper.Aobscan(ctx.Handle, hash);
			if (addr == -1)
				CreateFunctions(fs, hash);
			else
				ReadFunctions(fs, addr + hash.Length);
		}
	}
}
