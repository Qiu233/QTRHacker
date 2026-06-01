using System;
using System.Runtime.InteropServices;
using System.Security;

namespace QTRHacker.Patches
{
	internal static unsafe class PatchState
	{
		public const string SignatureText = "QTRHackerPatchState1456";
		public const int SignatureSize = 32;
		public const int Version = 2;
		public static readonly State* Shared;

		static PatchState()
		{
			Shared = (State*)VirtualAlloc(
				IntPtr.Zero,
				(UIntPtr)sizeof(State),
				AllocationType.Commit | AllocationType.Reserve,
				MemoryProtection.ExecuteReadWrite);
			if (Shared == null)
				throw new InvalidOperationException("Could not allocate QTRHacker patch state.");
			*Shared = default;
			for (int i = 0; i < SignatureText.Length && i < SignatureSize - 1; i++)
				Shared->Signature[i] = (byte)SignatureText[i];
			Shared->Version = Version;
			Shared->Initialized = 1;
			Shared->AimBot_TargetedPlayerIndex = -1;
			Shared->AimBot_MaxDistance_NPC = 9600f;
			Shared->AimBot_HostileNPCsOnly = 1;
			Shared->AimBot_MaxDistance_Player = 9600f;
			Shared->AimBot_HostilePlayersOnly = 1;
		}

		public static bool GetBool(int value) => value != 0;
		public static int SetBool(bool value) => value ? 1 : 0;

		[DllImport("kernel32.dll", SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		private static extern void* VirtualAlloc(
			IntPtr lpAddress,
			UIntPtr dwSize,
			AllocationType flAllocationType,
			MemoryProtection flProtect);

		[Flags]
		private enum AllocationType : uint
		{
			Commit = 0x1000,
			Reserve = 0x2000
		}

		private enum MemoryProtection : uint
		{
			ExecuteReadWrite = 0x40
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public unsafe struct State
		{
			public fixed byte Signature[SignatureSize];
			public int Version;
			public int Initialized;

			public int InfiniteLife;
			public int InfiniteMana;
			public int InfiniteOxygen;
			public int InfiniteMinion;
			public int InfiniteAmmo;
			public int InfiniteFlyTime;
			public int CreativeMenu;
			public int ImmuneToDebuffs;
			public int SlowFall;
			public int FastSpeed;
			public int SuperGrabRange;
			public int CoinPortalDropsBags;
			public int FishCratesOnly;
			public int BonusTwoSlots;
			public int HighLight;
			public int SuperRange;
			public int FastTileAndWallPlacingSpeed;
			public int MechanicalRuler;
			public int MechanicalLens;
			public int RightClickToTP;
			public int EnableAllRecipes;
			public int StrengthenVampireKnives;

			public int UnlockAllDuplicationsRequested;
			public int RevealTheWholeMapRequested;
			public int ToggleLanternNightRequested;
			public int UnlockAllDuplicationsCompleted;
			public int RevealTheWholeMapCompleted;
			public int ToggleLanternNightCompleted;
			public int LastErrorCode;

			public int AimBot_Mode;
			public int AimBot_TargetedPlayerIndex;
			public float AimBot_MaxDistance_NPC;
			public int AimBot_HostileNPCsOnly;
			public float AimBot_MaxDistance_Player;
			public int AimBot_HostilePlayersOnly;

			public int AutoFishing_Mode;
			public int AutoFishing_CratesOnly;
			public int AutoFishing_QuestItemsOnly;
		}
	}
}
