using QHackCLR.Clr.Builders;
using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac.Helpers;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public class ClrAppDomain : ClrEntity
	{
		public readonly DacpAppDomainData Data;
		protected readonly IAppDomainHelper AppDomainHelper;
		internal readonly IXCLRDataAppDomain DataAppDomain;
		public string Name { get; }
		public ClrAppDomain(IAppDomainHelper helper, nuint handle) : base(handle)
		{
			AppDomainHelper = helper;
			Name = helper.SOSDac.GetAppDomainName(ClrHandle);
			helper.SOSDac.GetAppDomainData(handle, out Data);
			helper.DacLibrary.ClrDataProcess.GetAppDomainByUniqueID(Data.DwId, out DataAppDomain);
		}

		public int ID => (int)Data.DwId;

		public ClrRuntime Runtime => AppDomainHelper.Runtime;

		public IEnumerable<ClrModule> EnumerateModules() => AppDomainHelper.EnumerateModules(this);

		private IReadOnlyList<ClrModule> _Modules;
		public IReadOnlyList<ClrModule> Modules => _Modules ??= EnumerateModules().ToImmutableList();
	}
}
