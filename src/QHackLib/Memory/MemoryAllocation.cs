using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Memory
{
	/// <summary>
	/// Represents a block of memory in target's process.
	/// </summary>
	public class MemoryAllocation : IDisposable
	{
		public QHackContext Context { get; }
		public nuint AllocationBase { get; }
		public uint AllocationSize { get; }

		public MemoryAllocation(QHackContext ctx, uint size = 0x1000)
		{
			Context = ctx;
			AllocationBase = Alloc(ctx.Handle, size);
			AllocationSize = size;
		}

		/// <summary>
		/// Do not use this.
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="base"></param>
		/// <param name="size"></param>
		internal MemoryAllocation(QHackContext ctx, nuint @base, uint size)
		{
			Context = ctx;
			AllocationBase = @base;
			AllocationSize = size;
		}

		public RemoteMemorySpan this[Range r]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				(int off, int len) = r.GetOffsetAndLength((int)AllocationSize);
				return new RemoteMemorySpan(Context, AllocationBase + (uint)off, len);
			}
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
			Free(Context.Handle, AllocationBase);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Write(byte[] data, uint length, uint offset) =>
			Context.DataAccess.Write(AllocationBase + offset, data, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Read(byte[] data, uint length, uint offset) =>
			Context.DataAccess.Read(AllocationBase + offset, data, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Write<T>(T val, uint offset) where T : unmanaged =>
			Context.DataAccess.Write(AllocationBase + offset, val);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Read<T>(out T val, uint offset) where T : unmanaged =>
			Context.DataAccess.Read(AllocationBase + offset, out val);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual T Read<T>(uint offset) where T : unmanaged =>
			Context.DataAccess.Read<T>(AllocationBase + offset);


		public static nuint Alloc(nuint handle, uint size) => NativeFunctions.VirtualAllocEx(handle, 0, size,
				NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);
		public static bool Free(nuint handle, nuint addr) => NativeFunctions.VirtualFreeEx(handle, addr, 0);
	}
}
