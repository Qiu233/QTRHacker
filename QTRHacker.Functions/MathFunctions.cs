using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class MathFunctions : CustomFunctions
	{
		public MathFunctions(Context ctx) : base(ctx)
		{

		}


		public AssemblyCode AddI2F()
		{
			return (Instruction)("call " + Functions["Imp_AddI2F"]);
		}


		[CustomFunction]
		private static AssemblyCode Imp_AddI2F()
		{
			return AssemblySnippet.FromCode(new AssemblyCode[]{
				(Instruction)"fild dword ptr [esp+4]",
				(Instruction)"fild dword ptr [esp+8]",
				(Instruction)"fadd",
				(Instruction)"fstp dword ptr [esp-4]",
				(Instruction)"mov eax,[esp-4]",
				(Instruction)"ret 8",
			});
		}
	}
}
