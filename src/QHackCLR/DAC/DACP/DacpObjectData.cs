using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

public enum DacpObjectType { OBJ_STRING = 0, OBJ_FREE, OBJ_OBJECT, OBJ_ARRAY, OBJ_OTHER }
[StructLayout(LayoutKind.Sequential)]
public struct DacpObjectData
{
	public CLRDATA_ADDRESS MethodTable = 0;
	public DacpObjectType ObjectType = DacpObjectType.OBJ_STRING;
	public ulong Size = 0;
	public CLRDATA_ADDRESS ElementTypeHandle = 0;
	public CorElementType ElementType = CorElementType.ELEMENT_TYPE_END;
	public uint dwRank = 0;
	public ulong dwNumComponents = 0;
	public ulong dwComponentSize = 0;
	public CLRDATA_ADDRESS ArrayDataPtr = 0;
	public CLRDATA_ADDRESS ArrayBoundsPtr = 0;
	public CLRDATA_ADDRESS ArrayLowerBoundsPtr = 0;

	public CLRDATA_ADDRESS RCW = 0;
	public CLRDATA_ADDRESS CCW = 0;
	public DacpObjectData() { }
}
