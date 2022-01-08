using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public abstract class AssemblyCode
	{
		public abstract string GetCode();
		public abstract byte[] GetByteCode(nuint ip);
		public abstract AssemblyCode Copy();
		public override string ToString() => GetCode();
	}
}
