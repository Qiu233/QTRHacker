using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace QHackCLR.Dac.COM
{
	public sealed unsafe class VTableBuilder
	{
		private readonly Guid Guid;
		private readonly COMCallableIUnknown Wrapper;
		private readonly List<Delegate> Delegates = new();

		internal VTableBuilder(COMCallableIUnknown wrapper, Guid guid)
		{
			Guid = guid;
			Wrapper = wrapper;
		}

		public void AddMethod(Delegate func)
		{
			if (func is null)
				throw new ArgumentNullException(nameof(func));
			Delegates.Add(func);
		}

		public IntPtr Complete()
		{
			IntPtr obj = Marshal.AllocHGlobal(IntPtr.Size);

			IntPtr* vtable = (IntPtr*)Marshal.AllocHGlobal(Delegates.Count * IntPtr.Size + sizeof(IUnknownVTable));
			*(void**)obj = vtable;

			IUnknownVTable iunk = Wrapper.IUnknown;
			*vtable++ = new IntPtr(iunk.QueryInterface);
			*vtable++ = new IntPtr(iunk.AddRef);
			*vtable++ = new IntPtr(iunk.Release);

			foreach (Delegate d in Delegates)
				*vtable++ = Marshal.GetFunctionPointerForDelegate(d);

			Wrapper.RegisterInterface(Guid, obj, Delegates);
			return obj;
		}
	}
}
