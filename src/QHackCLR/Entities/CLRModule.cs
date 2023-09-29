using QHackCLR.Builders;
using QHackCLR.Common;
using QHackCLR.DAC.DACP;
using QHackCLR.DAC.Defs;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QHackCLR.Entities;

public unsafe class CLRModule : CLREntity
{
	private IReadOnlyList<CLRType?>? m_DefinedTypes;
	private IReadOnlyList<CLRType?>? m_ReferencedTypes;
	internal readonly IModuleHelper ModuleHelper;
	internal readonly IXCLRDataModule DataModule;
	public string Name { get; }
	public string FileName { get; }
	internal readonly DacpModuleData Data;

	public IReadOnlyList<CLRType?> DefinedTypes => m_DefinedTypes ??= Traverse(ModuleMapType.TYPEDEFTOMETHODTABLE);
	public IReadOnlyList<CLRType?> ReferencedTypes => m_ReferencedTypes ??= Traverse(ModuleMapType.TYPEDEFTOMETHODTABLE);

	internal CLRModule(IModuleHelper helper, nuint ClrHandle) : base(ClrHandle)
	{
		ModuleHelper = helper;
		helper.SOSDac.GetModule(NativeHandle, out DataModule);

		char[] nameBuffer = new char[1024];
		fixed (char* ptr = nameBuffer)
		{
			DataModule.GetName(1024, out _, ptr);
			Name = new string(ptr);
			DataModule.GetFileName(1024, out _, ptr);
			FileName = new string(ptr);
		}
		Data = new DacpModuleData();
		fixed (DacpModuleData* ptr = &Data)
			helper.SOSDac.GetModuleData(NativeHandle, ptr);
	}

	internal IReadOnlyList<CLRType?> Traverse(ModuleMapType type)
	{
		var holder = new List<CLRDATA_ADDRESS>();
		ModuleHelper.SOSDac.TraverseModuleMap(type, NativeHandle, (i, mt, token) => holder.Add(mt), null);
		return holder.Select(mt => ModuleHelper.TypeFactory.GetCLRType(mt)).ToList();
	}

	public CLRType? GetTypeByName(string name)
	{
		return DefinedTypes.FirstOrDefault(t => t?.Name == name);
	}

	internal IMetaDataImport MetaDataImport => ModuleHelper.GetMetadataImport(this);
}
