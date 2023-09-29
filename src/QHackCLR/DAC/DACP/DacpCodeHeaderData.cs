using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpCodeHeaderData
{
	public CLRDATA_ADDRESS GCInfo = 0;
	public JITTypes JITType = JITTypes.TYPE_UNKNOWN;
	public CLRDATA_ADDRESS MethodDescPtr = 0;
	public CLRDATA_ADDRESS MethodStart = 0;
	public uint MethodSize = 0;
	public CLRDATA_ADDRESS ColdRegionStart = 0;
	public uint ColdRegionSize = 0;
	public uint HotRegionSize = 0;
	public DacpCodeHeaderData() { }
}
