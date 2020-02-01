using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRInjectionBase
{
	public enum HookType
	{
		BEFORE, AFTER
	}
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
		public HookType HookType
		{
			get;
		}
		public HookAttribute(string AssemblyName, string TargetType, string TargetMethodName, HookType hookType = HookType.BEFORE)
		{
			this.AssemblyName = AssemblyName;
			this.TargetType = TargetType;
			this.TargetMethodName = TargetMethodName;
			HookType = hookType;
		}
	}
}
