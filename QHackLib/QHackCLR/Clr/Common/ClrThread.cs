using QHackCLR.Clr.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public abstract class ClrThread : ClrEntity
	{
		protected ClrThread(ClrRuntime runtime, nuint handle) : base(handle)
		{
		}
	}
}
