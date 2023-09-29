using QHackCLR.Common;
using QHackCLR.DAC.DACP;
using QHackCLR.DAC.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QHackCLR.DAC;

internal unsafe static class DACHelpers
{
	public static IEnumerable<CLRDATA_ADDRESS> GetAppDomainList(this ISOSDacInterface sos)
	{
		DacpAppDomainStoreData data;
		sos.GetAppDomainStoreData(&data);
		uint needed = data.DomainCount;
		if (needed == 0)
			return Array.Empty<CLRDATA_ADDRESS>();
		CLRDATA_ADDRESS[] a = new CLRDATA_ADDRESS[needed];
		fixed (CLRDATA_ADDRESS* ptr = a)
			sos.GetAppDomainList(needed, ptr, &needed);
		return a;
	}

	public static IEnumerable<CLRDATA_ADDRESS> GetAssemblyList(this ISOSDacInterface sos, CLRDATA_ADDRESS appDomain)
	{
		DacpAppDomainData data;
		sos.GetAppDomainData(appDomain, &data);

		int needed = (int)data.AssemblyCount;
		if (needed == 0)
			return Array.Empty<CLRDATA_ADDRESS>();
		CLRDATA_ADDRESS[] buffer = new CLRDATA_ADDRESS[needed];
		fixed (CLRDATA_ADDRESS* ptr = buffer)
			sos.GetAssemblyList(appDomain, needed, ptr, &needed);
		return buffer;
	}
	public static IEnumerable<CLRDATA_ADDRESS> GetAssemblyModuleList(this ISOSDacInterface sos, CLRDATA_ADDRESS appDomain, CLRDATA_ADDRESS assembly)
	{
		DacpAssemblyData data;
		sos.GetAssemblyData(appDomain, assembly, &data);

		uint needed = data.ModuleCount;
		if (needed == 0)
			return Array.Empty<CLRDATA_ADDRESS>();
		CLRDATA_ADDRESS[] buffer = new CLRDATA_ADDRESS[needed];
		fixed (CLRDATA_ADDRESS* ptr = buffer)
			sos.GetAssemblyModuleList(assembly, needed, ptr, &needed);
		return buffer;
	}

	public static string? GetAppDomainName(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS addrName)
	{
		uint needed = 0;
		SOSDac.GetAppDomainName(addrName, 0, null, &needed);
		if (needed <= 1)
			return null;
		char[] buffer = new char[needed];
		fixed (char* ptr = buffer)
			SOSDac.GetAppDomainName(addrName, needed, ptr, &needed);
		return new string(buffer.SkipLast(1).ToArray());
	}
	public static string? GetMethodTableName(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS addrName)
	{
		uint needed = 0;
		SOSDac.GetMethodTableName(addrName, 0, null, &needed);
		if (needed <= 1)
			return null;
		char[] buffer = new char[needed];
		fixed (char* ptr = buffer)
			SOSDac.GetMethodTableName(addrName, needed, ptr, &needed);
		return new string(buffer.SkipLast(1).ToArray());
	}
}
