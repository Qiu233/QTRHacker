using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QTRHacker.Contrast.Structs
{
	[StructLayout(LayoutKind.Sequential)]
	public struct STile
	{
		public ushort Type;
		public ushort Wall;
		public byte Liquid;
		public short STileHeader;
		public byte BTileHeader;
		public byte BTileHeader2;
		public byte BTileHeader3;
		public short FrameX;
		public short FrameY;

		public bool Active()
		{
			return (STileHeader & 0x20) == 0x20;
		}
	}
}
