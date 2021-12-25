using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpGCInterestingInfoData
	{
		public nuint InterestingDataPoints_0;
		public nuint InterestingDataPoints_1;
		public nuint InterestingDataPoints_2;
		public nuint InterestingDataPoints_3;
		public nuint InterestingDataPoints_4;
		public nuint InterestingDataPoints_5;
		public nuint InterestingDataPoints_6;
		public nuint InterestingDataPoints_7;
		public nuint InterestingDataPoints_8;
		public nuint CompactReasons_0;
		public nuint CompactReasons_1;
		public nuint CompactReasons_2;
		public nuint CompactReasons_3;
		public nuint CompactReasons_4;
		public nuint CompactReasons_5;
		public nuint CompactReasons_6;
		public nuint CompactReasons_7;
		public nuint CompactReasons_8;
		public nuint CompactReasons_9;
		public nuint CompactReasons_10;

		public nuint ExpandMechanisms_0;
		public nuint ExpandMechanisms_1;
		public nuint ExpandMechanisms_2;
		public nuint ExpandMechanisms_3;
		public nuint ExpandMechanisms_4;
		public nuint ExpandMechanisms_5;

		public nuint BitMechanisms_0;
		public nuint BitMechanisms_1;
		public nuint GlobalMechanisms_0;
		public nuint GlobalMechanisms_1;
		public nuint GlobalMechanisms_2;
		public nuint GlobalMechanisms_3;
		public nuint GlobalMechanisms_4;
		public nuint GlobalMechanisms_5;
	}
}
