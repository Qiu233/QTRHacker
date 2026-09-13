using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static QTRHacker.Functions.Test.VehJoinChecks;

namespace QTRHacker.Functions.Test;

internal static class VehItemChecks
{
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool FlushInstructionCache(nint process, nuint address, nuint size);
	private sealed class Result
	{
		public string Mode { get; set; }
		public bool Collect { get; set; }
		public bool ForcedWindow { get; set; }
		public int Requested { get; set; }
		public int Completed { get; set; }
		public double Seconds { get; set; }
		public string LastBlock { get; set; }
		public string Failure { get; set; }
		public string TargetErrors { get; set; }
		public int? ExitCode { get; set; }
	}

	public static void Verify(string[] args)
	{
		if (args.Length < 2 || args.Length > 4)
			throw new ArgumentException("Usage: --verify-veh-items <VehItemTarget.exe> <Terraria.exe> [iterations=10000] [repetitions=3]");
		string target = Path.GetFullPath(args[0]), game = Path.GetFullPath(args[1]);
		int iterations = args.Length > 2 ? int.Parse(args[2], CultureInfo.InvariantCulture) : 10000;
		int repetitions = args.Length > 3 ? int.Parse(args[3], CultureInfo.InvariantCulture) : 3;
		Require(iterations > 0 && repetitions > 0, "Iterations and repetitions must be positive.");
		var results = new List<Result>();
		string report = Path.Combine(AppContext.BaseDirectory, "veh-items-results.json");
		for (int trial = 0; trial < repetitions; trial++)
		foreach (bool collect in new[] { false, true })
		foreach (bool veh in trial % 2 == 0 ? new[] { false, true } : new[] { true, false })
		{
			Console.WriteLine($"START trial={trial + 1} {(veh ? "VEH" : "original flag")} GC={collect}, {iterations} real remote Item.SetDefaults calls");
			var result = Run(target, game, veh, collect, iterations);
			results.Add(result);
			File.WriteAllText(report, JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
			Console.WriteLine($"{(result.Failure == null ? "PASS" : "FAIL")}: {result.Mode} GC={collect}, {result.Completed}/{iterations}, {result.Seconds:F2}s, last allocation={result.LastBlock}");
			if (result.Failure != null) Console.WriteLine(result.Failure + "\n" + result.TargetErrors);
		}
		Console.WriteLine("A/B report: " + report);
		Require(results.Where(r => r.Mode == "VEH").All(r => r.Failure == null), "VEH Item.SetDefaults stress failed; inspect the A/B report.");
		Console.WriteLine(results.Any(r => r.Mode == "original flag" && r.Failure != null)
			? "Original failures were observed. Compare their exception addresses with LastBlock before attributing them to reclamation."
			: "Both paths survived this run. This does not reproduce the historical game crash or prove the old flag safe.");
	}

	public static void VerifyWindow(string[] args)
	{
		if (args.Length != 2) throw new ArgumentException("Usage: --verify-veh-items-window <VehItemTarget.exe> <Terraria.exe>");
		var results = new[] {
			Run(Path.GetFullPath(args[0]), Path.GetFullPath(args[1]), false, false, 1, true),
			Run(Path.GetFullPath(args[0]), Path.GetFullPath(args[1]), true, false, 1, true)
		};
		string report = Path.Combine(AppContext.BaseDirectory, "veh-items-window-results.json");
		File.WriteAllText(report, JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
		foreach (var result in results)
			Console.WriteLine($"{result.Mode}: completed={result.Completed}, exit=0x{result.ExitCode:X8}, block={result.LastBlock}\n{result.TargetErrors}");
		Console.WriteLine("Forced-window report: " + report);
		Require(results[0].Failure != null && unchecked((uint)results[0].ExitCode.GetValueOrDefault()) == 0xC0000005,
			"Original Item.SetDefaults control did not fail with access violation in the forced window.");
		var fault = Regex.Match(results[0].TargetErrors, @"NATIVE_CRASH code=C0000005 EIP=([0-9A-F]{8})");
		Require(fault.Success, "Missing original control's native exception address.");
		uint ip = uint.Parse(fault.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		uint block = uint.Parse(results[0].LastBlock.Split('-')[0].Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		Require(ip >= block && ip - block < 4096, "Original control failed outside the released code block.");
		Require(results[1].Failure == null && results[1].Completed == 1, "VEH did not survive freeing code before handler return.");
		Console.WriteLine("PASS: same real Item.SetDefaults call; old tail faults after free, VEH returns correctly after free.");
	}

	private static Result Run(string target, string game, bool veh, bool collect, int iterations, bool forcedWindow = false)
	{
		var result = new Result { Mode = veh ? "VEH" : "original flag", Collect = collect, Requested = iterations, ForcedWindow = forcedWindow };
		var start = StartInfo(target);
		start.ArgumentList.Add(game);
		if (collect) start.ArgumentList.Add("gc");
		using var process = Process.Start(start);
		var errors = process.StandardError.ReadToEndAsync();
		var elapsed = Stopwatch.StartNew();
		Task worker = null;
		try
		{
			string[] ready = ReadLine(process).Split(' ');
			Require(ready.Length == 4 && ready[0] == "READY" && ready[1] == process.Id.ToString(), "CLR target startup failed.");
			nuint record = uint.Parse(ready[2], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			nuint mailbox = uint.Parse(ready[3], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			worker = Task.Run(() =>
			{
				using var context = QHackContext.Create(process.Id);
				var helper = context.GetCLRHelper("VehItemTarget");
				var item = helper.GetStaticHackObject("VehItemTarget.ItemFixture", "Item");
				var method = item.GetMethodCall("Terraria.Item.SetDefaults(Int32, Terraria.GameContent.Items.ItemVariant)");
				nuint itemSlot = helper.GetStaticFieldAddress("VehItemTarget.ItemFixture", "Item");
				nuint tick = helper.GetFunctionAddress("VehItemTarget.ItemFixture", "Tick");
				Require(tick != 0 && method.Method.InternalClrMethod.NativeCode != 0, "Methods were not JIT-compiled.");
				for (int i = 1; i <= iterations; i++)
				{
					// Exactly the same call generator and argument ABI as Item.SetDefaults.
					// The host finishes its explicit GC before acknowledgement. Refresh
					// the receiver for each call instead of keeping an address across GC.
					var body = method.Method.Call(context.DataAccess.Read<nuint>(itemSlot))
						.Call(true, null, null, new object[] { 1 + i % 5000, (nuint)0 });
					var hook = InlineHook.Hook(context, body, new HookParameters(tick, 4096, true, true));
					result.LastBlock = $"0x{hook.MemoryAllocation.AllocationBase:X8}-0x{hook.MemoryAllocation.AllocationBase + 4096:X8}";
					if (veh || forcedWindow) ConfigureTail(context, hook, process, record, veh, forcedWindow);
					Win32(FlushInstructionCache(process.Handle, tick, 32), "flush installed entry");
					Win32(FlushInstructionCache(process.Handle, hook.MemoryAllocation.AllocationBase, 4096), "flush trampoline");
					Write(process, mailbox, (uint)i);
					Require(hook.WaitToDetach(), "Hook did not detach.");
					if (veh)
					{
						WaitUntil(() => Read(process, record) == 2, "VEH left block");
						hook.Dispose();
					}
					else hook.WaitToDispose(); // Unmodified production flag polling/free.
					if (forcedWindow) Write(process, record + 4, 1); // Release the test-only gate AFTER freeing B.
					// This acknowledgement is AFTER freeing in both paths. It is only
					// used to prevent entry/metadata reuse, never to make the free safe.
					WaitUntil(() => Read(process, mailbox + 4) == (uint)i, "managed Tick returned");
					result.Completed = i;
					if (i % 5000 == 0) Console.WriteLine($"  {result.Mode} GC={collect}: {i} completed");
				}
				Write(process, mailbox, uint.MaxValue);
			});
			if (!worker.Wait(TimeSpan.FromMinutes(3))) throw new TimeoutException("A/B invocation worker exceeded 3 minutes.");
			Require(ReadLine(process) == $"PASS {iterations} real Item.SetDefaults calls", "Target validation failed.");
			Require(process.WaitForExit(10000) && process.ExitCode == 0, "Target shutdown failed.");
		}
		catch (Exception ex)
		{
			result.Failure = ex.GetBaseException().ToString();
			// A failed remote read can race the target's exception reporter/exit.
			// Let it finish before classifying the native exception or killing it.
			process.WaitForExit(250);
			if (process.HasExited) result.ExitCode = process.ExitCode;
		}
		finally
		{
			if (!process.HasExited) { process.Kill(); process.WaitForExit(10000); }
			// Killing a hung fixture makes the existing busy ReadProcessMemory polls
			// fail. Wait for that worker before disposing its process/context handles.
			if (worker != null) { try { Require(worker.Wait(TimeSpan.FromSeconds(10)), "A/B worker did not stop."); } catch (AggregateException) { } }
			result.TargetErrors = errors.GetAwaiter().GetResult();
			result.Seconds = elapsed.Elapsed.TotalSeconds;
			result.ExitCode ??= process.ExitCode;
		}
		return result;
	}

	private static void ConfigureTail(QHackContext context, InlineHook hook, Process process, nuint record, bool veh, bool held)
	{
		// Use the SAME production trampoline in A and B, changing only its final
		// flag-store/jump to int3. Derive offsets from its actual header layout;
		// reject a changed emitter instead of silently patching an arbitrary match.
		Type header = typeof(InlineHook).GetNestedType("HookInfo", BindingFlags.NonPublic);
		nuint code = hook.MemoryAllocation.AllocationBase;
		nuint safeFlag = code + (uint)Marshal.OffsetOf(header, "SafeFreeFlag").ToInt32();
		uint rawLength = context.DataAccess.Read<uint>(code + (uint)Marshal.OffsetOf(header, "RawCodeLength").ToInt32());
		byte[] clearFlag = Assembler.Assemble($"mov dword ptr [{safeFlag}],0", 0);
		byte[] bytes = context.DataAccess.ReadBytes(code, 4096);
		int found = -1;
		for (int offset = Marshal.SizeOf(header); offset + clearFlag.Length + 5 <= bytes.Length; offset++)
		{
			if (!bytes.AsSpan(offset, clearFlag.Length).SequenceEqual(clearFlag) || bytes[offset + clearFlag.Length] != 0xE9) continue;
			Require(found < 0, "Ambiguous InlineHook completion tail.");
			found = offset;
		}
		Require(found >= 0, "InlineHook completion tail changed.");
		Write(process, record, 0);
		Write(process, record + 4, held ? 0u : 1u);
		Write(process, record + 8, 0); // handler returning
		Write(process, record + 16, (uint)(code + (uint)found));
		Write(process, record + 20, (uint)(hook.Parameters.TargetAddress + rawLength));
		if (veh) context.DataAccess.WriteBytes(code + (uint)found, new byte[] { 0xCC, 0x0F, 0x0B });
		else
		{
			// TEST ONLY: preserve the original completion store, but stretch the
			// following interval so the controller certainly frees before the JMP.
			nuint tail = code + (uint)(found + clearFlag.Length);
			byte[] heldTail = Assembler.Assemble($"pushfd; waiting: cmp dword ptr [{record + 4}],0; je waiting; popfd; jmp {hook.Parameters.TargetAddress + rawLength}", tail);
			Require(tail + (uint)heldTail.Length <= code + 4096, "Test gate exceeds allocation.");
			context.DataAccess.WriteBytes(tail, heldTail);
		}
		Write(process, record, 1);
	}
}
