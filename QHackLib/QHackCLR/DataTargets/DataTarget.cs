using QHackLib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DataTargets
{
	public sealed unsafe class DataTarget : IDisposable
	{
		public DataAccess DataAccess { get; }
		public int ProcessId { get; }
		public ImmutableArray<ClrInfo> ClrVersions { get; }
		private DataTarget(int pid)
		{
			ProcessId = pid;
			nuint handle = NativeFunctions.OpenProcess(NativeFunctions.PROCESS_ALL_ACCESS, false, (uint)pid);
			DataAccess = new DataAccess(handle);
			ImmutableArray<ClrInfo>.Builder versionBuilder = ImmutableArray.CreateBuilder<ClrInfo>();
			NativeFunctions.EnumProcessModules(handle, null, 0, out uint needed);
			nuint[] buffer = new nuint[needed / sizeof(nuint)];
			fixed (nuint* ptr = buffer)
				NativeFunctions.EnumProcessModules(handle, ptr, needed, out needed);
			StringBuilder nameBuilder = new(2048);
			foreach (var module in buffer)
			{
				_ = NativeFunctions.GetModuleFileNameEx(handle, module, nameBuilder, (uint)nameBuilder.Capacity);
				string fileName = nameBuilder.ToString();
				if (ClrInfoProvider.IsSupportedRuntime(fileName, out ClrFlavor flavor, out OSPlatform platform))
				{
					string dacPath = Path.Combine(Path.GetDirectoryName(fileName), ClrInfoProvider.GetDacFileName(flavor, platform));
					versionBuilder.Add(new ClrInfo(this, flavor, module, dacPath));
				}
			}
			ClrVersions = versionBuilder.ToImmutable();
		}
		public static DataTarget AttachToProcess(int pid) => new DataTarget(pid);

		public void Dispose() => NativeFunctions.CloseHandle(DataAccess.ProcessHandle);
	}
}
