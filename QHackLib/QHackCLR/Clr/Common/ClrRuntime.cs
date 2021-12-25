using QHackCLR.Clr.Builders;
using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Helpers;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public class ClrRuntime
	{
		protected nuint BaseAddress { get; }
		protected IRuntimeHelper RuntimeHelper { get; }
		public ClrInfo ClrInfo { get; }
		public ClrRuntime(ClrInfo clrInfo, IRuntimeHelper helper, nuint baseAddress)
		{
			ClrInfo = clrInfo;
			RuntimeHelper = helper;
			BaseAddress = baseAddress;
		}
		public DataTarget DataTarget => ClrInfo.DataTarget;
		public DacLibrary DacLibrary => RuntimeHelper.DacLibrary;

		public void Flush()
		{
			_AppDomain = null;
			_Heap = null;
			RuntimeHelper.Flush();
		}

		private ClrAppDomain _AppDomain;

		private ClrHeap _Heap;
		public ClrHeap Heap => _Heap ??= new ClrHeap(this, RuntimeHelper.HeapHelper);


		/// <summary>
		/// a.k.a MSCORLIB or SYSTEM.PRIVATE.CORELIB
		/// </summary>
		/// <param name="runtime"></param>
		/// <returns></returns>
		public ClrModule BaseClassLibrary => Heap.ObjectType.Module;

		/// <summary>
		/// Supports only one appdomain, CORE style.
		/// </summary>
		public ClrAppDomain AppDomain => _AppDomain ??= RuntimeHelper.GetAppDomain(RuntimeHelper.SOSDac.GetAppDomainList()[0]);
	}
}
