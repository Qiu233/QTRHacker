using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Interfaces
{
	public enum OptimizationTier : uint
	{
		OptimizationTier0,
		OptimizationTier1,
		OptimizationTier1OSR,
		OptimizationTierOptimized,
	}
}
