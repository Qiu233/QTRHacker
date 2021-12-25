using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Builders.Helpers
{
	public interface ITypeFactory
	{
		ClrType GetClrType(nuint typeHandle);
	}
}
