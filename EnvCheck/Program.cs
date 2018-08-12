using QTRHacker.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvCheck
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("正在生成报告");
			StringBuilder s = new StringBuilder("");
			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id))
			{
				foreach (var c in typeof(GameContext).GetProperties())
					s.AppendLine("GameContext:\t" + c.Name + ":\t\t" + c.GetValue(gc).ToString());
			}
			File.WriteAllText("./EnvCheck.txt", s.ToString());
			Console.WriteLine("完成");
			Console.WriteLine("请将修改器文件夹下的EnvCheck.txt发送给开发者以确定您遇到的问题");
			Console.WriteLine("按任意键继续......");
			Console.ReadKey();
		}
	}
}
