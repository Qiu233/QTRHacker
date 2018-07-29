/*
 * Created by SharpDevelop.
 * User: Qiu
 * Date: 2016/7/17
 * Time: 16:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Terraria_Hacker
{
	/// <summary>
	/// Description of INI.
	/// </summary>
	public class INI
	{
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);
		private static string ReadString(string section, string key, string def, string filePath)
		{
			StringBuilder temp = new StringBuilder(1024);
			try
			{
				GetPrivateProfileString(section, key, def, temp, 1024, filePath);
			}
			catch
			{ }
			return temp.ToString();
		}
		public static string[] ReadIniAllKeys(string section,string filePath)
		{
			UInt32 MAX_BUFFER = 32767;

			string[] items = new string[0];

			IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));

			UInt32 bytesReturned = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, filePath);

			if (!(bytesReturned == MAX_BUFFER - 2) || (bytesReturned == 0))
			{
				string returnedString = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned);

				items = returnedString.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
			}

			Marshal.FreeCoTaskMem(pReturnedString);

			return items;
		}

		public static string ReadIniKeys(string section, string keys, string filePath)
		{
			return ReadString(section, keys, "", filePath);
		}
		public static void WriteIniKeys(string section, string key, string value, string filePath)
		{
			WritePrivateProfileString(section, key, value, filePath);
		}
	}
}
