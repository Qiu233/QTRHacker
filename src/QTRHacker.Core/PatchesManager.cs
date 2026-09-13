using QTRHacker.Core.GameObjects;
using System;
using QHackLib.Assemble;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace QTRHacker.Core;

public sealed class PatchesManager
{
	[StructLayout(LayoutKind.Sequential)]
	public struct STile
	{
		public ushort Type;
		public ushort Wall;
		public byte Liquid;
		public ushort STileHeader;
		public byte BTileHeader;
		public byte BTileHeader2;
		public byte BTileHeader3;
		public short FrameX;
		public short FrameY;

		public void Active(bool active)
		{
			if (active)
				STileHeader |= 32;
			else
				STileHeader = (ushort)(STileHeader & 0xFFDF);
		}
		public bool Active()
		{
			return (STileHeader & 0x20) == 0x20;
		}
		public int WallFrameX()
		{
			return (BTileHeader2 & 0xF) * 36;
		}
		public int WallFrameY()
		{
			return (BTileHeader3 & 7) * 36;
		}
	}
	public QHackLib.CLRHelper PatchHelper => Context.HContext.GetCLRHelper("QTRHacker.Patches");
	public GameContext Context { get; }
	private readonly object patchLock = new();
	private Task<bool> initialization;
	public PatchesManager(GameContext context)
	{
		Context = context;
	}

	public bool IsInitialized => PatchHelper != null;

