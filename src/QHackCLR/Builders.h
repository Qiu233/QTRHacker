#pragma once
#include "pre.h"
#include "defs.h"

using namespace System;
using namespace System::Linq;
using namespace System::Reflection;
using namespace System::Collections;
using namespace System::Collections::Concurrent;
using namespace System::Collections::Generic;
using namespace QHackCLR;
using namespace QHackCLR::Dac;
using namespace QHackCLR::DataTargets;

namespace QHackCLR {
	namespace Builders {
		namespace Helpers {
			public interface class IAppDomainHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property IModuleHelper^ ModuleHelper {IModuleHelper^ get(); }
				property IAssemblyHelper^ AssemblyHelper { IAssemblyHelper^ get(); }
				property Dac::DacLibrary^ DacLibrary { Dac::DacLibrary^ get(); }
				property Common::ClrRuntime^ Runtime { Common::ClrRuntime^ get(); }
				Common::ClrModule^ GetModule(nuint moduleBase);
				System::Collections::Generic::IEnumerable<Common::ClrModule^>^ EnumerateModules(Common::ClrAppDomain^ appDomain);
			};
			public interface class IAssemblyHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property IModuleHelper^ ModuleHelper {IModuleHelper^ get(); }
			};
			public interface class IClrObjectHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property ITypeFactory^ TypeFactory { ITypeFactory^ get(); }
				property DataTargets::DataAccess^ DataAccess { DataTargets::DataAccess^ get(); }
			};
			public interface class IFieldHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property ITypeFactory^ TypeFactory { ITypeFactory^ get(); }
				property DataTargets::DataAccess^ DataAccess { DataTargets::DataAccess^ get(); }
				bool GetFieldProps(Common::ClrType^ declaringType, int token, String^% name, FieldAttributes% attributes);
				UIntPtr GetStaticFieldAddress(Common::ClrStaticField^ field);
			};
			public interface class IHeapHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property ITypeFactory^ TypeFactory { ITypeFactory^ get(); }
			};
			public interface class IMethodHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property ITypeFactory^ TypeFactory { ITypeFactory^ get(); }
			};
			public interface class IModuleHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property ITypeFactory^ TypeFactory { ITypeFactory^ get(); }
				property ITypeHelper^ TypeHelper { ITypeHelper^ get(); }
				property Common::ClrHeap^ Heap { Common::ClrHeap^ get(); }
				property Common::ClrAppDomain^ AppDomain { Common::ClrAppDomain^ get(); }
				IMetaDataImport* GetMetadataImport(Common::ClrModule^ module);
				Common::ClrAppDomain^ GetAppDomain(UIntPtr handle);
			};
			public interface class IRuntimeHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property IXCLRDataProcess* CLRDataProcess { IXCLRDataProcess* get(); }
				property ITypeFactory^ TypeFactory { ITypeFactory^ get(); }
				property IHeapHelper^ HeapHelper { IHeapHelper^ get(); }
				property DataTargets::DataAccess^ DataAccess { DataTargets::DataAccess^ get(); }
				property Dac::DacLibrary^ DacLibrary { Dac::DacLibrary^ get(); }
				Common::ClrAppDomain^ GetAppDomain(UIntPtr handle);
				void Flush();
			};
			public interface class ITypeHelper {
				property ISOSDacInterface* SOSDac { ISOSDacInterface* get(); }
				property ITypeFactory^ TypeFactory { ITypeFactory^ get(); }
				property Common::ClrHeap^ Heap { Common::ClrHeap^ get(); }
				property DataTargets::DataAccess^ DataAccess { DataTargets::DataAccess^ get(); }
				property IClrObjectHelper^ ClrObjectHelper { IClrObjectHelper^ get(); }
				System::Collections::Generic::IEnumerable<Common::ClrField^>^ EnumerateFields(Common::ClrType^ type);
				System::Collections::Generic::IEnumerable<Common::ClrMethod^>^ EnumerateVTableMethods(Common::ClrType^ type);
				Common::ClrModule^ GetModule(UIntPtr handle);
			};
		}
	}
}
using namespace QHackCLR::Builders::Helpers;
namespace QHackCLR {
	namespace Builders {
		public interface class ITypeFactory {
			Common::ClrType^ GetClrType(UIntPtr typeHandle);
		};

