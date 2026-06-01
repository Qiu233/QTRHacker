using QHackLib.Memory;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace QTRHacker.Core;

internal sealed class RemotePatchState
{
	private const string SignatureText = "QTRHackerPatchState1456";
	private const int SignatureSize = 32;
	private const int ExpectedVersion = 2;
	private readonly GameContext context;
	private nuint baseAddress;

	public RemotePatchState(GameContext context)
	{
		this.context = context;
	}

	public bool IsInitialized => TryFind(false);

	public void EnsureInitialized()
	{
		if (!TryFind(true))
			throw new InvalidOperationException("QTRHacker.Patches shared state was not initialized.");
	}

	public int GetInt(Field field)
	{
		EnsureInitialized();
		return context.HContext.DataAccess.Read<int>(AddressOf(field));
	}

	public void SetInt(Field field, int value)
	{
		EnsureInitialized();
		context.HContext.DataAccess.Write(AddressOf(field), value);
	}

	public bool GetBool(Field field) => GetInt(field) != 0;

	public void SetBool(Field field, bool value) => SetInt(field, value ? 1 : 0);

	public float GetFloat(Field field)
	{
		EnsureInitialized();
		return context.HContext.DataAccess.Read<float>(AddressOf(field));
	}

	public void SetFloat(Field field, float value)
	{
		EnsureInitialized();
		context.HContext.DataAccess.Write(AddressOf(field), value);
	}

	private bool TryFind(bool retry)
	{
		if (baseAddress != 0 && IsValid(baseAddress))
			return true;

		byte[] signature = GetSignature();
		int attempts = retry ? 50 : 1;
		for (int i = 0; i < attempts; i++)
		{
			baseAddress = AobscanHelper.Aobscan(context.HContext.Handle, signature).FirstOrDefault(IsValid);
			if (baseAddress != 0)
				return true;
			if (retry)
				Thread.Sleep(100);
		}
		return false;
	}

	private bool IsValid(nuint address)
	{
		if (address == 0)
			return false;
		try
		{
			int version = context.HContext.DataAccess.Read<int>(address + SignatureSize);
			int initialized = context.HContext.DataAccess.Read<int>(address + SignatureSize + sizeof(int));
			return version == ExpectedVersion && initialized != 0;
		}
		catch
		{
			return false;
		}
	}

	private nuint AddressOf(Field field) => baseAddress + (uint)Marshal.OffsetOf<State>(field.ToString()).ToInt32();

	private static byte[] GetSignature()
	{
		byte[] signature = new byte[SignatureSize];
		Encoding.ASCII.GetBytes(SignatureText, signature);
		return signature;
	}

	public enum Field
	{
		Initialized,
		InfiniteLife,
		InfiniteMana,
		InfiniteOxygen,
		InfiniteMinion,
		InfiniteAmmo,
		InfiniteFlyTime,
		CreativeMenu,
		ImmuneToDebuffs,
		SlowFall,
		FastSpeed,
		SuperGrabRange,
		CoinPortalDropsBags,
		FishCratesOnly,
		BonusTwoSlots,
		HighLight,
		SuperRange,
		FastTileAndWallPlacingSpeed,
		MechanicalRuler,
		MechanicalLens,
		RightClickToTP,
		EnableAllRecipes,
		StrengthenVampireKnives,
		UnlockAllDuplicationsRequested,
		RevealTheWholeMapRequested,
		ToggleLanternNightRequested,
		UnlockAllDuplicationsCompleted,
		RevealTheWholeMapCompleted,
		ToggleLanternNightCompleted,
		LastErrorCode,
		AimBot_Mode,
		AimBot_TargetedPlayerIndex,
		AimBot_MaxDistance_NPC,
		AimBot_HostileNPCsOnly,
		AimBot_MaxDistance_Player,
		AimBot_HostilePlayersOnly,
		AutoFishing_Mode,
		AutoFishing_CratesOnly,
		AutoFishing_QuestItemsOnly
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private unsafe struct State
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
