using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRInjectionBase
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class HookAttribute : Attribute
	{
		public string AssemblyName
		{
			get;
		}
		public string TargetType
		{
			get;
		}
		public string TargetMethodName
		{
			get;
		}
		public bool PassArguments
		{
			get;
			set;
		}
		public HookAttribute(string AssemblyName, string TargetType, string TargetMethodName)
		{
			this.AssemblyName = AssemblyName;
			this.TargetType = TargetType;
			this.TargetMethodName = TargetMethodName;
		}
	}
}
