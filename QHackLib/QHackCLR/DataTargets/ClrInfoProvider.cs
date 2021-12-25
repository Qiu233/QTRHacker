using System;
using System.IO;
using System.Runtime.InteropServices;

namespace QHackCLR.DataTargets
{
	public static class ClrInfoProvider
	{
		private const string c_desktopModuleName1 = "clr.dll";
		private const string c_desktopModuleName2 = "mscorwks.dll";
		private const string c_coreModuleName = "coreclr.dll";
		private const string c_linuxCoreModuleName = "libcoreclr.so";
		private const string c_macOSCoreModuleName = "libcoreclr.dylib";

		private const string c_desktopDacFileNameBase = "mscordacwks";
		private const string c_coreDacFileNameBase = "mscordaccore";
		private const string c_desktopDacFileName = c_desktopDacFileNameBase + ".dll";
		private const string c_coreDacFileName = c_coreDacFileNameBase + ".dll";
		private const string c_linuxCoreDacFileName = "libmscordaccore.so";
		private const string c_macOSCoreDacFileName = "libmscordaccore.dylib";


		public static bool IsSupportedRuntime(string moduleFile, out ClrFlavor flavor, out OSPlatform platform)
		{
			if (moduleFile is null)
				throw new ArgumentNullException(nameof(moduleFile));

			flavor = default;
			platform = default;

			string moduleName = Path.GetFileName(moduleFile);
			if (moduleName is null)
				return false;

			if (moduleName.Equals(c_desktopModuleName1, StringComparison.OrdinalIgnoreCase) ||
				moduleName.Equals(c_desktopModuleName2, StringComparison.OrdinalIgnoreCase))
			{
				flavor = ClrFlavor.Desktop;
				platform = OSPlatform.Windows;
				return true;
			}
			if (moduleName.Equals(c_coreModuleName, StringComparison.OrdinalIgnoreCase))
			{
				flavor = ClrFlavor.Core;
				platform = OSPlatform.Windows;
				return true;
			}
			switch (moduleName)
			{
				case c_linuxCoreModuleName:
					flavor = ClrFlavor.Core;
					platform = OSPlatform.Linux;
					return true;

				case c_macOSCoreModuleName:
					flavor = ClrFlavor.Core;
					platform = OSPlatform.OSX;
					return true;
			}
			return false;
		}

		public static string GetDacFileName(ClrFlavor flavor, OSPlatform platform)
		{
			if (platform == OSPlatform.Linux)
				return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? c_coreDacFileName : c_linuxCoreDacFileName;
			if (platform == OSPlatform.OSX)
				return c_macOSCoreDacFileName;
			return flavor == ClrFlavor.Core ? c_coreDacFileName : c_desktopDacFileName;
		}

		public static string GetDacRequestFileName(ClrFlavor flavor, Architecture currentArchitecture, Architecture targetArchitecture, VersionInfo version, OSPlatform platform)
		{
			if (platform == OSPlatform.Linux)
				return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? c_coreDacFileName : c_linuxCoreDacFileName;
			if (platform == OSPlatform.OSX)
				return c_macOSCoreDacFileName;
			var dacNameBase = flavor == ClrFlavor.Core ? c_coreDacFileNameBase : c_desktopDacFileNameBase;
			return $"{dacNameBase}_{currentArchitecture}_{targetArchitecture}_{version.Major}.{version.Minor}.{version.Revision}.{version.Patch:D2}.dll";
		}
	}
}