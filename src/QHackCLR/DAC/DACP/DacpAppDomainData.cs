using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;
public enum DacpAppDomainDataStage
{
	STAGE_CREATING,
	STAGE_READYFORMANAGEDCODE,
	STAGE_ACTIVE,
	STAGE_OPEN,
	STAGE_UNLOAD_REQUESTED,
	STAGE_EXITING,
	STAGE_EXITED,
	STAGE_FINALIZING,
	STAGE_FINALIZED,
	STAGE_HANDLETABLE_NOACCESS,
	STAGE_CLEARED,
	STAGE_COLLECTED,
	STAGE_CLOSED
}
[StructLayout(LayoutKind.Sequential)]
public struct DacpAppDomainData
{
	public CLRDATA_ADDRESS AppDomainPtr = 0;
	public CLRDATA_ADDRESS AppSecDesc = 0;
	public CLRDATA_ADDRESS pLowFrequencyHeap = 0;
	public CLRDATA_ADDRESS pHighFrequencyHeap = 0;
	public CLRDATA_ADDRESS pStubHeap = 0;
	public CLRDATA_ADDRESS DomainLocalBlock = 0;
	public CLRDATA_ADDRESS pDomainLocalModules = 0;
	// The creation sequence number of this app domain (starting from 1)
	public uint dwId = 0;
	public uint AssemblyCount = 0;
	public uint FailedAssemblyCount = 0;
	public DacpAppDomainDataStage appDomainStage = DacpAppDomainDataStage.STAGE_CREATING;
	public DacpAppDomainData() { }
}
