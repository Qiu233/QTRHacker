using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace QHackCLR.DataTargets;

public unsafe sealed class DataTarget : IDisposable
{
    internal readonly int Pid;
	internal readonly nuint Handle;
    public readonly ImmutableArray<ClrInfo> ClrVersions;
    public DataAccess DataAccess => new(Handle);
    public DataTarget(int pid)
    {
        Pid = pid;
        Handle = NativeMethods.OpenProcess(NativeMethods.ProcessAccessFlags.All, false, pid);
        if (Handle == 0)
        {
            uint hr = NativeMethods.GetLastError();
            throw new QHackCLRException($"Could not attach to process {pid}, error: {hr:X}");
        }
        if (!NativeMethods.IsWow64Process(Handle, out bool targetx32))
            throw new QHackCLRException("Failed to fetch architecture info, please make sure you have the permission.");

        if (targetx32 != (sizeof(nuint) == 4))
        {
            throw new QHackCLRException($"Mismatched architecture between this process and the target process.\nThis process is {(sizeof(nuint) == 8 ? "x64" : "x86")} while the target is {(targetx32 ? "x86" : "x64")}.");
        }

        NativeMethods.EnumProcessModules(Handle, null, 0, out uint needed);
        uint len = needed / (uint)sizeof(nuint);
        nuint[] modules = new nuint[len];
        fixed (nuint* ptr = modules)
            NativeMethods.EnumProcessModules(Handle, ptr, len, out needed);
        char[] nameBuffer = new char[2048];
        List<string> moduleNames = new();
        List<ClrInfo> clrVersions = new();
        fixed (char* ptr = nameBuffer)
        {
            foreach (var module in modules)
            {
                NativeMethods.GetModuleFileNameExW(Handle, module, ptr, 2048);
                string fileName = new(ptr);
                moduleNames.Add(fileName);
                if (ClrInfoProvider.IsSupportedRuntime(fileName, out ClrFlavor flavor, out OSPlatform platform))
                {
                    string dacPath = Path.Combine(Path.GetDirectoryName(fileName)!, ClrInfoProvider.GetDacFileName(flavor, platform));
                    clrVersions.Add(new ClrInfo(this, flavor, module, dacPath, fileName));
                }
            }
        }
        ClrVersions = clrVersions.ToImmutableArray();
        if (!clrVersions.Any())
        {
            throw new QHackCLRException("Could not find any supported clr runtime.\n[" + string.Join(",", moduleNames.ToArray()) + "]");
        }
    }

    public void Dispose()
    {
        NativeMethods.CloseHandle(Handle);
    }
}