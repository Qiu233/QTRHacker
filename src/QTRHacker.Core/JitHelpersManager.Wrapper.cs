using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QTRHacker.Core;

/*
Jit helpers are code snippets used frequently to perform performance critical and important actions,
such as object allocating, array creating ,and so on.

Since DAC does not expose globals(see source code of coreclr), we have to find jit helpers on our own.
Resource "COREXTERNALDATAACCESSRESOURCE" contains a list of rva of globals, 
and we shall load the "clr.dll", extract the resource, and find the rva of table of jit helpers.

For more information, please read source code of coreclr.
*/
partial class JitHelpersManager
{

	[LibraryImport("Kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
	private static partial nuint LoadLibraryW(string lpFileName);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool FreeLibrary(nuint hModule);

	[LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
	private static partial nuint FindResourceW(nuint hModule, string lpName, nuint lpType);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	private static partial nuint LoadResource(nuint hModule, nuint hResInfo);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	private static partial nuint LockResource(nuint hResData);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	private static partial uint SizeofResource(nuint hModule, nuint hResInfo);


	private static byte[] GetResourceFromExecutable(string lpFileName, string lpName, short lpType)
	{
		nuint hModule = LoadLibraryW(lpFileName);
		try
		{
			if (hModule == 0)
				return null;
			nuint hResource = FindResourceW(hModule, lpName, (nuint)lpType);
			if (hResource == 0)
				return null;
			uint resSize = SizeofResource(hModule, hResource);
			nuint resData = LoadResource(hModule, hResource);
			if (resData == 0)
				return null;
			byte[] uiBytes = new byte[resSize];
			nuint ipMemorySource = LockResource(resData);
			Marshal.Copy((IntPtr)ipMemorySource, uiBytes, 0, (int)resSize);
			return uiBytes;
		}
		finally
		{
			FreeLibrary(hModule);
		}
	}

	public static nuint GetJitHelpersRVA(string clrPath)
	{
		byte[] data = GetResourceFromExecutable(clrPath, "COREXTERNALDATAACCESSRESOURCE", 10);
		return (nuint)BitConverter.ToInt32(data, 52); // TODO: 52 works only in x86
		// This is not magic number.
	}

}
