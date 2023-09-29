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
internal struct DacpWorkRequestData
{
	public CLRDATA_ADDRESS Function = 0;
	public CLRDATA_ADDRESS Context = 0;
	public CLRDATA_ADDRESS NextWorkRequest = 0;
	public DacpWorkRequestData() { }
}

[StructLayout(LayoutKind.Sequential)]
internal struct DacpUsefulGlobalsData
{
	public CLRDATA_ADDRESS ArrayMethodTable = 0;
	public CLRDATA_ADDRESS StringMethodTable = 0;
	public CLRDATA_ADDRESS ObjectMethodTable = 0;
	public CLRDATA_ADDRESS ExceptionMethodTable = 0;
	public CLRDATA_ADDRESS FreeMethodTable = 0;
	public DacpUsefulGlobalsData() { }
};