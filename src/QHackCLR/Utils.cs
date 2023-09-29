using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QHackCLR;

internal static class Utils
{
	public static CorElementType GetCorElementTypeFromName(string name)
	{
		return name switch
		{
			"System.SByte" => CorElementType.ELEMENT_TYPE_I1,
			"System.Int16" => CorElementType.ELEMENT_TYPE_I2,
			"System.Int32" => CorElementType.ELEMENT_TYPE_I4,
			"System.Int64" => CorElementType.ELEMENT_TYPE_I8,
			"System.IntPtr" => CorElementType.ELEMENT_TYPE_I,
			"System.Byte" => CorElementType.ELEMENT_TYPE_U1,
			"System.UInt16" => CorElementType.ELEMENT_TYPE_U2,
			"System.UInt32" => CorElementType.ELEMENT_TYPE_U4,
			"System.UInt64" => CorElementType.ELEMENT_TYPE_U8,
			"System.UIntPtr" => CorElementType.ELEMENT_TYPE_U,
			"System.Boolean" => CorElementType.ELEMENT_TYPE_BOOLEAN,
			"System.Single" => CorElementType.ELEMENT_TYPE_R4,
			"System.Double" => CorElementType.ELEMENT_TYPE_R8,
			"System.Char" => CorElementType.ELEMENT_TYPE_CHAR,
			_ => CorElementType.ELEMENT_TYPE_VALUETYPE,
		};
	}

	public static bool IsPrimitive(this CorElementType cet)
	{
		return cet >= CorElementType.ELEMENT_TYPE_BOOLEAN && cet <= CorElementType.ELEMENT_TYPE_R8
			|| cet == CorElementType.ELEMENT_TYPE_I || cet == CorElementType.ELEMENT_TYPE_U;
	}
	public static bool IsValueType(this CorElementType cet)
	{
		return cet.IsPrimitive() || cet == CorElementType.ELEMENT_TYPE_VALUETYPE;
	}
	public static bool IsObjectReference(this CorElementType cet)
	{
		return cet == CorElementType.ELEMENT_TYPE_STRING || cet == CorElementType.ELEMENT_TYPE_CLASS
			|| cet == CorElementType.ELEMENT_TYPE_ARRAY || cet == CorElementType.ELEMENT_TYPE_SZARRAY
			|| cet == CorElementType.ELEMENT_TYPE_OBJECT;
	}
}
