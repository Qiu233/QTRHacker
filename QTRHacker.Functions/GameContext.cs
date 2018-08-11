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
	public class GameContext : IDisposable
	{
		private int My_Player_Address;


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

		}


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
