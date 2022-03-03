using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST
{
	public class Stmt_FIXED : Statement
	{
		public Expr_BTuple RelativeLocation { get; set; }
		public Expr_BTuple RelativeSpeed { get; set; }
		public IEnumerable<Statement> Statements { get; set; }
		public Stmt_FIXED(int off) : base(off)
		{

		}
	}
}
