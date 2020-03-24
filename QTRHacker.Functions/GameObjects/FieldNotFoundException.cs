using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public class FieldNotFoundException : Exception
	{
		public FieldNotFoundException(string text) : base(text)
		{

		}
	}
}
