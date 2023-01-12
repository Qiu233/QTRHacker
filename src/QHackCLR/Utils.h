#pragma once
#include "pre.h"

using namespace System;
namespace QHackCLR {
	public ref class Utils abstract sealed {
	internal:
		static CorElementType GetCorElementTypeFromName(String^ name);
		static bool CorElementTypeIsPrimitive(CorElementType cet);
		static bool CorElementTypeIsValueType(CorElementType cet);
		static bool CorElementTypeIsObjectReference(CorElementType cet);
	public:
		static String^ GetJitHelperFunctionName(ISOSDacInterface* sosDac, UIntPtr addr);
	};

	ref class QHackCLRException : Exception {
	public:
		QHackCLRException(String^ str) : Exception(str) {}
	};

	ref class QHackCLRMismatchedArchitectureException : QHackCLRException {
	public:
		QHackCLRMismatchedArchitectureException(String^ str) : QHackCLRException(str) {}
	};
}
