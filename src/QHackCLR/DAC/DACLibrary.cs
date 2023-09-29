using QHackCLR.Common;
using QHackCLR.DAC.Defs;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC;

internal unsafe class DACLibrary
{
	public readonly DacDataTargetImpl DataTarget;
	public readonly IXCLRDataProcess ClrDataProcess;
	public readonly nuint DacModule;
	private ISOSDacInterface? _SOSDac;
	public DACLibrary(DataTarget target, string dacPath, ulong runtimeBase)
	{
		DacModule = NativeMethods.LoadLibraryW(dacPath);
		if (DacModule == 0)
		{
			var e = NativeMethods.GetLastError();
			throw new QHackCLRException($"Failure loading DAC: LoadLibraryW failed when loading file: \"{dacPath}\", with LastError=0x{e:X8}");
		}
		nuint addr = NativeMethods.GetProcAddress(DacModule, "CLRDataCreateInstance");
		if (addr == 0)
			throw new QHackCLRException("Failed to obtain Dac CLRDataCreateInstance");
		DataTarget = new DacDataTargetImpl(target);
		var f = (delegate* unmanaged[Stdcall]<ref Guid, nuint, out nuint, HRESULT>)addr;
		var guid = Guid.Parse("5c552ab6-fc09-4cb3-8e36-22fa03c798b7");
		var res = f(ref guid, (nuint)DataTarget.IDacDataTarget, out nuint iUnk);
		if (res != HRESULT.S_OK)
			throw new QHackCLRException($"Failure loading DAC: CreateDacInstance failed 0x{res.Value:X8}");
		ClrDataProcess = (IXCLRDataProcess)Marshal.GetObjectForIUnknown((nint)iUnk);
	}

	public ISOSDacInterface SOSDac
	{
		get
		{
			if (_SOSDac is not null)
				return _SOSDac;
			var p = Marshal.GetComInterfaceForObject<IXCLRDataProcess, ISOSDacInterface>(ClrDataProcess);
			return _SOSDac = (ISOSDacInterface)Marshal.GetObjectForIUnknown(p);
		}
	}
}
