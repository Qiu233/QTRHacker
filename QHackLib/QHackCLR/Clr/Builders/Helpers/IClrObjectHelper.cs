using QHackCLR.Dac.Interfaces;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface IClrObjectHelper
	{
		ISOSDacInterface SOSDac { get; }
		ITypeFactory TypeFactory { get; }
		DataAccess DataAccess { get; }
	}
}
