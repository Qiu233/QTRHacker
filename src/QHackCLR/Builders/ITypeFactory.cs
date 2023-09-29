using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface ITypeFactory
{
	CLRType? GetCLRType(nuint typeHandle);
}
