#pragma once
#include "pre.h"

using namespace System;
namespace QHackCLR {
	ref class Utils abstract sealed {
	public:
		static CorElementType GetCorElementTypeFromName(String^ name);
		static bool CorElementTypeIsPrimitive(CorElementType cet);
		static bool CorElementTypeIsValueType(CorElementType cet);
		static bool CorElementTypeIsObjectReference(CorElementType cet);
	};

	ref class QHackCLRException : Exception {
	public:
		QHackCLRException(String^ str): Exception(str) {}
	};

	ref class QHackCLRMismatchedArchitectureException : QHackCLRException {
	public:
		QHackCLRMismatchedArchitectureException(String^ str): QHackCLRException(str) {}
	};
}
