using QHackCLR.Builders;
using QHackCLR.Common;
using QHackCLR.DAC.DACP;
using QHackCLR.DAC.Defs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public unsafe class CLRMethod : CLREntity
{
	private CLRType? m_DeclaringType;
	internal readonly IMethodHelper MethodHelper;
	internal readonly DacpMethodDescData Data;
	public string Signature { get; }
	internal CLRMethod(IMethodHelper helper, nuint handle) : base(handle)
	{
		MethodHelper = helper;
		Data = new DacpMethodDescData();
		fixed (DacpMethodDescData* ptr = &Data)
			helper.SOSDac.GetMethodDescData(NativeHandle, 0, ptr, 0, null, null);

		Signature = GetMethodDescName(helper.SOSDac, NativeHandle)!;
	}

	public CLRType? DeclaringType => m_DeclaringType ??= MethodHelper.TypeFactory.GetCLRType(Data.MethodTablePtr);

	public nuint NativeCode => Data.NativeCodeAddr;
	public string Name
	{
		get
		{
			string? signature = Signature;
			int last = signature.LastIndexOf('(');
			if (last > 0)
			{
				int first = signature.LastIndexOf('.', last - 1);
				if (first != -1 && signature[first - 1] == '.')
					first--;
				return signature.Substring(first + 1, last - first - 1);
			}
			return "{error}";
		}
	}

	internal static string? GetMethodDescName(ISOSDacInterface SOSDac, CLRDATA_ADDRESS md)
	{
		if (md == 0)
			return null;
		uint needed;
		if (SOSDac.GetMethodDescName(md, 0, null, &needed).Failed)
			return null;
		char[] buffer = new char[needed];
		uint actuallyNeeded;
		fixed (char* ptr = buffer)
			if (SOSDac.GetMethodDescName(md, needed, ptr, &actuallyNeeded).Failed)
				return null;
		if (needed != actuallyNeeded)
		{
			buffer = new char[actuallyNeeded];
			fixed (char* ptr = buffer)
				if (SOSDac.GetMethodDescName(md, actuallyNeeded, ptr, &actuallyNeeded).Failed)
					return null;
		}
		fixed (char* ptr = buffer)
			return new string(ptr);
	}

}
