using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Common;

[StructLayout(LayoutKind.Sequential)]
internal struct MethodTable
{
	public uint Flags;
	public uint BaseSize;
	public ushort Flags2;
	public ushort Token;
	public ushort NumVirtuals;
	public ushort NumInterfaces;
	public nuint ParentMethodTable;
	public nuint LoaderModule;
	public nuint WriteableData;

	public nuint EEClass; //nuint CanonMT;
	public nuint ElementTypeHnd; //nuint PerInstInfo;
						  //nuint MultipurposeSlot1;
	public nuint InterfaceMap; //nuint MultipurposeSlot2;
}
