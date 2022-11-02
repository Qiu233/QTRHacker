namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public abstract class Expression
{
	public int Offset { get; }
	public Expression(int offset)
	{
		Offset = offset;
	}
}
