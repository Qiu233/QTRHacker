using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST
{
	public class Stmt_INSERT : Statement
	{
		public string Name { get; set; }
		public Expr_BTuple Location { get; set; }
		public Expr_BTuple Speed { get; set; }
		public Stmt_INSERT(int off) : base(off)
		{

		}
	}
}
