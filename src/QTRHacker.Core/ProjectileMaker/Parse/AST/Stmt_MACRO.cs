namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public class Stmt_MACRO : Statement
{
	public string Name { get; set; }
	public Expression Value { get; set; }
	public Stmt_MACRO(int off) : base(off)
	{

	}
}
