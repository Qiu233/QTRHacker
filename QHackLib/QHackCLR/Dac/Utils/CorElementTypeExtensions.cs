using System;

namespace QHackCLR.Dac.Utils
{
	internal static class CorElementTypeExtensions
	{
		public static bool IsPrimitive(this CorElementType cet)
		{
			return cet >= CorElementType.Boolean && cet <= CorElementType.Double
				|| cet == CorElementType.NativeInt || cet == CorElementType.NativeUInt;
		}

		public static bool IsValueType(this CorElementType cet)
		{
			return IsPrimitive(cet) || cet == CorElementType.Struct;
		}

		public static bool IsObjectReference(this CorElementType cet)
		{
			return cet == CorElementType.String || cet == CorElementType.Class
				|| cet == CorElementType.Array || cet == CorElementType.SZArray
				|| cet == CorElementType.Object;
		}

		public static string GetBasicTypeFullName(this CorElementType type) => type switch
		{
			CorElementType.Void => "System.Void",
			CorElementType.Boolean => "System.Boolean",
			CorElementType.Char => "System.Char",

			CorElementType.Int8 => "System.SByte",
			CorElementType.UInt8 => "System.Byte",
			CorElementType.Int16 => "System.Int16",
			CorElementType.UInt16 => "System.UInt16",
			CorElementType.Int32 => "System.Int32",
			CorElementType.UInt32 => "System.UInt32",
			CorElementType.Int64 => "System.Int64",
			CorElementType.UInt64 => "System.UInt64",

			CorElementType.Float => "System.Single",
			CorElementType.Double => "System.Double",

			CorElementType.NativeInt => "System.IntPtr",
			CorElementType.NativeUInt => "System.UIntPtr",

			CorElementType.String => "System.String",
			CorElementType.Object => "System.Object",

			_ => "UNKNOWN",
		};

		public static string GetBasicTypeFriendlyName(this CorElementType type) => type switch
		{
			CorElementType.Void => "void",
			CorElementType.Boolean => "bool",
			CorElementType.Char => "char",

			CorElementType.Int8 => "sbyte",
			CorElementType.UInt8 => "byte",
			CorElementType.Int16 => "short",
			CorElementType.UInt16 => "ushort",
			CorElementType.Int32 => "int",
			CorElementType.UInt32 => "uint",
			CorElementType.Int64 => "long",
			CorElementType.UInt64 => "ulong",

			CorElementType.Float => "float",
			CorElementType.Double => "double",

			CorElementType.NativeInt => "nint",
			CorElementType.NativeUInt => "nuint",

			CorElementType.String => "string",
			CorElementType.Object => "object",

			_ => "UNKNOWN",
		};


		public static Type GetTypeForElementType(this CorElementType type) => type switch
		{
			CorElementType.Boolean => typeof(bool),
			CorElementType.Char => typeof(char),
			CorElementType.Double => typeof(double),
			CorElementType.Float => typeof(float),
			CorElementType.NativeInt => typeof(IntPtr),
			CorElementType.NativeUInt => typeof(UIntPtr),

			CorElementType.UInt16 => typeof(ushort),
			CorElementType.Int16 => typeof(short),
			CorElementType.Int32 => typeof(int),
			CorElementType.UInt32 => typeof(uint),
			CorElementType.Int64 => typeof(long),
			CorElementType.UInt64 => typeof(ulong),
			CorElementType.Int8 => typeof(sbyte),
			CorElementType.UInt8 => typeof(byte),
			_ => null,
		};
	}
}