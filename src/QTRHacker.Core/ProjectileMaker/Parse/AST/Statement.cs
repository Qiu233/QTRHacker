namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public abstract class Statement
{
	public int Offset { get; }
	public Statement(int offset)
	{
		Offset = offset;
	}
}
