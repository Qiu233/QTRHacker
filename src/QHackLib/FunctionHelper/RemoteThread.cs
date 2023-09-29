﻿using QHackCLR.DataTargets;
using QHackLib.Assemble;
using QHackLib.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	public sealed class RemoteThread : IDisposable
	{
		[DllImport("kernel32.dll")]
		internal static extern nuint CreateRemoteThread(
			nuint hProcess,
			nuint lpThreadAttributes,
			int dwStackSize,
			nuint lpStartAddress, // in remote process
			nuint lpParameter,
			uint dwCreationFlags,
			out int lpThreadId
		);
		private readonly RemoteThreadHeader Header;

		/// <summary>
		/// Indicates whether the code memory can be safely released.<br/>
		/// Note that this 
		/// </summary>
		public nuint SafeFreeFlagAddress => Header.Address_SafeFreeFlag;
		public nuint CodeAddress => Header.Address_Code;

		public QHackContext Context
		{
			get;
		}
		public MemoryAllocation Allocation
		{
			get;
		}
		public int ThreadID
		{
			get;
			private set;
		}
		private RemoteThread(QHackContext ctx, AssemblyCode asm, uint size = 0x1000)
		{
			Context = ctx;
			Allocation = new MemoryAllocation(ctx, size);
			Header = new RemoteThreadHeader(Allocation.AllocationBase);

			Assembler assembler = new();
			assembler.Emit(DataHelper.GetBytes(Header));
			assembler.Emit((Instruction)$"mov dword ptr [{Header.Address_SafeFreeFlag}],1");
			assembler.Emit(asm);
			assembler.Emit((Instruction)$"mov dword ptr [{Header.Address_SafeFreeFlag}],0");
			assembler.Emit((Instruction)"ret");
			Context.DataAccess.WriteBytes(Header.AllocationAddress, assembler.GetByteCode(Header.AllocationAddress));
		}

		/// <summary>
		/// Allocates space and fills the code in before calling <see cref="RunOnNativeThread"/> to start a remote native thread.<br/>
		/// To avoid a memory leak, call <see cref="Dispose"/> to release the allocated space when the thread is not running.
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="asm"></param>
		/// <returns></returns>
		public static RemoteThread Create(QHackContext ctx, AssemblyCode asm) => new(ctx, asm);

		/// <summary>
		/// Directly starts a remote native thread.<br/>
		/// Note that native threads have no clr info and hence cannot do such things like allocating space on clr heaps.
		/// </summary>
		/// <returns>ThreadID of the remote thread created</returns>
		public int RunOnNativeThread()
		{
			CreateRemoteThread(Context.Handle, 0, 0, Header.Address_Code, 0, 0, out int tid);
			ThreadID = tid;
			return ThreadID;
		}

		/// <summary>
		/// Starts a managed thread.<br/>
		/// Pending implementation.
		/// </summary>
		public void RunOnManagedThread()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks if the code region is ready to be released,
		/// Only when this method returns true can you dispose this object.
		/// </summary>
		/// <returns></returns>
		public bool ReadyToRelease() => Context.DataAccess.ReadValue<int>(Header.Address_SafeFreeFlag) == 0;

		/// <summary>
		/// Calling this method will forcefully release the code region, 
		/// even when the code is being executed.
		/// </summary>
		public void Dispose() => Allocation.Dispose();

		/// <summary>
		/// Waits to dispose until <see cref="ReadyToRelease"/> returns true
		/// </summary>
		public void WaitToDispose()
		{
			while (!ReadyToRelease()) ;
			Dispose();
		}

		[StructLayout(LayoutKind.Sequential)]
		private unsafe readonly struct RemoteThreadHeader
		{
			public static readonly int HeaderSize = sizeof(RemoteThreadHeader);
			public static readonly int Offset_SafeFreeFlag = (int)Marshal.OffsetOf<RemoteThreadHeader>(nameof(SafeFreeFlag));
			public nuint Address_Code => AllocationAddress + (nuint)HeaderSize;
			public nuint Address_SafeFreeFlag => AllocationAddress + (nuint)Offset_SafeFreeFlag;

			public RemoteThreadHeader(nuint allocationAddress)
			{
				AllocationAddress = allocationAddress;
				SafeFreeFlag = 1;
			}

			public readonly nuint AllocationAddress;
			public readonly int SafeFreeFlag;
		}
	}
}
