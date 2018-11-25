using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse.AST
{
	public class Expr_MACRO : Expression
	{
		public string Name { get; set; }
		public Expr_MACRO(int off) : base(off)
		{

		}
	}
}
