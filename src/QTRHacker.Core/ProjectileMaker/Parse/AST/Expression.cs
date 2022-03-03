using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileMaker.Parse.AST
{
	public abstract class Expression
	{
		public int Offset { get; }
		public Expression(int offset)
		{
			Offset = offset;
		}
	}
}
