namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public class Expr_Value : Expression
{
	public string Value { get; set; }
	public Expr_Value(int off) : base(off)
	{

	}
}
