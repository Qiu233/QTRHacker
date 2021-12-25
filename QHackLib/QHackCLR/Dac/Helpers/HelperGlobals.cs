using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Helpers
{
	public unsafe delegate HRESULT GetNameDelegateFlags(uint flags, uint count, out uint lenName, char* buf);

	public unsafe delegate HRESULT GetBufferDelegate<T>(uint count, T* buf, out uint needed) where T : unmanaged;

	public unsafe delegate HRESULT GetStringDelegate(uint count, char* buf, out uint needed);
	public unsafe delegate HRESULT GetListDelegate(uint count, CLRDATA_ADDRESS* buf, out uint needed);

	public unsafe delegate HRESULT GetStringDelegateObj<in T>(T obj, uint count, char* buf, out uint needed) where T : class;
	public unsafe delegate HRESULT GetListDelegateObj<in T>(T obj, uint count, CLRDATA_ADDRESS* buf, out uint needed) where T : class;

	public unsafe delegate HRESULT GetStringDelegateFromAddr(CLRDATA_ADDRESS addr, uint count, char* buf, out uint needed);
	public unsafe delegate HRESULT GetListDelegateFromAddr(CLRDATA_ADDRESS addr, uint count, CLRDATA_ADDRESS* buf, out uint needed);

	public unsafe static class HelperGlobals
	{
		public static T[] GetBuffer<T>(GetBufferDelegate<T> getBuffer, uint count = 0) where T : unmanaged
		{
			uint needed = count;
			if (needed == 0) try { getBuffer(0, null, out needed); } catch (Exception) { }
			T[] buffer = new T[needed];
			fixed (T* ptr = buffer) getBuffer(needed, ptr, out needed);
			return buffer;
		}

		public static string GetNameWithFlags(GetNameDelegateFlags getName, uint flags = 0, uint count = 0)
			=> GetString((uint count, char* buf, out uint needed) => getName(flags, count, out needed, buf), count);

		public static string GetStringFromObj<T>(GetStringDelegateObj<T> getStr, T obj, uint count = 0) where T : class
			=> GetString((uint count, char* buf, out uint needed) => getStr(obj, count, buf, out needed), count);
		public static string GetStringFromAddr(GetStringDelegateFromAddr getStr, CLRDATA_ADDRESS addr, uint count = 0)
			=> GetString((uint count, char* buf, out uint needed) => getStr(addr, count, buf, out needed), count);

		public static string GetString(GetStringDelegate getStr, uint count = 0)
			=> new(GetBuffer((uint count, char* buf, out uint needed)
				=> getStr(count, buf, out needed), count).TakeWhile(c => c != '\0').ToArray());

		public static CLRDATA_ADDRESS[] GetListFromObj<T>(GetListDelegateObj<T> getList, T obj, int count = 0) where T : class =>
			GetList((uint count, CLRDATA_ADDRESS* buf, out uint needed) => getList(obj, count, buf, out needed), count);
		public static CLRDATA_ADDRESS[] GetListFromAddr(GetListDelegateFromAddr getList, CLRDATA_ADDRESS addr, int count = 0) =>
			GetList((uint count, CLRDATA_ADDRESS* buf, out uint needed) => getList(addr, count, buf, out needed), count);

		public static CLRDATA_ADDRESS[] GetList(GetListDelegate getList, int count = 0) =>
			GetBuffer((uint count, CLRDATA_ADDRESS* buf, out uint needed) => getList(count, buf, out needed), (uint)count);
	}

}
