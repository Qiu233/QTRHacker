using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace QTRHacker.Functions.Test;

// The observer owns a separate handle and never uses DAC, injects code or attaches
// a debugger. In particular, keep observing while an old remote call is blocked.
internal sealed class DiagnosticSession : IDisposable
{
	private readonly object gate = new();
	private readonly StreamWriter writer;
	private readonly Stopwatch clock = Stopwatch.StartNew();
	private readonly ManualResetEventSlim stop = new();
	private Thread observer;
	private nuint handle;
	private bool disposed;
	public string OutputPath { get; }
	public bool HasExited => handle != 0 && WaitForSingleObject(handle, 0) == 0;

	public DiagnosticSession()
	{
		string file = $"QTRHacker-Diagnostic-{DateTime.Now:yyyyMMdd-HHmmss}-{Environment.ProcessId}.log";
		try
		{
			OutputPath = Path.Combine(AppContext.BaseDirectory, file);
			writer = OpenLog(OutputPath);
		}
		catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
		{
			string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QTRHacker", "diagnostics");
			Directory.CreateDirectory(directory);
			OutputPath = Path.Combine(directory, file);
			writer = OpenLog(OutputPath);
		}
	}

	private static StreamWriter OpenLog(string path) => new(new FileStream(path,
		FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite), new UTF8Encoding(true)) { AutoFlush = true };

	public void Log(string message, bool display = false)
	{
		lock (gate)
		{
			if (disposed) return;
			writer.WriteLine($"{DateTimeOffset.Now:O} +{clock.Elapsed.TotalMilliseconds:F1}ms [T{Environment.CurrentManagedThreadId}] {message}");
			if (display) Console.WriteLine(message);
		}
	}

	public void Error(string stage, Exception exception)
	{
		// Capture process state before formatting a potentially large exception tree.
		Snapshot(stage + " exception observed");
		Log($"EXCEPTION {stage}: {exception}", true);
		Details(exception, "root");
	}

	private void Details(Exception exception, string path)
	{
		Log($"EXCEPTION-CODE {path}: Type={exception.GetType().FullName}; HResult=0x{exception.HResult:X8}" +
			(exception is Win32Exception win32 ? $"; NativeErrorCode={win32.NativeErrorCode} ({new Win32Exception(win32.NativeErrorCode).Message})" : ""));
		if (exception is AggregateException aggregate)
			for (int i = 0; i < aggregate.InnerExceptions.Count; i++) Details(aggregate.InnerExceptions[i], path + $"[{i}]");
		else if (exception.InnerException != null) Details(exception.InnerException, path + ".Inner");
	}

	public void Observe(int pid)
	{
		handle = OpenProcess(0x00100410, false, pid); // SYNCHRONIZE | QUERY_INFORMATION | VM_READ
		if (handle == 0) throw new Win32Exception(Marshal.GetLastWin32Error(), $"OpenProcess observer PID={pid}");
		Log($"OBSERVER PID={pid}; handle=0x{handle:X}; interval=200ms; heartbeat=5s; no debugger attached by this tool");
		Snapshot("observer opened");
		observer = new Thread(() =>
		{
			try
			{
				int ticks = 0;
				while (!stop.Wait(200))
				{
					uint state = WaitForSingleObject(handle, 0);
					if (state != 258)
					{
						Snapshot("PROCESS SIGNAL OBSERVED");
						Log(state == 0 ? "检测到游戏进程已退出，退出状态已保存。" : "进程状态查询失败，详情已保存。", true);
						break;
					}
					if (++ticks % 25 == 0) Snapshot("heartbeat");
				}
			}
			catch (Exception ex) { Error("observer", ex); }
		}) { IsBackground = true, Name = "Diagnostic process observer" };
		observer.Start();
	}

	public void Snapshot(string reason, nuint actualHandle = 0)
	{
		if (handle != 0) LogState(reason + " observer", handle);
		if (actualHandle != 0) LogState(reason + " QHackContext", actualHandle);
	}

