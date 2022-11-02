using System.IO;

namespace QTRHacker.AssetLoaders;

public static class BinLoader
{
	public static Dictionary<string, byte[]> ReadBinFromStream(Stream s)
	{
		Dictionary<string, byte[]> r = new();
		BinaryReader br = new(s);
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
