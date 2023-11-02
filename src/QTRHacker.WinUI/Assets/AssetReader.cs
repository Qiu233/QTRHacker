using QTRHacker.AssetLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace QTRHacker.Assets;

internal static class AssetReader
{
	public static async Task<byte[]> ReadData(string uriString)
	{
		Uri uri = new(uriString);
		StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
		using var stream = await file.OpenStreamForReadAsync();
		byte[] buffer = new byte[stream.Length];
		stream.Read(buffer);
		return buffer;
	}
}
