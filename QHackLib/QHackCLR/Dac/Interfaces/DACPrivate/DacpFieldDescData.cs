using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpFieldDescData
	{
		public CorElementType Type;
		public CorElementType SigType;
		public CLRDATA_ADDRESS MTOfType;
		public CLRDATA_ADDRESS ModuleOfType;
		public int TypeToken;
		public int FieldToken;
		public CLRDATA_ADDRESS MTOfEnclosingClass;
		public uint DwOffset;
		public int BIsThreadLocal;
		public int BIsContextLocal;
		public int BIsStatic;
		public CLRDATA_ADDRESS NextField;
	}
}
