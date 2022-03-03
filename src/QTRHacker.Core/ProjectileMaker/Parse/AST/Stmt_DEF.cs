using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST
{
	public class Stmt_DEF : Statement
	{
		public string Name { get; set; }
		public IEnumerable<Statement> Statements { get; set; }

		public Stmt_DEF(int off) : base(off)
		{

		}
	}
}
