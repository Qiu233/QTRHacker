using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class GameFieldOffsetFieldNameAttribute : Attribute
	{
		public string FieldName
		{
			get;
		}
		public string TypeName
		{
			get;
		}
		/// <summary>
		/// 这里传入的TypeName的优先级高于所在类的GamePropertyOffsetTypeNameAttribute属性提供的TypeName
		/// 如果TypeName传入默认值(null)，则按照所在类的GamePropertyOffsetTypeNameAttribute属性提供的TypeName进行处理
		/// </summary>
		/// <param name="FieldName"></param>
		/// <param name="TypeName"></param>
		public GameFieldOffsetFieldNameAttribute(string FieldName, string TypeName = null)
		{
			this.FieldName = FieldName;
			this.TypeName = TypeName;
		}
	}
}
