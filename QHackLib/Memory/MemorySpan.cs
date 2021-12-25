using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Memory
{
	public unsafe readonly ref struct MemorySpan
	{
		public QHackContext Context { get; }
		public nuint Base { get; }
		public int Size { get; }

		/// <summary>
		/// Size is in bytes
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="base"></param>
		/// <param name="size"></param>
		public MemorySpan(QHackContext ctx, nuint @base, int size)
		{
			Context = ctx;
			Base = @base;
			Size = size;
		}

		/// <summary>
		/// The range works as index's range.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public MemorySpan this[Range r]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				(int offset, int len) = r.GetOffsetAndLength(Size);
				return new MemorySpan(Context, Base + (uint)offset, len);
			}
		}

		public MemoryStream GetStream(uint offset = 0) => new(Context, Base, offset);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Write(in ReadOnlySpan<byte> data, uint length, uint offset = 0) =>
			Context.DataAccess.Write(Base + offset, data, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Read(in Span<byte> data, uint length, uint offset = 0) =>
			Context.DataAccess.Read(Base + offset, data, length);
	}
}
