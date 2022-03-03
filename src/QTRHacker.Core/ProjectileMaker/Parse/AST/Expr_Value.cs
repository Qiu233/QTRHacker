using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST
{
	public class Expr_Value : Expression
	{
		public string Value { get; set; }
		public Expr_Value(int off) : base(off)
		{

		}
	}
}
