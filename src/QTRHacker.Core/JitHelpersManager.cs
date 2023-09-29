using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace QTRHacker.Core;

public unsafe partial class JitHelpersManager
{
	private const int MaxNumJitHelpers = 500;
	private ImmutableArray<JitHelperEntry> _JitHelpers;
	public record struct JitHelperEntry(string Name, nuint Address);
	public GameContext Context { get; }
	public nuint JitHelpersBaseAddress { get; }
	public nuint this[int index] => JitHelpersBaseAddress + (nuint)(index * sizeof(nuint));
	public ImmutableArray<JitHelperEntry> JitHelpers
	{
		get
		{
			if (_JitHelpers.IsDefault)
				_JitHelpers = EnumerateJitHelpers().ToImmutableArray();
			return _JitHelpers;
		}
	}
	public JitHelpersManager(GameContext context)
	{
		Context = context;
		nuint offset = GetJitHelpersRVA(Context.HContext.Runtime.ClrInfo.ClrModulePath);
		JitHelpersBaseAddress = Context.HContext.Runtime.BaseAddress + offset;
	}
	public IEnumerable<JitHelperEntry> EnumerateJitHelpers()
	{
		return EnumerateJitHelpers(MaxNumJitHelpers).Where(t => t.Name is not null && t.Name.Trim().Length != 0).Distinct();
	}

	public nuint GetJitHelperAddress(string name)
	{
		return JitHelpers.First(t => t.Name == name).Address;
	}

	private string GetJitHelperName(nuint addr)
	{
		return Context.HContext.Runtime.GetJitHelperFunctionName(addr);
		//return QHackCLR.Utils.GetJitHelperFunctionName(Context.HContext.Runtime.DacLibrary.SOSDac, addr);
	}
	private IEnumerable<JitHelperEntry> EnumerateJitHelpers(int len)
	{
		int i = 0;
		while (i < len)
		{
			nuint addr = Context.HContext.DataAccess.ReadValue<nuint>(this[i++]);
			string name = GetJitHelperName(addr);
			yield return new JitHelperEntry(name, addr);
		}
	}
}
