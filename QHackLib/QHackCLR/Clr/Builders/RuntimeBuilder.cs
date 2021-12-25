using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
using QHackCLR.DataTargets;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QHackCLR.Metadata.Parse;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Helpers;
using System.Runtime.CompilerServices;

namespace QHackCLR.Clr.Builders
{
	/// <summary>
	/// <para>Flattend data builder for multiple kinds of entity.<br/>
	/// There would be a single RuntimeBuilder instance for each runtime in target process.<br/>
	/// Different from what clrmd does, this builder won't construct topological type system.<br/>
	/// Instead, each entity itself should build it lazily at runtime.<br/>
	/// </para>
	/// 
	/// <para>To keep singleton well, all instantiation should be done in this builder.<br/>
	/// Being requested for an entity by its handle, 
	/// this builder will check the cache first and only instantiate if not cached.<br/>
	/// This principle works for almost everthing that has what called handle, e.g. Types.<br/>
	/// </para>
	/// Note, Modules act as the main elements of this library instead of AppDomains.
	/// </summary>
	internal sealed unsafe class RuntimeBuilder : IClrObjectHelper, ITypeFactory, IMethodHelper, IRuntimeHelper, IHeapHelper,
		IAppDomainHelper, IAssemblyHelper, IModuleHelper, ITypeHelper, IFieldHelper
	{
		private readonly ConcurrentDictionary<nuint, ClrAppDomain> AppDomains = new();
		private readonly ConcurrentDictionary<nuint, ClrModule> Modules = new();
		private readonly ConcurrentDictionary<nuint, ClrType> Types = new();

		public DacLibrary DacLibrary { get; }
		public ClrInfo ClrInfo { get; }
		public ClrRuntime Runtime { get; }

		public ClrHeap Heap => Runtime.Heap;
		public ClrAppDomain AppDomain => Runtime.AppDomain;

		public DataAccess DataAccess => Runtime.DataTarget.DataAccess;
		public ISOSDacInterface SOSDac => DacLibrary.SOSDac;
		public IXCLRDataProcess CLRDataProcess => DacLibrary.ClrDataProcess;

		public ITypeFactory TypeFactory => this;
		public IAssemblyHelper AssemblyHelper => this;
		public IModuleHelper ModuleHelper => this;
		public ITypeHelper TypeHelper => this;
		public IClrObjectHelper ClrObjectHelper => this;
		public IHeapHelper HeapHelper => this;

		internal RuntimeBuilder(ClrInfo clrInfo, DacLibrary dacCibrary)
		{
			ClrInfo = clrInfo;
			DacLibrary = dacCibrary;
			Runtime = new ClrRuntime(clrInfo, this, clrInfo.RuntimeBase);
		}

		public IEnumerable<ClrModule> EnumerateModules(ClrAppDomain appDomain)
		{
			return from asm in SOSDac.GetAssemblyList(appDomain.ClrHandle)
				   from module in SOSDac.GetAssemblyModuleList(appDomain.ClrHandle, asm)
				   select GetModule(module);
		}

		public void Flush()
		{
			CLRDataProcess.Flush();
			AppDomains.Clear();
			Modules.Clear();
			Types.Clear();
		}

		bool IFieldHelper.GetFieldProps(ClrType parentType, int token, out string name, out FieldAttributes attributes, out SigParser sigParser)
		{
			IMetadataImport import = parentType.Module.MetadataImport;
			if (import is null || !import.GetFieldAttributesAndSig(token, out attributes, out sigParser))
			{
				name = null;
				attributes = default;
				sigParser = default;
				return false;
			}
			name = import.GetFieldName(token);

			return true;
		}

		public IMetadataImport GetMetadataImport(ClrModule module)
		{
			return SOSDac.GetMetadataImport(module.ClrHandle);
		}

		public nuint GetStaticFieldAddress(ClrStaticField field)
		{
			ClrType type = field.DeclaringType;
			ClrModule module = type.Module;
			bool shared = type.IsShared;
			if (shared)
			{
				if (AppDomain is null)
					return 0;
				if (!SOSDac.GetModuleData(module.ClrHandle, out DacpModuleData data))
					return 0;
				if (!SOSDac.GetDomainLocalModuleDataFromAppDomain(AppDomain.ClrHandle, (int)data.DwModuleID, out DacpDomainLocalModuleData dlmd))
					return 0;
				if (!shared && !IsInitialized(dlmd, (int)type.MDToken))
					return 0;

				if (field.CorElementType.IsPrimitive())
					return (nuint)(dlmd.PNonGCStaticDataStart + field.Offset);
				else
					return (nuint)(dlmd.PGCStaticDataStart + field.Offset);
			}
			else
			{
				if (!SOSDac.GetDomainLocalModuleDataFromModule(module.ClrHandle, out DacpDomainLocalModuleData dlmd))
					return 0;
				if (field.CorElementType.IsPrimitive())
					return (nuint)(dlmd.PNonGCStaticDataStart + field.Offset);
				else
					return (nuint)(dlmd.PGCStaticDataStart + field.Offset);
			}
		}

		/// <summary>
		/// I don't know what this method is.<br/>
		/// But it seems working well.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		private bool IsInitialized(in DacpDomainLocalModuleData data, int token)
		{
			nuint flagsAddr = (nuint)(data.PClassData + (uint)(token & ~0x02000000u) - 1);
			DataAccess.Read(flagsAddr, out byte flags);
			return (flags & 1) != 0;
		}

		public IEnumerable<ClrField> EnumerateFields(ClrType type)
		{
			SOSDac.GetMethodTableFieldData(type.ClrHandle, out DacpMethodTableFieldData info);
			nuint field = info.FirstField;
			while (field != 0)
			{
				DacpFieldDescData data;
				try { SOSDac.GetFieldDescData(field, out data); } catch { break; }
				if (data.BIsStatic != 0)
					yield return new ClrStaticField(type, this, field);
				else
					yield return new ClrInstanceField(type, this, field);
				field = data.NextField;
			}
		}

		public IEnumerable<ClrMethod> EnumerateVTableMethods(ClrType type)
		{
			nuint mt = type.ClrHandle;
			SOSDac.GetMethodTableData(mt, out DacpMethodTableData mtData);
			for (uint i = 0; i < mtData.WNumMethods; i++)
			{
				SOSDac.GetMethodTableSlot(mt, i, out CLRDATA_ADDRESS slot);
				SOSDac.GetCodeHeaderData(slot, out DacpCodeHeaderData chdata);
				yield return new ClrMethod(this, chdata.MethodDescPtr);
			}
		}
		#region GetOrCreates

		public ClrAppDomain GetAppDomain(nuint handle) => TryGetCache(AppDomains, handle, t => new ClrAppDomain(this, t));
		public ClrType GetClrType(nuint typeHandle) => TryGetCache(Types, typeHandle, t => new ClrType(this, t));
		public ClrModule GetModule(nuint moduleBase) => TryGetCache(Modules, moduleBase, t => new ClrModule(this, t));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T TryGetCache<T>(IDictionary<nuint, T> dict, nuint handle, Func<nuint, T> ctor) where T : class
		{
			if (handle == 0) return null;
			return dict.TryGetValue(handle, out T value) ? value : (dict[handle] = ctor(handle));
		}


		#endregion
	}
}
