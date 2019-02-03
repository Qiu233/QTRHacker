using Newtonsoft.Json;
using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	/// <summary>
	/// GameObject的实例是从GameContext中现取现用的。
	/// 不要保存任何GameObject的实例，任何时候都无法保证一个以前获得的GameObject实例仍然可用。
	/// </summary>
	public abstract class GameObject
	{
		/// <summary>
		/// 基址
		/// </summary>
		[JsonIgnore]
		public int BaseAddress
		{
			get;
		}
		/// <summary>
		/// 相关GameContext实例
		/// </summary>
		[JsonIgnore]
		public GameContext Context
		{
			get;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context">相关GameContext实例</param>
		/// <param name="bAddr">基址</param>
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
			v = BitConverter.ToInt64(bs, 0);
		}
		public void ReadFromOffset(int offset, out ulong v)
		{
			byte[] bs = new byte[8];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 8, 0);
			v = BitConverter.ToUInt64(bs, 0);
		}
		public void ReadFromOffset(int offset, out double v)
		{
			byte[] bs = new byte[8];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 8, 0);
			v = BitConverter.ToDouble(bs, 0);
		}
		public void ReadFromOffset(int offset, out int v)
		{
			byte[] bs = new byte[4];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
			v = BitConverter.ToInt32(bs, 0);
		}
		public void ReadFromOffset(int offset, out uint v)
		{
			byte[] bs = new byte[4];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
			v = BitConverter.ToUInt32(bs, 0);
		}
		public void ReadFromOffset(int offset, out float v)
		{
			byte[] bs = new byte[4];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
			v = BitConverter.ToSingle(bs, 0);
		}
		public void ReadFromOffset(int offset, out short v)
		{
			byte[] bs = new byte[2];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 2, 0);
			v = BitConverter.ToInt16(bs, 0);
		}
		public void ReadFromOffset(int offset, out ushort v)
		{
			byte[] bs = new byte[2];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 2, 0);
			v = BitConverter.ToUInt16(bs, 0);
		}
		public void ReadFromOffset(int offset, out byte v)
		{
			byte[] bs = new byte[1];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
			v = bs[0];
		}
		public void ReadFromOffset(int offset, out sbyte v)
		{
			byte[] bs = new byte[1];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
			v = (sbyte)bs[0];
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
		public void WriteFromOffset(int offset, ulong v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 8, 0);
		}
		public void WriteFromOffset(int offset, double v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 8, 0);
		}
		public void WriteFromOffset(int offset, int v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
		}
		public void WriteFromOffset(int offset, uint v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
		}
		public void WriteFromOffset(int offset, float v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 4, 0);
		}
		public void WriteFromOffset(int offset, short v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 2, 0);
		}
		public void WriteFromOffset(int offset, ushort v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 2, 0);
		}
		public void WriteFromOffset(int offset, byte v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
		}
		public void WriteFromOffset(int offset, sbyte v)
		{
			byte[] bs = BitConverter.GetBytes(v);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, BaseAddress + offset, bs, 1, 0);
		}

	}
}
