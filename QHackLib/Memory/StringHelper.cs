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
		public static void WriteWCHARArray(this MemoryStream stream, string str)
		{
			byte[] data = Encoding.Unicode.GetBytes(str);
			stream.Write(data, (uint)data.Length);
			stream.Write<short>(0);
		}
		/// <summary>
		/// For ASCII string
		/// </summary>
		/// <param name="str"></param>
		public static void WriteCHARArray(this MemoryStream stream, string str)
		{
			byte[] data = Encoding.ASCII.GetBytes(str);
			stream.Write(data, (uint)data.Length);
			stream.Write<byte>(0);
		}

		public static void FakeManagedString(this MemoryStream stream, string str)
		{
			stream.Write<nuint>(0);//sync block
			stream.Write(stream.Context.Runtime.Heap.StringType.ClrHandle);//handle
			stream.Write((nuint)str.Length);//length
			WriteWCHARArray(stream, str);
		}
	}
}
