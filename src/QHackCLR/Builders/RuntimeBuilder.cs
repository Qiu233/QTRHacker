using QHackCLR.Common;
using QHackCLR.DAC;
using QHackCLR.DAC.DACP;
using QHackCLR.DAC.Defs;
using QHackCLR.DataTargets;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Builders;

internal interface IRuntimeHelper
{
	ISOSDacInterface SOSDac { get; }
	IXCLRDataProcess CLRDataProcess { get; }
	ITypeFactory TypeFactory { get; }
	IHeapHelper HeapHelper { get; }
	DataAccess DataAccess { get; }
	DACLibrary DACLibrary { get; }
	CLRAppDomain GetAppDomain(nuint handle);
	void Flush();
}

internal unsafe class RuntimeBuilder :
	IRuntimeHelper, IAppDomainHelper,
	IAssemblyHelper, IModuleHelper,
	ITypeFactory, ITypeHelper,
	IFieldHelper, IMethodHelper,
	IHeapHelper, IObjectHelper
{
	private readonly ClrInfo ClrInfo;
	private readonly Dictionary<nuint, CLRAppDomain> AppDomains = new();
	private readonly Dictionary<nuint, CLRModule> Modules = new();
	private readonly Dictionary<nuint, CLRType> Types = new();
	public CLRRuntime Runtime { get; }

	public ISOSDacInterface SOSDac => DACLibrary.SOSDac;

	public IXCLRDataProcess CLRDataProcess => DACLibrary.ClrDataProcess;

	public DataAccess DataAccess => Runtime.DataTarget.DataAccess;

	public DACLibrary DACLibrary { get; }

	public ITypeFactory TypeFactory => this;
	public IHeapHelper HeapHelper => this;
	public IModuleHelper ModuleHelper => this;
	public IAssemblyHelper AssemblyHelper => this;

	public ITypeHelper TypeHelper => this;

	public CLRHeap Heap => Runtime.Heap;

	public CLRAppDomain AppDomain => Runtime.AppDomain;

	public IObjectHelper ObjectHelper => this;

	public RuntimeBuilder(ClrInfo clrInfo, DACLibrary dac)
	{
		ClrInfo = clrInfo;
		DACLibrary = dac;
		Runtime = new CLRRuntime(clrInfo, this);
		dac.DataTarget.SetMagicCallback(() => dac.ClrDataProcess.Flush());
	}

	void IRuntimeHelper.Flush()
	{
		DACLibrary.DataTarget.EnterMagicCallbackContext();
		try
		{
			DacpWorkRequestData a;
			DACLibrary.SOSDac.GetWorkRequestData(DacDataTargetImpl.MAGIC_CALLBACK_CONSTANT, &a);
		}
		finally
		{
			DACLibrary.DataTarget.ExitMagicCallbackContext();
		}
	}

	public CLRAppDomain GetAppDomain(nuint handle)
	{
		if (handle == 0)
			throw new ArgumentException("Handle is null");
		if (AppDomains.TryGetValue(handle, out var v))
			return v;
		return AppDomains[handle] = new CLRAppDomain(this, handle);
	}

	public CLRType? GetCLRType(nuint typeHandle)
	{
		if (typeHandle == 0)
			return null;
		if (Types.TryGetValue(typeHandle, out var v))
			return v;
		return Types[typeHandle] = new CLRType(this, typeHandle);
	}

	public CLRModule GetModule(nuint moduleBase)
	{
		if (moduleBase == 0)
			throw new ArgumentException("ModuleBase is null");
		if (Modules.TryGetValue(moduleBase, out var v))
			return v;
		return Modules[moduleBase] = new CLRModule(this, moduleBase);
	}

	public IEnumerable<CLRModule> EnumerateModules(CLRAppDomain appDomain)
	{
		List<CLRModule> res = new();
		var assemblies = SOSDac.GetAssemblyList(appDomain.NativeHandle);
		foreach (var assembly in assemblies)
		{
			var modules = SOSDac.GetAssemblyModuleList(appDomain.NativeHandle, assembly);
			foreach (var module in modules)
				res.Add(GetModule(module));
		}
		return res;
	}

	public IEnumerable<CLRField> EnumerateFields(CLRType type)
	{
		DacpMethodTableFieldData info;
		SOSDac.GetMethodTableFieldData(type.NativeHandle, &info);
		List<CLRField> fields = new();
		var field = info.FirstField;
		while (field != 0)
		{
			DacpFieldDescData data;
			if (SOSDac.GetFieldDescData(field, &data).Failed)
				break;
			if (data.bIsStatic != 0)
				fields.Add(new CLRStaticField(type, this, field));
			else
				fields.Add(new CLRInstanceField(type, this, field));
			field = data.NextField;
		}
		return fields;
	}

	public IEnumerable<CLRMethod> EnumerateVTableMethods(CLRType type)
	{
		var mt = type.NativeHandle;
		DacpMethodTableData mtData;
		SOSDac.GetMethodTableData(mt, &mtData);
		List<CLRMethod> methods = new();
		for (int i = 0; i < mtData.wNumMethods; i++)
		{
			CLRDATA_ADDRESS slot;
			DacpCodeHeaderData chdata;
			SOSDac.GetMethodTableSlot(mt, (uint)i, &slot);
			SOSDac.GetCodeHeaderData(slot, &chdata);
			methods.Add(new CLRMethod(this, chdata.MethodDescPtr));
		}
		return methods;
	}

	public bool GetFieldProps(CLRType declType, uint token, out string? name, out FieldAttributes? attributes)
	{
		IMetaDataImport import = declType.Module.MetaDataImport;
		uint needed, attr;
		if (import.GetFieldProps(token, null, null, 0, &needed, &attr, null, null, null, null, null).Failed)
		{
			name = null;
			attributes = null;
			return false;
		}
		attributes = (FieldAttributes)attr;
		char[] buffer = new char[needed];
		fixed (char* ptr = buffer)
		{
			import.GetFieldProps(token, null, ptr, needed, &needed, null, null, null, null, null, null);
			name = new string(ptr);
		}
		return true;
	}

	private bool IsInitialized(DacpDomainLocalModuleData* data, int token)
	{
		CLRDATA_ADDRESS flagsAddr = (data->pClassData + (ulong)(token & ~0x02000000u) - 1);
		byte flags = DataAccess.ReadValue<byte>(flagsAddr);
		return (flags & 1) != 0;
	}

	public nuint GetStaticFieldAddress(CLRStaticField field)
	{
		CLRType type = field.DeclaringType;
		CLRModule module = type.Module;
		bool shared = type.IsShared;
		DacpDomainLocalModuleData dlmd;
		if (shared)
		{
			DacpModuleData data;
			if (SOSDac.GetModuleData(module.NativeHandle, &data).Failed)
				return nuint.Zero;
			if (SOSDac.GetDomainLocalModuleDataFromAppDomain(AppDomain.NativeHandle, (int)data.dwModuleID, &dlmd).Failed)
				return nuint.Zero;
			if (!shared && !IsInitialized(&dlmd, (int)type.MDToken))
				return nuint.Zero;

			if (field.ElementType.IsPrimitive())
				return dlmd.pNonGCStaticDataStart + (nuint)field.Offset;
			else
				return dlmd.pGCStaticDataStart + (nuint)field.Offset;
		}
		else
		{
			if (SOSDac.GetDomainLocalModuleDataFromModule(module.NativeHandle, &dlmd).Failed)
				return nuint.Zero;
		}
		if (field.ElementType.IsPrimitive())
			return dlmd.pNonGCStaticDataStart + (nuint)field.Offset;
		else
			return dlmd.pGCStaticDataStart + (nuint)field.Offset;
	}

	public IMetaDataImport GetMetadataImport(CLRModule module)
	{
		SOSDac.GetModule(module.NativeHandle, out IXCLRDataModule dataModule);
		Guid g = Guid.Parse("7DAC8207-D3AE-4c75-9B67-92801A497D44");
		nint addr = Marshal.GetIUnknownForObject(dataModule);
		Marshal.QueryInterface(addr, ref g, out nint ppv);
		return (IMetaDataImport)Marshal.GetTypedObjectForIUnknown(ppv, typeof(IMetaDataImport));
	}
}
