using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class GameFieldOffsetTypeNameAttribute : Attribute
	{
		public string TypeName
		{
			get;
		}
		public GameFieldOffsetTypeNameAttribute(string TypeName)
		{
			this.TypeName = TypeName;
		}
	}
}
