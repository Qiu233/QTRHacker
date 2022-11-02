namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public class Stmt_INSERT : Statement
{
	public string Name { get; set; }
	public Expr_BTuple Location { get; set; }
	public Expr_BTuple Speed { get; set; }
	public Stmt_INSERT(int off) : base(off)
	{

	}
}
