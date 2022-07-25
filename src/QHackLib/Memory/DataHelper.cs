using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Memory
{
	public static class DataHelper
	{
		public unsafe static byte[] GetBytes<T>(in T t) where T : unmanaged
		{
			byte[] data = new byte[sizeof(T)];
			fixed (T* ptr = &t)
				for (int i = 0; i < data.Length; i++)
					data[i] = ((byte*)ptr)[i];
			return data;
		}
		public unsafe static byte[] GetBytes(ValueType t)
		{
			return typeof(DataHelper)
				.GetMethods()
				.First(t => t.Name == "GetBytes" && t.IsGenericMethodDefinition)
				.MakeGenericMethod(t.GetType())
				.Invoke(null, new object[] { t }) as byte[];
		}
		public unsafe static T GetValueFromBytes<T>(in ReadOnlySpan<byte> data) where T : unmanaged
		{
			T t = default;
			for (int i = 0; i < data.Length; i++)
				((byte*)&t)[i] = data[i];
			return t;
		}
	}
}
