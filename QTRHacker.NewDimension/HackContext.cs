using QHackLib;
using QHackLib.Utilities;
using QTRHacker.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.NewDimension
{
	public class HackContext
	{
		public static readonly string SignHeadAob = "F3B354B2F6314D5AB44D946B4962AE82";
		public const int SignSize = 1024 * 8;
		public static GameContext GameContext
		{
			get; set;
		}
		public static int SignHead
		{
			get; set;
		}
		public static void InitSign()
		{
			int s = AobscanHelper.Aobscan(GameContext.HContext, SignHeadAob);
			SignHead = s + 20;
			if (s != -1)
				return;
			int t = NativeFunctions.VirtualAllocEx(
				GameContext.HContext.Handle, 0, SignSize,
				NativeFunctions.AllocationType.Commit,
				NativeFunctions.MemoryProtection.ExecuteReadWrite);
			NativeFunctions.WriteProcessMemory(GameContext.HContext.Handle, t, AobscanHelper.GetHexCodeFromString(SignHeadAob), 16, 0);
			SignHead = t + 20;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns>未初始化返回-1</returns>
		public static int GetSignNumber()
		{
			if (SignHead == 0)
				return -1;
			int number = 0;
			NativeFunctions.ReadProcessMemory(GameContext.HContext.Handle, SignHead - 4, ref number, 4, 0);
			return number;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ID"></param>
		/// <returns>未初始化或数据未定义返回-1</returns>
		public static int GetSign(string ID)
		{
			if (SignHead == 0)
				return -1;
			byte[] content = new byte[1024 * 8];
			NativeFunctions.ReadProcessMemory(GameContext.HContext.Handle, SignHead, content, SignSize - 20, 0);//去掉头部的20个位置，这20个位置最后四个是数量(int)
			byte[] h = AobscanHelper.GetHexCodeFromString(ID);
			int j = AobscanHelper.Memmem(content, content.Length, h, h.Length);
			if (j == -1) return -1;
			return BitConverter.ToInt32(content, j + 16);
		}
		public static void SetSign(string ID, int v)
		{
			if (SignHead == 0)
				return;
			byte[] content = new byte[1024 * 8];
			NativeFunctions.ReadProcessMemory(GameContext.HContext.Handle, SignHead, content, SignSize - 20, 0);//去掉头部的20个位置，这20个位置最后四个是数量(int)
			byte[] h = AobscanHelper.GetHexCodeFromString(ID);
			int j = AobscanHelper.Memmem(content, content.Length, h, h.Length);
			if (j != -1)//-1说明没找到
			{
				NativeFunctions.WriteProcessMemory(GameContext.HContext.Handle, SignHead + j + 16, ref v, 4, 0);
			}
			else
			{
				int u = GetSignNumber();
				NativeFunctions.WriteProcessMemory(GameContext.HContext.Handle, SignHead + u * 20, h, 16, 0);//写入标记
				NativeFunctions.WriteProcessMemory(GameContext.HContext.Handle, SignHead + u * 20 + 16, ref v, 4, 0);//写入数据
				u++;
				NativeFunctions.WriteProcessMemory(GameContext.HContext.Handle, SignHead - 4, ref u, 4, 0);//长度+1
			}
		}
	}
}
