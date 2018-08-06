using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;

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
