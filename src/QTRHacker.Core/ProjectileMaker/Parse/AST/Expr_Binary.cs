namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public class Expr_Binary : Expression
{
	public string OPTR { get; set; }
	public Expression Left { get; set; }
	public Expression Right { get; set; }

	public Expr_Binary(int off) : base(off)
	{

	}
}
