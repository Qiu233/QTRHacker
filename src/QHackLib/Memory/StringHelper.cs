using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Memory
{
	public unsafe static class StringHelper
	{
		/// <summary>
		/// For unicode string
		/// </summary>
		/// <param name="str"></param>
		public static void WriteWCHARArray(this RemoteMemoryStream stream, string str)
		{
			byte[] data = Encoding.Unicode.GetBytes(str);
			stream.Write(data, (uint)data.Length);
			stream.Write<short>(0);
		}
		/// <summary>
		/// For ASCII string
		/// </summary>
		/// <param name="str"></param>
		public static void WriteCHARArray(this RemoteMemoryStream stream, string str)
		{
			byte[] data = Encoding.ASCII.GetBytes(str);
			stream.Write(data, (uint)data.Length);
			stream.Write<byte>(0);
		}
	}
}
