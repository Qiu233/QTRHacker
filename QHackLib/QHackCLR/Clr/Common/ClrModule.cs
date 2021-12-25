using QHackCLR.Clr.Builders;
using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Helpers;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public unsafe class ClrModule : ClrEntity
	{
		public readonly DacpModuleData Data;
		protected IModuleHelper ModuleHelper { get; }
		internal IXCLRDataModule DataModule { get; }
		public string Name { get; }
		public IMetadataImport MetadataImport => ModuleHelper.GetMetadataImport(this);
		public ClrModule(IModuleHelper helper, nuint handle) : base(handle)
		{
			ModuleHelper = helper;
			helper.SOSDac.GetModule(new CLRDATA_ADDRESS(ClrHandle), out IXCLRDataModule dataModule);
			DataModule = dataModule;
			Name = GetName();
			helper.SOSDac.GetModuleData(new CLRDATA_ADDRESS(ClrHandle), out Data);
		}

		public unsafe string GetName() => HelperGlobals.GetString(
				(uint count, char* buf, out uint needed) =>
				DataModule.GetName(count, out needed, buf));

		public unsafe string GetFileName() => HelperGlobals.GetString(
				(uint count, char* buf, out uint needed) =>
				DataModule.GetFileName(count, out needed, buf), 1024);

		public ClrType GetTypeByName(string name) => DefinedTypes.FirstOrDefault(t => t.Name == name);

		/// <summary>
		/// Finds a type by its def or ref token.<br/>
		/// </summary>
		/// <param name="token"></param>
		/// <returns>null if no such type</returns>
		public ClrType ResolveToken(int token) =>
			DefinedTypes.FirstOrDefault(t => t.MDToken == (uint)token) ??
			ReferencedTypes.FirstOrDefault(t => t.MDToken == (uint)token);

		private IReadOnlyList<ClrType> _DefinedTypes;
		private IReadOnlyList<ClrType> _ReferencedTypes;

		public IReadOnlyList<ClrType> DefinedTypes
		{
			get
			{
				if (_DefinedTypes is not null)
					return _DefinedTypes;
				List<ClrType> types = new();
				ModuleHelper.SOSDac.TraverseModuleMap(ModuleMapType.TypeDefToMethodTable, ClrHandle,
					(_, th, _) =>
					{
						types.Add(ModuleHelper.TypeFactory.GetClrType(th));
					}, (void*)0);
				return _DefinedTypes = types;
			}
		}
		public IReadOnlyList<ClrType> ReferencedTypes
		{
			get
			{
				if (_ReferencedTypes is not null)
					return _ReferencedTypes;
				List<ClrType> types = new();
				ModuleHelper.SOSDac.TraverseModuleMap(ModuleMapType.TypeRefToMethodTable, ClrHandle,
					(_, th, _) =>
					{
						types.Add(ModuleHelper.TypeFactory.GetClrType(th));
					}, (void*)0);
				return _ReferencedTypes = types;
			}
		}
	}
}
