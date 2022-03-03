using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST
{
	public class Stmt_MACRO : Statement
	{
		public string Name { get; set; }
		public Expression Value { get; set; }
		public Stmt_MACRO(int off) : base(off)
		{

		}
	}
}
