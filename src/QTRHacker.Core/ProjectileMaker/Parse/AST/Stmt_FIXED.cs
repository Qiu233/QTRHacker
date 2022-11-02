using System.Collections.Generic;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public class Stmt_FIXED : Statement
{
	public Expr_BTuple RelativeLocation { get; set; }
	public Expr_BTuple RelativeSpeed { get; set; }
	public IEnumerable<Statement> Statements { get; set; }
	public Stmt_FIXED(int off) : base(off)
	{

	}
}
