using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Res
{
	public class GameASMResLoader
	{
		public Dictionary<string, byte[]> Cache = new();
		public GameASMResLoader(string asmPath)
		{
			using var fs = File.OpenRead(asmPath);
			PEReader per = new(fs);
			MetadataReader mr = per.GetMetadataReader();
			foreach (var resHandle in mr.ManifestResources)
			{
				var res = mr.GetManifestResource(resHandle);
				PEMemoryBlock resourceDirectory = per.GetSectionData(per.PEHeaders.CorHeader.ResourcesDirectory.RelativeVirtualAddress);
				var reader = resourceDirectory.GetReader((int)res.Offset, resourceDirectory.Length - (int)res.Offset);
				uint size = reader.ReadUInt32();
				Cache[mr.GetString(res.Name)] = reader.ReadBytes((int)size);
			}
		}
		public byte[] this[string name] => Cache[name];

		/// <summary>
		/// Such as en-US.Items
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetLocalizationText(string name, Encoding encoding)
		{
			return encoding.GetString(Cache["Terraria.Localization.Content." + name + ".json"]);
		}
	}
}
