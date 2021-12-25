using QHackCLR.Dac.COM;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using QHackCLR.Metadata.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Helpers
{
	public unsafe static class IMetadataImportHelper
	{
		public static IEnumerable<int> EnumParams(this IMetadataImport metadata, int mdMethodDef)
		{
			nuint handle = 0;
			int[] ps = new int[2];
			while (metadata.EnumParams(ref handle, mdMethodDef, ps, (uint)ps.Length, out uint count).IsOK() && count > 0)
				for (int i = 0; i < count; i++)
					yield return ps[i];
		}
		public static string GetFieldName(this IMetadataImport metadata, int mdFieldDef) => HelperGlobals.GetString((uint count, char* buf, out uint needed) => metadata.GetFieldProps(mdFieldDef, out _, buf, count, out needed, out _, out _, out _, out _, out _, out _));
		
		public static string GetMethodName(this IMetadataImport metadata, int mdMethodDef) => HelperGlobals.GetString((uint count, char* buf, out uint needed) => metadata.GetMethodProps(mdMethodDef,out _, buf, count, out needed, out _, out _, out _, out _, out _));

		public static string GetParamName(this IMetadataImport metadata, int mdParamDef) => HelperGlobals.GetString((uint count, char* buf, out uint needed) => metadata.GetParamProps(mdParamDef, out _, out _, buf, count, out needed, out _, out _, out _, out _));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="metadata"></param>
		/// <param name="mdFieldDef"></param>
		/// <param name="attr"></param>
		/// <returns>returns <see cref="HRESULT.S_OK"/> only if succeeds</returns>
		public static HRESULT GetFieldAttributesAndSig(this IMetadataImport metadata, int mdFieldDef, out FieldAttributes attr, out SigParser sig)
		{
			HRESULT hr = metadata.GetFieldProps(mdFieldDef, out _, null, 0, out _, out attr, out IntPtr pSig, out uint cSig, out _, out _, out _);
			sig = new SigParser(pSig, (int)cSig);
			return hr ? HRESULT.S_OK : hr;
		}

		public static HRESULT GetMethodProps(this IMetadataImport metadata, int mdMethodDef, out MethodAttributes attr, out SigParser sig)
		{
			HRESULT hr = metadata.GetMethodProps(mdMethodDef, out _, null, 0, out _, out attr, out IntPtr pSig, out uint cSig, out _, out _);
			sig = new SigParser(pSig, (int)cSig);
			return hr ? HRESULT.S_OK : hr;
		}

		public static HRESULT GetParamProps(this IMetadataImport metadata, int mdParamDef, out uint index, out ParameterAttributes attr, out SigParser sig)
		{
			HRESULT hr = metadata.GetParamProps(mdParamDef, out _, out index, null, 0, out _, out attr, out _, out IntPtr pSig, out uint cSig);
			sig = new SigParser(pSig, (int)cSig);
			return hr ? HRESULT.S_OK : hr;
		}
	}
}
