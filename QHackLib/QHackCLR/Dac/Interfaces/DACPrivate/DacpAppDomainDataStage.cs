using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum DacpAppDomainDataStage : uint
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
		STAGE_CLOSED,
	}
}
