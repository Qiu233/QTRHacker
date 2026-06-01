using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using GameObjectArray = QTRHacker.Core.GameObjects.GameObjectArray;
using TerrariaItem = QTRHacker.Core.GameObjects.Terraria.Item;
using XnaVector2 = QTRHacker.Core.GameObjects.ValueTypeRedefs.Xna.Vector2;

namespace QTRHacker.Functions.Test;

unsafe class Program
{
    unsafe static void Main(string[] args)
    {
        if (HasArg(args, "--static"))
        {
            RunStaticTests();
            return;
        }

        var procs = Process.GetProcessesByName("Terraria");
        if (procs.Length == 0)
        {
            Console.WriteLine("Terraria not running!");
            return;
        }
        using GameContext ctx = GameContext.OpenGame(procs[0]);
        var helper = ctx.GameModuleHelper;
        bool diagnostic = HasArg(args, "--diagnostic");
        bool smoke = HasArg(args, "--smoke");

        if (HasArg(args, "--patches-only"))
        {
            SmokePatches(ctx);
            DumpPatchModules(ctx);
            return;
        }

        if (HasArg(args, "--load-patches-bytes-only"))
        {
            SmokePatchLoadMode(ctx, useLoadFrom: false);
            DumpPatchModules(ctx);
            return;
        }

        if (HasArg(args, "--load-patches-from-only"))
        {
            SmokePatchLoadMode(ctx, useLoadFrom: true);
            DumpPatchModules(ctx);
            return;
        }

        if (HasArg(args, "--dump-modules"))
        {
            DumpAllModules(ctx);
            return;
        }

        if (HasArg(args, "--runtime-actions"))
        {
            SmokePatchRuntimeActions(ctx);
            return;
        }

        if (diagnostic)
        {
            Console.WriteLine("=== VTable Method Diagnostic ===\n");

            Console.WriteLine("--- Terraria.Main ---");
            try
            {
                var type = helper.GetClrType("Terraria.Main");
                if (type == null)
                {
                    Console.WriteLine("  Type 'Terraria.Main' NOT FOUND!");
                }
                else
                {
                    Console.WriteLine($"  IsShared: {type.IsShared}");
                    Console.WriteLine($"  Declared Methods count: {type.Methods.Count}");
                    var methods = type.MethodsInVTable;
                    Console.WriteLine($"  MethodsInVTable count: {methods.Count}");
                    bool foundUpdate = false;
                    for (int i = 0; i < methods.Count; i++)
                    {
                        string name = methods[i].Name ?? "(null)";
                        if (name == "Update")
                        {
                            Console.WriteLine($"  [{i}] {name} *** FOUND ***");
                            foundUpdate = true;
                        }
                        else if (i < 30 || name.Contains("Update"))
                        {
                            Console.WriteLine($"  [{i}] {name}");
                        }
                    }
                    if (!foundUpdate)
                        Console.WriteLine("  'Update' NOT FOUND in VTable methods!");
                    Console.WriteLine($"  ... ({methods.Count} total methods)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR: {ex.Message}");
            }

            Console.WriteLine("\n--- Terraria.Player ---");
            try
            {
                var type = helper.GetClrType("Terraria.Player");
                if (type == null)
                {
                    Console.WriteLine("  Type 'Terraria.Player' NOT FOUND!");
                }
                else
                {
                    Console.WriteLine($"  Declared Methods count: {type.Methods.Count}");
                    foreach (var m in type.Methods.Where(m => m.Name == "GetItemGrabRange" || m.Name == "AddBuff"))
                        Console.WriteLine($"  declared {m.Name}: {m.Signature} @ 0x{m.NativeCode:X}");
                    var methods = type.MethodsInVTable;
                    Console.WriteLine($"  MethodsInVTable count: {methods.Count}");
                    bool found = false;
                    for (int i = 0; i < methods.Count; i++)
                    {
                        string name = methods[i].Name ?? "(null)";
                        if (name == "GetItemGrabRange" || name == "AddBuff")
                        {
                            Console.WriteLine($"  [{i}] {name} *** FOUND ***");
                            found = true;
                        }
                    }
                    if (!found)
                        Console.WriteLine("  'GetItemGrabRange' and 'AddBuff' NOT FOUND!");
                    Console.WriteLine("  First 20 methods:");
                    for (int i = 0; i < Math.Min(20, methods.Count); i++)
                        Console.WriteLine($"    [{i}] {methods[i].Name}");
                    Console.WriteLine($"  ... ({methods.Count} total)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR: {ex.Message}");
            }
        }

        Console.WriteLine("\n--- Method Address Tests ---");
        Method(helper, "Terraria.Main", "Update");
        Method(helper, "Terraria.Main", "get_LocalPlayer");
        Method(helper, "Terraria.Player", "GetItemGrabRange");
        MethodCount(helper, "Terraria.Player", "GetItemGrabRange", 2);
        Method(helper, "Terraria.Player", "AddBuff");
        Method(helper, "Terraria.Recipe", "UpdateRecipeList");
        Method(helper, "Terraria.Map.WorldMap", "UpdateLighting");
        Method(helper, "Terraria.GameContent.Creative.ItemsSacrificedUnlocksTracker", "RegisterItemSacrifice");
        Method(helper, "Terraria.Item", m => m.Name == "NewItem" && m.Signature.Contains("IEntitySource") && m.Signature.Contains("Int32"));
        Method(helper, "Terraria.NPC", m => m.Name == "NewNPC" && m.Signature.Contains("IEntitySource"));
        Method(helper, "Terraria.Projectile", m => m.Name == "NewProjectile" && m.Signature.Contains("IEntitySource"));

        Console.WriteLine("\n--- Static Field Tests ---");
        Console.WriteLine($"  OK Terraria.ID.ItemID.Count fallback = {GameConstants.MaxItemTypesFallback}");
        Console.WriteLine($"  OK Terraria.ID.NPCID.Count fallback = {GameConstants.MaxNPCTypesFallback}");
        StaticField<int>(helper, "Terraria.Recipe", "maxRecipes");
        StaticField<int>(helper, "Terraria.Main", "numAvailableRecipes");
        StaticField<bool>(helper, "Terraria.GameContent.Events.Sandstorm", "Happening");
        StaticField<bool>(helper, "Terraria.GameContent.Events.LanternNight", "ManualLanterns");

        Console.WriteLine("\n--- Field Offset Tests ---");
        Offset(helper, "Terraria.Player", "difficulty");
        Offset(helper, "Terraria.Player", "statLife");
        Offset(helper, "Terraria.Player", "statMana");
        Offset(helper, "Terraria.Player", "breath");
        Offset(helper, "Terraria.Player", "maxMinions");
        Offset(helper, "Terraria.Player", "maxTurrets");
        Offset(helper, "Terraria.Player", "wingTime");
        Offset(helper, "Terraria.Player", "empressBrooch");
        Offset(helper, "Terraria.Player", "slowFall");
        Offset(helper, "Terraria.Player", "findTreasure");
        Offset(helper, "Terraria.Player", "moveSpeed");
        Offset(helper, "Terraria.Player", "boneArmor");
        Offset(helper, "Terraria.Player", "extraAccessory");
        Offset(helper, "Terraria.Player", "wallSpeed");
        Offset(helper, "Terraria.Player", "tileSpeed");
        Offset(helper, "Terraria.Player", "rulerGrid");
        Offset(helper, "Terraria.Player", "rulerLine");
        Offset(helper, "Terraria.Player", "InfoAccMechShowWires");
        Offset(helper, "Terraria.Player", "accJarOfSouls");
        Offset(helper, "Terraria.Entity", "position");

        if (diagnostic)
        {
            Console.WriteLine("\n--- AOB Pattern Tests ---");
            Aob(ctx, "CreativeMenu", $"80 B8 {MOff(helper, "Terraria.Player", "difficulty")} 03 74");
            Aob(ctx, "InfiniteLife", $"29 82 {MOff(helper, "Terraria.Player", "statLife")} 83 7D");
            AobAsm(ctx, "InfiniteMana/sub-edi", $"sub [esi+{OffsetForAsm(helper, "Terraria.Player", "statMana")}],edi");
            AobAsm(ctx, "InfiniteMana/sub-eax", $"sub [esi+{OffsetForAsm(helper, "Terraria.Player", "statMana")}],eax");
            AobAsm(ctx, "InfiniteOxygen", $"dec dword ptr [eax+{OffsetForAsm(helper, "Terraria.Player", "breath")}]\ncmp dword ptr [eax+{OffsetForAsm(helper, "Terraria.Player", "breath")}],0");
            AobAsm(ctx, "InfiniteMinion", $"mov dword ptr [esi+{OffsetForAsm(helper, "Terraria.Player", "maxMinions")}],1\nmov dword ptr [esi+{OffsetForAsm(helper, "Terraria.Player", "maxTurrets")}],1");
            Aob(ctx, "InfiniteAmmo/old1", "FF 88 B0 00 00 00 8B 45 E0 83 B8");
            Aob(ctx, "InfiniteAmmo/old2", "FF 89 B0 00 00 00 8B 45 0C 8B 55 F4");
            Aob(ctx, "InfiniteFlyTime", $"D9 99 {MOff(helper, "Terraria.Player", "wingTime")} 80 B9 {MOff(helper, "Terraria.Player", "empressBrooch")} 00");
            Aob(ctx, "SlowFall", $"88 96 {MOff(helper, "Terraria.Player", "slowFall")} 88 96 {MOff(helper, "Terraria.Player", "findTreasure")}");
            Aob(ctx, "FastSpeed", $"D9 E8 D9 9E {MOff(helper, "Terraria.Player", "moveSpeed")} 88 96 {MOff(helper, "Terraria.Player", "boneArmor")}");
            Aob(ctx, "FishCratesOnly", "8B 45 0C C6 00 00 8B 45 08 C6 00 00 B9");
            Aob(ctx, "FastTileAndWallPlacingSpeed", $"D9 E8 D9 9E {MOff(helper, "Terraria.Player", "wallSpeed")} D9 E8 D9 9E {MOff(helper, "Terraria.Player", "tileSpeed")} 88 96");
            Aob(ctx, "MachanicalRuler", $"88 96 {MOff(helper, "Terraria.Player", "rulerGrid")} C6 86 {MOff(helper, "Terraria.Player", "rulerLine")} 01");
            Aob(ctx, "MachanicalLens", $"88 96 {MOff(helper, "Terraria.Player", "InfoAccMechShowWires")} 88 96 {MOff(helper, "Terraria.Player", "accJarOfSouls")}");
            DescribeAob(ctx, "SlowFall owner", $"88 96 {MOff(helper, "Terraria.Player", "slowFall")} 88 96 {MOff(helper, "Terraria.Player", "findTreasure")}");
            DescribeAob(ctx, "FastSpeed owner", $"D9 E8 D9 9E {MOff(helper, "Terraria.Player", "moveSpeed")} 88 96 {MOff(helper, "Terraria.Player", "boneArmor")}");
            DescribeAob(ctx, "MechanicalLens owner", $"88 96 {MOff(helper, "Terraria.Player", "InfoAccMechShowWires")} 88 96 {MOff(helper, "Terraria.Player", "accJarOfSouls")}");
            ProbeHookCandidates(ctx, helper);
        }

        if (smoke)
            RunFunctionSmokeTests(ctx);
        SmokePatches(ctx);

        if (diagnostic)
        {
            Console.WriteLine("\n--- DacpMethodTableData for Terraria.Main ---");
            try
            {
                var type = helper.GetClrType("Terraria.Main");
                if (type != null)
                    Console.WriteLine($"  wNumVtableSlots (via MethodsInVTable.Count): {type.MethodsInVTable.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR: {ex.Message}");
            }
        }

        Console.WriteLine("\n=== Done ===");
        if (!Console.IsInputRedirected)
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    private static bool HasArg(string[] args, string arg)
        => args.Any(a => string.Equals(a, arg, StringComparison.OrdinalIgnoreCase));

    private static void Method(CLRHelper helper, string typeName, string methodName)
    {
        try
        {
            var method = helper.GetClrMethod(typeName, methodName);
            PrintMethodAddress(helper, typeName + "." + methodName, method);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL {typeName}.{methodName}: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void Method(CLRHelper helper, string typeName, Predicate<ClrMethod> filter)
    {
        try
        {
            var method = helper.GetClrMethod(typeName, filter);
            PrintMethodAddress(helper, method.Signature, method);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL {typeName}.<filter>: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void PrintMethodAddress(CLRHelper helper, string label, ClrMethod method)
    {
        nuint addr = helper.GetNativeCode(method);
        string state = addr == 0 ? "FAIL" : "OK";
        Console.WriteLine(
            $"  {state} {label}: code=0x{addr:X}, hasNative={method.HasNativeCode}, hr=0x{method.MethodDataResult:X8}, rep=0x{method.RepresentativeEntryAddress:X}, repHr=0x{method.RepresentativeEntryResult:X8}, slot=0x{method.AddressOfNativeCodeSlot:X}, md=0x{method.ClrHandle:X}, mt=0x{method.MethodTable:X}");
    }

    private static void MethodCount(CLRHelper helper, string typeName, string methodName, int expectedAtLeast)
    {
        try
        {
            var type = helper.GetClrType(typeName);
            int count = type.Methods.Count(m => m.Name == methodName && helper.GetNativeCode(m) != 0);
            Console.WriteLine(count >= expectedAtLeast
                ? $"  OK {typeName}.{methodName} overloads: {count}"
                : $"  FAIL {typeName}.{methodName} overloads: {count}, expected >= {expectedAtLeast}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL {typeName}.{methodName} overload count: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void StaticField<T>(CLRHelper helper, string typeName, string fieldName) where T : unmanaged
    {
        try
        {
            nuint addr = helper.GetStaticFieldAddress(typeName, fieldName);
            T value = helper.GetStaticFieldValue<T>(typeName, fieldName);
            Console.WriteLine($"  OK {typeName}.{fieldName}: 0x{addr:X} = {value}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL {typeName}.{fieldName}: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void Offset(CLRHelper helper, string typeName, string fieldName)
    {
        try
        {
            uint raw = helper.GetInstanceFieldOffset(typeName, fieldName);
            Console.WriteLine($"  OK {typeName}.{fieldName}: raw=0x{raw:X}, object=0x{raw + (uint)IntPtr.Size:X}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL {typeName}.{fieldName}: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static string MOff(CLRHelper helper, string typeName, string fieldName)
    {
        return AobscanHelper.GetMByteCode(OffsetForAsm(helper, typeName, fieldName));
    }

    private static int OffsetForAsm(CLRHelper helper, string typeName, string fieldName)
    {
        return checked((int)helper.GetInstanceFieldOffset(typeName, fieldName) + IntPtr.Size);
    }

    private static void Aob(GameContext ctx, string name, string pattern)
    {
        try
        {
            nuint[] matches = AobscanHelper.Aobscan(ctx.HContext.Handle, pattern).Take(8).ToArray();
            Console.WriteLine($"  {name}: {matches.Length}{(matches.Length > 0 ? $" first=0x{matches[0]:X}" : "")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  {name}: ERROR {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void AobAsm(GameContext ctx, string name, string asm)
    {
        try
        {
            nuint[] matches = AobscanHelper.AobscanASM(ctx.HContext.Handle, asm).Take(8).ToArray();
            Console.WriteLine($"  {name}: {matches.Length}{(matches.Length > 0 ? $" first=0x{matches[0]:X}" : "")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  {name}: ERROR {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void DescribeAob(GameContext ctx, string name, string pattern)
    {
        try
        {
            foreach (nuint addr in AobscanHelper.Aobscan(ctx.HContext.Handle, pattern).Take(3))
            {
                Console.WriteLine($"  {name}: {QHackCLR.Utils.DescribeCodeAddress(ctx.HContext.Runtime.DacLibrary.SOSDac, addr)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  {name}: ERROR {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void ProbeHookCandidates(GameContext ctx, CLRHelper helper)
    {
        Console.WriteLine("\n--- Hook Candidate Runtime Probe ---");
        var candidates = new List<(string Name, nuint Address)>();
        AddProbeCandidates(candidates, ctx, "SlowFall",
            $"88 96 {MOff(helper, "Terraria.Player", "slowFall")} 88 96 {MOff(helper, "Terraria.Player", "findTreasure")}",
            new[] { 0, 6, 12 });
        AddProbeCandidates(candidates, ctx, "FastSpeed",
            $"D9 E8 D9 9E {MOff(helper, "Terraria.Player", "moveSpeed")} 88 96 {MOff(helper, "Terraria.Player", "boneArmor")}",
            new[] { 0, 2, 8, 14 });
        AddProbeCandidates(candidates, ctx, "MinionSlots",
            $"C7 86 {MOff(helper, "Terraria.Player", "maxMinions")} 01 00 00 00 C7 86 {MOff(helper, "Terraria.Player", "maxTurrets")} 01 00 00 00",
            new[] { 0, 10, 20 });
        AddProbeCandidates(candidates, ctx, "TileAndWallSpeed",
            $"D9 E8 D9 9E {MOff(helper, "Terraria.Player", "wallSpeed")} D9 E8 D9 9E {MOff(helper, "Terraria.Player", "tileSpeed")} 88 96",
            new[] { 0, 2, 8, 10, 16, 22 });
        AddProbeCandidates(candidates, ctx, "MechanicalLens",
            $"88 96 {MOff(helper, "Terraria.Player", "InfoAccMechShowWires")} 88 96 {MOff(helper, "Terraria.Player", "accJarOfSouls")}",
            new[] { 0, 6, 12 });
        AddProbeCandidates(candidates, ctx, "MechanicalRuler",
            $"88 96 {MOff(helper, "Terraria.Player", "rulerGrid")} C6 86 {MOff(helper, "Terraria.Player", "rulerLine")} 01",
            new[] { 0, 6, 13 });
        AddProbeCandidates(candidates, ctx, "InfiniteFlyTime",
            $"D9 99 {MOff(helper, "Terraria.Player", "wingTime")} 80 B9 {MOff(helper, "Terraria.Player", "empressBrooch")} 00",
            new[] { 0, 6, 13 });

        using MemoryAllocation flag = new(ctx.HContext, 0x1000);
        for (int i = 0; i < candidates.Count; i++)
        {
            flag.Write(0, 0);
            var candidate = candidates[i];
            try
            {
                var hook = InlineHook.Hook(ctx.HContext,
                    AssemblySnippet.FromASMCode($"mov dword ptr [{flag.AllocationBase}],{i + 1}"),
                    new HookParameters(candidate.Address, 0x1000, false, true));
                Thread.Sleep(500);
                int value = flag.Read<int>(0);
                InlineHook.FreeHook(ctx.HContext, candidate.Address, forceRelease: false, timeout: 1000);
                Console.WriteLine(value == i + 1
                    ? $"  HIT {candidate.Name} @ 0x{candidate.Address:X}"
                    : $"  miss {candidate.Name} @ 0x{candidate.Address:X}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR {candidate.Name} @ 0x{candidate.Address:X}: {ex.GetType().Name}: {ex.Message}");
                try { InlineHook.FreeHook(ctx.HContext, candidate.Address, forceRelease: true, timeout: 100); } catch { }
            }
        }
    }

    private static void AddProbeCandidates(List<(string Name, nuint Address)> candidates, GameContext ctx, string name, string pattern, int[] offsets)
    {
        nuint baseAddr = AobscanHelper.Aobscan(ctx.HContext.Handle, pattern).FirstOrDefault();
        if (baseAddr == 0)
            return;
        foreach (int offset in offsets)
            candidates.Add(($"{name}+{offset}", baseAddr + (uint)offset));
    }

    private static void RunFunctionSmokeTests(GameContext ctx)
    {
        Console.WriteLine("\n--- Function Behavior Smoke Tests ---");
        if (!IsInWorld(ctx))
        {
            Console.WriteLine($"  SKIP function smoke tests: {DescribeWorldState(ctx)}");
            return;
        }

        SmokePatchToggle(ctx, "InfiniteLife",
            value => ctx.Patches.InfiniteLife = value,
            () =>
            {
                int targetMax = Math.Max(ctx.MyPlayer.StatLifeMax2, 100);
                ctx.MyPlayer.StatLifeMax2 = targetMax;
                ctx.MyPlayer.StatLife = 1;
            },
            () => ctx.MyPlayer.StatLife,
            () => ctx.MyPlayer.StatLifeMax2,
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "InfiniteMana",
            value => ctx.Patches.InfiniteMana = value,
            () =>
            {
                int targetMax = Math.Max(ctx.MyPlayer.StatManaMax2, 20);
                ctx.MyPlayer.StatManaMax2 = targetMax;
                ctx.MyPlayer.StatMana = 0;
            },
            () => ctx.MyPlayer.StatMana,
            () => ctx.MyPlayer.StatManaMax2,
            (actual, expected) => actual == expected);
        SmokeInfiniteAmmo(ctx);
        SmokePatchToggle(ctx, "InfiniteOxygen",
            value => ctx.Patches.InfiniteOxygen = value,
            () => ctx.MyPlayer.Breath = 0,
            () => ctx.MyPlayer.Breath,
            () => ctx.MyPlayer.BreathMax,
            (actual, _) => actual > 0);
        SmokePatchToggle(ctx, "InfiniteMinion",
            value => ctx.Patches.InfiniteMinion = value,
            () => ctx.MyPlayer.MaxMinions = 1,
            () => ctx.MyPlayer.MaxMinions,
            () => 9999,
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "InfiniteFlyTime",
            value => ctx.Patches.InfiniteFlyTime = value,
            () => ctx.MyPlayer.RocketTime = 0,
            () => ctx.MyPlayer.RocketTime,
            () => ctx.MyPlayer.RocketTimeMax,
            (actual, _) => actual > 0);
        SmokePatchToggle(ctx, "SlowFall",
            value => ctx.Patches.SlowFall = value,
            () => ctx.MyPlayer.SlowFall = false,
            () => ctx.MyPlayer.SlowFall ? 1 : 0,
            () => 1,
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "FastSpeed",
            value => ctx.Patches.FastSpeed = value,
            () => ctx.MyPlayer.MoveSpeed = 1f,
            () => BitConverter.SingleToInt32Bits(ctx.MyPlayer.MoveSpeed),
            () => BitConverter.SingleToInt32Bits(10f),
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "SuperRange",
            value => ctx.Patches.SuperRange = value,
            () =>
            {
                ctx.GameModuleHelper.SetStaticFieldValue("Terraria.Player", "tileRangeX", 5);
                ctx.GameModuleHelper.SetStaticFieldValue("Terraria.Player", "tileRangeY", 3);
            },
            () => ctx.GameModuleHelper.GetStaticFieldValue<int>("Terraria.Player", "tileRangeX"),
            () => 0x1000,
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "FastTileAndWallPlacingSpeed",
            value => ctx.Patches.FastTileAndWallPlacingSpeed = value,
            () => ctx.MyPlayer.TileSpeed = 1f,
            () => BitConverter.SingleToInt32Bits(ctx.MyPlayer.TileSpeed),
            () => BitConverter.SingleToInt32Bits(1f / 3f),
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "MechanicalRuler",
            value => ctx.Patches.MechanicalRuler = value,
            () => ctx.MyPlayer.RulerGrid = false,
            () => ctx.MyPlayer.RulerGrid ? 1 : 0,
            () => 1,
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "MechanicalLens",
            value => ctx.Patches.MechanicalLens = value,
            () => ctx.MyPlayer.InfoAccMechShowWires = false,
            () => ctx.MyPlayer.InfoAccMechShowWires ? 1 : 0,
            () => 1,
            (actual, expected) => actual == expected);
        SmokePatchToggle(ctx, "SuperGrabRange",
            value => ctx.Patches.SuperGrabRange = value,
            () =>
            {
                ctx.MyPlayer.GoldRing = false;
                ctx.MyPlayer.ManaMagnet = false;
                ctx.MyPlayer.LifeMagnet = false;
                ctx.MyPlayer.TreasureMagnet = false;
            },
            () => (ctx.MyPlayer.GoldRing && ctx.MyPlayer.ManaMagnet && ctx.MyPlayer.LifeMagnet && ctx.MyPlayer.TreasureMagnet) ? 1 : 0,
            () => 1,
            (actual, expected) => actual == expected);
        SmokeHighLight(ctx);
        SmokeImmuneToDebuffs(ctx);
        SmokeManagedToggle(ctx, "CreativeMenu", value => ctx.Patches.CreativeMenu = value);
        SmokeManagedToggle(ctx, "FishCratesOnly", value => ctx.Patches.FishCratesOnly = value);
        SmokeManagedToggle(ctx, "CoinPortalDropsBags", value => ctx.Patches.CoinPortalDropsBags = value);
        SmokeManagedToggle(ctx, "StrengthenVampireKnives", value => ctx.Patches.StrengthenVampireKnives = value);
        SmokeCoinPortalDropsBags(ctx);
        SmokeStrengthenVampireKnives(ctx);
        SmokeLanternNight(ctx);
    }

    private static void RunStaticTests()
    {
        Console.WriteLine("--- Static Compatibility Tests ---");
        bool ok = true;
        ok &= RequireProperty(typeof(PatchesManager), "CoinPortalDropsBags");
        ok &= RequireProperty(typeof(PatchesManager), "StrengthenVampireKnives");
        ok &= RequireSourceContains("src/QTRHacker.Patches/PlayerToggles.cs", "public static bool CoinPortalDropsBags");
        ok &= RequireSourceContains("src/QTRHacker.Patches/PlayerToggles.cs", "public static bool StrengthenVampireKnives");
        ok &= RequireSourceContains("src/QTRHacker/Scripts/Functions/BuiltIn-2.cs", "Add<CoinPortalDropsBags>();");
        ok &= RequireSourceContains("src/QTRHacker/Scripts/Functions/BuiltIn-2.cs", "Add<StrengthenVampireKnives>();");
        ok &= RequireSourceContains("src/QHackCLR/Common.cpp", "GC::KeepAlive(del);");
        ok &= RequireSourceContains("src/QTRHacker.Patches/PatchState.cs", "QTRHackerPatchState1456");
        ok &= RequireSourceContains("src/QTRHacker.Patches/PatchState.cs", "Version = 2");
        ok &= RequireSourceContains("src/QTRHacker.Core/RemotePatchState.cs", "ExpectedVersion = 2");
        ok &= RequireSourceContains("src/QTRHacker.Core/RemotePatchState.cs", "QTRHackerPatchState1456");
        ok &= RequireSourceContains("src/QTRHacker.Core/PatchesManager.cs", "RemotePatchState");
        ok &= RequireSourceContains("src/QTRHacker.Core/ItemCheckHookManager.cs", "GetPlayerItemCheckHookAddress");
        ok &= RequireSourceContains("src/QTRHacker.Core/PatchesManager.cs", "UpdateNativeItemCheckHook");
        ok &= RequireSourceContains("src/QTRHacker.Core/PlayerUpdateSnippets.cs", "SuperRange");
        ok &= RequireSourceContains("src/QTRHacker.Core/PlayerUpdateSnippets.cs", "FloatOneThird");
        ok &= RequireSourceContains("src/QTRHacker.Patches/PlayerToggles.cs", "Player.tileRangeX = 0x1000");
        ok &= RequireSourceContains("src/QTRHacker.Patches/PlayerToggles.cs", "player.tileSpeed = 3f");
        ok &= RequireSourceContains("src/QTRHacker.Patches/RuntimeActions.cs", "ToggleManualLanterns");
        ok &= RequireSourceContains("src/QTRHacker.Core/PatchesManager.cs", "ToggleLanternNight");
        ok &= RequireSourceContains("src/QTRHacker/Scripts/Functions/BuiltIn-4.cs", "ctx.Patches.ToggleLanternNight()");
        ok &= RequireSourceNotContains("src/QTRHacker/Scripts/Functions/BuiltIn-1.cs", "GlobalBrightness");
        ok &= RequireSourceNotContains("src/QTRHacker.Core/GameContext.cs", "SetStaticFieldValue(\"Terraria.GameContent.Events.LanternNight\"");
        ok &= RequireSourceNotContains("src/QTRHacker.Functions.Test/Program.cs", "RevealTheWhole" + "Map();");

        Console.WriteLine(ok ? "  OK static compatibility checks" : "  FAIL static compatibility checks");
        if (!ok)
            Environment.ExitCode = 1;
    }

    private static bool RequireProperty(Type type, string name)
    {
        PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
        bool ok = property != null && property.PropertyType == typeof(bool) && property.CanRead && property.CanWrite;
        Console.WriteLine(ok
            ? $"  OK {type.FullName}.{name}"
            : $"  FAIL {type.FullName}.{name}");
        return ok;
    }

    private static bool RequireSourceContains(string relativePath, string text)
    {
        string path = Path.GetFullPath(relativePath);
        bool ok = File.Exists(path) && File.ReadAllText(path).Contains(text, StringComparison.Ordinal);
        Console.WriteLine(ok
            ? $"  OK {relativePath} contains {text}"
            : $"  FAIL {relativePath} missing {text}");
        return ok;
    }

    private static bool RequireSourceNotContains(string relativePath, string text)
    {
        string path = Path.GetFullPath(relativePath);
        bool ok = File.Exists(path) && !File.ReadAllText(path).Contains(text, StringComparison.Ordinal);
        Console.WriteLine(ok
            ? $"  OK {relativePath} does not contain {text}"
            : $"  FAIL {relativePath} still contains {text}");
        return ok;
    }

    private static bool IsInWorld(GameContext ctx)
    {
        try
        {
            if (ctx.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "gameMenu"))
                return false;
            if (ctx.MyPlayerIndex < 0)
                return false;
            return ctx.MyPlayer.Active;
        }
        catch
        {
            return false;
        }
    }

    private static string DescribeWorldState(GameContext ctx)
    {
        try
        {
            bool gameMenu = ctx.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.Main", "gameMenu");
            int myPlayer = ctx.MyPlayerIndex;
            bool active = myPlayer >= 0 && ctx.MyPlayer.Active;
            return $"gameMenu={gameMenu}, myPlayer={myPlayer}, active={active}";
        }
        catch (Exception ex)
        {
            return $"{ex.GetType().Name}: {ex.Message}";
        }
    }

    private static void SmokeInfiniteAmmo(GameContext ctx)
    {
        try
        {
            var ammo = ctx.MyPlayer.Inventory[54];
            int oldType = ammo.Type;
            int oldStack = ammo.Stack;
            byte oldPrefix = ammo.Prefix;

            ammo.SetDefaultsAndPrefix(40, 0); // Wooden Arrow
            ammo.Stack = 1;

            ctx.Patches.InfiniteAmmo = true;
            Thread.Sleep(250);
            int after = ctx.MyPlayer.Inventory[54].Stack;
            ctx.Patches.InfiniteAmmo = false;

            ammo = ctx.MyPlayer.Inventory[54];
            if (oldType > 0)
            {
                ammo.SetDefaultsAndPrefix(oldType, oldPrefix);
                ammo.Stack = oldStack;
            }
            else
            {
                ammo.SetDefaultsAndPrefix(0, 0);
                ammo.Stack = oldStack;
            }

            Console.WriteLine(after >= 999
                ? $"  OK InfiniteAmmo topped ammo stack to {after}"
                : $"  FAIL InfiniteAmmo stack={after}, expected >= 999");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL InfiniteAmmo: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void SmokeImmuneToDebuffs(GameContext ctx)
    {
        try
        {
            var buffType = ctx.MyPlayer.BuffType;
            var buffTime = ctx.MyPlayer.BuffTime;
            int oldType = buffType[0];
            int oldTime = buffTime[0];

            buffType[0] = 24; // On Fire!
            buffTime[0] = 600;
            ctx.Patches.ImmuneToDebuffs = true;
            Thread.Sleep(250);
            int afterType = buffType[0];
            int afterTime = buffTime[0];
            ctx.Patches.ImmuneToDebuffs = false;

            buffType[0] = oldType;
            buffTime[0] = oldTime;

            Console.WriteLine(afterType == 0 || afterTime <= 0
                ? $"  OK ImmuneToDebuffs cleared buff type={afterType}, time={afterTime}"
                : $"  FAIL ImmuneToDebuffs buff type={afterType}, time={afterTime}");
        }
        catch (Exception ex)
        {
            try { ctx.Patches.ImmuneToDebuffs = false; } catch { }
            Console.WriteLine($"  FAIL ImmuneToDebuffs: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void SmokeLanternNight(GameContext ctx)
    {
        try
        {
            bool before = ctx.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.GameContent.Events.LanternNight", "ManualLanterns");
            ctx.Patches.ToggleLanternNight();
            bool after = WaitForBool(
                () => ctx.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.GameContent.Events.LanternNight", "ManualLanterns"),
                value => value != before);
            if (after != before)
            {
                ctx.Patches.ToggleLanternNight();
                WaitForBool(
                    () => ctx.GameModuleHelper.GetStaticFieldValue<bool>("Terraria.GameContent.Events.LanternNight", "ManualLanterns"),
                    value => value == before);
            }

            Console.WriteLine(after != before
                ? $"  OK ToggleLanternNight manual={after}"
                : $"  FAIL ToggleLanternNight manual={after}, expected {!before}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL ToggleLanternNight: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static bool WaitForBool(Func<bool> readActual, Func<bool, bool> isOk)
    {
        bool actual = readActual();
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (!isOk(actual) && stopwatch.ElapsedMilliseconds < 1500)
        {
            Thread.Sleep(50);
            actual = readActual();
        }
        return actual;
    }

    private static void SmokeCoinPortalDropsBags(GameContext ctx)
    {
        int itemIndex = -1;
        int portalIndex = -1;
        try
        {
            XnaVector2 center = GetPlayerCenter(ctx) + new XnaVector2(600f, 0f);
            portalIndex = NewProjectile(ctx, center, XnaVector2.Zero, 518, 0, 0f, ctx.MyPlayerIndex);
            Thread.Sleep(100);

            itemIndex = CreateWorldCoin(ctx, center);
            ctx.Patches.CoinPortalDropsBags = true;
            int actual = WaitForSmokeValue(
                () => ReadWorldItemType(ctx, itemIndex),
                3332,
                (value, expected) => value == expected);
            ctx.Patches.CoinPortalDropsBags = false;

            Console.WriteLine(actual == 3332
                ? $"  OK CoinPortalDropsBags converted item={itemIndex} type={actual}"
                : $"  FAIL CoinPortalDropsBags item={itemIndex} type={actual}, expected 3332; {DescribeWorldItem(ctx, itemIndex)}; {DescribeProjectile(ctx, portalIndex)}");
        }
        catch (Exception ex)
        {
            try { ctx.Patches.CoinPortalDropsBags = false; } catch { }
            Console.WriteLine($"  FAIL CoinPortalDropsBags behavior: {ex.GetType().Name}: {ex.Message}");
        }
        finally
        {
            ClearWorldItem(ctx, itemIndex);
            ClearProjectile(ctx, portalIndex);
        }
    }

    private static void SmokeStrengthenVampireKnives(GameContext ctx)
    {
        HashSet<int> before = new();
        try
        {
            GameObjectArray projectiles = GetMainArray(ctx, "projectile");
            int owner = ctx.MyPlayerIndex;
            before = FindProjectileIndices(projectiles, 304, owner);
            XnaVector2 center = GetPlayerCenter(ctx) + new XnaVector2(0f, -120f);

            ctx.Patches.StrengthenVampireKnives = true;
            int sourceIndex = NewProjectile(ctx, center, new XnaVector2(12f, 0f), 304, 20, 1f, owner);
            int actual = WaitForSmokeValue(
                () => CountNewProjectiles(projectiles, 304, owner, before),
                13,
                (value, expected) => value >= expected);
            ctx.Patches.StrengthenVampireKnives = false;

            Console.WriteLine(actual >= 13
                ? $"  OK StrengthenVampireKnives spawned {actual} knives from source={sourceIndex}"
                : $"  FAIL StrengthenVampireKnives spawned {actual}, expected >= 13; source={DescribeProjectile(ctx, sourceIndex)}");
        }
        catch (Exception ex)
        {
            try { ctx.Patches.StrengthenVampireKnives = false; } catch { }
            Console.WriteLine($"  FAIL StrengthenVampireKnives behavior: {ex.GetType().Name}: {ex.Message}");
        }
        finally
        {
            ClearNewProjectiles(ctx, 304, ctx.MyPlayerIndex, before);
        }
    }

    private static GameObjectArray GetMainArray(GameContext ctx, string fieldName)
        => new(ctx, ctx.GameModuleHelper.GetStaticHackObject("Terraria.Main", fieldName));

    private static XnaVector2 GetPlayerCenter(GameContext ctx)
    {
        XnaVector2 position = ctx.MyPlayer.Position;
        return new XnaVector2(position.X + ctx.MyPlayer.Width / 2f, position.Y + ctx.MyPlayer.Height / 2f);
    }

    private static int ReadWorldItemType(GameContext ctx, int itemIndex)
    {
        if (itemIndex < 0)
            return 0;
        dynamic item = GetMainArray(ctx, "item")[itemIndex];
        return (int)item.inner.type;
    }

    private static string DescribeWorldItem(GameContext ctx, int itemIndex)
    {
        if (itemIndex < 0)
            return "item=<none>";
        try
        {
            dynamic item = GetMainArray(ctx, "item")[itemIndex];
            dynamic inner = item.inner;
            return $"item innerType={TryRead<int>(inner, "type")}, innerStack={TryRead<int>(inner, "stack")}, timeSince={TryRead<int>(item, "timeSinceItemSpawned")}, pos={TryFormatVector2Field(item, "position")}";
        }
        catch (Exception ex)
        {
            return $"item read failed: {ex.GetType().Name}: {ex.Message}";
        }
    }

    private static int CreateWorldCoin(GameContext ctx, XnaVector2 position)
    {
        GameObjectArray items = GetMainArray(ctx, "item");
        int index = TerrariaItem.NewItem(ctx, (int)position.X, (int)position.Y, 1, 1, 73, 1, noBroadcast: true, pfix: 0, noGrabDelay: true);
        if (index < 0 || index >= items.Length)
            throw new InvalidOperationException($"Terraria.Item.NewItem returned invalid item slot {index}.");

        dynamic worldItem = items[index];
        worldItem.position = position;
        worldItem.velocity = XnaVector2.Zero;
        worldItem.timeSinceItemSpawned = 0;
        return index;
    }

    private static string DescribeProjectile(GameContext ctx, int projectileIndex)
    {
        if (projectileIndex < 0)
            return "projectile=<none>";
        try
        {
            dynamic projectile = GetMainArray(ctx, "projectile")[projectileIndex];
            string position = FormatVector2(projectile.position);
            string velocity = FormatVector2(projectile.velocity);
            return $"projectile#{projectileIndex} active={(bool)projectile.active}, type={(int)projectile.type}, owner={(int)projectile.owner}, timeLeft={(int)projectile.timeLeft}, pos={position}, vel={velocity}";
        }
        catch (Exception ex)
        {
            return $"projectile#{projectileIndex} read failed: {ex.GetType().Name}: {ex.Message}";
        }
    }

    private static string TryRead<T>(dynamic target, string field)
    {
        try
        {
            return ((T)target.InternalGetMember(field)).ToString();
        }
        catch (Exception ex)
        {
            return $"<{field}:{ex.GetType().Name}>";
        }
    }

    private static string TryFormatVector2Field(dynamic target, string field)
    {
        try
        {
            return FormatVector2(target.InternalGetMember(field));
        }
        catch (Exception ex)
        {
            return $"<{field}:{ex.GetType().Name}>";
        }
    }

    private static string FormatVector2(dynamic vector)
    {
        try
        {
            return $"({((float)vector.X):0.0},{((float)vector.Y):0.0})";
        }
        catch
        {
            return "(unreadable)";
        }
    }

    private static void ClearWorldItem(GameContext ctx, int itemIndex)
    {
        if (itemIndex < 0)
            return;
        try
        {
            dynamic item = GetMainArray(ctx, "item")[itemIndex];
            item.inner.type = 0;
            item.inner.stack = 0;
        }
        catch { }
    }

    private static int NewProjectile(GameContext ctx, XnaVector2 position, XnaVector2 velocity, int type, int damage, float knockBack, int owner)
    {
        using MemoryAllocation ret = new(ctx.HContext);
        ctx.RunByHookUpdate(
            new HackMethod(ctx.HContext,
                ctx.GameModuleHelper.GetClrMethodBySignature("Terraria.Projectile",
                "Terraria.Projectile.NewProjectile(Terraria.DataStructures.IEntitySource, Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2, Int32, Int32, Single, Int32, Single, Single, Single, Terraria.NewProjectileModifier)"))
            .Call(null)
            .Call(true, null, ret.AllocationBase, new object[] { 0, position, velocity, type, damage, knockBack, owner, 0f, 0f, 0f, (nuint)0 }));
        return ctx.HContext.DataAccess.Read<int>(ret.AllocationBase);
    }

    private static HashSet<int> FindProjectileIndices(GameObjectArray projectiles, int type, int owner)
    {
        HashSet<int> result = new();
        for (int i = 0; i < projectiles.Length; i++)
        {
            dynamic projectile = projectiles[i];
            if ((bool)projectile.active && (int)projectile.type == type && (int)projectile.owner == owner)
                result.Add(i);
        }
        return result;
    }

    private static int CountNewProjectiles(GameObjectArray projectiles, int type, int owner, HashSet<int> before)
    {
        int count = 0;
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (before.Contains(i))
                continue;
            dynamic projectile = projectiles[i];
            if ((bool)projectile.active && (int)projectile.type == type && (int)projectile.owner == owner)
                count++;
        }
        return count;
    }

    private static void ClearNewProjectiles(GameContext ctx, int type, int owner, HashSet<int> before)
    {
        try
        {
            GameObjectArray projectiles = GetMainArray(ctx, "projectile");
            for (int i = 0; i < projectiles.Length; i++)
            {
                if (before.Contains(i))
                    continue;
                dynamic projectile = projectiles[i];
                if ((bool)projectile.active && (int)projectile.type == type && (int)projectile.owner == owner)
                    projectile.active = false;
            }
        }
        catch { }
    }

    private static void ClearProjectile(GameContext ctx, int projectileIndex)
    {
        if (projectileIndex < 0)
            return;
        try
        {
            dynamic projectile = GetMainArray(ctx, "projectile")[projectileIndex];
            projectile.active = false;
        }
        catch { }
    }

    private static void SmokeHighLight(GameContext ctx)
    {
        try
        {
            float before = ReadGlobalBrightness(ctx);
            if (Math.Abs(before - 100f) < 0.001f)
            {
                WriteGlobalBrightness(ctx, 1f);
                before = 1f;
            }

            ctx.Patches.HighLight = true;
            float enabled = WaitForSmokeFloat(
                () => ReadGlobalBrightness(ctx),
                value => Math.Abs(value - 100f) < 0.001f);

            ctx.Patches.HighLight = false;
            float restored = WaitForSmokeFloat(
                () => ReadGlobalBrightness(ctx),
                value => Math.Abs(value - before) < 0.001f);

            bool ok = Math.Abs(enabled - 100f) < 0.001f && Math.Abs(restored - before) < 0.001f;
            Console.WriteLine(ok
                ? $"  OK HighLight enabled={enabled}, restored={restored}"
                : $"  FAIL HighLight enabled={enabled}, restored={restored}, expected restore={before}");
        }
        catch (Exception ex)
        {
            try { ctx.Patches.HighLight = false; } catch { }
            Console.WriteLine($"  FAIL HighLight: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static float ReadGlobalBrightness(GameContext ctx)
        => ctx.GameModuleHelper.GetStaticFieldValue<float>("Terraria.Lighting", "<GlobalBrightness>k__BackingField");

    private static void WriteGlobalBrightness(GameContext ctx, float value)
        => ctx.GameModuleHelper.SetStaticFieldValue("Terraria.Lighting", "<GlobalBrightness>k__BackingField", value);

    private static float WaitForSmokeFloat(Func<float> readActual, Func<float, bool> isOk)
    {
        float actual = readActual();
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (!isOk(actual) && stopwatch.ElapsedMilliseconds < 1500)
        {
            Thread.Sleep(50);
            actual = readActual();
        }
        return actual;
    }

    private static void SmokePatchToggle(
        GameContext ctx,
        string name,
        Action<bool> setEnabled,
        Action arrange,
        Func<int> readActual,
        Func<int> readExpected,
        Func<int, int, bool> isOk)
    {
        try
        {
            arrange();
            setEnabled(true);
            int expected = readExpected();
            int actual = WaitForSmokeValue(readActual, expected, isOk);
            setEnabled(false);

            Console.WriteLine(isOk(actual, expected)
                ? $"  OK {name} actual={actual}"
                : $"  FAIL {name} actual={actual}, expected {expected}");
        }
        catch (Exception ex)
        {
            try { setEnabled(false); } catch { }
            Console.WriteLine($"  FAIL {name}: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static int WaitForSmokeValue(Func<int> readActual, int expected, Func<int, int, bool> isOk)
    {
        int actual = readActual();
        Stopwatch stopwatch = Stopwatch.StartNew();
        while (!isOk(actual, expected) && stopwatch.ElapsedMilliseconds < 1500)
        {
            Thread.Sleep(50);
            actual = readActual();
        }
        return actual;
    }

    private static void SmokeManagedToggle(GameContext ctx, string name, Action<bool> setEnabled)
    {
        try
        {
            setEnabled(true);
            Thread.Sleep(100);
            setEnabled(false);
            Console.WriteLine($"  OK {name} managed toggle");
        }
        catch (Exception ex)
        {
            try { setEnabled(false); } catch { }
            Console.WriteLine($"  FAIL {name}: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void SmokePatches(GameContext ctx)
    {
        try
        {
            ctx.Patches.Init();
            int mode = ctx.Patches.AutoFishing_Mode;
            Console.WriteLine($"  OK QTRHacker.Patches shared state initialized, AutoFishing_Mode={mode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL QTRHacker.Patches: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void SmokePatchLoadMode(GameContext ctx, bool useLoadFrom)
    {
        string path = ResolvePatchesAssemblyPath();
        Console.WriteLine($"  Loading QTRHacker.Patches via {(useLoadFrom ? "LoadFrom" : "Load(Byte[])")}: {path}");
        try
        {
            bool ok = useLoadFrom
                ? ctx.LoadAssemblyFrom(path, "QTRHacker.Patches.Boot")
                : ctx.LoadAssemblyAsBytes(path, "QTRHacker.Patches.Boot");
            Console.WriteLine(ok ? "  OK load request completed" : "  FAIL load request returned false");
            ctx.Flush();
            ctx.Patches.Init();
            Console.WriteLine($"  OK shared state reachable, AutoFishing_Mode={ctx.Patches.AutoFishing_Mode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  FAIL load mode: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static string ResolvePatchesAssemblyPath()
    {
        string[] candidates =
        {
            Path.Combine(AppContext.BaseDirectory, "QTRHacker.Patches.dll"),
            Path.GetFullPath("./QTRHacker.Patches.dll"),
            Path.GetFullPath("./bin/Debug/QTRHacker.Patches.dll"),
            Path.GetFullPath("./bin/Release/QTRHacker.Patches.dll"),
            Path.GetFullPath("./src/QTRHacker.Patches/bin/x86/Debug/QTRHacker.Patches.dll"),
            Path.GetFullPath("./src/QTRHacker.Patches/bin/x86/Release/QTRHacker.Patches.dll"),
            Path.GetFullPath("./src/QTRHacker.Patches/bin/Debug/QTRHacker.Patches.dll"),
            Path.GetFullPath("./src/QTRHacker.Patches/bin/Release/QTRHacker.Patches.dll"),
        };
        string path = candidates
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(File.Exists)
            .OrderByDescending(File.GetLastWriteTimeUtc)
            .FirstOrDefault();
        if (path != null)
            return path;
        throw new FileNotFoundException("Could not locate QTRHacker.Patches.dll.", candidates[0]);
    }

	private static void SmokePatchRuntimeActions(GameContext ctx)
	{
		try
		{
			ctx.Patches.Init();
			ctx.Patches.UnlockAllDuplications();
			Thread.Sleep(250);
			Console.WriteLine("  OK QTRHacker.Patches.RuntimeActions queued via shared state");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"  FAIL QTRHacker.Patches.RuntimeActions: {ex.GetType().Name}: {ex.Message}");
		}
	}

    private static void DumpPatchModules(GameContext ctx)
    {
        ctx.Flush();
        Console.WriteLine("  CLR modules containing QTRHacker/Patches:");
        foreach (var helper in ctx.HContext.CLRHelpers.Values
            .Where(h => (h.Module.Name?.IndexOf("QTRHacker", StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                || (h.Module.FileName?.IndexOf("QTRHacker", StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                || (h.Module.Name?.IndexOf("Patches", StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                || (h.Module.FileName?.IndexOf("Patches", StringComparison.OrdinalIgnoreCase) ?? -1) >= 0))
        {
            bool hasBoot = false;
            try { hasBoot = helper.GetClrType("QTRHacker.Patches.Boot") != null; } catch { }
            Console.WriteLine($"    name={helper.Module.Name}, file={helper.Module.FileName}, hasBoot={hasBoot}");
        }
    }

    private static void DumpAllModules(GameContext ctx)
    {
        ctx.Flush();
        Console.WriteLine($"  CLR modules: {ctx.HContext.CLRHelpers.Count}");
        foreach (var helper in ctx.HContext.CLRHelpers.Values)
            Console.WriteLine($"    name={helper.Module.Name}, file={helper.Module.FileName}");
    }
}
