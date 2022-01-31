using QHackLib;
using QHackLib.Memory;
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
		private const uint SIZE_SIGN = 1024 * 8;
		private const uint SIZE_SIGN_HEADER = 16;
		private const string SignHeadAob = "F3B354B2F6314D5AB44D946B4962AE82";

		private nuint BaseAddress;

		public GameContext Context { get; }

		/// <summary>
		/// 4 bytes
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public unsafe nint this[int index]
		{
			get => Context.HContext.DataAccess.Read<nint>(BaseAddress + SIZE_SIGN_HEADER + (uint)(index * sizeof(nint)));
			set => Context.HContext.DataAccess.Write(BaseAddress + SIZE_SIGN_HEADER + (uint)(index * sizeof(nint)), value);
		}


		private RemoteSignsManager(GameContext ctx) => Context = ctx;

		public static RemoteSignsManager CreateFromProcess(GameContext ctx)
		{
			RemoteSignsManager rsm = new(ctx);
			rsm.InitializeSign();
			return rsm;
		}

		private void InitializeSign()
		{
			byte[] hex = AobscanHelper.GetHexCodeFromString(SignHeadAob);
			BaseAddress = AobscanHelper.Aobscan(Context.HContext.Handle, hex)
							  .FirstOrDefault(t => true);
			if (BaseAddress == 0)
			{
				BaseAddress = MemoryAllocation.Alloc(Context.HContext.Handle, SIZE_SIGN);
				Context.HContext.DataAccess.WriteBytes(BaseAddress, hex);
			}
			BaseAddress += (uint)hex.Length;
		}
	}
}
