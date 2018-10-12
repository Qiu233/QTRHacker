using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Utilities;

namespace QTRHacker.Functions
{
	/// <summary>
	/// Terraria游戏上下文环境
	/// </summary>
	public class GameContext : IDisposable
	{
		public const int MaxItemTypes = 3930;
		public int My_Player_Address
		{
			get;
		}
		/// <summary>
		/// 相关的Context实例，不需要手动操作
		/// </summary>
		public Context HContext
		{
			get;
		}

		public int Player_Array_Address
		{
			get;
		}
		public int NPC_Array_Address
		{
			get;
		}
		public int Main_Map_Address
		{
			get;
		}
		public int Main_RefreshMap_Address
		{
			get;
		}
		public int MapFullScreen_Address
		{
			get;
		}
		public int MouseRight_Address
		{
			get;
		}
		public int MouseRightRelease_Address
		{
			get;
		}
		public int ScreenWidth_Address
		{
			get;
		}
		public int ScreenHeight_Address
		{
			get;
		}
		public int MapFullscreenPos_Address
		{
			get;
		}

		public int MouseScreen_X_Address
		{
			get;
		}
		public int MouseScreen_Y_Address
		{
			get;
		}
		public int MapFullScreenScale_Address
		{
			get;
		}
		public int Tile_Address
		{
			get;
		}
		public int NetMode_Address
		{
			get;
		}


