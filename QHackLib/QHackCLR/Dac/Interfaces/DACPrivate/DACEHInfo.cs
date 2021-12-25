using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DACEHInfo
	{
		public EHClauseType ClauseType;
		public CLRDATA_ADDRESS TryStartOffset;
		public CLRDATA_ADDRESS TryEndOffset;
		public CLRDATA_ADDRESS HandlerStartOffset;
		public CLRDATA_ADDRESS HandlerEndOffset;
		public int IsDuplicateClause;
		public CLRDATA_ADDRESS FilterOffset;
		public int IsCatchAllHandler;
		public CLRDATA_ADDRESS ModuleAddr;
		public CLRDATA_ADDRESS MtCatch;
		public int TokCatch;
	}
}
