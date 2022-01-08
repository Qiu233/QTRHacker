using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.ProjectileMaker.Parse.AST
{
	public class Expr_Binary : Expression
	{
		public string OPTR { get; set; }
		public Expression Left { get; set; }
		public Expression Right { get; set; }

		public Expr_Binary(int off) : base(off)
		{

		}
	}
}
