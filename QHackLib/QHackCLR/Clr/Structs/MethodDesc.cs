using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr.Structs
{
	/// <summary>
	/// This struct is from runtime.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct MethodDesc
	{
		[Flags]
		public enum Flags3Enum : ushort
		{
			TokenRemainderMask = 0x3FFF,
			ValueTypeParametersWalked = 0x4000,
			DoesNotHaveEquivalentValuetypeParameters = 0x8000,
		}
		[Flags]
		public enum Flags2Enum : byte
		{
			enum_flag2_HasStableEntryPoint = 0x01,
			enum_flag2_HasPrecode = 0x02,
			enum_flag2_IsUnboxingStub = 0x04,
			// unused                                       = 0x08,
			enum_flag2_IsJitIntrinsic = 0x10,
			enum_flag2_IsEligibleForTieredCompilation = 0x20,
			enum_flag2_RequiresCovariantReturnTypeChecking = 0x40
			// unused                           = 0x80,
		};
		public enum MethodClassification : ushort
		{
			IL = 0, // IL
			FCall = 1, // FCall (also includes tlbimped ctor, Delegate ctor)
			NDirect = 2, // N/Direct
			EEImpl = 3, // special method; implementation provided by EE (like Delegate Invoke)
			Array = 4, // Array ECall
			Instantiated = 5, // Instantiated generic methods, including descriptors
							  // for both shared and unshared code (see InstantiatedMethodDesc)
			ComInterop = 6,
			Dynamic = 7, // for method desc with no metadata behind
			Mask = 0b0111
		};
		[Flags]
		public enum MethodDescClassification : ushort
		{
			// Method is IL, FCall etc., see MethodClassification above.
			Classification = 0x0007,
			ClassificationCount = Classification + 1,

			// Note that layout of code:MethodDesc::s_ClassificationSizeTable depends on the exact values
			// of mdcHasNonVtableSlot and mdcMethodImpl

			// Has local slot (vs. has real slot in MethodTable)
			HasNonVtableSlot = 0x0008,

			// Method is a body for a method impl (MI_MethodDesc, MI_NDirectMethodDesc, etc)
			// where the function explicitly implements IInterface.foo() instead of foo().
			MethodImpl = 0x0010,

			// Has slot for native code
			HasNativeCodeSlot = 0x0020,

			HasComPlusCallInfo = 0x0040,

			// Method is static
			Static = 0x0080,

			// unused                           = 0x0100,
			// unused                           = 0x0200,

			// Duplicate method. When a method needs to be placed in multiple slots in the
			// method table, because it could not be packed into one slot. For eg, a method
			// providing implementation for two interfaces, MethodImpl, etc
			Duplicate = 0x0400,

			// Has this method been verified?
			VerifiedState = 0x0800,

			// Is the method verifiable? It needs to be verified first to determine this
			Verifiable = 0x1000,

			// Is this method ineligible for inlining?
			NotInline = 0x2000,

			// Is the method synchronized
			Synchronized = 0x4000,

			// Does the method's slot number require all 16 bits
			RequiresFullSlotNumber = 0x8000
		};
		public enum Kind : byte
		{
			KindMask = 0x07,
			GenericMethodDefinition = 0x00,
			UnsharedMethodInstantiation = 0x01,
			SharedMethodInstantiation = 0x02,
			WrapperStubWithInstantiations = 0x03,
			EnCAddedMethod = 0x07,

			Unrestored = 0x08,

			HasComPlusCallInfo = 0x10, // this IMD contains an optional ComPlusCallInfo
		};
		public Flags3Enum Flags3AndTokenRemainder;
		public byte ChunkIndex;
		public Flags2Enum Flags2;
		public ushort SlotNumber;
		public ushort Flags;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct StoredSigMethodDesc
	{
		public ushort Flags3AndTokenRemainder;
		public byte ChunkIndex;
		public byte Flags2;
		public ushort SlotNumber;
		public ushort Flags;
		public nuint PSig;
		public nuint CSigANDExtendedFlags;
	}
}
