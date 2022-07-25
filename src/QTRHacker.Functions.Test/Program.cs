using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test;

class Program
{
	public static unsafe int GetOffset(GameContext context, string module, string type, string field) => (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
	public static unsafe int GetOffset(GameContext context, string type, string field) => (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
	unsafe static void Main()
	{
		using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("dotnet")[0], "tModLoader");
		/*var type = ctx.GameModuleHelper.GetClrType("Terraria.Item");
		var method = type.MethodsInVTable.Where(t => t.Signature == "Terraria.Item.SetDefaults(Int32)").First();
		using InlineHook hook = new InlineHook(ctx.HContext, AssemblySnippet.FromCode(
			new AssemblyCode[] {
				(Instruction)"push rcx",
				(Instruction)"push rdx",
				(Instruction)"push rbx",//for alignment
				(Instruction)"push r8",
				(Instruction)"push r9",

				(Instruction)"sub rsp, 0x10",
				(Instruction)"movdqu [rsp], xmm0",
				(Instruction)"sub rsp, 0x10",
				(Instruction)"movdqu [rsp], xmm1",

				(Instruction)"sub rsp, 32",

				(Instruction)$"mov rcx, {ctx.MyPlayer.Inventory[0].BaseAddress}",
				(Instruction)$"mov edx, 3064",
				(Instruction)$"mov rax, {method.NativeCode}",
				(Instruction)$"call rax",

				(Instruction)"add rsp, 32",

				(Instruction)"movdqu xmm1, [rsp]",
				(Instruction)"add rsp, 0x10",
				(Instruction)"movdqu xmm0, [rsp]",
				(Instruction)"add rsp, 0x10",

				(Instruction)"pop r9",
				(Instruction)"pop r8",
				(Instruction)"pop rbx",
				(Instruction)"pop rdx",
				(Instruction)"pop rcx",
			}
			), new HookParameters(targetAddress: ctx.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update"), 4096, true, true));
		Console.WriteLine(hook.MemoryAllocation.AllocationBase.ToString("X16"));
		Console.ReadKey();
		hook.Attach();
		Console.WriteLine("Attached");
		Console.ReadKey();
		hook.WaitToDetach();
		hook.WaitToDispose();*/
		/*Console.WriteLine(method.Signature);
		Console.WriteLine(method.NativeCode.ToString("X16"));*/
		/*var head = InlineHook.GetHeadBytes(ctx.HContext.DataAccess.ReadBytes(method.NativeCode, 32), 12, true);
		Console.WriteLine(head.Length);*/
		/*nuint handle = unchecked((nuint)0x00007FFED0E9EFE0);
		var ts = ctx.GameModuleHelper.Module.ReferencedTypes.Where(t => t.ClrHandle == handle).ToArray();
		Console.WriteLine(ts[0].Name);
		Console.WriteLine(ctx.GameModuleHelper.GetStaticFieldValue<nuint>("Terraria.Main", "instance").ToString("X16"));
		Console.WriteLine(ctx.GameModuleHelper.GetStaticHackObject("Terraria.Main", "player").GetArrayLength());*/
		/*Console.WriteLine(ctx.MyPlayer.Inventory[0].Prefix);
		var code = ctx.MyPlayer.Inventory[0].TypedInternalObject.GetMethodCall("Terraria.Item.SetDefaults(Int32)").Call(true, null, null, new object[] { 3063 });
		Console.WriteLine(code.ToString());*/

		/*var type = ctx.GameModuleHelper.GetClrType("Terraria.Item");
		var method = type.MethodsInVTable.Where(t => t.Signature == "Terraria.Item.SetDefaults(Int32)").First();

		var code = AssemblySnippet.FromClrCall(method.NativeCode, true, ctx.MyPlayer.Inventory[0].BaseAddress, null, null, new object[] { 3063 });
		Console.WriteLine(code);
		using InlineHook hook = new InlineHook(ctx.HContext, code, new HookParameters(targetAddress: ctx.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update"), 4096, true, true));
		Console.WriteLine(hook.MemoryAllocation.AllocationBase.ToString("X16"));
		Console.ReadKey();
		hook.Attach();
		Console.WriteLine("Attached");
		Console.ReadKey();
		hook.WaitToDetach();
		hook.WaitToDispose();*/

		/*var type = ctx.GameModuleHelper.GetClrType("Terraria.NPC");
		var method = type.MethodsInVTable.Where(t => t.Name == "NewNPC").First();

		var code = AssemblySnippet.FromClrCall(method.NativeCode, true, null, null, null,
			new object[] { 0, (int)ctx.MyPlayer.Position.X, (int)ctx.MyPlayer.Position.Y, 50, 0, 0f, 0f, 0f, 0f, 255 });
		Console.WriteLine(code);*/
		/*using InlineHook hook = new InlineHook(ctx.HContext, code, new HookParameters(targetAddress: ctx.GameModuleHelper.GetFunctionAddress("Terraria.Main", "Update"), 4096, true, true));
		Console.WriteLine(hook.MemoryAllocation.AllocationBase.ToString("X16"));
		Console.ReadKey();
		hook.Attach();
		Console.WriteLine("Attached");
		Console.ReadKey();
		hook.WaitToDetach();
		hook.WaitToDispose();*/

		/*var code = AssemblySnippet.Loop(AssemblySnippet.FromASMCode("xor edx,edx"), 100, true);
		ctx.RunByHookUpdate(code);*/

		Projectile.NewProjectile(ctx, null, ctx.MyPlayer.Position.X, ctx.MyPlayer.Position.Y, 10f, 0f, 502, 0, 0, ctx.MyPlayerIndex, 0f, 0f);
	}
}