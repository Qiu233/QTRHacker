using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse.AST
{
	public abstract class Statement
	{
		public int Offset { get; }
		public Statement(int offset)
		{
			Offset = offset;
		}
	}
}
