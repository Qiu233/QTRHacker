using QHackLib.Memory;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QTRHacker.Core;

public class RemoteDataManager<T> where T : unmanaged
{
	private const uint SIZE_SIGN = 4096;//1 pages
	private readonly byte[] Header;

	private nuint BaseAddress;

	public GameContext Context { get; }

	/// <summary>
	/// 4 bytes
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public unsafe T this[int index]
	{
		get => Context.HContext.DataAccess.ReadValue<T>(GetAddress(index));
		set => Context.HContext.DataAccess.WriteValue(GetAddress(index), value);
	}


	private RemoteDataManager(GameContext ctx, byte[] signHead)
	{
		Context = ctx;
		int len = signHead.Length;
		Header = new byte[len];
		Array.Copy(signHead, Header, Header.Length);
	}

	public static RemoteDataManager<T> Create(GameContext ctx, byte[] header)
	{
		RemoteDataManager<T> rsm = new(ctx, header);
		rsm.InitializeSign();
		return rsm;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe nuint GetAddress(int index)
	{
		return BaseAddress + (uint)(index * sizeof(T));
	}

	private void InitializeSign()
	{
		BaseAddress = AobscanHelper.Aobscan(Context.HContext.Handle, Header)
						  .FirstOrDefault(t => true);
		if (BaseAddress == 0)
		{
			BaseAddress = MemoryAllocation.Alloc(Context.HContext.Handle, SIZE_SIGN);
			Context.HContext.DataAccess.WriteBytes(BaseAddress, Header);
		}
		BaseAddress += (uint)Header.Length;
	}
}
