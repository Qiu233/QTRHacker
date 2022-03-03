using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST
{
	public class Stmt_RECT_FILLED : Statement
	{
		public Expression Type { get; set; }
		public Expr_BTuple Unit { get; set; }
		public Expr_BTuple Location { get; set; }
		public Expr_BTuple Size { get; set; }
		public Expr_BTuple Speed { get; set; }
		public Stmt_RECT_FILLED(int off) : base(off)
		{

		}
	}
}
