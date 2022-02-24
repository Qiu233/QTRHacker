using QHackCLR.DataTargets;
using QHackLib.Assemble;
using QHackLib.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	public class InlineHook : IDisposable
	{
		[StructLayout(LayoutKind.Sequential)]
		private unsafe struct HookInfo
		{
			public const int RAW_CODE_BYTES_LENGTH = 32;
			public static readonly int HeaderSize = sizeof(HookInfo);
			public static readonly int Offset_OnceFlag = (int)Marshal.OffsetOf<HookInfo>(nameof(OnceFlag));
			public static readonly int Offset_SafeFreeFlag = (int)Marshal.OffsetOf<HookInfo>(nameof(SafeFreeFlag));
			public static readonly int Offset_RawCodeLength = (int)Marshal.OffsetOf<HookInfo>(nameof(RawCodeLength));
			public static readonly int Offset_RawCodeBytes = (int)Marshal.OffsetOf<HookInfo>(nameof(RawCodeBytes));

			public nuint Address_Code => AllocBase + (uint)HeaderSize;

			public nuint Address_OnceFlag => AllocBase + (uint)Offset_OnceFlag;
			public nuint Address_SafeFreeFlag => AllocBase + (uint)Offset_SafeFreeFlag;
			public nuint Address_RawCodeLength => AllocBase + (uint)Offset_RawCodeLength;
			public nuint Address_RawCodeBytes => AllocBase + (uint)Offset_RawCodeBytes;

			public HookInfo(nuint allocBase, byte[] rawCodeBytes)
			{
				if (rawCodeBytes.Length > RAW_CODE_BYTES_LENGTH)
					throw new ArgumentOutOfRangeException(nameof(rawCodeBytes));
				AllocBase = allocBase;
				OnceFlag = 1;
				SafeFreeFlag = 0; //initially safe
				RawCodeLength = (uint)rawCodeBytes.Length;
				for (int i = 0; i < rawCodeBytes.Length; i++)
					RawCodeBytes[i] = rawCodeBytes[i];
			}

			public nuint AllocBase;
			public int OnceFlag;
			public int SafeFreeFlag;
			public uint RawCodeLength;
			public fixed byte RawCodeBytes[RAW_CODE_BYTES_LENGTH];
		}

		public MemoryAllocation MemoryAllocation { get; }
		public HookParameters Parameters { get; }
		public QHackContext Context { get; }
		public AssemblyCode Code { get; }

		private bool _IsDisposed = false;
		public bool IsDisposed => _IsDisposed;

		private readonly byte[] JmpHeadBytes;

		private InlineHook(QHackContext ctx, AssemblyCode code, HookParameters parameters)
		{
			Context = ctx;
			Code = code.Copy();
			MemoryAllocation = new MemoryAllocation(ctx);
			Parameters = parameters;

			byte[] headInstBytes = GetHeadBytes(Context.DataAccess.ReadBytes(Parameters.TargetAddress, 32));

			nuint allocAddr = MemoryAllocation.AllocationBase;
			nuint safeFreeFlagAddr = allocAddr + (uint)HookInfo.Offset_SafeFreeFlag;
			nuint onceFlagAddr = allocAddr + (uint)HookInfo.Offset_OnceFlag;
			nuint codeAddr = allocAddr + (uint)HookInfo.HeaderSize;
			nuint retAddr = Parameters.TargetAddress + (uint)headInstBytes.Length;

			HookInfo info = new(allocAddr, headInstBytes);

			Assembler assembler = new();
			assembler.Emit(DataHelper.GetBytes(info));//emit the header before runnable code
			assembler.Emit((Instruction)$"mov dword ptr [{safeFreeFlagAddr}],1");
			assembler.Emit(Parameters.IsOnce ? GetOnceCheckedCode(Code, onceFlagAddr) : Code);//once or not
			if (Parameters.RawCode)
				assembler.Emit(headInstBytes);//emit the raw code replaced by hook jmp
			assembler.Emit((Instruction)$"mov dword ptr [{safeFreeFlagAddr}],0");
			assembler.Emit((Instruction)$"jmp {retAddr}");

			Context.DataAccess.WriteBytes(allocAddr, assembler.GetByteCode(allocAddr));

			JmpHeadBytes = new byte[headInstBytes.Length];
			Array.Fill<byte>(JmpHeadBytes, 0x90);
			Assembler.Assemble($"jmp {codeAddr}", Parameters.TargetAddress).CopyTo(JmpHeadBytes, 0);
		}

		public static InlineHook Hook(QHackContext ctx, AssemblyCode code, HookParameters parameters)
		{
			var hook = new InlineHook(ctx, code, parameters);
			hook.Attach();
			return hook;
		}

		public bool IsAttached()
		{
			byte h = Context.DataAccess.Read<byte>(Parameters.TargetAddress);
			if (h != 0xE9)
				return false;
			nuint addr =
				Parameters.TargetAddress +
				(uint)(Context.DataAccess.Read<int>(Parameters.TargetAddress + 1)
				+ 5 - HookInfo.HeaderSize);
			return Context.DataAccess.Read<nuint>(addr) == addr;
		}

		/// <summary>
		/// Attaches the hook.
		/// Note, this method first checks if the hook is attached.
		/// </summary>
		/// <returns>true if attached successfully, otherwise false</returns>
		public bool Attach()
		{
			if (IsAttached())
				return false;
			Context.DataAccess.WriteBytes(Parameters.TargetAddress, JmpHeadBytes);
			return true;
		}

		/// <summary>
		/// Detaches the hook.<br/>
		/// You can reattach this hook before <see cref="Dispose"/> is called.<br/>
		/// Note, this method first checks if the hook is attached.
		/// </summary>
		/// <returns>true if detached successfully, otherwise false</returns>
		public unsafe bool Detach()
		{
			if (!IsAttached())
				return false;
			HookInfo info = GetHookInfo();
			byte[] bs = new byte[info.RawCodeLength];
			for (int i = 0; i < bs.Length; i++)
				bs[i] = info.RawCodeBytes[i];
			Context.DataAccess.WriteBytes(Parameters.TargetAddress, bs);
			return true;
		}

		private HookInfo GetHookInfo() => Context.DataAccess.Read<HookInfo>(
				Parameters.TargetAddress +
				(uint)(Context.DataAccess.Read<int>(Parameters.TargetAddress + 1)
				+ 5 - HookInfo.HeaderSize));

		/// <summary>
		/// Waits to detach until the code is executed at least once.<br/>
		/// Only available for hooks whose <see cref="HookParameters.IsOnce"/> is true.
		/// </summary>
		/// <returns>true if detached successfully</returns>
		public bool WaitToDetach()
		{
			if (!Parameters.IsOnce)
				throw new InvalidOperationException("Not a once hook.");
			HookInfo hook = GetHookInfo();
			while (Context.DataAccess.Read<int>(hook.Address_OnceFlag) != 0) { }
			return Detach();
		}

		public void WaitToDispose()
		{
			HookInfo hook = GetHookInfo();
			while (Context.DataAccess.Read<int>(hook.Address_SafeFreeFlag) != 0) { }
			Dispose();
		}

		/// <summary>
		/// Disposes this hook WITHOUT any check.<br/>
		/// Disposing a hook means to detach and then free memory regions the hook uses.<br/>
		/// It's extremely dangerous to call this method.
		/// Instead, you should call <see cref="WaitToDispose(int)"/>.
		/// </summary>
		public void Dispose()
		{
			lock (this)
			{
				if (_IsDisposed)
					return;
				GC.SuppressFinalize(this);
				Detach();
				MemoryAllocation.Dispose();
				_IsDisposed = true;
			}
		}

		private unsafe static byte[] GetHeadBytes(byte[] code)
		{
			fixed (byte* p = code)
			{
				byte* i = p;
				while (i - p < 5)
					i += Ldasm.GetInst(i, out _, false);
				return code[..(int)(i - p)];
			}
		}

		private static AssemblyCode GetOnceCheckedCode(AssemblyCode code, nuint onceFlagAddr)
		{
			AssemblySnippet result = AssemblySnippet.FromEmpty();
			result.Content.Add(Instruction.Create("cmp dword ptr [" + onceFlagAddr + "],0"));
			result.Content.Add(Instruction.Create("jle bodyEnd"));
			result.Content.Add(code);
			result.Content.Add(Instruction.Create("dec dword ptr [" + onceFlagAddr + "]"));
			result.Content.Add(Instruction.Create("bodyEnd:"));
			return result;
		}

		public static bool HookOnce(QHackContext Context, AssemblyCode code, nuint targetAddr, int timeout = 1000, uint size = 4096)
		{
			var hook = new InlineHook(Context, code, new HookParameters(targetAddr, size, true, true));
			if (!hook.Attach())
				return false;
			if (!Task.Run(() => hook.WaitToDetach()).Wait(timeout))
				return false;
			return Task.Run(() => hook.WaitToDispose()).Wait(timeout);
		}

		/// <summary>
		/// Unconditionally detaches and releases the hook.<br/>
		/// For when hook objects get lost.<br/>
		/// </summary>
		/// <param name="Context"></param>
		/// <param name="targetAddr"></param>
		/// <returns>false if the target is not a valid hook</returns>
		public unsafe static bool FreeHook(QHackContext Context, nuint targetAddr, bool forceRelease = true, int timeout = 1000)
		{
			byte h = Context.DataAccess.Read<byte>(targetAddr);
			if (h != 0xE9)
				return false;
			nuint addr = targetAddr +
				(uint)(Context.DataAccess.Read<int>(targetAddr + 1)
				+ 5 - HookInfo.HeaderSize);
			HookInfo hook = Context.DataAccess.Read<HookInfo>(addr);
			if (addr != hook.AllocBase)
				return false;

			byte[] bs = new byte[hook.RawCodeLength];
			for (int i = 0; i < bs.Length; i++)
				bs[i] = hook.RawCodeBytes[i];
			Context.DataAccess.WriteBytes(targetAddr, bs);
			nuint sa = hook.Address_SafeFreeFlag;
			bool result = Task.Run(() => { while (Context.DataAccess.Read<int>(sa) != 0) { } }).Wait(timeout);
			if (forceRelease || result)
			{
				NativeFunctions.VirtualFreeEx(Context.Handle, hook.AllocBase, 0);
				return true;
			}
			return false;
		}
	}
}