		public ref class RuntimeBuilder sealed : IClrObjectHelper, ITypeFactory, IMethodHelper, IRuntimeHelper, IHeapHelper,
			IAppDomainHelper, IAssemblyHelper, IModuleHelper, ITypeHelper, IFieldHelper
		{
		private:
			initonly ConcurrentDictionary<UIntPtr, Common::ClrAppDomain^>^ AppDomains = gcnew ConcurrentDictionary<UIntPtr, Common::ClrAppDomain^>();
			initonly ConcurrentDictionary<UIntPtr, Common::ClrModule^>^ Modules = gcnew ConcurrentDictionary<UIntPtr, Common::ClrModule^>();
			initonly ConcurrentDictionary<UIntPtr, Common::ClrType^>^ Types = gcnew ConcurrentDictionary<UIntPtr, Common::ClrType^>();
			Dac::DacLibrary^ m_DacLibrary;
			DataTargets::ClrInfo^ m_ClrInfo;
			Common::ClrRuntime^ m_Runtime;

			bool IsInitialized(DacpDomainLocalModuleData* data, int token);

		public:

			virtual property Common::ClrHeap^ Heap {
				Common::ClrHeap^ get() sealed;
			}
			virtual property Common::ClrAppDomain^ AppDomain {
				Common::ClrAppDomain^ get() sealed;
			}

			virtual property Dac::DacLibrary^ DacLibrary {
				Dac::DacLibrary^ get() sealed { return m_DacLibrary; }
			}
			virtual property Common::ClrRuntime^ Runtime {
				Common::ClrRuntime^ get() sealed { return m_Runtime; }
			}
			property DataTargets::ClrInfo^ ClrInfo {
				DataTargets::ClrInfo^ get() { return m_ClrInfo; }
			}

			virtual property ITypeFactory^ TypeFactory {ITypeFactory^ get() sealed { return this; }}
			virtual property IAssemblyHelper^ AssemblyHelper {IAssemblyHelper^ get() sealed { return this; }}
			virtual property IModuleHelper^ ModuleHelper {IModuleHelper^ get() sealed { return this; }}
			virtual property ITypeHelper^ TypeHelper {ITypeHelper^ get() sealed { return this; }}
			virtual property IClrObjectHelper^ ClrObjectHelper {IClrObjectHelper^ get() sealed { return this; }}
			virtual property IHeapHelper^ HeapHelper {IHeapHelper^ get() sealed { return this; }}

		internal:
			RuntimeBuilder(DataTargets::ClrInfo^ clrInfo, Dac::DacLibrary^ dacCibrary);
		public:
			virtual property IXCLRDataProcess* CLRDataProcess {
				IXCLRDataProcess* get() sealed;
			}
			virtual property ISOSDacInterface* SOSDac {
				ISOSDacInterface* get() sealed;
			}
			virtual property DataTargets::DataAccess^ DataAccess {
				DataTargets::DataAccess^ get() sealed;
			}

		public:
			virtual System::Collections::Generic::IEnumerable<Common::ClrModule^>^ EnumerateModules(Common::ClrAppDomain^ appDomain) sealed;

			virtual IMetaDataImport* GetMetadataImport(Common::ClrModule^ module) sealed;
			virtual UIntPtr GetStaticFieldAddress(Common::ClrStaticField^ field) sealed;
			virtual System::Collections::Generic::IEnumerable<Common::ClrField^>^ EnumerateFields(Common::ClrType^ type) sealed;
			virtual System::Collections::Generic::IEnumerable<Common::ClrMethod^>^ EnumerateVTableMethods(Common::ClrType^ type) sealed;

			virtual bool GetFieldProps(Common::ClrType^ parentType, int token, String^% name, FieldAttributes% attributes) sealed;

			virtual void Flush() sealed;

			virtual Common::ClrAppDomain^ GetAppDomain(UIntPtr handle) sealed;
			virtual Common::ClrType^ GetClrType(UIntPtr handle) sealed;
			virtual Common::ClrModule^ GetModule(UIntPtr handle) sealed;
		};
	}
}