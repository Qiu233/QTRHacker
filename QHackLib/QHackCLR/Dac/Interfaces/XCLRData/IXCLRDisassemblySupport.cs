using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("1F0F7134-D3F3-47DE-8E9B-C2FD358A2936")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDisassemblySupport
	{
		/*HResult SetTranslateAddrCallback([In] CDSTranslateAddrCB cb);
		HResult PvClientSet([In] void* pv);
		nuint CbDisassemble(ClrDataAddress , void* , nuint );
		nuint Cinstruction();
		int FSelectInstruction(nuint );
		nuint CchFormatInstr(char* , nuint );
		void PvClient();
		HResult SetTranslateFixupCallback([In] CDSTranslateFixupCB cb);
		HResult SetTranslateConstCallback([In] CDSTranslateConstCB cb);
		HResult SetTranslateRegrelCallback([In] CDSTranslateRegrelCB cb);
		int TargetIsAddress();*/
	}
}
