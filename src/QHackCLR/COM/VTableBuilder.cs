using System.Runtime.InteropServices;

namespace QHackCLR.COM;

public sealed unsafe class VTableBuilder
{
	private readonly Guid _guid;
	private readonly COMCallableIUnknown _wrapper;
	private readonly bool _forceValidation;
	private readonly List<Delegate> _delegates = new();

	private bool _complete;

	internal VTableBuilder(COMCallableIUnknown wrapper, Guid guid, bool forceValidation)
	{
		_guid = guid;
		_wrapper = wrapper;
		_forceValidation = forceValidation;
	}

	public void AddMethod(Delegate func, bool validate = false)
	{
		if (func is null)
			throw new ArgumentNullException(nameof(func));

		if (_complete)
			throw new InvalidOperationException();

		if (_forceValidation || validate)
		{
			if (func.Method.GetParameters().First().ParameterType != typeof(IntPtr))
				throw new InvalidOperationException();
		}

		_delegates.Add(func);
	}

	public IntPtr Complete()
	{
		if (_complete)
			throw new InvalidOperationException();

		_complete = true;

		IntPtr obj = Marshal.AllocHGlobal(IntPtr.Size);

		int vtablePartSize = _delegates.Count * IntPtr.Size;
		IntPtr* vtable = (IntPtr*)Marshal.AllocHGlobal(vtablePartSize + sizeof(IUnknownVTable));
		*(void**)obj = vtable;

		IUnknownVTable iunk = _wrapper.IUnknown;
		*vtable++ = new IntPtr(iunk.QueryInterface);
		*vtable++ = new IntPtr(iunk.AddRef);
		*vtable++ = new IntPtr(iunk.Release);

		foreach (Delegate d in _delegates)
			*vtable++ = Marshal.GetFunctionPointerForDelegate(d);

		_wrapper.RegisterInterface(_guid, obj, _delegates);
		return obj;
	}
}