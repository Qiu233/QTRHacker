using QHackLib;
using QTRHacker.Core;
using QTRHacker.Scripts.Functions;
using System;
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
		Console.WriteLine("正在诊断“解锁所有研究”。请保持游戏在世界中，并关闭旅行模式菜单。");
		Console.WriteLine("完成后，将程序旁的 QTRHacker-Diagnostic-*.log 文件发回即可。");
		Console.WriteLine("输出文件：" + session.OutputPath);
		UnhandledExceptionEventHandler unhandled = (_, e) => session.Error(
			$"diagnostic process unhandled; terminating={e.IsTerminating}",
			e.ExceptionObject as Exception ?? new Exception(Convert.ToString(e.ExceptionObject)));
		EventHandler<UnobservedTaskExceptionEventArgs> unobserved = (_, e) => session.Error("unobserved background task", e.Exception);
		AppDomain.CurrentDomain.UnhandledException += unhandled;
		TaskScheduler.UnobservedTaskException += unobserved;
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
			}
			else
			{
				session.Log("诊断尚未完成：" + (session.HasExited ? "游戏进程已退出，执行或后续观测已中止。" : "已达到 120 秒诊断时限。"), true);
				// Do not abort the remote call or dispose a context still in use.
				// Ending this diagnostic process does not kill or suspend Terraria.
				result = 3;
			}
			session.Snapshot("automatic run finished");
			if (session.HasExited) result = 3;
		}
		catch (Exception ex) { session.Error("diagnostic startup", ex); }
		finally
		{
			session.Log($"FINISHED result={result}; output={session.OutputPath}", true);
			AppDomain.CurrentDomain.UnhandledException -= unhandled;
			TaskScheduler.UnobservedTaskException -= unobserved;
		}
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
		bool callStarted = false;
		try
		{
			session.Snapshot("ATTACH OK", context.HContext.Handle);
			bool fieldsValid = GameSnapshot(session, context);
			if (!fieldsValid) throw new InvalidOperationException("读取游戏状态失败，后续调用已跳过。");
			if (readOnly) return;
			if (context.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "gameMenu") ||
				context.MaxTilesX <= 0 || context.MaxTilesY <= 0)
				throw new InvalidOperationException("游戏尚未进入世界，后续调用已跳过。进入世界后重新运行即可。");
			HackObject menu = context.GameModuleHelper.GetStaticHackObject("Terraria.Main", "CreativeMenu");
			bool menuEnabled = ((HackValue)menu.InternalGetMember("<Enabled>k__BackingField")).InternalConvert<bool>();
			session.Log($"CREATIVE MENU reference=0x{menu.BaseAddress:X}; enabled={menuEnabled}");
			if (menuEnabled)
				throw new InvalidOperationException("请先关闭/折叠旅行模式菜单后重新运行。本次没有执行解锁。");
			Action<string> research = null;
			Try(session, "research observation setup", () => research = CreateResearchObserver(session, context));
			Try(session, "RegisterItemSacrifice signature", () =>
			{
				var method = context.GameModuleHelper.GetClrMethod(
					"Terraria.GameContent.Creative.ItemsSacrificedUnlocksTracker", "RegisterItemSacrifice");
				session.Log($"RESEARCH METHOD signature={method.Signature}; MethodDesc=0x{method.ClrHandle:X}; NativeCode=0x{method.NativeCode:X}");
			});
			foreach (string name in new[] { "System.Type", "System.Runtime.InteropServices.Marshal", "System.Threading.Tasks.Task", "System.Action" })
				Try(session, "remote thread prerequisite " + name, () =>
				{
					var type = context.HContext.BCLHelper.GetClrType(name)
						?? throw new InvalidOperationException("Required BCL type is not loaded: " + name);
					session.Log($"BCL PREREQUISITE {name}; MethodTable=0x{type.ClrHandle:X}; TypeDef=0x{type.MDToken:X8}");
				});
			research?.Invoke("before call");
			// Resume a game that pauses when losing focus to the diagnostic console.
			bool focused = SetForegroundWindow(process.MainWindowHandle);
			session.Log($"GAME FOREGROUND requested: success={focused}");
			session.Log("开始执行一次现有的“解锁所有研究”（会改变研究进度），随后继续观测 30 秒。", true);
			_ = System.IO.Packaging.PackUriHelper.UriSchemePack;
			var unlock = new UnlockAllDuplications();
			session.Snapshot("UNLOCK RESEARCH BEGIN", context.HContext.Handle);
			callStarted = true;
			try
			{
				unlock.Enable(context);
				session.Snapshot("UNLOCK RESEARCH ENABLE RETURNED", context.HContext.Handle);
				session.Log("功能入口已返回，继续观测游戏状态与研究数据。", true);
			}
			finally
			{
				// Older installed builds can return while their Task is still running.
				// Observe after success or failure, without disposing a handle still in use.
				for (int i = 0; i < 30 && !session.HasExited; i++)
				{
					research?.Invoke("after call " + i + "s");
					Thread.Sleep(1000);
				}
				session.Snapshot("UNLOCK RESEARCH OBSERVATION END", context.HContext.Handle);
			}
			session.Log("观测结束。入口返回或进程存活不等于所有研究已经解锁，请结合研究数据与游戏表现判断。", true);
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
			// No completion/join handle is exposed by the production function. Keep
			// this context alive until process exit instead of closing a live call's handle.
			if (!callStarted) context.Dispose();
			GC.KeepAlive(context);
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
		Try(session, "Update address", () => session.Log($"METHOD Terraria.Main.Update=0x{context.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update"):X}"));
		return valid;
	}

	private static Action<string> CreateResearchObserver(DiagnosticSession session, GameContext context)
	{
		var helper = context.GameModuleHelper;
		nuint playerIndexSlot = helper.GetStaticFieldAddress("Terraria.Main", "myPlayer");
		nuint playersSlot = helper.GetStaticFieldAddress("Terraria.Main", "player");
		var creative = helper.GetClrType("Terraria.Player").GetInstanceFieldByName("creativeTracker");
		var sacrifices = creative.Type.GetInstanceFieldByName("ItemSacrifices");
		var editId = sacrifices.Type.GetInstanceFieldByName("<LastEditId>k__BackingField");
		uint creativeOffset = 4 + creative.Offset, sacrificesOffset = 4 + sacrifices.Offset, editOffset = 4 + editId.Offset;
		var data = context.HContext.DataAccess;
		// Resolve metadata before the production Task starts. Observation uses only
		// ReadProcessMemory, follows current roots, and never races a DAC query/Flush.
		return stage => Try(session, "research snapshot " + stage, () =>
		{
			int index = data.Read<int>(playerIndexSlot);
			nuint players = Reference(playersSlot);
			int length = data.Read<int>(players + 4);
			if (index < 0 || index >= length) throw new InvalidOperationException("Player index changed outside the player array.");
			nuint player = Reference(players + 8 + checked((uint)index * 4));
			nuint tracker = Reference(player + creativeOffset);
			nuint items = Reference(tracker + sacrificesOffset);
			session.Log($"RESEARCH {stage}: playerIndex={index}; player=0x{player:X}; creativeTracker=0x{tracker:X}; ItemSacrifices=0x{items:X}; LastEditId={data.Read<int>(items + editOffset)}");
		});
		nuint Reference(nuint slot)
		{
			nuint value = data.Read<nuint>(slot);
			if (value == 0) throw new InvalidOperationException($"Research reference at 0x{slot:X} is null.");
			return value;
		}
	}

	private static void Metadata(DiagnosticSession session)
	{
		session.Log($"SCENARIO unlock-all-research; calls=1; observeAfterReturn=30s; totalLimit=120s; runnerMvid={typeof(FieldDiagnostics).Module.ModuleVersionId}");
		session.Log($"TOOL {typeof(FieldDiagnostics).Assembly.FullName}; OS={RuntimeInformation.OSDescription}; OSArch={RuntimeInformation.OSArchitecture}; ProcessArch={RuntimeInformation.ProcessArchitecture}; Runtime={RuntimeInformation.FrameworkDescription}; RuntimeDir={RuntimeEnvironment.GetRuntimeDirectory()}");
		if (IntPtr.Size != 4) throw new PlatformNotSupportedException("Use the bundled x86 runtime.");
		Try(session, "runner host", () => FileMetadata(session, Environment.ProcessPath));
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
