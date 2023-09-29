using QHackCLR.Builders;
using QHackCLR.DAC;
using QHackCLR.DAC.DACP;
using QHackCLR.DAC.Defs;

namespace QHackCLR.Entities;

public unsafe class CLRAppDomain : CLREntity
{
	private readonly IAppDomainHelper AppDomainHelper;
	private IReadOnlyList<CLRModule>? m_Modules;
	public string Name { get; }

	internal readonly DacpAppDomainData Data;
	internal readonly IXCLRDataAppDomain DataAppDomain;
	internal CLRAppDomain(IAppDomainHelper helper, nuint handle) : base(handle)
	{
		AppDomainHelper = helper;
		Name = helper.SOSDac.GetAppDomainName(NativeHandle) ?? throw new QHackCLRException($"Cannot get name of AppDomain: {NativeHandle}");
		Data = new DacpAppDomainData();
		fixed (DacpAppDomainData* ptr = &Data)
			helper.SOSDac.GetAppDomainData(NativeHandle, ptr);

		helper.DACLibrary.ClrDataProcess.GetAppDomainByUniqueID(Data.dwId, out DataAppDomain);
	}

	public CLRRuntime Runtime => AppDomainHelper.Runtime;

	public IReadOnlyList<CLRModule> Modules => m_Modules ??= AppDomainHelper.EnumerateModules(this).ToList();
}
