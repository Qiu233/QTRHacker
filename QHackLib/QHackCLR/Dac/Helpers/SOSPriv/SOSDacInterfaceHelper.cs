using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Helpers
{
	public unsafe static class SOSDacInterfaceHelper
	{
		public static string GetAppDomainName(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS appDomain) => HelperGlobals.GetStringFromAddr(SOSDac.GetAppDomainName, appDomain);
		public static string GetMethodTableName(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS mt) => HelperGlobals.GetStringFromAddr(SOSDac.GetMethodTableName, mt);
		public static string GetMethodDescName(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS mt) => HelperGlobals.GetStringFromAddr(SOSDac.GetMethodDescName, mt);
		public static CLRDATA_ADDRESS[] GetAppDomainList(this ISOSDacInterface SOSDac)
		{
			SOSDac.GetAppDomainStoreData(out DacpAppDomainStoreData adsData);
			uint needed = (uint)adsData.DomainCount;
			CLRDATA_ADDRESS[] buffer = new CLRDATA_ADDRESS[needed];
			fixed (CLRDATA_ADDRESS* ptr = buffer)
				SOSDac.GetAppDomainList(needed, ptr, out needed);
			return buffer;
		}

		public static CLRDATA_ADDRESS[] GetAssemblyList(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS appDomain)
		{
			SOSDac.GetAppDomainData(appDomain, out DacpAppDomainData data);
			return HelperGlobals.GetListFromAddr(SOSDac.GetAssemblyList, appDomain, data.AssemblyCount);
		}

		public static CLRDATA_ADDRESS[] GetAssemblyModuleList(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS appDomain, CLRDATA_ADDRESS assembly)
		{
			SOSDac.GetAssemblyData(appDomain, assembly, out DacpAssemblyData data);
			return HelperGlobals.GetListFromAddr(SOSDac.GetAssemblyModuleList, assembly, (int)data.ModuleCount);
		}

		public static IMetadataImport GetMetadataImport(this ISOSDacInterface SOSDac, CLRDATA_ADDRESS module)
		{
			SOSDac.GetModule(module, out IXCLRDataModule dataModule);
#pragma warning disable CA1416
			return Marshal.GetObjectForIUnknown(Marshal.GetComInterfaceForObject<IXCLRDataModule, IMetadataImport>(dataModule)) as IMetadataImport;
		}
	}
}
