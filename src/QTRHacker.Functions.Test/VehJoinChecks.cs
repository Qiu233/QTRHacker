using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test;

internal static class VehJoinChecks
{
	[DllImport("kernel32.dll", SetLastError = true)]
	internal static extern bool ReadProcessMemory(nint process, nuint address, out uint value, nuint size, out nuint read);
	[DllImport("kernel32.dll", SetLastError = true)]
	internal static extern bool WriteProcessMemory(nint process, nuint address, ref uint value, nuint size, out nuint written);
	[DllImport("kernel32.dll", SetLastError = true)]
	internal static extern bool VirtualFreeEx(nint process, nuint address, nuint size, uint kind);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern nuint VirtualAllocEx(nint process, nuint address, nuint size, uint kind, uint protection);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern nuint VirtualQueryEx(nint process, nuint address, out MemoryInformation info, nuint size);
	[StructLayout(LayoutKind.Sequential)]
	private struct MemoryInformation
	{
		public nuint BaseAddress, AllocationBase;
		public uint AllocationProtect;
		public nuint RegionSize;
		public uint State, Protect, Type;
	}

	public static void Verify(string executable)
	{
		executable = Path.GetFullPath(executable);
		Require(File.Exists(executable), "Build Native/VehJoinTarget.vcxproj first.");
		using (var legacy = new Fixture(executable))
		{
			var run = legacy.Run("legacy held");
			WaitUntil(() => Read(legacy.Process, run.State) == 2, "legacy flag");
			Win32(VirtualFreeEx(legacy.Process.Handle, run.Code, 0, 0x8000), "free legacy code");
			Require(legacy.Process.WaitForExit(10000), "Legacy control did not fail in the forced window.");
			Require(unchecked((uint)legacy.Process.ExitCode) == 0xC0000005, "Legacy control failed for a reason other than access violation.");
			Console.WriteLine("PASS: old flag-before-return control fails with 0xC0000005 when its code page is freed.");
		}
		using (var fixture = new Fixture(executable))
		{
			int count = 0;
			foreach (string mode in new[] { "return", "inline" })
			foreach (bool high in new[] { false, true })
			foreach (bool held in new[] { true, false })
			{
				int cycles = held ? 16 : 64;
				for (int i = 0; i < cycles; i++)
				{
					var run = fixture.Run($"{mode} {(held ? "held" : "free")} {(high ? "high" : "low")}");
					WaitUntil(() => Read(fixture.Process, run.State) == 2, "VEH completion");
					if (held) Require(Read(fixture.Process, run.Returning) == 0, "Handler escaped the test gate.");
					Win32(VirtualFreeEx(fixture.Process.Handle, run.Code, 0, 0x8000), "free code while VEH can still be active");
					Win32(VirtualQueryEx(fixture.Process.Handle, run.Code, out var info, (nuint)Marshal.SizeOf<MemoryInformation>()) != 0, "query freed code");
					Require(info.State == 0x10000, "Code allocation was not MEM_FREE.");
					// Poison the entire original page: an accidental return cannot pass by
					// finding stale instructions or executable memory reused at this address.
					Require(VirtualAllocEx(fixture.Process.Handle, run.Code, 4096, 0x3000, 1) == run.Code, "Could not poison the released page.");
					Write(fixture.Process, run.Gate, 1);
					fixture.Send("finish");
					string result = fixture.Line();
					Require(result.StartsWith("PASS EIP_delta=", StringComparison.Ordinal), result);
					if (count++ == 0) Console.WriteLine("Observed breakpoint context: " + result);
					Win32(VirtualFreeEx(fixture.Process.Handle, run.Code, 0, 0x8000), "release poisoned reservation");
				}
				Console.WriteLine($"PASS: {cycles} {mode}, {(high ? "above 2 GB" : "default address")}, {(held ? "free before VEH returns" : "immediate polling")}; GP/ESP/EFLAGS/x87/XMM preserved.");
			}
			fixture.Send("quit");
			Require(fixture.Process.WaitForExit(10000) && fixture.Process.ExitCode == 0, "Native fixture shutdown failed.");
			Console.WriteLine($"PASS: {count} reclamations; unrelated exceptions and wrong-thread breakpoints reach SEH.");
		}
		VerifyControllerExit(executable);
	}

