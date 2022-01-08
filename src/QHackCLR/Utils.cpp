#include "pre.h"
#include "Utils.h"

using namespace System;
namespace QHackCLR {
	CorElementType Utils::GetCorElementTypeFromName(String^ name) {
		if (name == "System.SByte")
			return CorElementType::ELEMENT_TYPE_I1;
		else if (name == "System.Int16")
			return CorElementType::ELEMENT_TYPE_I2;
		else if (name == "System.Int32")
			return CorElementType::ELEMENT_TYPE_I4;
		else if (name == "System.Int64")
			return CorElementType::ELEMENT_TYPE_I8;
		else if (name == "System.IntPtr")
			return CorElementType::ELEMENT_TYPE_I;

		else if (name == "System.Byte")
			return CorElementType::ELEMENT_TYPE_U1;
		else if (name == "System.UInt16")
			return CorElementType::ELEMENT_TYPE_U2;
		else if (name == "System.UInt32")
			return CorElementType::ELEMENT_TYPE_U4;
		else if (name == "System.UInt64")
			return CorElementType::ELEMENT_TYPE_U8;
		else if (name == "System.UIntPtr")
			return CorElementType::ELEMENT_TYPE_U;

		else if (name == "System.Boolean")
			return CorElementType::ELEMENT_TYPE_BOOLEAN;

		else if (name == "System.Single")
			return CorElementType::ELEMENT_TYPE_R4;
		else if (name == "System.Double")
			return CorElementType::ELEMENT_TYPE_R8;
		else if (name == "System.Char")
			return CorElementType::ELEMENT_TYPE_CHAR;

		return CorElementType::ELEMENT_TYPE_VALUETYPE;
	}


	bool Utils::CorElementTypeIsPrimitive(CorElementType cet)
	{
		return cet >= CorElementType::ELEMENT_TYPE_BOOLEAN && cet <= CorElementType::ELEMENT_TYPE_R8
			|| cet == CorElementType::ELEMENT_TYPE_I || cet == CorElementType::ELEMENT_TYPE_U;
	}

	bool Utils::CorElementTypeIsValueType(CorElementType cet)
	{
		return CorElementTypeIsPrimitive(cet) || cet == CorElementType::ELEMENT_TYPE_VALUETYPE;
	}

	bool Utils::CorElementTypeIsObjectReference(CorElementType cet)
	{
		return cet == CorElementType::ELEMENT_TYPE_STRING || cet == CorElementType::ELEMENT_TYPE_CLASS
			|| cet == CorElementType::ELEMENT_TYPE_ARRAY || cet == CorElementType::ELEMENT_TYPE_SZARRAY
			|| cet == CorElementType::ELEMENT_TYPE_OBJECT;
	}
}
