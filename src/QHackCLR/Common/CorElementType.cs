using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Common;

public enum CorElementType
{
	ELEMENT_TYPE_END = 0x00,
	ELEMENT_TYPE_VOID = 0x01,
	ELEMENT_TYPE_BOOLEAN = 0x02,
	ELEMENT_TYPE_CHAR = 0x03,
	ELEMENT_TYPE_I1 = 0x04,
	ELEMENT_TYPE_U1 = 0x05,
	ELEMENT_TYPE_I2 = 0x06,
	ELEMENT_TYPE_U2 = 0x07,
	ELEMENT_TYPE_I4 = 0x08,
	ELEMENT_TYPE_U4 = 0x09,
	ELEMENT_TYPE_I8 = 0x0a,
	ELEMENT_TYPE_U8 = 0x0b,
	ELEMENT_TYPE_R4 = 0x0c,
	ELEMENT_TYPE_R8 = 0x0d,
	ELEMENT_TYPE_STRING = 0x0e,

	// every type above PTR will be simple type
	ELEMENT_TYPE_PTR = 0x0f,     // PTR <type>
	ELEMENT_TYPE_BYREF = 0x10,     // BYREF <type>

	// Please use ELEMENT_TYPE_VALUETYPE. ELEMENT_TYPE_VALUECLASS is deprecated.
	ELEMENT_TYPE_VALUETYPE = 0x11,     // VALUETYPE <class Token>
	ELEMENT_TYPE_CLASS = 0x12,     // CLASS <class Token>
	ELEMENT_TYPE_VAR = 0x13,     // a class type variable VAR <number>
	ELEMENT_TYPE_ARRAY = 0x14,     // MDARRAY <type> <rank> <bcount> <bound1> ... <lbcount> <lb1> ...
	ELEMENT_TYPE_GENERICINST = 0x15,     // GENERICINST <generic type> <argCnt> <arg1> ... <argn>
	ELEMENT_TYPE_TYPEDBYREF = 0x16,     // TYPEDREF  (it takes no args) a typed referece to some other type

	ELEMENT_TYPE_I = 0x18,     // native integer size
	ELEMENT_TYPE_U = 0x19,     // native unsigned integer size
	ELEMENT_TYPE_FNPTR = 0x1b,     // FNPTR <complete sig for the function including calling convention>
	ELEMENT_TYPE_OBJECT = 0x1c,     // Shortcut for System.Object
	ELEMENT_TYPE_SZARRAY = 0x1d,     // Shortcut for single dimension zero lower bound array
									 // SZARRAY <type>
	ELEMENT_TYPE_MVAR = 0x1e,     // a method type variable MVAR <number>

	// This is only for binding
	ELEMENT_TYPE_CMOD_REQD = 0x1f,     // required C modifier : E_T_CMOD_REQD <mdTypeRef/mdTypeDef>
	ELEMENT_TYPE_CMOD_OPT = 0x20,     // optional C modifier : E_T_CMOD_OPT <mdTypeRef/mdTypeDef>

	// This is for signatures generated internally (which will not be persisted in any way).
	ELEMENT_TYPE_INTERNAL = 0x21,     // INTERNAL <typehandle>

	// Note that this is the max of base type excluding modifiers
	ELEMENT_TYPE_MAX = 0x22,     // first invalid element type


	ELEMENT_TYPE_MODIFIER = 0x40,
	ELEMENT_TYPE_SENTINEL = 0x01 | ELEMENT_TYPE_MODIFIER, // sentinel for varargs
	ELEMENT_TYPE_PINNED = 0x05 | ELEMENT_TYPE_MODIFIER,

}
