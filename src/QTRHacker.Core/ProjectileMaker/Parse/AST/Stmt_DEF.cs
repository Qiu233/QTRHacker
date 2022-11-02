using System.Collections.Generic;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST;

public class Stmt_DEF : Statement
{
	public string Name { get; set; }
	public IEnumerable<Statement> Statements { get; set; }

	public Stmt_DEF(int off) : base(off)
	{

	}
}
