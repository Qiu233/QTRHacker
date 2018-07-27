using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	/// <summary>
	/// You should always get an instance of GameObject from GameContext objects and never save it.
	/// Because no one can promise that the instance will still be valid later.
	/// </summary>
	public abstract class GameObject
	{
		public int BaseAddress
		{
			get;
		}
		public GameContext Context
		{
			get;
		}
		public GameObject(GameContext context, int bAddr)
		{
			Context = context;
			BaseAddress = bAddr;
		}
		public void ReadFromOffset(int offset, out bool v)
		{
			byte[] bs = new byte[1];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
			v = BitConverter.ToBoolean(bs, 0);
		}
		public void ReadFromOffset(int offset, out long v)
		{
			byte[] bs = new byte[8];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 8, 0);
			v = BitConverter.ToInt32(bs, 0);
		}
		public void ReadFromOffset(int offset, out int v)
		{
			byte[] bs = new byte[4];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
			v = BitConverter.ToInt32(bs, 0);
		}
		public void ReadFromOffset(int offset, out short v)
		{
			byte[] bs = new byte[2];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 2, 0);
			v = BitConverter.ToInt16(bs, 0);
		}
		public void ReadFromOffset(int offset, out byte v)
		{
			byte[] bs = new byte[2];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
			v = bs[0];
		}



		public void WriteFromOffset(int offset, bool v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
		}
		public void WriteFromOffset(int offset, long v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 8, 0);
		}
		public void WriteFromOffset(int offset, int v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
		}
		public void WriteFromOffset(int offset, short v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 2, 0);
		}
		public void WriteFromOffset(int offset, byte v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
		}
	}
}
