using QHackLib;
using QHackLib.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class RemoteSignsManager
	{
		private const int SIZE_SIGN = 1024 * 8;
		private const int SIZE_SIGN_HEADER = 20;
		private static readonly string SignHeadAob = "F3B354B2F6314D5AB44D946B4962AE82";

		private int BaseAddress
		{
			get;
			set;
		}

		public GameContext Context
		{
			get;
		}

		/// <summary>
		/// 4 bit
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int this[int index]
		{
			get
			{
				if (BaseAddress <= 0) return 0;
				int addr = BaseAddress + SIZE_SIGN_HEADER + index * 4;
				int v = 0;
				NativeFunctions.ReadProcessMemory(Context.HContext.Handle, addr, ref v, 4, 0);
				return v;
			}
			set
			{
				if (BaseAddress <= 0) return;
				int addr = BaseAddress + SIZE_SIGN_HEADER + index * 4;
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, addr, ref value, 4, 0);
			}
		}


		private RemoteSignsManager(GameContext ctx)
		{
			Context = ctx;
		}

		public static RemoteSignsManager CreateFromProcess(GameContext ctx)
		{
			RemoteSignsManager rsm = new RemoteSignsManager(ctx);
			rsm.InitializeSign();
			return rsm;
		}

		private void InitializeSign()
		{
			BaseAddress = AobscanHelper.Aobscan(Context.HContext.Handle, SignHeadAob);
			if (BaseAddress == -1)
			{
				BaseAddress = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, SIZE_SIGN, NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);
				byte[] hex = AobscanHelper.GetHexCodeFromString(SignHeadAob);
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress, hex, hex.Length, 0);
			}
		}
	}
}
