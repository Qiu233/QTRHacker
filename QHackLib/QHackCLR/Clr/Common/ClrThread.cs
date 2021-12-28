using QHackCLR.Clr.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public class ClrThread : ClrEntity
	{
		public ClrThread(nuint handle) : base(handle)
		{
		}
	}
}
