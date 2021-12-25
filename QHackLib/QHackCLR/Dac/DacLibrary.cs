using QHackCLR;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
using QHackCLR.DataTargets;
using System;
using System.Runtime.InteropServices;
using static QHackLib.NativeFunctions;

namespace QHackCLR.Dac.Interfaces
{
	public unsafe sealed class DacLibrary
	{
		private DacDataTargetImpl DataTarget { get; }
		internal IXCLRDataProcess ClrDataProcess { get; }
		internal ISOSDacInterface SOSDac { get; }
		public DacLibrary(DataTarget target, string dacPath, ulong runtimeBase)
		{
			nuint dac = LoadLibrary(dacPath);
			nuint initAddr = GetProcAddress(dac, "DAC_PAL_InitializeDLL");

			if (initAddr == 0)
				initAddr = GetProcAddress(dac, "PAL_InitializeDLL");
			if (initAddr != 0)
			{
				nuint dllMain = GetProcAddress(dac, "DllMain");
				if (dllMain == 0)
					throw new Exception("Failed to obtain Dac DllMain");

				var main = (delegate* unmanaged[Stdcall]<nuint, int, nuint, int>)dllMain;
				main(dac, 1, 0);
			}

			nuint addr = GetProcAddress(dac, "CLRDataCreateInstance");
			if (addr == 0)
				throw new Exception("Failed to obtain Dac CLRDataCreateInstance");
			DataTarget = new DacDataTargetImpl(target, runtimeBase);
			var func = (delegate* unmanaged[Stdcall]<in Guid, IntPtr, out IntPtr, HRESULT>)addr;
			Guid guid = new("5c552ab6-fc09-4cb3-8e36-22fa03c798b7");
			HRESULT res = func(guid, DataTarget.IDacDataTarget, out IntPtr iUnk);
			if (res != HRESULT.S_OK)
				throw new Exception($"Failure loading DAC: CreateDacInstance failed 0x{res:X8}");
#pragma warning disable CA1416
			ClrDataProcess = Marshal.GetObjectForIUnknown(iUnk) as IXCLRDataProcess;
			SOSDac = Marshal.GetObjectForIUnknown(Marshal.GetComInterfaceForObject<IXCLRDataProcess, ISOSDacInterface>(ClrDataProcess)) as ISOSDacInterface;
		}
	}
}
