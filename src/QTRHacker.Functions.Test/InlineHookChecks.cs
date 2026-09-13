using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace QTRHacker.Functions.Test;

internal static class InlineHookChecks
{
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern nuint CreateRemoteThread(nuint process, nuint attributes, nuint stackSize,
		nuint startAddress, nuint parameter, uint flags, out uint threadId);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern uint WaitForSingleObject(nuint handle, uint timeout);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool GetExitCodeThread(nuint thread, out uint exitCode);
	[DllImport("kernel32.dll")]
	private static extern bool CloseHandle(nuint handle);

	public static void Verify(string targetExecutable)
	{
		using var process = Process.Start(new ProcessStartInfo(Path.GetFullPath(targetExecutable))
		{
			UseShellExecute = false, CreateNoWindow = true,
			RedirectStandardInput = true, RedirectStandardOutput = true,
		});
		try
		{
			var ready = process.StandardOutput.ReadLineAsync();
			if (!ready.Wait(TimeSpan.FromSeconds(15)) || ready.Result != process.Id.ToString())
				throw new InvalidOperationException("CLR fixture did not start.");
			using var context = QHackContext.Create(process.Id);
			using var target = new MemoryAllocation(context);
			using var counter = new MemoryAllocation(context);
			// Native thread entry: five relocatable NOPs, return 42, pop its argument.
			byte[] original = { 0x90, 0x90, 0x90, 0x90, 0x90, 0xB8, 42, 0, 0, 0, 0xC2, 4, 0 };
			context.DataAccess.WriteBytes(target.AllocationBase, original);
			var body = AssemblySnippet.FromCode(new[] { (Instruction)$"inc dword ptr [{counter.AllocationBase}]" });
			for (int i = 1; i <= 20; i++)
			{
				using var hook = InlineHook.Hook(context, body, new HookParameters(target.AllocationBase, 4096, true));
				RunTarget();
				RunTarget();
				Require(context.DataAccess.Read<int>(counter.AllocationBase) == i, "Hook body ran more than once.");
				Require(hook.WaitToDetach(), "Hook did not detach.");
				Require(context.DataAccess.ReadBytes(target.AllocationBase, (uint)original.Length).SequenceEqual(original), "Original instructions were not restored.");
				RunTarget();
				Require(context.DataAccess.Read<int>(counter.AllocationBase) == i, "Detached hook still executed.");
				// Regression: the restored NOPs must not be interpreted as a JMP displacement here.
				hook.WaitToDispose();
				Require(hook.IsDisposed, "Hook allocation was not disposed.");
				try
				{
					context.DataAccess.Read<byte>(hook.MemoryAllocation.AllocationBase);
					throw new InvalidOperationException("Hook allocation is still readable after disposal.");
				}
				catch (IOException) { }
			}
			Console.WriteLine("PASS: 20 one-shot hook cycles; body executes once, original code is restored, detached header remains readable, and allocation is released.");

			void RunTarget()
			{
				nuint thread = CreateRemoteThread(context.Handle, 0, 0, target.AllocationBase, 0, 0, out _);
				Require(thread != 0, "Could not start fixture native thread.");
				try
				{
					Require(WaitForSingleObject(thread, 5000) == 0, "Fixture native thread timed out.");
					Require(GetExitCodeThread(thread, out uint exitCode) && exitCode == 42, "Hook changed the original function result.");
				}
				finally { CloseHandle(thread); }
			}
		}
		finally
		{
			if (!process.HasExited)
			{
				process.StandardInput.WriteLine();
				if (!process.WaitForExit(5000)) process.Kill();
			}
		}
	}

	private static void Require(bool condition, string message)
	{
		if (!condition) throw new InvalidOperationException(message);
	}
}
