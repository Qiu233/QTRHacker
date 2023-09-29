using QHackCLR.Common;
using QHackCLR.DAC.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpThreadStoreData
{
	public uint threadCount = 0;
	public uint unstartedThreadCount = 0;
	public uint backgroundThreadCount = 0;
	public uint pendingThreadCount = 0;
	public uint deadThreadCount = 0;
	public CLRDATA_ADDRESS firstThread = 0;
	public CLRDATA_ADDRESS finalizerThread = 0;
	public CLRDATA_ADDRESS gcThread = 0;
	public uint fHostConfig = 0;
	public DacpThreadStoreData() { }
}