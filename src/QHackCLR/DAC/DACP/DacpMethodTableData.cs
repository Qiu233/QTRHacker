using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpMethodTableData
{
	public uint bIsFree = 0; // everything else is NULL if this is true.
	public CLRDATA_ADDRESS Module = 0;
	public CLRDATA_ADDRESS Class = 0;
	public CLRDATA_ADDRESS ParentMethodTable = 0;
	public ushort wNumInterfaces = 0;
	public ushort wNumMethods = 0;
	public ushort wNumVtableSlots = 0;
	public ushort wNumVirtuals = 0;
	public uint BaseSize = 0;
	public uint ComponentSize = 0;
	public uint cl = 0; // Metadata token
	public uint dwAttrClass = 0; // cached metadata
	public uint bIsShared = 0;  // Always false, preserved for backward compatibility
	public uint bIsDynamic = 0;
	public uint bContainsPointers = 0;
	public DacpMethodTableData() { }
}
