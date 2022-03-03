using QHackLib.Assemble;
using QTRHacker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.RemoteExecution
{
	public class ActionOnManagedThread
	{
		public GameContext Context { get; }
		public AssemblyCode Code { get; }
		public uint Size { get; }
		public ActionOnManagedThread(GameContext ctx, AssemblyCode code, uint size = 0x1000)
		{
			Context = ctx;
			Code = code;
			Size = size;
		}

		public QHackLib.FunctionHelper.RemoteThread Execute() => Context.RunOnManagedThread(Code, Size);
	}
}
