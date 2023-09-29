using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpDomainLocalModuleData
{
	public CLRDATA_ADDRESS appDomainAddr = 0;
	public ulong ModuleID = 0;

	public CLRDATA_ADDRESS pClassData = 0;
	public CLRDATA_ADDRESS pDynamicClassTable = 0;
	public CLRDATA_ADDRESS pGCStaticDataStart = 0;
	public CLRDATA_ADDRESS pNonGCStaticDataStart = 0;
	public DacpDomainLocalModuleData() { }
}
