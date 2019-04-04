using Microsoft.Win32;
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
			if (!GetDotNetRelease(394802) && !GetDotNetRelease(394806))
			{
				MessageBox.Show("请先安装.NET Framework v4.6.2或更高版本的.Net Framework");
				return;
			}
			Application.ApplicationExit += Application_ApplicationExit;
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		/// <summary>
		/// 这个方法来源于https://blog.csdn.net/sun_zeliang/article/details/81479775
		/// </summary>
		/// <param name="release"></param>
		/// <returns></returns>
		private static bool GetDotNetRelease(int release)
		{
			const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
			using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
			{
				if (ndpKey != null && ndpKey.GetValue("Release") != null)
				{
					return (int)ndpKey.GetValue("Release") >= release ? true : false;
				}
				return false;
			}
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			/*if (e.Exception is DllNotFoundException && e.Exception.ToString().Contains("keystone"))
			{
				MessageBox.Show("请先安装VC运行库");
			}
			else*/
			{
				MessageBox.Show(e.Exception.ToString());
			}
		}

		private static void Application_ApplicationExit(object sender, EventArgs e)
		{
			HackContext.GameContext?.Close();
		}
	}
}
