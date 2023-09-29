using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using QHackCLR.Common;

namespace QHackCLR.COM;
public unsafe class COMCallableIUnknown : COMHelper
{
	// NOTE do not make this field readonly, as otherwise we operate upon a copy of it
	private GCHandle _handle;
	private int _refCount;

	private readonly Dictionary<Guid, IntPtr> _interfaces = new();
	private readonly List<Delegate> _delegates = new();

	/// <summary>
	/// Gets the IUnknown pointer to this object.
	/// </summary>
	public IntPtr IUnknownObject { get; }

	/// <summary>
	/// Gets the IUnknown VTable for this object.
	/// </summary>
	public IUnknownVTable IUnknown => **(IUnknownVTable**)IUnknownObject;

	/// <summary>
	/// Constructor.
	/// </summary>
	public COMCallableIUnknown()
	{
		_handle = GCHandle.Alloc(this);

		IUnknownVTable* vtable = (IUnknownVTable*)Marshal.AllocHGlobal(sizeof(IUnknownVTable)).ToPointer();
		QueryInterfaceDelegate qi = QueryInterfaceImpl;
		vtable->QueryInterface = (delegate* unmanaged[Stdcall]<IntPtr, in Guid, out IntPtr, HRESULT>)Marshal.GetFunctionPointerForDelegate(qi);
		_delegates.Add(qi);

		AddRefDelegate addRef = new AddRefDelegate(AddRefImpl);
		vtable->AddRef = (delegate* unmanaged[Stdcall]<IntPtr, int>)Marshal.GetFunctionPointerForDelegate(addRef);
		_delegates.Add(addRef);

		ReleaseDelegate release = new ReleaseDelegate(ReleaseImpl);
		vtable->Release = (delegate* unmanaged[Stdcall]<IntPtr, int>)Marshal.GetFunctionPointerForDelegate(release);
		_delegates.Add(release);

		IUnknownObject = Marshal.AllocHGlobal(IntPtr.Size);
		*(void**)IUnknownObject = vtable;

		_interfaces.Add(IUnknownGuid, IUnknownObject);
	}

	/// <summary>
	/// AddRef.
	/// </summary>
	/// <returns>The new ref count.</returns>
	public int AddRef() => AddRefImpl(IUnknownObject);

	/// <summary>
	/// Release.
	/// </summary>
	/// <returns>The new RefCount.</returns>
	public int Release() => ReleaseImpl(IUnknownObject);

	/// <summary>
	/// Adds an IUnknown based interface to this COM object.
	/// </summary>
	/// <param name="guid">The GUID of this interface.</param>
	/// <param name="validate">Whether or not to validate the delegates that
	/// used to build this COM interface's methods.</param>
	/// <returns>A VTableBuilder to construct this interface.  Note that until VTableBuilder.Complete
	/// is called, the interface will not be registered.</returns>
	public VTableBuilder AddInterface(Guid guid, bool validate)
	{
#if DEBUG
		validate = true;
#endif

		return new VTableBuilder(this, guid, validate);
	}

	internal void RegisterInterface(Guid guid, IntPtr clsPtr, List<Delegate> keepAlive)
	{
		_interfaces.Add(guid, clsPtr);
		_delegates.AddRange(keepAlive);
	}

	private HRESULT QueryInterfaceImpl(IntPtr _, in Guid guid, out IntPtr ptr)
	{
		if (_interfaces.TryGetValue(guid, out IntPtr value))
		{
			Interlocked.Increment(ref _refCount);
			ptr = value;
			return HRESULT.S_OK;
		}

		ptr = IntPtr.Zero;
		return HRESULT.E_NOINTERFACE;
	}

	private int ReleaseImpl(IntPtr self)
	{
		int count = Interlocked.Decrement(ref _refCount);
		if (count <= 0)
		{
			// Only free memory the first time we reach here.
			if (_handle.IsAllocated)
			{
				try
				{
					Destroy();
				}
				finally
				{
					foreach (IntPtr ptr in _interfaces.Values)
					{
						IntPtr* val = (IntPtr*)ptr;
						Marshal.FreeHGlobal(*val);
						Marshal.FreeHGlobal(ptr);
					}

					_handle.Free();
					_interfaces.Clear();
					_delegates.Clear();
				}
			}
		}

		return count;
	}

	private int AddRefImpl(IntPtr self) => Interlocked.Increment(ref _refCount);

	protected virtual void Destroy()
	{
	}
}