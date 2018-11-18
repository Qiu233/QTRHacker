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
				/*ProjImage p = ProjImage.FromImage("./Qiu.png", 355, 16);
				for (int i = 0; i < p.Projs.GetLength(0); i++)
				{
					for (int j = 0; j < p.Projs.GetLength(1); j++)
					{
						//p.Projs[i, j].Speed = new System.Drawing.PointF(3f, 0f);
					}
				}
				p.Emit(gc, gc.MyPlayer.X, gc.MyPlayer.Y);*/
			}
		}
	}
}
