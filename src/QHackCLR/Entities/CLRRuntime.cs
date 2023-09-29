using QHackCLR.Builders;
using QHackCLR.DAC;
using QHackCLR.DAC.DACP;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public unsafe class CLRRuntime
{
	internal readonly IRuntimeHelper RuntimeHelper;
	private CLRAppDomain? m_AppDomain;
	private CLRHeap? m_Heap;
	public ClrInfo ClrInfo { get; }

	internal CLRRuntime(ClrInfo info, IRuntimeHelper helper)
	{
		ClrInfo = info;
		RuntimeHelper = helper;
		DacpUsefulGlobalsData tables = new();
		helper.SOSDac.GetUsefulGlobals(&tables);
	}

	public nuint BaseAddress => ClrInfo.RuntimeBase;

	public DataTarget DataTarget => ClrInfo.DataTarget;

	internal DACLibrary DACLibrary => RuntimeHelper.DACLibrary;

	public CLRAppDomain AppDomain
	{
		get
		{
			if (m_AppDomain is null)
			{
				var domains = this.DACLibrary.SOSDac.GetAppDomainList();
				m_AppDomain = this.RuntimeHelper.GetAppDomain(domains.First());
			}
			return m_AppDomain;
		}
	}
	public CLRHeap Heap
	{
		get
		{
			m_Heap ??= new CLRHeap(this, this.RuntimeHelper.HeapHelper);
			return m_Heap;
		}
	}

	public CLRModule BaseClassLibrary => Heap.ObjectType.Module;

	public void Flush()
	{
		m_AppDomain = null;
		m_Heap = null;
		RuntimeHelper.Flush();
	}

	public string? GetJitHelperFunctionName(nuint addr)
	{
		uint needed = 0;
		var naddr = addr.ToUInt64();
		RuntimeHelper.SOSDac.GetJitHelperFunctionName(naddr, 0, null, &needed);
		if (needed <= 1)
			return null;
		byte[] buffer = new byte[needed];
		fixed (byte* ptr = buffer)
			RuntimeHelper.SOSDac.GetJitHelperFunctionName(naddr, needed, ptr, &needed);
		int index = Array.IndexOf(buffer, (byte)0);
		return Encoding.UTF8.GetString(buffer, 0, index >= 0 ? index : buffer.Length);
	}
}
