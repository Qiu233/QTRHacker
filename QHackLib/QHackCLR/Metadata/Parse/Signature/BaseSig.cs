using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Metadata.Parse.Signature
{
	public unsafe class BaseSig
	{
		protected SigParser Parser { get; }
		protected BaseSig(SigParser parser) => Parser = parser;

	}
}
