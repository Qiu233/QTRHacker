using Keystone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	/// <summary>
	/// A thread-safe assembler for continuous emitting
	/// </summary>
	public sealed class Assembler
	{
		private readonly AssemblySnippet InternalData;
		public Assembler()
		{
			InternalData = AssemblySnippet.FromEmpty();
		}

		/// <summary>
		/// This method is thread safe.
		/// </summary>
		/// <param name="inst"></param>
		public void Emit(AssemblyCode inst)
		{
			lock (InternalData)
			{
				InternalData.Add(inst);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Emit(string inst) => Emit(Instruction.Create(inst));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(byte v) => Emit($".byte {v}");
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(sbyte v) => Emit($".byte {(byte)v}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(ushort v) => Emit($".word {v}");
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(short v) => Emit($".word {(ushort)v}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(uint v) => Emit($".int {v}");
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(int v) => Emit($".int {(uint)v}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(ulong v) => Emit($".long {v}");
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(long v) => Emit($".long {(ulong)v}");

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(float v) => Emit($".float {v}");
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void Emit(double v) => Emit($".double {v}");

		public void Emit(in ReadOnlySpan<byte> bs)
		{
			foreach (var elem in bs)
				Emit(elem);
		}

		public byte[] GetByteCode(nuint IP) => InternalData.GetByteCode(IP);

		public unsafe static byte[] Assemble(string code, nuint IP)
		{
			using Engine keystone = new(Keystone.Architecture.X86, IntPtr.Size == 4 ? Mode.X32 : Mode.X64) { ThrowOnError = true };
			EncodedData enc = keystone.Assemble(code, IP);
			return enc.Buffer;
		}
	}
}
