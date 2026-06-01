using System;
using System.Collections.Generic;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;

namespace QTRHacker.Core;

public static class ItemCheckHookManager
{
	private static readonly object Lock = new();
	private static readonly Dictionary<string, AssemblyCode> Callbacks = new();
	private static InlineHook _hook;
	private static GameContext _ctx;

	public static void Register(GameContext ctx, string name, AssemblyCode code)
	{
		lock (Lock)
		{
			if (_ctx != null && !ReferenceEquals(_ctx, ctx))
			{
				ReleaseHook();
				Callbacks.Clear();
			}
			Callbacks[name] = code;
			_ctx = ctx;
			RebuildHook();
		}
	}

	public static void Unregister(string name)
	{
		lock (Lock)
		{
			if (Callbacks.Remove(name))
				RebuildHook();
		}
	}

	private static void RebuildHook()
	{
		ReleaseHook();
		if (Callbacks.Count == 0)
			return;

		var combined = AssemblySnippet.FromEmpty();
		foreach (var callback in Callbacks.Values)
		{
			combined.Content.Add((Instruction)"pushad");
			combined.Content.Add(callback);
			combined.Content.Add((Instruction)"popad");
		}

		_hook = InlineHook.Hook(
			_ctx.HContext,
			combined,
			new HookParameters(TerrariaHookPoints.GetPlayerItemCheckHookAddress(_ctx), 0x4000, false, true));
	}

	private static void ReleaseHook()
	{
		if (_hook == null)
			return;
		InlineHook.FreeHook(_hook.Context, _hook.Parameters.TargetAddress, forceRelease: false, timeout: 2000);
		_hook = null;
	}
}
