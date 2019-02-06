using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.ApplicationExit += Application_ApplicationExit;
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString());
		}

		private static void Application_ApplicationExit(object sender, EventArgs e)
		{
			HackContext.GameContext?.Close();
		}
	}
}
