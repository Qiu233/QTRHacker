using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DataTargets;

public unsafe readonly struct DataAccess
{
	public readonly nuint ProcessHandle;
	public DataAccess(nuint handle)
	{
		ProcessHandle = handle;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Read(nuint addr, byte* data, int length, out nuint bytesRead)
	{
		return NativeMethods.ReadProcessMemory(ProcessHandle, addr, data, (nuint)length, out bytesRead);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Read<T>(nuint addr, Span<T> data, out nuint bytesRead) where T : unmanaged
	{
		fixed (T* ptr = data)
			return Read(addr, (byte*)ptr, data.Length * sizeof(T), out bytesRead);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Read<T>(nuint addr, Span<T> data) where T : unmanaged => Read(addr, data, out _);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ReadValue<T>(nuint addr, out T value) where T : unmanaged
	{
		fixed (T* ptr = &value)
			return Read(addr, (byte*)ptr, sizeof(T), out _);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T ReadValue<T>(nuint addr) where T : unmanaged
	{
		ReadValue(addr, out T t);
		return t;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte[] ReadBytes(nuint addr, int bytes)
	{
		byte[] data = new byte[bytes];
		Read(addr, data.AsSpan());
		return data;
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Write(nuint addr, byte* data, int length, out nuint bytesWritten)
	{
		return NativeMethods.WriteProcessMemory(ProcessHandle, addr, data, (nuint)length, out bytesWritten);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Write<T>(nuint addr, ReadOnlySpan<T> data, out nuint bytesWritten) where T : unmanaged
	{
		fixed (T* ptr = data)
			return Write(addr, (byte*)ptr, data.Length * sizeof(T), out bytesWritten);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Write<T>(nuint addr, ReadOnlySpan<T> data) where T : unmanaged => Write(addr, data, out _);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool WriteValue<T>(nuint addr, T data) where T : unmanaged => Write(addr, (byte*)&data, sizeof(T), out _);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool WriteBytes(nuint addr, ReadOnlySpan<byte> data) => Write(addr, data);

	public object ReadObject(nuint addr, Type type)
	{
		if (!type.IsValueType)
			throw new InvalidOperationException("Not a ValueType");
		var method = typeof(DataAccess).GetMethod("ReadValue", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, new Type[] { typeof(nuint) })!.MakeGenericMethod(type);
		return method!.Invoke(this, new object[] { addr })!;
	}

	public void WriteObject(nuint addr, object value)
	{
		var type = value.GetType();
		if (!type.IsValueType)
			throw new InvalidOperationException("Not a ValueType");
		var method = typeof(DataAccess).GetMethod("WriteValue", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)!.MakeGenericMethod(type);
		method.Invoke(this, new object[] { addr, value });
	}
}
