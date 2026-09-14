using QHackLib.Memory;
using QTRHacker.Core;
using QTRHacker.Scripts.Functions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test;

// Double-click entry: one automatic run, one output file, no prompts. All remote
// calls below are the existing production implementations, not the VEH prototype.
internal static class FieldDiagnostics
{
	public static int Run(string[] args)
	{
		Console.OutputEncoding = new UTF8Encoding(false);
		using var session = new DiagnosticSession();
		Console.WriteLine("正在自动诊断。完成后，将程序旁的 QTRHacker-Diagnostic-*.log 文件发回即可。");
		Console.WriteLine("输出文件：" + session.OutputPath);
		int result = 1;
		try
		{
			// Developer-only options for read-only smoke checks; never needed by a customer.
			bool once = false;
			int? pid = null;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "--once") { once = true; continue; }
				if (args[i] == "--pid" && ++i < args.Length && int.TryParse(args[i], out int value) && value > 0) pid = value;
				else throw new ArgumentException("Usage: --diagnose [--pid <PID>] [--once]");
			}
			Metadata(session);
			using var process = SelectProcess(pid, session);
			if (process == null)
			{
				session.Log("未找到唯一的 Terraria 进程。请只保留一个游戏、进入世界后重新运行。", true);
				return result = 2;
			}
			session.Observe(process.Id);
			Try(session, "target metadata", () =>
			{
				session.Log($"TARGET PID={process.Id}; started={process.StartTime:O}");
				FileMetadata(session, process.MainModule.FileName);
				foreach (ProcessModule module in process.Modules)
					if (new[] { "clr.dll", "mscorwks.dll", "coreclr.dll" }.Contains(module.ModuleName.ToLowerInvariant()))
					{
						session.Log($"MODULE {module.ModuleName}; Base=0x{unchecked((nuint)module.BaseAddress):X}");
						FileMetadata(session, module.FileName);
						string dac = Path.Combine(Path.GetDirectoryName(module.FileName), "mscordacwks.dll");
						if (File.Exists(dac)) FileMetadata(session, dac);
					}
			});
			session.MemoryMap();
			var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
			var worker = new Thread(() =>
			{
				try { Execute(session, process, once); completion.SetResult(true); }
				catch (Exception ex) { session.Error("automatic diagnostic", ex); completion.SetResult(false); }
			}) { IsBackground = true, Name = "Diagnostic calls" };
			worker.SetApartmentState(ApartmentState.STA);
			worker.Start();
			var elapsed = Stopwatch.StartNew();
			while (!completion.Task.Wait(200) && !session.HasExited && elapsed.Elapsed.TotalSeconds < 120) { }
			if (completion.Task.IsCompleted)
			{
				result = completion.Task.Result ? 0 : 1;
				// Capture exits immediately following a successful remote-call return.
				if (!once) for (int i = 0; i < 25 && !session.HasExited; i++) Thread.Sleep(200);
			}
			else
			{
				session.Log("调用尚未返回：" + (session.HasExited ? "游戏进程已退出。" : "已达到 120 秒诊断时限。"), true);
				// Do not abort the remote call or dispose a context still in use.
				// Ending this diagnostic process does not kill or suspend Terraria.
				result = 3;
			}
			session.Snapshot("automatic run finished");
			if (session.HasExited) result = 3;
		}
		catch (Exception ex) { session.Error("diagnostic startup", ex); }
		finally { session.Log($"FINISHED result={result}; output={session.OutputPath}", true); }
		return result;
	}

	private static Process SelectProcess(int? pid, DiagnosticSession session)
	{
		if (pid.HasValue) return Process.GetProcessById(pid.Value);
		var processes = Process.GetProcessesByName("Terraria");
		try
		{
			foreach (var process in processes) session.Log($"CANDIDATE Terraria PID={process.Id}");
			return processes.Length == 1 ? Process.GetProcessById(processes[0].Id) : null;
		}
		finally { foreach (var process in processes) process.Dispose(); }
	}

	private static void Execute(DiagnosticSession session, Process process, bool readOnly)
	{
		session.Snapshot("ATTACH BEGIN");
		var context = GameContext.OpenGame(process);
		bool pendingPatchLoad = false;
		try
		{
			session.Snapshot("ATTACH OK", context.HContext.Handle);
			bool fieldsValid = GameSnapshot(session, context);
			if (!fieldsValid) throw new InvalidOperationException("读取游戏状态失败，后续调用已跳过。");
			if (readOnly) return;
			if (context.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "gameMenu") ||
				context.Map.BaseAddress == 0 || context.MaxTilesX <= 0 || context.MaxTilesY <= 0)
				throw new InvalidOperationException("游戏尚未进入世界，后续调用已跳过。进入世界后重新运行即可。");
			AllocationProbe(session, context);
			// Resume a game that pauses when losing focus to the diagnostic console.
			bool focused = SetForegroundWindow(process.MainWindowHandle);
			session.Log($"GAME FOREGROUND requested: success={focused}");
			session.Log("开始测试现有的揭示地图功能（会揭示地图），随后测试基础补丁加载。", true);
			_ = System.IO.Packaging.PackUriHelper.UriSchemePack;
			var reveal = new RevealTheWholeMap();
			session.Snapshot("REVEAL MAP BEGIN", context.HContext.Handle);
			reveal.Enable(context);
			session.Snapshot("REVEAL MAP RETURNED", context.HContext.Handle);
			session.Snapshot("PATCH LOAD BEGIN", context.HContext.Handle);
			pendingPatchLoad = true;
			context.Patches.Init();
			pendingPatchLoad = false;
			session.Snapshot("PATCH LOAD RETURNED", context.HContext.Handle);
			session.Log("调用均已返回。此结果只表示调用返回，功能效果需结合游戏表现。", true);
		}
		catch (Exception ex)
		{
			session.Snapshot("CALL FAILED", context.HContext.Handle);
			session.Error("game diagnostic", ex);
			session.MemoryMap();
			throw;
		}
		finally
		{
			// Init can throw a timeout while its worker still uses this handle.
			if (!pendingPatchLoad) context.Dispose();
		}
	}

	private static bool GameSnapshot(DiagnosticSession session, GameContext context)
	{
		bool valid = Try(session, "game fields", () => session.Log($"GAME gameMenu={context.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "gameMenu")}; " +
			$"netMode={context.NetMode}; dimensions={context.MaxTilesX}x{context.MaxTilesY}; myPlayerIndex={context.MyPlayerIndex}"));
		valid &= Try(session, "player array", () =>
		{
			var players = context.Players;
			session.Log($"PLAYERS reference=0x{players.BaseAddress:X}");
			session.Log($"PLAYERS length={players.Length}");
		});
		valid &= Try(session, "map", () => session.Log($"MAP reference=0x{context.Map.BaseAddress:X}; refreshMap={context.RefreshMap}"));
		Try(session, "patch status", () => session.Log($"PATCH alreadyLoaded={context.Patches.IsInitialized}"));
		Try(session, "Update address", () => session.Log($"METHOD Terraria.Main.Update=0x{context.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update"):X}"));
		Try(session, "UpdateLighting address", () => session.Log($"METHOD Terraria.Map.WorldMap.UpdateLighting=0x{context.GameModuleHelper.GetFunctionAddress("Terraria.Map.WorldMap", "UpdateLighting"):X}"));
		return valid;
	}

	private static void AllocationProbe(DiagnosticSession session, GameContext context)
	{
		string path = Path.Combine(AppContext.BaseDirectory, "QTRHacker.Patches.dll");
		uint size = checked((uint)(new FileInfo(path).Length + Encoding.Unicode.GetByteCount("QTRHacker.Patches.Boot\0")));
		session.Log($"ALLOC BEGIN: size={size}; reserve+commit; PAGE_EXECUTE_READWRITE (same as production)");
		nuint address = MemoryAllocation.Alloc(context.HContext.Handle, size);
		int error = address == 0 ? Marshal.GetLastWin32Error() : 0;
		session.Snapshot("allocation returned", context.HContext.Handle);
		session.Log($"ALLOC RESULT: address=0x{address:X}; NativeErrorCode={error}");
		if (address == 0) throw new Win32Exception(error, $"Could not allocate {size} bytes in the target process.");
		try
		{
			var data = context.HContext.DataAccess;
			foreach (uint offset in new[] { 0U, size - 4 })
			{
				data.Write(address + offset, 0x13572468);
				int value = data.Read<int>(address + offset);
				if (value != 0x13572468) throw new IOException($"Round-trip mismatch at 0x{address + offset:X}.");
				session.Log($"ROUNDTRIP address=0x{address + offset:X}; bytes=4; OK");
			}
		}
		finally
		{
			bool freed = MemoryAllocation.Free(context.HContext.Handle, address);
			int freeError = freed ? 0 : Marshal.GetLastWin32Error();
			session.Log($"FREE address=0x{address:X}; success={freed}; NativeErrorCode={freeError}");
			if (!freed) session.Error("allocation probe cleanup", new Win32Exception(freeError));
		}
	}

	private static void Metadata(DiagnosticSession session)
	{
		session.Log($"TOOL {typeof(FieldDiagnostics).Assembly.FullName}; OS={RuntimeInformation.OSDescription}; OSArch={RuntimeInformation.OSArchitecture}; ProcessArch={RuntimeInformation.ProcessArchitecture}; Runtime={RuntimeInformation.FrameworkDescription}; RuntimeDir={RuntimeEnvironment.GetRuntimeDirectory()}");
		if (IntPtr.Size != 4) throw new PlatformNotSupportedException("Use the bundled x86 runtime.");
		string runnerDll = Path.Combine(AppContext.BaseDirectory, "QTRHacker.Functions.Test.dll");
		Try(session, "runner", () => FileMetadata(session, File.Exists(runnerDll) ? runnerDll : Environment.ProcessPath));
		foreach (string name in new[] { "QHackCLR.dll", "QHackLib.dll", "QTRHacker.Core.dll", "QTRHacker.dll", "QTRHacker.Patches.dll", "Ijwhost.dll", "keystone.dll" })
			Try(session, "file " + name, () => FileMetadata(session, Path.Combine(AppContext.BaseDirectory, name)));
	}

	private static void FileMetadata(DiagnosticSession session, string path)
	{
		var info = new FileInfo(path);
		using var stream = File.OpenRead(path);
		session.Log($"FILE {path}; bytes={info.Length}; version={FileVersionInfo.GetVersionInfo(path).FileVersion}; SHA256={Convert.ToHexString(SHA256.HashData(stream))}");
	}

	private static bool Try(DiagnosticSession session, string stage, Action action)
	{
		try { action(); return true; }
		catch (Exception ex) { session.Error(stage, ex); return false; }
	}

	[DllImport("user32.dll")] private static extern bool SetForegroundWindow(nint window);
}