		public int NetMode
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, NetMode_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, NetMode_Address, ref value, 4, 0);
			}
		}

		public int MapFullScreenScale
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullScreenScale_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullScreenScale_Address, ref value, 4, 0);
			}
		}

		public int MouseScreen_X
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseScreen_X_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseScreen_X_Address, ref value, 4, 0);
			}
		}
		public int MouseScreen_Y
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseScreen_Y_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseScreen_Y_Address, ref value, 4, 0);
			}
		}

		public float MapFullscreenPos_X
		{
			get
			{
				byte[] b = new byte[4];
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4, b, 4, 0);
				return BitConverter.ToSingle(b, 0);
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4, BitConverter.GetBytes(value), 4, 0);
			}
		}
		public float MapFullscreenPos_Y
		{
			get
			{
				byte[] b = new byte[4];
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4 + 4, b, 4, 0);
				return BitConverter.ToSingle(b, 0);
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullscreenPos_Address + 4 + 4, BitConverter.GetBytes(value), 4, 0);
			}
		}

		public int ScreenWidth
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, ScreenWidth_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, ScreenWidth_Address, ref value, 4, 0);
			}
		}

		public int ScreenHeight
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, ScreenHeight_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, ScreenHeight_Address, ref value, 4, 0);
			}
		}

		public bool MouseRightRelease
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseRightRelease_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseRightRelease_Address, ref value, 1, 0);
			}
		}

		public bool MouseRight
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MouseRight_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MouseRight_Address, ref value, 1, 0);
			}
		}


		public bool MapFullScreen
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, MapFullScreen_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, MapFullScreen_Address, ref value, 1, 0);
			}
		}


		public bool RefreshMap
		{
			get
			{
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, Main_RefreshMap_Address, ref v, 1, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, Main_RefreshMap_Address, ref value, 1, 0);
			}
		}


		public int MyPlayerIndex
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, My_Player_Address, ref v, 4, 0);
				return v;
			}
			set
			{
				NativeFunctions.WriteProcessMemory(HContext.Handle, My_Player_Address, ref value, 4, 0);
			}
		}

		public Tile2DArray Tile
		{
			get
			{
				return new Tile2DArray(this, Tile_Address);
			}
		}

		public PlayerArray Players
		{
			get
			{
				return new PlayerArray(this, Player_Array_Address);
			}
		}

		public NPCArray NPC
		{
			get
			{
				return new NPCArray(this, NPC_Array_Address);
			}
		}

		public Player MyPlayer
		{
			get
			{
				int v = 0;
				NativeFunctions.ReadProcessMemory(HContext.Handle, Player_Array_Address + 0x08 + 0x04 * MyPlayerIndex, ref v, 4, 0);
				return new Player(this, v);
			}
		}

		public WorldMap Map
		{
			get
			{
				return new WorldMap(this, Main_Map_Address);
			}
		}

		public string UUID
		{
			get
			{
				int v = AobscanHelper.Aobscan(HContext, "74 0d 8b 4e 0c 8b 01 8b 40 28 ff 50 10 eb 02") - 0x13;
				NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
				NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
				byte[] bs = new byte[(32 + 4) * 2];
				NativeFunctions.ReadProcessMemory(HContext.Handle, v + 8, bs, bs.Length, 0);
				return Encoding.Unicode.GetString(bs);
			}
			set
			{
				int v = AobscanHelper.Aobscan(HContext, "74 0d 8b 4e 0c 8b 01 8b 40 28 ff 50 10 eb 02") - 0x13;
				NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
				NativeFunctions.ReadProcessMemory(HContext.Handle, v, ref v, 4, 0);
				byte[] bs = Encoding.Unicode.GetBytes(value);
				NativeFunctions.WriteProcessMemory(HContext.Handle, v + 8, bs, (32 + 4) * 2, 0);
			}
		}

		public int MaxTilesX
		{
			get;
		}

		public int MaxTilesY
		{
			get;
		}

		public bool DayTime
		{
			get
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateMenu") + 0x5E;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateMenu") + 0x5E;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public bool FastForwardTime
		{
			get
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateSundial") + 2;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateSundial") + 2;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public bool PumpkinMoon
		{
			get
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateTime") + 0x1d;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateTime") + 0x1d;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public bool BloodMoon
		{
			get
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateTime") + 0x26;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateTime") + 0x26;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}
		public bool SnowMoon
		{
			get
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateTime") + 0x2d;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateTime") + 0x2d;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}


		public bool Eclipse
		{
			get
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.NPC::AI_007_TownEntities") + 0x48;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				bool v = false;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref v, 1, 0);
				return v;
			}
			set
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.NPC::AI_007_TownEntities") + 0x48;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, ref value, 1, 0);
			}
		}

		public double Time
		{
			get
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateMenu") + 0x52;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				var bs = new byte[8];
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, bs, 8, 0);
				return BitConverter.ToDouble(bs, 0);
			}
			set
			{
				int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::UpdateMenu") + 0x52;
				NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
				var bs = BitConverter.GetBytes(value);
				NativeFunctions.WriteProcessMemory(HContext.Handle, vvv, bs, 8, 0);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pid">游戏的进程ID</param>
		private GameContext(int pid)
		{
			HContext = Context.Create(pid);
			int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_LocalPlayer") + 1;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			Player_Array_Address = vvv;


			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_LocalPlayer") + 7;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			My_Player_Address = vvv;


			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::NPCAddHeight") + 6;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NPC_Array_Address = vvv;


			vvv = AobscanHelper.Aobscan(HContext, "75 08 8d 65 f4 5b 5e 5f 5d c3 a1");
			int b = vvv + 0xb;
			NativeFunctions.ReadProcessMemory(HContext.Handle, b, ref b, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, b, ref b, 4, 0);
			MaxTilesX = b;
			int c = vvv + 0x1c;
			NativeFunctions.ReadProcessMemory(HContext.Handle, c, ref c, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, c, ref c, 4, 0);
			MaxTilesY = c;


			vvv = AobscanHelper.Aobscan(HContext, "8b 40 04 8b 55 f0 8b 4d ec 2b 50 10 3b 50 08") - 4;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			Main_Map_Address = vvv;

			vvv = AobscanHelper.Aobscan(HContext, "01 8b 45 f0 8d 65 f4 5b 5e 5f 5d c2 04 00") - 4;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			Main_RefreshMap_Address = vvv;


			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::DrawInterface_16_MapOrMinimap") + 0x2f;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			MapFullScreen_Address = vvv;

			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::DrawInterface_41_InterfaceLogic4") + 5;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			MouseRight_Address = vvv;


			vvv = AobscanHelper.Aobscan(HContext, "89 75 e0 33 d2 89 55 ec 80 3d") + 0x1c;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			MouseRightRelease_Address = vvv;


			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_TextMaxLengthForScreen") + 1;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			ScreenWidth_Address = vvv;


			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_ShouldDrawInfoIconsHorizontally") + 0x17;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			ScreenHeight_Address = vvv;


			vvv = AobscanHelper.Aobscan(HContext, "ff ff d8 28 d9 18 8b 05") + 8;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			MapFullscreenPos_Address = vvv;

			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_MouseScreen") + 0x3;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			MouseScreen_X_Address = vvv;

			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_MouseScreen") + 0xf;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			MouseScreen_Y_Address = vvv;

			vvv = AobscanHelper.Aobscan(HContext, "d9 5d e0 eb 1d 83 3d") - 4;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			MapFullScreenScale_Address = vvv;


			vvv = AobscanHelper.Aobscan(HContext, "89 45 e0 89 4d f0 8b fa 8b 45 f0 8b d7") + 0xf;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			Tile_Address = vvv;
			
			vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_ShouldPVPDraw") + 0x2;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NetMode_Address = vvv;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pid">游戏的进程ID</param>
		/// <returns></returns>
		public static GameContext OpenGame(int pid)
		{
			return new GameContext(pid);
		}
		public void Close()
		{
			HContext?.Close();
		}

		public void Dispose()
		{
			Close();
		}


	}
}
