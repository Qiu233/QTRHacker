#pragma once
using namespace System;
using namespace System::Runtime::CompilerServices;
using namespace Microsoft::CodeAnalysis;

namespace Microsoft {
	namespace CodeAnalysis {
		[CompilerGeneratedAttribute]
		ref class EmbeddedAttribute sealed : Attribute {

		};
	}
}
namespace System {
	namespace Runtime {
		namespace CompilerServices {
			[CompilerGenerated]
			[Embedded]
			ref class IsUnmanagedAttribute sealed : Attribute
			{
			};

			[CompilerGenerated]
			[Embedded]
			[AttributeUsage(AttributeTargets::Class | AttributeTargets::Property | AttributeTargets::Field | AttributeTargets::Event
				| AttributeTargets::Parameter | AttributeTargets::ReturnValue | AttributeTargets::GenericParameter, AllowMultiple = false, Inherited = false)]
			ref class NativeIntegerAttribute : Attribute {
			public:
				NativeIntegerAttribute() {
					this->TransformFlags = gcnew array<bool>{true};
				}
				NativeIntegerAttribute(array<bool>^ A_1) {
					this->TransformFlags = A_1;
				}
				initonly array<bool>^ TransformFlags;
			};
		}
	}
}

#define nuint [NativeInteger] System::UIntPtr
