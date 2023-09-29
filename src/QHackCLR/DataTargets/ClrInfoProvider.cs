using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DataTargets;

public enum ClrFlavor
{
    Desktop = 0,
    Core = 3
};
internal static class ClrInfoProvider
{
    internal const string c_desktopModuleName1 = "clr.dll";
    internal const string c_desktopModuleName2 = "mscorwks.dll";
    internal const string c_coreModuleName = "coreclr.dll";
    internal const string c_linuxCoreModuleName = "libcoreclr.so";
    internal const string c_macOSCoreModuleName = "libcoreclr.dylib";

    internal const string c_desktopDacFileNameBase = "mscordacwks";
    internal const string c_coreDacFileNameBase = "mscordaccore";
    internal const string c_desktopDacFileName = "mscordacwks.dll";
    internal const string c_coreDacFileName = "mscordaccore.dll";
    internal const string c_linuxCoreDacFileName = "libmscordaccore.so";
    internal const string c_macOSCoreDacFileName = "libmscordaccore.dylib";


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
        else if (moduleName.Equals(c_coreModuleName, StringComparison.OrdinalIgnoreCase))
        {
            flavor = ClrFlavor.Core;
            platform = OSPlatform.Windows;
            return true;
        }
        else if (moduleName.Equals(c_linuxCoreModuleName))
        {
            flavor = ClrFlavor.Core;
            platform = OSPlatform.Linux;
            return true;
        }
        else if (moduleName.Equals(c_macOSCoreModuleName))
        {
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
}