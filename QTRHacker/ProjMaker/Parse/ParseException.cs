using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ProjMaker.Parse
{
	public class ParseException : Exception
	{
		public int Offset
		{
			get;
		}
		public ParseException(string t, int offset) : base(t)
		{
			Offset = offset;
		}
	}
}
