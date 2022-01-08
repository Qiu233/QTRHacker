#pragma once
#include "dummys.h"

namespace QHackCLR {
	ref class Utils;

	namespace Builders {
		interface class ITypeFactory;
		ref class RuntimeBuilder;
		namespace Helpers {
			interface class IAppDomainHelper;
			interface class IAssemblyHelper;
			interface class IClrObjectHelper;
			interface class IFieldHelper;
			interface class IHeapHelper;
			interface class IMethodHelper;
			interface class IModuleHelper;
			interface class IRuntimeHelper;
			interface class ITypeHelper;
		}
	}
	namespace DacHelpers {
		ref class GlobalHelpers;
		ref class SOSHelpers;
	}
	namespace DataTargets {
		interface class IDataReader;
		ref class DataAccess;
		enum class ClrFlavor;
		ref class ClrInfo;
		ref class DataTarget;
		ref class ClrInfoProvider;
	}
	namespace Dac {
		ref class DacLibrary;
	}
	namespace Common {
		interface class IClrHandled;
		interface class IHasMetaData;
		ref class ClrEntity;
		ref class ClrRuntime;
		ref class ClrHeap;
		ref class ClrAppDomain;
		ref class ClrModule;
		ref class ClrType;
		ref class ClrMethod;
		ref class ClrField;
		ref class ClrInstanceField;
		ref class ClrStaticField;
		ref class AddressableTypedEntity;
		ref class ClrObject;
		ref class ClrValue;
	}
}
