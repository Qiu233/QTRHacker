using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpMethodTableFieldData
{
	public ushort wNumInstanceFields = 0;
	public ushort wNumStaticFields = 0;
	public ushort wNumThreadStaticFields = 0;

	public CLRDATA_ADDRESS FirstField = 0; // If non-null, you can retrieve more

	public ushort wContextStaticOffset = 0;
	public ushort wContextStaticsSize = 0;
	public DacpMethodTableFieldData() { }
}
