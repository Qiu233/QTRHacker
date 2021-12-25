using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Functions.GameObjects.Terraria.IO;
using QTRHacker.Functions.GameObjects.Terraria.Map;

namespace QTRHacker.Functions
{
	/// <summary>
	/// The context of Terraria
	/// </summary>
	public class GameContext : IDisposable
	{
		public nuint My_Player_Address
		{
			get;
		}
		public RemoteSignsManager Signs
		{
			get;
		}

		public QHackContext HContext
		{
			get;
		}

		public nuint Main_RefreshMap_Address
		{
			get;
		}
		public nuint MapFullScreen_Address
		{
			get;
		}
		public nuint MouseRight_Address
		{
			get;
		}
		public nuint MouseRightRelease_Address
		{
			get;
		}
		public nuint ScreenWidth_Address
		{
			get;
		}
		public nuint ScreenHeight_Address
		{
			get;
		}
		public nuint MapFullscreenPos_Address
		{
			get;
		}
		public nuint MapFullscreenPos_Pointer
		{
			get;
		}

		public nuint MouseX_Address
		{
			get;
		}
		public nuint MouseY_Address
		{
			get;
		}
		public nuint TileTargetX_Address
		{
			get;
		}
		public nuint TileTargetY_Address
		{
			get;
		}
		public nuint MapFullScreenScale_Address
		{
			get;
		}
		public nuint NetMode_Address
		{
			get;
		}

		public bool[] CachedDebuff
		{
			get;
		}

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

		public GameString UUID => new(this, GameModuleHelper.GetStaticHackObject("Terraria.Main", "clientUUID"));

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

		private GameContext(Process process)
		{
			GameProcess = process;
			HContext = QHackContext.Create(process.Id);

			My_Player_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "myPlayer");

			Main_RefreshMap_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "refreshMap");
			MapFullScreen_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreen");
			MouseRight_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseRight");
			MouseRightRelease_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseRightRelease");
			ScreenWidth_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "screenWidth");
			ScreenHeight_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "screenHeight");
			MapFullscreenPos_Pointer = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreenPos");

			MouseX_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseX");
			MouseY_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mouseY");

			MapFullScreenScale_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "mapFullscreenScale");

			NetMode_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Main", "netMode");

			TileTargetX_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Player", "tileTargetX");
			TileTargetY_Address = GameModuleHelper.GetStaticFieldAddress("Terraria.Player", "tileTargetY");

			var debuff = Debuff;
			CachedDebuff = new bool[debuff.Length];
			for (int i = 0; i < CachedDebuff.Length; i++)
				CachedDebuff[i] = debuff[i];

			Signs = RemoteSignsManager.CreateFromProcess(this);
		}

		public RemoteThread RunOnManagedThread(AssemblyCode codeToRun, uint size = 0x1000)
		{
			using MemoryAllocation alloc = new(HContext, size);
			byte[] bs = Encoding.Unicode.GetBytes("System.Action");
			alloc.Write(bs, (uint)bs.Length, 0);
			alloc.Write<short>(0, (uint)bs.Length);
			RemoteThread re = RemoteThread.Create(HContext, codeToRun);
			InlineHook.HookOnce(
				HContext,
				AssemblySnippet.StartManagedThread(
					HContext,
					re.CodeAddress,
					alloc.AllocationBase),
				GameModuleHelper.
				GetFunctionAddress("Terraria.Main", "DoUpdate")).Wait();
			return re;
		}


		public CLRHelper GameModuleHelper => HContext.CLRHelpers.First(t
			=> string.Equals(Path.GetFullPath(t.Key.GetFileName()),
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
	}
}
