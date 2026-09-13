using GameWikiResExporter;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace VehItemTarget;

public static unsafe class ItemFixture
{
	public static Terraria.Item Item;
	[DllImport("VehItemBridge.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr Initialize();
	[DllImport("VehItemBridge.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int* Mailbox();
	[DllImport("VehItemBridge.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern bool Shutdown();

	public static void Main(string[] args)
	{
		using var runtime = new GameRuntime(Path.GetFullPath(args[0]));
		runtime.Assembly.GetType("Terraria.Program").GetField("SavePath").SetValue(null, AppDomain.CurrentDomain.BaseDirectory);
		Run(args.Length > 1 && args[1] == "gc");
	}
	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Run(bool collect)
	{
		Assembly.Load("GameWikiResExporter").GetType("GameWikiResExporter.WikiExporter")
			.GetMethod("InitializeGameData", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
		// Some obsolete IDs redirect to other types (e.g. 226). Compare remote
		// calls with real managed baselines, not with an assumed type == input.
		var expectedTypes = new int[5001];
		var expectedDamage = new int[5001];
		var baseline = new Terraria.Item();
		for (int id = 1; id <= 5000; id++)
		{
			baseline.SetDefaults(id);
			expectedTypes[id] = baseline.type;
			expectedDamage[id] = baseline.damage;
		}
		Item = new Terraria.Item();
		Item.SetDefaults(1);
		Tick(0);
		IntPtr record = Initialize();
		if (record == IntPtr.Zero) throw new InvalidOperationException("VEH registration failed.");
		int* mailbox = Mailbox();
		Console.WriteLine($"READY {Process.GetCurrentProcess().Id} {unchecked((uint)record.ToInt32()):X8} {unchecked((uint)mailbox):X8}");
		Console.Out.Flush();
		int completed = 0;
		while (true)
		{
			int request = Volatile.Read(ref mailbox[0]);
			if (request < 0) break;
			if (request == completed) { Thread.SpinWait(32); continue; }
			if (Tick(request) != (request ^ 0x13579BDF)) throw new InvalidOperationException("Hook damaged Tick's result/registers.");
			int input = 1 + request % 5000;
			if (Item.type != expectedTypes[input] || Item.damage != expectedDamage[input])
				throw new InvalidOperationException($"SetDefaults({input}) differs from managed baseline: type={Item.type}, damage={Item.damage}.");
			if (collect && request % 32 == 0) { GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); }
			completed = request;
			Volatile.Write(ref mailbox[1], completed);
		}
		if (!Shutdown()) throw new InvalidOperationException("VEH removal failed.");
		Console.WriteLine($"PASS {completed} real Item.SetDefaults calls");
	}

	// Hook a managed method on the same CLR thread, just as Main.Update is hooked.
	// The host publishes a request only after installation and acknowledges it
	// after Tick returns, isolating reclamation from concurrent entry patching.
	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	public static int Tick(int sequence) => sequence ^ 0x13579BDF;
}
