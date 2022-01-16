using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.ApplicationExit += Application_ApplicationExit;
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Idle += (s, e) => { HackContext.DispatchGlobalUpdate(); };

			Application.Run(new MainForm
			{
				StartPosition = FormStartPosition.CenterScreen
			});
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString());
			MessageBox.Show("如果持续出现此问题\n请尝试以管理员权限启动修改器");
		}

		private static void Application_ApplicationExit(object sender, EventArgs e)
		{
			HackContext.GameContext?.Dispose();
		}
	}
}
