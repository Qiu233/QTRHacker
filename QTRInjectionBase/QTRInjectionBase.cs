using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRInjectionBase
{
	public class QTRInjectionBase
	{
		public static void ForceJit()
		{
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				foreach (MethodInfo methodInfo in types[i].GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (!methodInfo.IsAbstract && !methodInfo.ContainsGenericParameters && methodInfo.GetMethodBody() != null)
					{
						RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);
					}
				}
			}
		}
		static QTRInjectionBase()
		{
			ForceJit();
			AppDomain.CurrentDomain.AssemblyResolve += QTRInjectionBase_AssemblyResolve;
		}

		private static Assembly QTRInjectionBase_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name == Assembly.GetExecutingAssembly().FullName)
				return Assembly.GetExecutingAssembly();
			return null;
		}
	}
}