	private void LogState(string reason, nuint processHandle)
	{
		uint wait = WaitForSingleObject(processHandle, 0);
		int waitError = wait == uint.MaxValue ? Marshal.GetLastWin32Error() : 0;
		bool queried = GetExitCodeProcess(processHandle, out uint code);
		int exitError = queried ? 0 : Marshal.GetLastWin32Error();
		bool debuggerQuery = CheckRemoteDebuggerPresent(processHandle, out bool debugger);
		int debuggerError = debuggerQuery ? 0 : Marshal.GetLastWin32Error();
		var memory = new ProcessMemoryCounters { Size = (uint)Marshal.SizeOf<ProcessMemoryCounters>() };
		bool memoryQuery = GetProcessMemoryInfo(processHandle, ref memory, memory.Size);
		int memoryError = memoryQuery ? 0 : Marshal.GetLastWin32Error();
		Log($"STATE {reason}: handle=0x{processHandle:X}; Wait=0x{wait:X8}; WaitError={waitError}; " +
			$"ExitQuery={queried}; ExitCode=0x{code:X8}; ExitError={exitError}; " +
			$"DebuggerQuery={debuggerQuery}; Debugger={debugger}; DebuggerError={debuggerError}; " +
			$"MemoryQuery={memoryQuery}; MemoryError={memoryError}; WorkingSet={memory.WorkingSetSize}; PrivateBytes={memory.PrivateUsage}");
	}

	public void MemoryMap()
	{
		var status = new MemoryStatus { Length = (uint)Marshal.SizeOf<MemoryStatus>() };
		if (GlobalMemoryStatusEx(ref status))
			Log($"SYSTEM MEMORY: load={status.MemoryLoad}%; physicalAvailable={status.AvailPhys}; commitLimit={status.TotalPageFile}; commitAvailable={status.AvailPageFile}");
		else Log($"GlobalMemoryStatusEx error={Marshal.GetLastWin32Error()}");
		if (handle == 0) return;
		ulong address = 0, free = 0, largest = 0;
		int regions = 0, error = 0;
		// Runner is x86. The range includes high addresses of LAA game processes.
		while (address < 0x100000000UL)
		{
			if (VirtualQueryEx(handle, (nuint)address, out var region, (nuint)Marshal.SizeOf<MemoryRegion>()) == 0)
			{
				error = Marshal.GetLastWin32Error();
				break;
			}
			regions++;
			ulong length = (ulong)region.RegionSize;
			if (region.State == 0x10000) { free += length; largest = Math.Max(largest, length); }
			ulong next = (ulong)region.BaseAddress + length;
			if (next <= address) break;
			address = next;
		}
		Log($"VIRTUAL MEMORY: regions={regions}; scannedThrough=0x{address:X}; freeBytes={free}; largestFreeRegion={largest}; endError={error} (87 at the address-space limit is expected)");
	}

	public void Dispose()
	{
		stop.Set();
		observer?.Join();
		Snapshot("session end");
		lock (gate)
		{
			disposed = true;
			if (handle != 0) CloseHandle(handle);
			handle = 0;
			writer.Dispose();
		}
		stop.Dispose();
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct ProcessMemoryCounters
	{
		public uint Size, PageFaultCount;
		public nuint PeakWorkingSetSize, WorkingSetSize, QuotaPeakPagedPoolUsage, QuotaPagedPoolUsage,
			QuotaPeakNonPagedPoolUsage, QuotaNonPagedPoolUsage, PagefileUsage, PeakPagefileUsage, PrivateUsage;
	}
	[StructLayout(LayoutKind.Sequential)]
	private struct MemoryRegion
	{
		public nuint BaseAddress, AllocationBase;
		public uint AllocationProtect;
		public nuint RegionSize;
		public uint State, Protect, Type;
	}
	[StructLayout(LayoutKind.Sequential)]
	private struct MemoryStatus
	{
		public uint Length, MemoryLoad;
		public ulong TotalPhys, AvailPhys, TotalPageFile, AvailPageFile, TotalVirtual, AvailVirtual, AvailExtendedVirtual;
	}
	[DllImport("kernel32.dll", SetLastError = true)] private static extern nuint OpenProcess(uint access, bool inherit, int pid);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool CloseHandle(nuint handle);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern uint WaitForSingleObject(nuint handle, uint milliseconds);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool GetExitCodeProcess(nuint handle, out uint code);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool CheckRemoteDebuggerPresent(nuint handle, out bool present);
	[DllImport("psapi.dll", SetLastError = true)] private static extern bool GetProcessMemoryInfo(nuint handle, ref ProcessMemoryCounters counters, uint size);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern nuint VirtualQueryEx(nuint handle, nuint address, out MemoryRegion region, nuint length);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool GlobalMemoryStatusEx(ref MemoryStatus status);
}
