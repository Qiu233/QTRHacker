using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Core.GameObjects;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Core.GameObjects.Terraria.IO;
using QTRHacker.Core.GameObjects.Terraria.Map;

namespace QTRHacker.Core;

/// <summary>
/// The context of Terraria
/// </summary>
public class GameContext : IDisposable
{
	public nuint My_Player_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "myPlayer");

	public nuint Main_RefreshMap_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "refreshMap");
	public nuint MapFullScreen_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreen");
	public nuint MouseRight_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseRight");
	public nuint MouseRightRelease_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseRightRelease");
	public nuint ScreenWidth_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "screenWidth");
	public nuint ScreenHeight_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "screenHeight");
	public nuint MapFullscreenPos_Address => GameModuleHelper.GetStaticHackObject("Terraria.Main", "mapFullscreenPos").BaseAddress;

	public nuint MouseX_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseX");
	public nuint MouseY_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseY");
	public nuint TileTargetX_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Player", "tileTargetX");
	public nuint TileTargetY_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Player", "tileTargetY");
	public nuint MapFullScreenScale_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreenScale");
	public nuint NetMode_Address => GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "netMode");

	public GameObjectArrayV<bool> Debuff
		=> new(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "debuff"));

	public int NetMode
	{
		get => HContext.DataAccess.Read<int>(NetMode_Address);
		set => HContext.DataAccess.Write(NetMode_Address, value);
	}

	public float MapFullScreenScale
	{
		get => HContext.DataAccess.Read<float>(MapFullScreenScale_Address);
		set => HContext.DataAccess.Write(MapFullScreenScale_Address, value);
	}
	public int MouseX
	{
		get => HContext.DataAccess.Read<int>(MouseX_Address);
		set => HContext.DataAccess.Write(MouseX_Address, value);
	}
	public int MouseY
	{
		get => HContext.DataAccess.Read<int>(MouseY_Address);
		set => HContext.DataAccess.Write(MouseY_Address, value);
	}

	public int TileTargetX
	{
		get => HContext.DataAccess.Read<int>(TileTargetX_Address);
		set => HContext.DataAccess.Write(TileTargetX_Address, value);
	}
	public int TileTargetY
	{
		get => HContext.DataAccess.Read<int>(TileTargetY_Address);
		set => HContext.DataAccess.Write(TileTargetY_Address, value);
	}

	public unsafe GameObjects.ValueTypeRedefs.Xna.Vector2 MapFullscreenPos
	{
		get => HContext.DataAccess.Read<GameObjects.ValueTypeRedefs.Xna.Vector2>(MapFullscreenPos_Address + (uint)sizeof(nuint));
		set => HContext.DataAccess.Write(MapFullscreenPos_Address + (uint)sizeof(nuint), value);
	}

	public int ScreenWidth
	{
		get => HContext.DataAccess.Read<int>(ScreenWidth_Address);
		set => HContext.DataAccess.Write(ScreenWidth_Address, value);
	}

	public int ScreenHeight
	{
		get => HContext.DataAccess.Read<int>(ScreenHeight_Address);
		set => HContext.DataAccess.Write(ScreenHeight_Address, value);
	}

	public bool MouseRightRelease
	{
		get => HContext.DataAccess.Read<bool>(MouseRightRelease_Address);
		set => HContext.DataAccess.Write(MouseRightRelease_Address, value);
	}

	public bool MouseRight
	{
		get => HContext.DataAccess.Read<bool>(MouseRight_Address);
		set => HContext.DataAccess.Write(MouseRight_Address, value);
	}


	public bool MapFullScreen
	{
		get => HContext.DataAccess.Read<bool>(MapFullScreen_Address);
		set => HContext.DataAccess.Write(MapFullScreen_Address, value);
	}


	public bool RefreshMap
	{
		get => HContext.DataAccess.Read<bool>(Main_RefreshMap_Address);
		set => HContext.DataAccess.Write(Main_RefreshMap_Address, value);
	}


	public int MyPlayerIndex
	{
		get => HContext.DataAccess.Read<int>(My_Player_Address);
		set => HContext.DataAccess.Write(My_Player_Address, value);
	}

	public GameObjectArray2D<Tile> Tile
		=> new(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "tile"));

	public GameObjectArray<Player> Players
		=> new(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "player"));

	public Player MyPlayer => Players[MyPlayerIndex];

	public GameObjectArray<NPC> NPC
		=> new(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "npc"));

	public WorldMap Map
		=> new(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "Map"));

	public string UUID
	{
		get => new GameString(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "clientUUID"));
		set => GameModuleHelper.SetStaticHackObject("Terraria.Main", "clientUUID",
			GameString.New(this, value).TypedInternalObject);
	}

	public int MaxTilesX => GameModuleHelper.GetStaticFieldValue<int>("Terraria.Main", "maxTilesX");
	public int MaxTilesY => GameModuleHelper.GetStaticFieldValue<int>("Terraria.Main", "maxTilesY");

	public bool DayTime
	{
		get => GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "dayTime");
		set => GameModuleHelper.SetStaticFieldValue("Terraria.Main", "dayTime", value);
	}

	public bool FastForwardTime
	{
		get => GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "fastForwardTime");
		set => GameModuleHelper.SetStaticFieldValue("Terraria.Main", "fastForwardTime", value);
	}

	public bool PumpkinMoon
	{
		get => GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "pumpkinMoon");
		set => GameModuleHelper.SetStaticFieldValue("Terraria.Main", "pumpkinMoon", value);
	}

	public bool BloodMoon
	{
		get => GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "bloodMoon");
		set => GameModuleHelper.SetStaticFieldValue("Terraria.Main", "bloodMoon", value);
	}
	public bool SnowMoon
	{
		get => GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "snowMoon");
		set => GameModuleHelper.SetStaticFieldValue("Terraria.Main", "snowMoon", value);
	}
	public bool Eclipse
	{
		get => GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "eclipse");
		set => GameModuleHelper.SetStaticFieldValue("Terraria.Main", "eclipse", value);
	}

	public double Time
	{
		get => GameModuleHelper.GetStaticFieldValue<double>("Terraria.Main", "time");
		set => GameModuleHelper.SetStaticFieldValue("Terraria.Main", "time", value);
	}

	public WorldFileData ActiveWorldFileData
		=> new(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "ActiveWorldFileData"));

	public string GameDir => Path.GetDirectoryName(GameModuleHelper.Module.FileName);
	public string GameContentDir => Path.Combine(Path.GetDirectoryName(GameModuleHelper.Module.FileName), "Content");


	private readonly object LOCK_UPDATE = new();

	public QHackContext HContext { get; }

	public PatchesManager Patches
	{
		get;
	}

	private GameContext(Process process)
	{
		GameProcess = process;
		HContext = QHackContext.Create(process.Id);
		Patches = new PatchesManager(this);
	}

	/// <summary>
	/// This method would be blocked for a long time if there were any other code running by a hook on Update.
	/// </summary>
	/// <param name="codeToRun"></param>
	/// <param name="size"></param>
	/// <returns></returns>
	public RemoteThread RunOnManagedThread(AssemblyCode codeToRun, uint size = 0x1000)
	{
		using MemoryAllocation alloc = new(HContext, size);
		byte[] bs = Encoding.Unicode.GetBytes("System.Action");
		alloc.Write(bs, (uint)bs.Length, 0);
		alloc.Write<short>(0, (uint)bs.Length);
		RemoteThread re = RemoteThread.Create(HContext, codeToRun);

		RunByHookUpdate(AssemblySnippet.StartManagedThread(
				HContext,
				re.CodeAddress,
				alloc.AllocationBase));

		return re;
	}

	public bool RunByHookUpdate(AssemblyCode codeToRun, uint size = 0x1000)
	{
		System.Threading.Monitor.Enter(LOCK_UPDATE);
		bool v = InlineHook.HookOnce(
				HContext, codeToRun,
				GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update"), size);
		System.Threading.Monitor.Exit(LOCK_UPDATE);
		return v;
	}
	private CLRHelper _GameModuleHelper;
	public CLRHelper GameModuleHelper => _GameModuleHelper ??= HContext.CLRHelpers.First(t
			=> string.Equals(Path.GetFullPath(t.Key.FileName),
				Path.GetFullPath(GameProcess.MainModule.FileName),
				StringComparison.OrdinalIgnoreCase))
			.Value;

	public static GameContext OpenGame(Process process)
	{
		return new GameContext(process);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		HContext?.Dispose();
	}

	public Process GameProcess { get; }

	public void Flush()
	{
		HContext.Flush();
		_GameModuleHelper = null;// Invalidate so it will be loaded again.
	}

	public bool LoadAssembly(string assemblyFile, string typeName)
	{
		using MemoryAllocation alloc = new(HContext);
		var stream = new RemoteMemorySpan(HContext, alloc.AllocationBase, (int)alloc.AllocationSize).GetStream();
		nuint pLibAsmStr = stream.IP; stream.WriteWCHARArray(assemblyFile);
		nuint pTypeStr = stream.IP; stream.WriteWCHARArray(typeName);
		nuint loadFrom = HContext.BCLHelper.GetClrMethodBySignature("System.Reflection.Assembly",
			"System.Reflection.Assembly.LoadFrom(System.String)").NativeCode;
		nuint getType = HContext.BCLHelper.GetClrMethodBySignature("System.Reflection.Assembly",
			"System.Reflection.Assembly.GetType(System.String)").NativeCode;
		nuint createInstance = HContext.BCLHelper.GetClrMethodBySignature("System.Activator",
			"System.Activator.CreateInstance(System.Type)").NativeCode;

		var thCode = AssemblySnippet.FromCode(
			new AssemblyCode[] {
				AssemblySnippet.FromConstructString(HContext, pLibAsmStr),
				(Instruction)$"mov ecx,eax",
				(Instruction)$"call {loadFrom}",
				(Instruction)$"push eax",
				AssemblySnippet.FromConstructString(HContext, pTypeStr),
				(Instruction)$"mov edx,eax",
				(Instruction)$"pop ecx",
				(Instruction)$"call {getType}",
				(Instruction)$"mov ecx,eax",
				(Instruction)$"call {createInstance}",
		});
		bool result = Task.Run(() => RunOnManagedThread(thCode).WaitToDispose()).Wait(5000);
		Flush();
		return result;
	}
	public unsafe bool LoadAssemblyAsBytes(string assemblyFile, string typeName)
	{
		byte[] data = File.ReadAllBytes(assemblyFile);
		using MemoryAllocation alloc = new(HContext, (uint)data.Length + 64);
		var stream = new RemoteMemorySpan(HContext, alloc.AllocationBase, (int)alloc.AllocationSize).GetStream();
		nuint pArray = stream.FakeManagedByteArray(data);
		nuint pTypeStr = stream.IP; stream.WriteWCHARArray(typeName);

		nuint load = HContext.BCLHelper.GetClrMethodBySignature("System.Reflection.Assembly",
			"System.Reflection.Assembly.Load(Byte[])").NativeCode;
		nuint getType = HContext.BCLHelper.GetClrMethodBySignature("System.Reflection.Assembly",
			"System.Reflection.Assembly.GetType(System.String)").NativeCode;
		nuint createInstance = HContext.BCLHelper.GetClrMethodBySignature("System.Activator",
			"System.Activator.CreateInstance(System.Type)").NativeCode;

		var thCode = AssemblySnippet.FromCode(
			new AssemblyCode[] {
				(Instruction)$"mov ecx,{pArray}",
				(Instruction)$"call {load}",
				(Instruction)$"push eax",
				AssemblySnippet.FromConstructString(HContext, pTypeStr),
				(Instruction)$"mov edx,eax",
				(Instruction)$"pop ecx",
				(Instruction)$"call {getType}",
				(Instruction)$"mov ecx,eax",
				(Instruction)$"call {createInstance}",
		});
		bool result = Task.Run(() => RunOnManagedThread(thCode).WaitToDispose()).Wait(5000);
		Flush();
		return result;
	}

	public bool LoadAssembly(string assemblyFile)
	{
		using MemoryAllocation alloc = new(HContext);
		var stream = new RemoteMemorySpan(HContext, alloc.AllocationBase, (int)alloc.AllocationSize).GetStream();
		stream.WriteWCHARArray(assemblyFile);

		nuint loadFrom = HContext.BCLHelper.GetClrMethodBySignature("System.Reflection.Assembly",
			"System.Reflection.Assembly.LoadFrom(System.String)").NativeCode;

		var thCode = AssemblySnippet.FromCode(
			new AssemblyCode[] {
				AssemblySnippet.FromConstructString(HContext, alloc.AllocationBase),
				(Instruction)$"mov ecx,eax",
				(Instruction)$"call {loadFrom}",
		});
		bool result = Task.Run(() => RunOnManagedThread(thCode).WaitToDispose()).Wait(5000);
		Flush();
		return result;
	}

	public unsafe bool LoadAssemblyAsBytes(string assemblyFile)
	{
		byte[] data = File.ReadAllBytes(assemblyFile);
		using MemoryAllocation alloc = new(HContext, (uint)data.Length + 64);
		var stream = new RemoteMemorySpan(HContext, alloc.AllocationBase, (int)alloc.AllocationSize).GetStream();
		nuint pArray = stream.FakeManagedByteArray(data);
		nuint load = HContext.BCLHelper.GetClrMethodBySignature("System.Reflection.Assembly",
			"System.Reflection.Assembly.Load(Byte[])").NativeCode;

		var thCode = AssemblySnippet.FromCode(
			new AssemblyCode[] {
				(Instruction)$"mov ecx,{pArray}",
				(Instruction)$"call {load}",
		});
		bool result = Task.Run(() => RunOnManagedThread(thCode).WaitToDispose()).Wait(5000);
		Flush();
		return result;
	}
}
