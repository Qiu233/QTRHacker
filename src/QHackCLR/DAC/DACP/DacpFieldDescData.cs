using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpFieldDescData
{
	public CorElementType Type = CorElementType.ELEMENT_TYPE_END;
	public CorElementType sigType = CorElementType.ELEMENT_TYPE_END;     // ELEMENT_TYPE_XXX from signature. We need this to disply pretty name for String in minidump's case
	public CLRDATA_ADDRESS MTOfType = 0; // NULL if Type is not loaded

	public CLRDATA_ADDRESS ModuleOfType = 0;
	public uint TokenOfType = 0;

	public uint mb = 0;
	public CLRDATA_ADDRESS MTOfEnclosingClass = 0;
	public uint dwOffset = 0;
	public uint bIsThreadLocal = 0;
	public uint bIsContextLocal = 0;
	public uint bIsStatic = 0;
	public CLRDATA_ADDRESS NextField = 0;
	public DacpFieldDescData() { }
}
