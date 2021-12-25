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
			AllocationBase = ctx.DataAccess.AllocMemory(size);
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

		public MemorySpan this[Range r]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				(int off, int len) = r.GetOffsetAndLength((int)AllocationSize);
				return new MemorySpan(Context, AllocationBase + (uint)off, len);
			}
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
			Context.DataAccess.FreeMemory(AllocationBase);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Write(in ReadOnlySpan<byte> data, uint length, uint offset) =>
			Context.DataAccess.Write(AllocationBase + offset, data, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Read(in Span<byte> data, uint length, uint offset) =>
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
	}
}