	public void Init()
	{
		lock (patchLock)
		{
			if (Context.GameProcess.HasExited)
				throw new InvalidOperationException("Terraria has exited.");
			if (initialization == null && !IsInitialized)
			{
				// A dedicated worker also avoids thread-pool starvation when several
				// function buttons are waiting for this same initialization lock.
				initialization = Task.Factory.StartNew(() => Context.LoadAssemblyAsBytes(
					Path.Combine(AppContext.BaseDirectory, "QTRHacker.Patches.dll"), "QTRHacker.Patches.Boot"),
					CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			}
			if (initialization != null && !initialization.Wait(30000))
				throw new TimeoutException("Patch loading is still in progress. Resume the game and retry; the existing load will be reused.");
			if (initialization != null && !initialization.GetAwaiter().GetResult())
				throw new InvalidOperationException("Couldn't load patches");
			if (!IsInitialized)
				throw new InvalidOperationException("The patch assembly could not be found after loading it.");
			if (!PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.Boot", "Initialized"))
				throw new InvalidOperationException("Patch initialization failed. See QTRHacker.Patches.boot.log in the game directory.");
		}
	}

	public void SetGameplayFeature(GameplayFeature feature, bool enabled)
	{
		// Different function buttons run on different worker threads. Keep the
		// load, remote call and shared result read in the same critical section.
		lock (patchLock)
		{
			Init();
			const string type = "QTRHacker.Patches.GameplayPatches";
			if (PatchHelper.GetClrType(type)?.GetStaticFieldByName("RequestId") == null)
				throw new InvalidOperationException("The game has an older patch assembly loaded. Restart Terraria before using the updated features.");
			int previous = PatchHelper.GetStaticFieldValue<int>(type, "RequestId");
			if (previous != PatchHelper.GetStaticFieldValue<int>(type, "CompletedRequestId"))
				throw new InvalidOperationException("A gameplay feature request is still pending. Resume the game and try again.");
			int request = unchecked(previous + 1);
			PatchHelper.SetStaticFieldValue(type, "RequestedFeature", (int)feature);
			PatchHelper.SetStaticFieldValue(type, "RequestedEnabled", enabled);
			// Publish after the payload. The managed update callback publishes its
			// completion ID only after updating the flags and error fields.
			PatchHelper.SetStaticFieldValue(type, "RequestId", request);
			var wait = Stopwatch.StartNew();
			while (PatchHelper.GetStaticFieldValue<int>(type, "CompletedRequestId") != request)
			{
				if (Context.GameProcess.HasExited)
					throw new InvalidOperationException("Terraria exited while updating gameplay features.");
				if (wait.ElapsedMilliseconds > 15000)
					throw new TimeoutException("The game has not processed the feature request. Resume the game before retrying.");
				Thread.Sleep(10);
			}
			if (!PatchHelper.GetStaticFieldValue<bool>(type, "LastChangeSucceeded"))
			{
				var error = new GameString(Context, PatchHelper.GetStaticHackObject(type, "LastError"));
				throw new InvalidOperationException($"Couldn't update {feature}: {error}");
			}
		}
	}

	public GameObjectArray2DV<STile> WorldPainter_ClipBoard
		=> new(Context, PatchHelper.GetStaticHackObject("QTRHacker.Patches.WorldPainter", "ClipBoard"));

	public void SetAllRecipesEnabled(bool enabled)
	{
		Init();
		if (!Context.RunByHookUpdate(AssemblySnippet.FromClrCall(
			PatchHelper.GetFunctionAddress("QTRHacker.Patches.RecipeUnlocks", "SetEnabled"),
			true, null, null, null, new object[] { enabled })))
			throw new InvalidOperationException("Couldn't update recipe unlocks");
	}

	public bool InfiniteAmmo_Enabled
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.AmmoConsumption", "Enabled");
		set
		{
			Init();
			PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AmmoConsumption", "Enabled", value);
		}
	}

	public bool WorldPainter_EyeDropperActive
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.WorldPainter", "EyeDropperActive");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "EyeDropperActive", value);
	}
	public bool WorldPainter_BrushActive
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.WorldPainter", "BrushActive");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "BrushActive", value);
	}
	public bool WorldPainter_Loading
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.WorldPainter", "Loading");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "Loading", value);
	}
	public nuint WorldPainter_Buffer
	{
		get => PatchHelper.GetStaticFieldValue<nuint>("QTRHacker.Patches.WorldPainter", "Buffer");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.WorldPainter", "Buffer", value);
	}

	public bool AimBot_HostileNPCsOnly
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.AimBot", "HostileNPCsOnly");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "HostileNPCsOnly", value);
	}
	public bool AimBot_HostilePlayersOnly
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.AimBot", "HostilePlayersOnly");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "HostilePlayersOnly", value);
	}
	public float AimBot_MaxDistance_NPC
	{
		get => PatchHelper.GetStaticFieldValue<float>("QTRHacker.Patches.AimBot", "MaxDistance_NPC");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "MaxDistance_NPC", value);
	}
	public float AimBot_MaxDistance_Player
	{
		get => PatchHelper.GetStaticFieldValue<float>("QTRHacker.Patches.AimBot", "MaxDistance_Player");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "MaxDistance_Player", value);
	}
	public int AimBot_TargetedPlayerIndex
	{
		get => PatchHelper.GetStaticFieldValue<int>("QTRHacker.Patches.AimBot", "TargetedPlayerIndex");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "TargetedPlayerIndex", value);
	}
	/// <summary>
	/// This is an enum
	/// </summary>
	public int AimBot_Mode
	{
		get => PatchHelper.GetStaticFieldValue<int>("QTRHacker.Patches.AimBot", "Mode");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AimBot", "Mode", value);
	}

	public int AutoFishing_Mode
	{
		get => PatchHelper.GetStaticFieldValue<int>("QTRHacker.Patches.AutoFishing", "Mode");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AutoFishing", "Mode", value);
	}
	public bool AutoFishing_CratesOnly
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.AutoFishing", "CratesOnly");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AutoFishing", "CratesOnly", value);
	}
	public bool AutoFishing_QuestItemsOnly
	{
		get => PatchHelper.GetStaticFieldValue<bool>("QTRHacker.Patches.AutoFishing", "QuestItemsOnly");
		set => PatchHelper.SetStaticFieldValue("QTRHacker.Patches.AutoFishing", "QuestItemsOnly", value);
	}
}
