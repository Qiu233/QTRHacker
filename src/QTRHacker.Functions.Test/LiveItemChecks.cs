using QHackLib;
using QHackLib.FunctionHelper;
using QTRHacker.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static QTRHacker.Functions.Test.VehJoinChecks;

namespace QTRHacker.Functions.Test;

internal static class LiveItemChecks
{
	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool CheckRemoteDebuggerPresent(nint process, [MarshalAs(UnmanagedType.Bool)] out bool present);

	private static bool DebuggerAttached(Process game)
	{
		if (!CheckRemoteDebuggerPresent(game.Handle, out bool present))
			throw new Win32Exception(Marshal.GetLastWin32Error(), "Cannot query the game's debugger state.");
		return present;
	}

	public static void Run(string[] args)
	{
		bool watch = args.Length > 2 && args[^1] == "--watch";
		int positionalCount = args.Length - (watch ? 1 : 0);
		if (positionalCount < 2 || positionalCount > 3)
			throw new ArgumentException("Usage: --stress-live-items <Terraria PID> <VehItemTarget.exe> [iterations=10000] [--watch]");
		int pid = int.Parse(args[0], CultureInfo.InvariantCulture);
		int iterations = positionalCount == 3 ? int.Parse(args[2], CultureInfo.InvariantCulture) : 10000;
		Require(iterations > 0, "Iterations must be positive.");
		using var game = Process.GetProcessById(pid);
		Require(game.ProcessName.Equals("Terraria", StringComparison.OrdinalIgnoreCase), "Expected the live Terraria process.");
		string directory = Path.Combine(AppContext.BaseDirectory, $"live-oldflag-{pid}-{DateTime.Now:yyyyMMdd-HHmmss}");
		Directory.CreateDirectory(directory);
		using var log = new StreamWriter(Path.Combine(directory, "controller.log")) { AutoFlush = true };
		void Log(string text) { Console.WriteLine(text); log.WriteLine(text); }
		Log($"LIVE PID={pid}; controller-side loop; requested={iterations}; mode={(watch ? "native-debugger" : "no-debugger")}; reports={directory}");
		Process watcher = null;
		Task<string> watcherErrors = null;
		Task watchOutput = null;
		string phase = "check debugger state";
		int completed = 0;
		int mismatches = 0;
		nuint lastBlock = 0;
		var time = Stopwatch.StartNew();
		try
		{
			bool attached = DebuggerAttached(game);
			Log($"Before test: debuggerAttached={attached}");
			Require(!attached, "Detach the existing debugger before starting this test.");
			if (watch)
			{
				phase = "attach crash collector";
				var start = StartInfo(Path.Combine(AppContext.BaseDirectory, "VehJoinTarget.exe"));
				foreach (string value in new[] { "--watch", pid.ToString(), directory, Environment.ProcessId.ToString() }) start.ArgumentList.Add(value);
				watcher = Process.Start(start);
				watcherErrors = watcher.StandardError.ReadToEndAsync();
				Require(ReadLine(watcher) == $"READY {pid}", "Crash collector did not attach.");
				Require(DebuggerAttached(game), "Crash collector did not register as a debugger.");
				watchOutput = Task.Run(async () =>
				{
					using var writer = new StreamWriter(Path.Combine(directory, "exceptions.log")) { AutoFlush = true };
					string line;
					while ((line = await watcher.StandardOutput.ReadLineAsync()) != null)
					{
						writer.WriteLine(line);
						Console.WriteLine("WATCH: " + line);
					}
				});
			}
			using var context = GameContext.OpenGame(game);
			Require(!context.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "gameMenu"), "Enter a world before stress testing.");
			phase = "load private scratch-item holder (existing loader)";
			Log(phase);
			const string probeType = "VehItemTarget.LiveItemProbe";
			if (context.HContext.GetCLRHelper("VehItemTarget") == null)
				context.LoadAssemblyAsBytes(Path.GetFullPath(args[1]), probeType);
			var helper = context.HContext.GetCLRHelper("VehItemTarget");
			Require(helper?.GetClrType(probeType) != null, "Live probe type is unavailable.");
			var reset = new HackMethod(context.HContext, helper.GetClrMethod(probeType, "Reset"));
			context.RunByHookUpdate(reset.Call(null).Call(true, null, null, Array.Empty<object>()));
			var method = new HackMethod(context.HContext, context.GameModuleHelper.GetClrMethodBySignature("Terraria.Item",
				"Terraria.Item.SetDefaults(Int32, Terraria.GameContent.Items.ItemVariant)"));
			nuint target = context.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update");
			Log($"Main.Update=0x{target:X8}; Item.SetDefaults=0x{method.InternalClrMethod.NativeCode:X8}; netMode={context.NetMode}");
			Log("Main.Update entry bytes: " + Convert.ToHexString(context.HContext.DataAccess.ReadBytes(target, 32)));
			uint typeOffset = context.GameModuleHelper.GetInstanceFieldOffset("Terraria.Item", "type");
			Log("START: each controller iteration installs one unchanged InlineHook, waits/detaches/frees using the original flags. No loop is injected into the game.");
			for (int i = 1; i <= iterations; i++)
			{
				// This is the same instance-call generator as the Item wrapper, with a
				// fresh reference to our privately rooted object on each iteration.
				var item = helper.GetStaticHackObject(probeType, "Scratch");
				int itemType = i % 2 == 0 ? 3063 : 1;
				var body = method.Call(item.BaseAddress).Call(true, null, null, new object[] { itemType, (nuint)0 });
				phase = "install";
				var hook = InlineHook.Hook(context.HContext, body, new HookParameters(target, 4096, true, true));
				lastBlock = hook.MemoryAllocation.AllocationBase;
				phase = "WaitToDetach";
				Require(hook.WaitToDetach(), "Old hook did not detach.");
				phase = "WaitToDispose";
				hook.WaitToDispose();
				phase = "validate item";
				// Validation happens after the original free, never gates reclamation.
				var current = helper.GetStaticHackObject(probeType, "Scratch");
				int actual = context.HContext.DataAccess.Read<int>(current.BaseAddress + 4 + typeOffset);
				if (actual != itemType)
				{
					mismatches++;
					if (mismatches <= 20) Log($"MISMATCH call={i}; requested={itemType}; actual={actual}; before=0x{item.BaseAddress:X8}; after=0x{current.BaseAddress:X8}; block=0x{lastBlock:X8}");
				}
				completed = i;
				if (i % 250 == 0)
				{
					attached = DebuggerAttached(game);
					Require(attached == watch, "Debugger state changed during the test.");
					Log($"PROGRESS {i}/{iterations}; {time.Elapsed.TotalSeconds:F1}s; lastBlock=0x{lastBlock:X8}; debuggerAttached={attached}");
				}
				if (File.Exists(Path.Combine(directory, "stop.request"))) { Log("Stop requested between invocations."); break; }
			}
			phase = "release scratch item";
			var release = new HackMethod(context.HContext, helper.GetClrMethod(probeType, "Release"));
			context.RunByHookUpdate(release.Call(null).Call(true, null, null, new object[] { true }));
			attached = DebuggerAttached(game);
			Require(attached == watch, "Debugger state changed during the test.");
			Log($"SURVIVED: {completed} controller-side remote Item.SetDefaults calls completed in {time.Elapsed.TotalSeconds:F1}s; mismatches={mismatches}; debuggerAttached={attached}; game remains alive.");
		}
		catch (Exception ex)
		{
			game.WaitForExit(500);
			Log($"FAIL: completed={completed}/{iterations}; phase={phase}; elapsed={time.Elapsed.TotalSeconds:F1}s; lastBlock=0x{lastBlock:X8}-0x{lastBlock + 4096:X8}; gameExited={game.HasExited}");
			if (game.HasExited) Log($"GAME EXIT: 0x{game.ExitCode:X8}");
			Log(ex.ToString());
			// Never kill the actual game or blindly detach/free an in-flight hook.
			Environment.ExitCode = 1;
		}
		finally
		{
			if (watcher != null)
			{
				File.WriteAllText(Path.Combine(directory, "watch.stop"), "stop");
				if (!watcher.WaitForExit(10000)) watcher.Kill(); // Kill-on-debugger-exit is disabled.
				watchOutput?.Wait(TimeSpan.FromSeconds(5));
				if (watcherErrors.Wait(TimeSpan.FromSeconds(2)) && watcherErrors.Result.Length > 0) Log(watcherErrors.Result);
				watcher.Dispose();
			}
		}
	}
}
