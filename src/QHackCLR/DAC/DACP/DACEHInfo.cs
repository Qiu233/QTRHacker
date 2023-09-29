using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;
public enum EHClauseType { EHFault, EHFinally, EHFilter, EHTyped, EHUnknown }

[StructLayout(LayoutKind.Sequential)]
public struct DACEHInfo
{
	public EHClauseType clauseType = EHClauseType.EHFault;
	public CLRDATA_ADDRESS tryStartOffset = 0;
	public CLRDATA_ADDRESS tryEndOffset = 0;
	public CLRDATA_ADDRESS handlerStartOffset = 0;
	public CLRDATA_ADDRESS handlerEndOffset = 0;
	public uint isDuplicateClause = 0;
	public CLRDATA_ADDRESS filterOffset = 0;   // valid when clauseType is EHFilter
	public uint isCatchAllHandler = 0;     // valid when clauseType is EHTyped
	public CLRDATA_ADDRESS moduleAddr = 0;     // when == 0 mtCatch contains a MethodTable, when != 0 tokCatch contains a type token
	public CLRDATA_ADDRESS mtCatch = 0;        // the method table of the TYPED clause type
	public uint tokCatch = 0;               // the type token of the TYPED clause type
	public DACEHInfo() { }
}