	// Launched only by VerifyControllerExit. The test kills this controller after
	// the target has opened its process handle; the target must continue unaided.
	public static void RunController(string executable, string report)
	{
		var start = StartInfo(executable);
		start.ArgumentList.Add("--orphan");
		start.ArgumentList.Add(Environment.ProcessId.ToString());
		start.ArgumentList.Add(report);
		var target = Process.Start(start);
		Require(ReadLine(target) == $"READY {target.Id}", "Orphan target startup failed.");
		Console.WriteLine(target.Id);
		Console.Out.Flush();
		Thread.Sleep(Timeout.Infinite);
	}
	private static void VerifyControllerExit(string executable)
	{
		string report = Path.Combine(Path.GetTempPath(), $"qtr-veh-{Guid.NewGuid():N}.txt");
		var start = StartInfo(Environment.ProcessPath);
		if (Path.GetFileNameWithoutExtension(Environment.ProcessPath).Equals("dotnet", StringComparison.OrdinalIgnoreCase))
			start.ArgumentList.Add(Assembly.GetExecutingAssembly().Location);
		start.ArgumentList.Add("--veh-join-controller");
		start.ArgumentList.Add(executable);
		start.ArgumentList.Add(report);
		using var controller = Process.Start(start);
		Process target = null;
		try
		{
			target = Process.GetProcessById(int.Parse(ReadLine(controller), CultureInfo.InvariantCulture));
			_ = target.Handle; // Keep an exit-status handle before killing the controller.
			controller.Kill();
			Require(controller.WaitForExit(10000), "Could not terminate fixture controller.");
			Require(target.WaitForExit(15000) && target.ExitCode == 0, "Target failed after controller termination.");
			Require(File.ReadAllText(report).StartsWith("PASS: 64 invocations", StringComparison.Ordinal), "Orphan invocations did not complete.");
			Console.WriteLine("PASS: target performs 64 additional calls and cleans up after its controller is forcibly terminated.");
		}
		finally
		{
			if (!controller.HasExited) controller.Kill(entireProcessTree: true);
			if (target != null) { if (!target.HasExited) target.Kill(); target.Dispose(); }
			if (File.Exists(report)) File.Delete(report);
		}
	}

	internal static uint Read(Process process, nuint address)
	{
		Win32(ReadProcessMemory(process.Handle, address, out uint value, 4, out nuint count) && count == 4, "ReadProcessMemory");
		return value;
	}
	internal static void Write(Process process, nuint address, uint value) =>
		Win32(WriteProcessMemory(process.Handle, address, ref value, 4, out nuint count) && count == 4, "WriteProcessMemory");
	internal static void WaitUntil(Func<bool> predicate, string label)
	{
		var timeout = Stopwatch.StartNew();
		while (!predicate())
		{
			if (timeout.ElapsedMilliseconds > 10000) throw new TimeoutException(label);
			Thread.Yield();
		}
	}
	internal static void Win32(bool ok, string message) { if (!ok) throw new Win32Exception(Marshal.GetLastWin32Error(), message); }
	internal static void Require(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
	internal static string ReadLine(Process process)
	{
		var line = process.StandardOutput.ReadLineAsync();
		if (!line.Wait(TimeSpan.FromSeconds(15))) throw new TimeoutException("Fixture response timed out.");
		return line.Result ?? throw new InvalidOperationException("Fixture exited before responding.");
	}
	internal static ProcessStartInfo StartInfo(string executable) => new(executable)
	{
		UseShellExecute = false, CreateNoWindow = true,
		RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true,
	};
	private sealed class Fixture : IDisposable
	{
		public Process Process { get; }
		private readonly Task<string> errors;
		public Fixture(string executable)
		{
			Process = Process.Start(StartInfo(executable));
			errors = Process.StandardError.ReadToEndAsync();
			try { Require(Line() == $"READY {Process.Id}", "Native fixture startup failed."); }
			catch { Dispose(); throw; }
		}
		public void Send(string command) { Process.StandardInput.WriteLine(command); Process.StandardInput.Flush(); }
		public string Line() => ReadLine(Process);
		public (nuint Code, nuint State, nuint Gate, nuint Returning) Run(string mode)
		{
			Send("run " + mode);
			string[] parts = Line().Split(' ');
			Require(parts.Length == 5 && parts[0] == "RUN", "Malformed fixture response.");
			nuint Address(int index) => uint.Parse(parts[index], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			return (Address(1), Address(2), Address(3), Address(4));
		}
		public void Dispose()
		{
			if (!Process.HasExited) { Process.Kill(); Process.WaitForExit(10000); }
			if (errors.Wait(TimeSpan.FromSeconds(2)) && errors.Result.Length > 0) Console.Error.Write(errors.Result);
			Process.Dispose();
		}
	}
}
