using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QHackLib;

namespace QTRHacker.Functions
{
	public class GameContext
	{
		public Context HContext
		{
			get;
		}

		public int Main_Player_Array_Base
		{
			get;
		}

		private GameContext(int pid)
		{
			HContext = Context.Create(pid);
			int vvv = HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_LocalPlayer") + 1;
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			NativeFunctions.ReadProcessMemory(HContext.Handle, vvv, ref vvv, 4, 0);
			Main_Player_Array_Base = vvv;
		}

		public static GameContext OpenGame(int pid)
		{
			return new GameContext(pid);
		}
		public void Close()
		{
			HContext?.Close();
		}
	}
}
