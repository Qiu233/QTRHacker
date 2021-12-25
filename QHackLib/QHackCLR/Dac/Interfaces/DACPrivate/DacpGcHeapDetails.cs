using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpGcHeapDetails
	{
		public CLRDATA_ADDRESS HeapAddr;
		public CLRDATA_ADDRESS Alloc_allocated;
		public CLRDATA_ADDRESS Mark_array;
		public CLRDATA_ADDRESS Current_c_gc_state;
		public CLRDATA_ADDRESS Next_sweep_obj;
		public CLRDATA_ADDRESS Saved_sweep_ephemeral_seg;
		public CLRDATA_ADDRESS Saved_sweep_ephemeral_start;
		public CLRDATA_ADDRESS Background_saved_lowest_address;
		public CLRDATA_ADDRESS Background_saved_highest_address;
		public DacpGenerationData Generation_table_0;
		public DacpGenerationData Generation_table_1;
		public DacpGenerationData Generation_table_2;
		public DacpGenerationData Generation_table_3;
		public CLRDATA_ADDRESS Ephemeral_heap_segment;
		public CLRDATA_ADDRESS Finalization_fill_pointers_0;
		public CLRDATA_ADDRESS Finalization_fill_pointers_1;
		public CLRDATA_ADDRESS Finalization_fill_pointers_2;
		public CLRDATA_ADDRESS Finalization_fill_pointers_3;
		public CLRDATA_ADDRESS Finalization_fill_pointers_4;
		public CLRDATA_ADDRESS Finalization_fill_pointers_5;
		public CLRDATA_ADDRESS Finalization_fill_pointers_6;
		public CLRDATA_ADDRESS Lowest_address;
		public CLRDATA_ADDRESS Highest_address;
		public CLRDATA_ADDRESS Card_table;
	}
}
