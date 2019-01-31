using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.NewDimension
{
	public class ResBinFileReader
	{
		private ResBinFileReader()
		{

		}
		public static Dictionary<string, byte[]> ReadFromStream(Stream s)
		{
			Dictionary<string, byte[]> r = new Dictionary<string, byte[]>();
			BinaryReader br = new BinaryReader(s);
			while (true)
			{
				if (br.PeekChar() == -1) break;
				string name = br.ReadString();
				long length = br.ReadInt64();
				byte[] data = br.ReadBytes((int)length);
				r[name] = data;
			}
			return r;
		}
	}
}
