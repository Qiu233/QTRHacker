using QHackCLR.Common;
using QHackCLR.DAC.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpAppDomainStoreData
{
	public CLRDATA_ADDRESS sharedDomain = 0;
	public CLRDATA_ADDRESS systemDomain = 0;
	public uint DomainCount = 0;
	public DacpAppDomainStoreData()
	{

	}
}