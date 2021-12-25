using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace QHackCLR.Dac.COM
{
	public unsafe class COMCallableIUnknown : COMHelper
	{
		private GCHandle Handle;
		private int RefCount;

		private readonly Dictionary<Guid, IntPtr> Interfaces = new();
		private readonly List<Delegate> Delegates = new();

		public IntPtr IUnknownObject { get; }
		public IUnknownVTable IUnknown => **(IUnknownVTable**)IUnknownObject;


		public COMCallableIUnknown()
		{
			Handle = GCHandle.Alloc(this);

			IUnknownVTable* vtable = (IUnknownVTable*)Marshal.AllocHGlobal(sizeof(IUnknownVTable));
			QueryInterfaceDelegate qi = QueryInterfaceImpl;
			AddRefDelegate addRef = AddRefImpl;
			ReleaseDelegate release = ReleaseImpl;

			vtable->QueryInterface = (delegate* unmanaged[Stdcall]<IntPtr, in Guid, out IntPtr, HRESULT>)Marshal.GetFunctionPointerForDelegate(qi);
			vtable->AddRef = (delegate* unmanaged[Stdcall]<IntPtr, int>)Marshal.GetFunctionPointerForDelegate(addRef);
			vtable->Release = (delegate* unmanaged[Stdcall]<IntPtr, int>)Marshal.GetFunctionPointerForDelegate(release);

			Delegates.Add(qi);
			Delegates.Add(addRef);
			Delegates.Add(release);

			IUnknownObject = Marshal.AllocHGlobal(IntPtr.Size);
			*(void**)IUnknownObject = vtable;

			Interfaces.Add(IUnknownGuid, IUnknownObject);
		}

		public int AddRef() => AddRefImpl(IUnknownObject);
		public int Release() => ReleaseImpl(IUnknownObject);
		public VTableBuilder AddInterface(Guid guid) => new(this, guid);

		internal void RegisterInterface(Guid guid, IntPtr clsPtr, List<Delegate> keepAlive)
		{
			Interfaces.Add(guid, clsPtr);
			Delegates.AddRange(keepAlive);
		}

		private HRESULT QueryInterfaceImpl(IntPtr _, in Guid guid, out IntPtr ptr)
		{
			if (Interfaces.TryGetValue(guid, out IntPtr value))
			{
				Interlocked.Increment(ref RefCount);
				ptr = value;
				return HRESULT.S_OK;
			}
			ptr = IntPtr.Zero;
			return HRESULT.E_NOINTERFACE;
		}

		private int AddRefImpl(IntPtr _) => Interlocked.Increment(ref RefCount);

		private int ReleaseImpl(IntPtr _)
		{
			int count = Interlocked.Decrement(ref RefCount);
			if (count > 0 || !Handle.IsAllocated)
				return count;
			try { Destroy(); } catch { }
			foreach (IntPtr ptr in Interfaces.Values)
			{
				Marshal.FreeHGlobal(*(IntPtr*)ptr);
				Marshal.FreeHGlobal(ptr);
			}
			Handle.Free();
			Interfaces.Clear();
			Delegates.Clear();
			return count;
		}

		protected virtual void Destroy()
		{
		}
	}
}