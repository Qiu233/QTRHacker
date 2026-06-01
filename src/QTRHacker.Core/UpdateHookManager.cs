using System;
using System.Collections.Generic;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;

namespace QTRHacker.Core;

/// <summary>
/// Centralized dispatcher for Main.Update hooks.
/// Multiple features (HighLight, BonusTwoSlots, EnableAllRecipes, SuperRange)
/// all need to run code every frame via Main.Update. Instead of each feature
/// creating its own InlineHook at the same address (which overwrites others),
/// they register their assembly code snippets here and this manager installs
/// a single combined hook.
/// </summary>
public static class UpdateHookManager
{
    private static readonly object _lock = new();
    private static readonly Dictionary<string, AssemblyCode> _callbacks = new();
    private static InlineHook _hook;
    private static GameContext _ctx;
    private static nuint _updateAddr;

    /// <summary>
    /// Register a named assembly code snippet to run on Main.Update every frame.
    /// The code is wrapped in pushad/popad for register safety.
    /// </summary>
    public static void Register(GameContext ctx, string name, AssemblyCode code)
    {
        lock (_lock)
        {
            if (_ctx != null && !ReferenceEquals(_ctx, ctx))
            {
                ReleaseHook();
                _callbacks.Clear();
            }
            _callbacks[name] = code;
            _ctx = ctx;
            RebuildHook();
        }
    }

    /// <summary>
    /// Unregister a named callback and rebuild the hook.
    /// </summary>
    public static void Unregister(string name)
    {
        lock (_lock)
        {
            if (_callbacks.Remove(name))
                RebuildHook();
        }
    }

    /// <summary>
    /// Check if a named callback is registered.
    /// </summary>
    public static bool IsRegistered(string name)
    {
        lock (_lock)
        {
            return _callbacks.ContainsKey(name);
        }
    }

    private static void RebuildHook()
    {
        ReleaseHook();

        if (_callbacks.Count == 0)
            return;

        _updateAddr = TerrariaHookPoints.GetPlayerUpdateHookAddress(_ctx);
        if (_updateAddr == 0)
            return;

        // Build combined assembly from all registered callbacks.
        // Each callback is wrapped in pushad/popad for register safety.
        var combined = AssemblySnippet.FromEmpty();
        foreach (var kvp in _callbacks)
        {
            combined.Content.Add((Instruction)$"pushad");
            combined.Content.Add(kvp.Value);
            combined.Content.Add((Instruction)$"popad");
        }

        _hook = InlineHook.Hook(
            _ctx.HContext,
            combined,
            new HookParameters(_updateAddr, 0x4000, false, true));
    }

    private static void ReleaseHook()
    {
        if (_hook == null)
            return;
        InlineHook.FreeHook(_hook.Context, _hook.Parameters.TargetAddress, forceRelease: false, timeout: 2000);
        _hook = null;
    }
}
