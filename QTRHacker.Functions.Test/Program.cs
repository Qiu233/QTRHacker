using Keystone;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		static void Main(string[] args)
		{

			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id))
			{
				var ass = AssemblySnippet.FromCode(
					new AssemblyCode[] {
						Instruction.Create("pushad"),
						Instruction.Create($"cmp byte ptr [{gc.MapFullScreen_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{gc.MouseRight_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{gc.MouseRightRelease_Address}],0"),
						Instruction.Create("je _rwualfna"),
						AssemblySnippet.FromCode(
							new AssemblyCode[]{
								Instruction.Create($"mov byte ptr [{gc.MapFullScreen_Address}],0"),
								Instruction.Create($"mov byte ptr [{gc.MouseRightRelease_Address}],0"),
								AssemblySnippet.FromDotNetCall(
									gc.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::get_LocalPlayer"), null, false),
								Instruction.Create("mov ebx,eax"),
								Instruction.Create("push eax"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{gc.ScreenWidth_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{gc.MouseScreen_X_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{gc.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{gc.MapFullscreenPos_Address + 4}]"),
								Instruction.Create("fsubrp"),
								Instruction.Create("mov dword ptr [esp],16"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fmulp"),
								Instruction.Create($"fstp dword ptr [ebx+{Player.OFFSET_X}]"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{gc.ScreenHeight_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{gc.MouseScreen_Y_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{gc.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{gc.MapFullscreenPos_Address + 8}]"),
								Instruction.Create("fsubrp"),
								Instruction.Create("mov dword ptr [esp],16"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fmulp"),
								Instruction.Create($"fstp dword ptr [ebx+{Player.OFFSET_Y}]"),
								
								Instruction.Create("pop eax"),
							}),
						Instruction.Create("_rwualfna:"),
						Instruction.Create("popad")
					});
				Console.WriteLine(ass.GetCode());
				InlineHook.Inject(gc.HContext, ass,
					gc.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::DoUpdate"), false);


			}
		}
	}
}
