using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Structs
{
	/// <summary>
	/// This struct is from runtime.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct MethodTable
	{
		public readonly uint Flags;
		public readonly uint BaseSize;
		public readonly ushort Flags2;
		public readonly ushort Token;
		public readonly ushort NumVirtuals;
		public readonly ushort NumInterfaces;
		public readonly nuint ParentMethodTable;
		public readonly nuint LoaderModule;
		public readonly nuint WriteableData;

		public readonly nuint EEClass; //nuint CanonMT;
		public readonly nuint ElementTypeHnd; //nuint PerInstInfo;
											  //nuint MultipurposeSlot1;
		public readonly nuint InterfaceMap; //nuint MultipurposeSlot2;
	}
}
