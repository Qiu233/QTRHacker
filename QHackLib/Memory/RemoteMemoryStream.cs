using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Memory
{
	public unsafe class RemoteMemoryStream
	{
		public QHackContext Context { get; }
		public nuint Base { get; }

		private uint _Position;
		public uint Position => _Position;

		public nuint IP
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Base + _Position;
		}

		public RemoteMemoryStream(QHackContext ctx, nuint @base, uint pos)
		{
			Context = ctx;
			Base = @base;
			_Position = pos;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Write(byte[] data, uint length)
		{
			if (!Context.DataAccess.Write(IP, data, length))
				return false;
			_Position += length;
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Read(byte[] data, uint length)
		{
			if (!Context.DataAccess.Read(IP, data, length))
				return false;
			_Position += length;
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Write<T>(T val) where T : unmanaged
		{
			if (!Context.DataAccess.Write(IP, val))
				return false;
			_Position += (uint)sizeof(T);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Read<T>(out T val) where T : unmanaged
		{
			if (!Context.DataAccess.Read(IP, out val))
				return false;
			_Position += (uint)sizeof(T);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Read<T>() where T : unmanaged
		{
			if (!Context.DataAccess.Read(IP, out T val))
				return default;
			_Position += (uint)sizeof(T);
			return val;
		}

		public nuint FakeManagedByteArray(byte[] data)
		{
			Write<nuint>(0);//sync block
			nuint ip = IP;
			Write(Context.Runtime.BaseClassLibrary.GetTypeByName("System.Byte").ClrHandle);//handle
			Write((nuint)data.Length);//length
			Write(data, (uint)data.Length);
			return ip;
		}
	}
}
