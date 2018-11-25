using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse.AST
{
	public class Stmt_RECT : Statement
	{
		public Expression Type { get; set; }
		public Expr_BTuple Unit { get; set; }
		public Expr_BTuple Location { get; set; }
		public Expr_BTuple Size { get; set; }
		public Expr_BTuple Speed { get; set; }
		public Stmt_RECT(int off) : base(off)
		{

		}
	}
}
