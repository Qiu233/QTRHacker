namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public class Stmt_POINT : Statement
{
	public Expression Type { get; set; }
	public Expr_BTuple Location { get; set; }
	public Expr_BTuple Speed { get; set; }
	public Stmt_POINT(int off) : base(off)
	{

	}
}
