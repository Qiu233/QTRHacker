using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpAssemblyData
{
	public CLRDATA_ADDRESS AssemblyPtr = 0; //useful to have
	public CLRDATA_ADDRESS ClassLoader = 0;
	public CLRDATA_ADDRESS ParentDomain = 0;
	public CLRDATA_ADDRESS BaseDomainPtr = 0;
	public CLRDATA_ADDRESS AssemblySecDesc = 0;
	public uint isDynamic = 0;
	public uint ModuleCount = 0;
	public uint LoadContext = 0;
	public uint isDomainNeutral = 0; // Always false, preserved for backward compatibility
	public uint dwLocationFlags = 0;
	public DacpAssemblyData() { }
}
