using Keystone;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Functions.ProjectileImage;
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
				/*Stopwatch watch = new Stopwatch();
				watch.Start();

				watch.Stop();
				TimeSpan timespan = watch.Elapsed;
				Console.WriteLine("执行时间：{0}(毫秒)", timespan.TotalMilliseconds);*/

			}
		}
	}
}
