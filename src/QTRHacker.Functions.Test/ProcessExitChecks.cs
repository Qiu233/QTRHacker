using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Memory;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace QTRHacker.Functions.Test;

internal static class ProcessExitChecks
{
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool GetExitCodeProcess(nuint process, out uint code);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern uint WaitForSingleObject(nuint handle, uint milliseconds);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool DuplicateHandle(nint sourceProcess, nuint source, nint targetProcess, out nuint copy, uint access, bool inherit, uint options);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(nuint handle);

	private record Observation(string Case, uint WaitResult, bool ExitQuerySucceeded, uint ExitCode, int ExitQueryError,
		bool ReadSucceeded, int ReadError, bool AllocationSucceeded, int AllocationError);

	public static void Verify(string targetExecutable)
	{
		const uint size = 2603566;
		foreach (bool forced in new[] { false, true })
		{
			using var process = Process.Start(new ProcessStartInfo(Path.GetFullPath(targetExecutable))
			{
				UseShellExecute = false, CreateNoWindow = true,
				RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true,
			});
			var errors = process.StandardError.ReadToEndAsync();
			try
			{
				var ready = process.StandardOutput.ReadLineAsync();
				if (!ready.Wait(TimeSpan.FromSeconds(15)) || ready.Result != process.Id.ToString())
					throw new InvalidOperationException("Process-exit fixture did not initialize.");
				using var context = QHackContext.Create(process.Id);
				nuint page = MemoryAllocation.Alloc(context.Handle, 4096);
				if (page == 0) throw new InvalidOperationException("Fixture page allocation failed.");
				context.DataAccess.Write(page, 123);
				var alive = Observe("alive", context.Handle, page, size);
				Require(alive.WaitResult == 258 && alive.ExitQuerySucceeded && alive.ReadSucceeded && alive.AllocationSucceeded, "Live baseline failed.");

				Require(MemoryAllocation.Free(context.Handle, page), "Could not release fixture page.");
				var stale = Observe("alive, freed address", context.Handle, page, size);
				Require(stale.WaitResult == 258 && stale.ExitQuerySucceeded && !stale.ReadSucceeded && stale.AllocationSucceeded, "Freed-address comparison failed.");
				page = MemoryAllocation.Alloc(context.Handle, 4096);
				Require(page != 0, "Could not allocate the exit-test page.");
				context.DataAccess.Write(page, 456);

				// Close only a duplicated handle owned by this test. Keep the original
				// context and target alive to distinguish invalid handles from exit.
				Require(DuplicateHandle(-1, context.Handle, -1, out nuint copy, 0, false, 2), "DuplicateHandle failed.");
				Require(CloseHandle(copy), "CloseHandle failed.");
				var closed = Observe("alive, closed handle", copy, page, size);
				Require(closed.WaitResult == uint.MaxValue && !closed.ExitQuerySucceeded && !closed.ReadSucceeded && !closed.AllocationSucceeded, "Closed-handle comparison failed.");

				Console.WriteLine($"BEFORE EXIT: HasExited={process.HasExited}; forced={forced}");
				if (forced) process.Kill(); // Only the disposable fixture, never a game.
				else process.StandardInput.WriteLine();
				Require(process.WaitForExit(5000), "Fixture did not exit.");
				// The original handle remains open, just as it does in the modifier.
				var exited = Observe(forced ? "terminated, handle retained" : "normal exit, handle retained", context.Handle, page, size);
				Require(exited.WaitResult == 0 && exited.ExitQuerySucceeded && !exited.ReadSucceeded && !exited.AllocationSucceeded, "Exited-process comparison failed.");
				try { using var allocation = new MemoryAllocation(context, size); }
				catch (System.ComponentModel.Win32Exception ex)
				{
					Console.WriteLine($"ACTUAL ALLOCATION EXCEPTION: NativeErrorCode={ex.NativeErrorCode}; Message={ex.Message}");
				}
			}
			finally
			{
				if (!process.HasExited)
				{
					process.StandardInput.WriteLine();
					if (!process.WaitForExit(5000)) process.Kill();
				}
				if (errors.Wait(TimeSpan.FromSeconds(5)) && errors.Result.Length > 0) Console.WriteLine(errors.Result);
			}
		}
	}

	private static Observation Observe(string name, nuint handle, nuint address, uint size)
	{
		uint wait = WaitForSingleObject(handle, 0);
		bool query = GetExitCodeProcess(handle, out uint exitCode);
		int queryError = query ? 0 : Marshal.GetLastWin32Error();
		bool read = true;
		int readError = 0;
		string readMessage = null;
		try { new DataAccess(handle).Read<int>(address); }
		catch (IOException ex)
		{
			read = false;
			readError = ex.HResult & 0xFFFF;
			readMessage = ex.Message;
		}
		// Read before allocating: allocation may reuse the just-freed address.
		nuint allocation = MemoryAllocation.Alloc(handle, size);
		int allocationError = allocation == 0 ? Marshal.GetLastWin32Error() : 0;
		if (allocation != 0) Require(MemoryAllocation.Free(handle, allocation), "Could not free comparison allocation.");
		var observation = new Observation(name, wait, query, exitCode, queryError, read, readError, allocation != 0, allocationError);
		Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(observation));
		if (readMessage != null) Console.WriteLine("ACTUAL READ EXCEPTION: " + readMessage);
		return observation;
	}

	private static void Require(bool condition, string message)
	{
		if (!condition) throw new InvalidOperationException(message);
	}
}
