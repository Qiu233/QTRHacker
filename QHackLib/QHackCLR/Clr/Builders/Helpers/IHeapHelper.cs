using QHackCLR.Dac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface IHeapHelper
	{
		ISOSDacInterface SOSDac { get; }
		ITypeFactory TypeFactory { get; }
	}
}
