using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.Defs;

[StructLayout(LayoutKind.Sequential)]
internal struct COR_FIELD_OFFSET
{
	public uint ridOfField;
	public uint ulOffset;
}

[ComImport, Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal unsafe interface IMetaDataImport
{
	[PreserveSig]
	void CloseEnum(void* hEnum);
	HRESULT CountEnum(void* hEnum, uint* pulCount);
	HRESULT ResetEnum(void* hEnum, uint ulPos);
	HRESULT EnumTypeDefs(void** phEnum, uint* rTypeDefs,
							uint cMax, uint* pcTypeDefs);
	HRESULT EnumInterfaceImpls(void** phEnum, uint td,
							uint* rImpls, uint cMax,
							uint* pcImpls);
	HRESULT EnumTypeRefs(void** phEnum, uint* rTypeRefs,
							uint cMax, uint* pcTypeRefs);

	HRESULT FindTypeDefByName(           // S_OK or error.
		string szTypeDef,              // [IN] Name of the Type.
		uint tkEnclosingClass,       // [IN] TypeDef/TypeRef for Enclosing class.
		uint* ptd);             // [OUT] Put the TypeDef token here.

	HRESULT GetScopeProps(               // S_OK or error.
			char* szName,                 // [OUT] Put the name here.
		uint cchName,                // [IN] Size of name buffer in wide chars.
		uint* pchName,               // [OUT] Put size of name (wide chars) here.
		Guid* pmvid);           // [OUT, OPTIONAL] Put MVID here.

	HRESULT GetModuleFromScope(          // S_OK.
		uint* pmd);             // [OUT] Put uint token here.

	HRESULT GetTypeDefProps(             // S_OK or error.
		uint td,                     // [IN] TypeDef token for inquiry.
			char* szTypeDef,              // [OUT] Put name here.
		uint cchTypeDef,             // [IN] size of name buffer in wide chars.
		uint* pchTypeDef,            // [OUT] put size of name (wide chars) here.
		uint* pdwTypeDefFlags,       // [OUT] Put flags here.
		uint* ptkExtends);      // [OUT] Put base class TypeDef/TypeRef here.

	HRESULT GetInterfaceImplProps(       // S_OK or error.
		uint iiImpl,             // [IN] InterfaceImpl token.
		uint* pClass,                // [OUT] Put implementing class token here.
		uint* ptkIface);        // [OUT] Put implemented interface token here.

	HRESULT GetTypeRefProps(             // S_OK or error.
		uint tr,                     // [IN] TypeRef token.
		uint* ptkResolutionScope,    // [OUT] Resolution scope, ModuleRef or AssemblyRef.
			char* szName,                 // [OUT] Name of the TypeRef.
		uint cchName,                // [IN] Size of buffer.
		uint* pchName);         // [OUT] Size of Name.

	HRESULT ResolveTypeRef(uint tr, Guid riid, void** ppIScope, uint* ptd);

	HRESULT EnumMembers(                 // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint cl,                     // [IN] TypeDef to scope the enumeration.
		uint* rMembers,             // [OUT] Put MemberDefs here.
		uint cMax,                   // [IN] Max MemberDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumMembersWithName(         // S_OK, S_FALSE, or error.
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint cl,                     // [IN] TypeDef to scope the enumeration.
		string szName,                 // [IN] Limit results to those with this name.
		uint* rMembers,             // [OUT] Put MemberDefs here.
		uint cMax,                   // [IN] Max MemberDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumMethods(                 // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint cl,                     // [IN] TypeDef to scope the enumeration.
		uint* rMethods,             // [OUT] Put MethodDefs here.
		uint cMax,                   // [IN] Max MethodDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumMethodsWithName(         // S_OK, S_FALSE, or error.
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint cl,                     // [IN] TypeDef to scope the enumeration.
		string szName,                 // [IN] Limit results to those with this name.
		uint* rMethods,             // [OU] Put MethodDefs here.
		uint cMax,                   // [IN] Max MethodDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumFields(                  // S_OK, S_FALSE, or error.
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint cl,                     // [IN] TypeDef to scope the enumeration.
		uint* rFields,              // [OUT] Put FieldDefs here.
		uint cMax,                   // [IN] Max FieldDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumFieldsWithName(          // S_OK, S_FALSE, or error.
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint cl,                     // [IN] TypeDef to scope the enumeration.
		string szName,                 // [IN] Limit results to those with this name.
		uint* rFields,              // [OUT] Put MemberDefs here.
		uint cMax,                   // [IN] Max MemberDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.


	HRESULT EnumParams(                  // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint mb,                     // [IN] MethodDef to scope the enumeration. 
		uint* rParams,              // [OUT] Put ParamDefs here.
		uint cMax,                   // [IN] Max ParamDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumMemberRefs(              // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint tkParent,               // [IN] Parent token to scope the enumeration.
		uint* rMemberRefs,          // [OUT] Put MemberRefs here.
		uint cMax,                   // [IN] Max MemberRefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumMethodImpls(             // S_OK, S_FALSE, or error
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint td,                     // [IN] TypeDef to scope the enumeration.
		uint* rMethodBody,          // [OUT] Put Method Body tokens here.
		uint* rMethodDecl,          // [OUT] Put Method Declaration tokens here.
		uint cMax,                   // [IN] Max tokens to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT EnumPermissionSets(          // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint tk,                     // [IN] if !NIL, token to scope the enumeration.
		uint dwActions,              // [IN] if !0, return only these actions.
		uint* rPermission,         // [OUT] Put Permissions here.
		uint cMax,                   // [IN] Max Permissions to put. 
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT FindMember(
		uint td,                     // [IN] given typedef
		string szName,                 // [IN] member name 
		void* pvSigBlob,          // [IN] point to a blob value of CLR signature 
		uint cbSigBlob,              // [IN] count of bytes in the signature blob
		uint* pmb);             // [OUT] matching memberdef 

	HRESULT FindMethod(
		uint td,                     // [IN] given typedef
		string szName,                 // [IN] member name 
		void* pvSigBlob,          // [IN] point to a blob value of CLR signature 
		uint cbSigBlob,              // [IN] count of bytes in the signature blob
		uint* pmb);             // [OUT] matching memberdef 

	HRESULT FindField(
		uint td,                     // [IN] given typedef
		string szName,                 // [IN] member name 
		void* pvSigBlob,          // [IN] point to a blob value of CLR signature 
		uint cbSigBlob,              // [IN] count of bytes in the signature blob
		uint* pmb);             // [OUT] matching memberdef 

	HRESULT FindMemberRef(
		uint td,                     // [IN] given typeRef
		string szName,                 // [IN] member name 
		void* pvSigBlob,          // [IN] point to a blob value of CLR signature 
		uint cbSigBlob,              // [IN] count of bytes in the signature blob
		uint* pmr);             // [OUT] matching memberref 

	HRESULT GetMethodProps(
		uint mb,                     // The method for which to get props.
		uint* pClass,                // Put method's class here. 
			char* szMethod,               // Put method's name here.
		uint cchMethod,              // Size of szMethod buffer in wide chars.
		uint* pchMethod,             // Put actual size here 
		uint* pdwAttr,               // Put flags here.
		void** ppvSigBlob,        // [OUT] point to the blob value of meta data
		uint* pcbSigBlob,            // [OUT] actual size of signature blob
		uint* pulCodeRVA,            // [OUT] codeRVA
		uint* pdwImplFlags);    // [OUT] Impl. Flags

	HRESULT GetMemberRefProps(           // S_OK or error.
		uint mr,                     // [IN] given memberref 
		uint* ptk,                   // [OUT] Put classref or classdef here. 
			char* szMember,               // [OUT] buffer to fill for member's name
		uint cchMember,              // [IN] the count of char of szMember
		uint* pchMember,             // [OUT] actual count of char in member name
		void** ppvSigBlob,        // [OUT] point to meta data blob value
		uint* pbSig);           // [OUT] actual size of signature blob

	HRESULT EnumProperties(              // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint td,                     // [IN] TypeDef to scope the enumeration.
		uint* rProperties,          // [OUT] Put Properties here.
		uint cMax,                   // [IN] Max properties to put.
		uint* pcProperties);    // [OUT] Put # put here.

	HRESULT EnumEvents(                  // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint td,                     // [IN] TypeDef to scope the enumeration.
		uint* rEvents,              // [OUT] Put events here.
		uint cMax,                   // [IN] Max events to put.
		uint* pcEvents);        // [OUT] Put # put here.

	HRESULT GetEventProps(               // S_OK, S_FALSE, or error. 
		uint ev,                     // [IN] event token 
		uint* pClass,                // [OUT] typedef containing the event declarion.
		string szEvent,                // [OUT] Event name 
		uint cchEvent,               // [IN] the count of wchar of szEvent
		uint* pchEvent,              // [OUT] actual count of wchar for event's name 
		uint* pdwEventFlags,         // [OUT] Event flags.
		uint* ptkEventType,          // [OUT] EventType class
		uint* pmdAddOn,              // [OUT] AddOn method of the event
		uint* pmdRemoveOn,           // [OUT] RemoveOn method of the event
		uint* pmdFire,               // [OUT] Fire method of the event
		uint* rmdOtherMethod,       // [OUT] other method of the event
		uint cMax,                   // [IN] size of rmdOtherMethod
		uint* pcOtherMethod);   // [OUT] total number of other method of this event 

	HRESULT EnumMethodSemantics(         // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint mb,                     // [IN] MethodDef to scope the enumeration. 
		uint* rEventProp,           // [OUT] Put Event/Property here.
		uint cMax,                   // [IN] Max properties to put.
		uint* pcEventProp);     // [OUT] Put # put here.

	HRESULT GetMethodSemantics(          // S_OK, S_FALSE, or error. 
		uint mb,                     // [IN] method token
		uint tkEventProp,            // [IN] event/property token.
		uint* pdwSemanticsFlags); // [OUT] the role flags for the method/propevent pair 

	HRESULT GetClassLayout(
		uint td,                     // [IN] give typedef
		uint* pdwPackSize,           // [OUT] 1, 2, 4, 8, or 16
		COR_FIELD_OFFSET* rFieldOffset,    // [OUT] field offset array 
		uint cMax,                   // [IN] size of the array
		uint* pcFieldOffset,         // [OUT] needed array size
		uint* pulClassSize);        // [OUT] the size of the class

	HRESULT GetFieldMarshal(
		uint tk,                     // [IN] given a field's memberdef
		void** ppvNativeType,     // [OUT] native type of this field
		uint* pcbNativeType);   // [OUT] the count of bytes of *ppvNativeType

	HRESULT GetRVA(                      // S_OK or error.
		uint tk,                     // Member for which to set offset
		uint* pulCodeRVA,            // The offset
		uint* pdwImplFlags);    // the implementation flags 

	HRESULT GetPermissionSetProps(
		uint pm,                    // [IN] the permission token.
		uint* pdwAction,             // [OUT] CorDeclSecurity.
		void** ppvPermission,        // [OUT] permission blob.
		uint* pcbPermission);   // [OUT] count of bytes of pvPermission.

	HRESULT GetSigFromToken(             // S_OK or error.
		uint mdSig,                  // [IN] Signature token.
		void** ppvSig,            // [OUT] return pointer to token.
		uint* pcbSig);          // [OUT] return size of signature.

	HRESULT GetModuleRefProps(           // S_OK or error.
		uint mur,                    // [IN] moduleref token.
			char* szName,                 // [OUT] buffer to fill with the moduleref name.
		uint cchName,                // [IN] size of szName in wide characters.
		uint* pchName);         // [OUT] actual count of characters in the name.

	HRESULT EnumModuleRefs(              // S_OK or error.
		void** phEnum,                // [IN|OUT] pointer to the enum.
		uint* rModuleRefs,          // [OUT] put modulerefs here.
		uint cmax,                   // [IN] max memberrefs to put.
		uint* pcModuleRefs);    // [OUT] put # put here.

	HRESULT GetTypeSpecFromToken(        // S_OK or error.
		uint typespec,                // [IN] TypeSpec token.
		void** ppvSig,            // [OUT] return pointer to TypeSpec signature
		uint* pcbSig);          // [OUT] return size of signature.

	HRESULT GetNameFromToken(            // Not Recommended! May be removed!
		uint tk,                     // [IN] Token to get name from.  Must have a name.
		char** pszUtf8NamePtr);  // [OUT] Return pointer to UTF8 name in heap.

	HRESULT EnumUnresolvedMethods(       // S_OK, S_FALSE, or error. 
		void** phEnum,                // [IN|OUT] Pointer to the enum.
		uint* rMethods,             // [OUT] Put MemberDefs here.
		uint cMax,                   // [IN] Max MemberDefs to put.
		uint* pcTokens);        // [OUT] Put # put here.

	HRESULT GetUserString(               // S_OK or error.
		uint stk,                    // [IN] String token.
			char* szString,               // [OUT] Copy of string.
		uint cchString,              // [IN] Max chars of room in szString.
		uint* pchString);       // [OUT] How many chars in actual string.

	HRESULT GetPinvokeMap(               // S_OK or error.
		uint tk,                     // [IN] FieldDef or MethodDef.
		uint* pdwMappingFlags,       // [OUT] Flags used for mapping.
			char* szImportName,           // [OUT] Import name.
		uint cchImportName,          // [IN] Size of the name buffer.
		uint* pchImportName,         // [OUT] Actual number of characters stored.
		uint* pmrImportDLL);    // [OUT] ModuleRef token for the target DLL.

	HRESULT EnumSignatures(              // S_OK or error.
		void** phEnum,                // [IN|OUT] pointer to the enum.
		uint* rSignatures,          // [OUT] put signatures here.
		uint cmax,                   // [IN] max signatures to put.
		uint* pcSignatures);    // [OUT] put # put here.

	HRESULT EnumTypeSpecs(               // S_OK or error.
		void** phEnum,                // [IN|OUT] pointer to the enum.
		uint* rTypeSpecs,           // [OUT] put TypeSpecs here.
		uint cmax,                   // [IN] max TypeSpecs to put.
		uint* pcTypeSpecs);     // [OUT] put # put here.

	HRESULT EnumUserStrings(             // S_OK or error.
		void** phEnum,                // [IN/OUT] pointer to the enum.
		uint* rStrings,             // [OUT] put Strings here.
		uint cmax,                   // [IN] max Strings to put.
		uint* pcStrings);       // [OUT] put # put here.

	HRESULT GetParamForMethodIndex(      // S_OK or error.
		uint md,                     // [IN] Method token.
		uint ulParamSeq,             // [IN] Parameter sequence.
		uint* ppd);             // [IN] Put Param token here.

	HRESULT EnumCustomAttributes(        // S_OK or error.
		void** phEnum,                // [IN, OUT] COR enumerator.
		uint tk,                     // [IN] Token to scope the enumeration, 0 for all.
		uint tkType,                 // [IN] Type of interest, 0 for all.
		uint* rCustomAttributes, // [OUT] Put custom attribute tokens here.
		uint cMax,                   // [IN] Size of rCustomAttributes.
		uint* pcCustomAttributes);  // [OUT, OPTIONAL] Put count of token values here.

	HRESULT GetCustomAttributeProps(     // S_OK or error.
		uint cv,               // [IN] CustomAttribute token.
		uint* ptkObj,                // [OUT, OPTIONAL] Put object token here.
		uint* ptkType,               // [OUT, OPTIONAL] Put AttrType token here.
		void** ppBlob,               // [OUT, OPTIONAL] Put pointer to data here.
		uint* pcbSize);         // [OUT, OPTIONAL] Put size of date here.

	HRESULT FindTypeRef(
		uint tkResolutionScope,      // [IN] ModuleRef, AssemblyRef or TypeRef.
		string szName,                 // [IN] TypeRef Name.
		uint* ptr);             // [OUT] matching TypeRef.

	HRESULT GetMemberProps(
		uint mb,                     // The member for which to get props.
		uint* pClass,                // Put member's class here. 
			char* szMember,               // Put member's name here.
		uint cchMember,              // Size of szMember buffer in wide chars.
		uint* pchMember,             // Put actual size here 
		uint* pdwAttr,               // Put flags here.
		void** ppvSigBlob,        // [OUT] point to the blob value of meta data
		uint* pcbSigBlob,            // [OUT] actual size of signature blob
		uint* pulCodeRVA,            // [OUT] codeRVA
		uint* pdwImplFlags,          // [OUT] Impl. Flags
		uint* pdwCPlusTypeFlag,      // [OUT] flag for value type. selected ELEMENT_TYPE_*
		void** ppValue,             // [OUT] constant value 
		uint* pcchValue);       // [OUT] size of constant string in chars, 0 for non-strings.

	HRESULT GetFieldProps(
		uint mb,                     // The field for which to get props.
		uint* pClass,                // Put field's class here.
			char* szField,                // Put field's name here.
		uint cchField,               // Size of szField buffer in wide chars.
		uint* pchField,              // Put actual size here 
		uint* pdwAttr,               // Put flags here.
		void** ppvSigBlob,        // [OUT] point to the blob value of meta data
		uint* pcbSigBlob,            // [OUT] actual size of signature blob
		uint* pdwCPlusTypeFlag,      // [OUT] flag for value type. selected ELEMENT_TYPE_*
		void** ppValue,             // [OUT] constant value 
		uint* pcchValue);       // [OUT] size of constant string in chars, 0 for non-strings.

	HRESULT GetPropertyProps(            // S_OK, S_FALSE, or error. 
		uint prop,                   // [IN] property token
		uint* pClass,                // [OUT] typedef containing the property declarion. 
		string szProperty,             // [OUT] Property name
		uint cchProperty,            // [IN] the count of wchar of szProperty
		uint* pchProperty,           // [OUT] actual count of wchar for property name
		uint* pdwPropFlags,          // [OUT] property flags.
		void** ppvSig,            // [OUT] property type. pointing to meta data internal blob 
		uint* pbSig,                 // [OUT] count of bytes in *ppvSig
		uint* pdwCPlusTypeFlag,      // [OUT] flag for value type. selected ELEMENT_TYPE_*
		void** ppDefaultValue,      // [OUT] constant value 
		uint* pcchDefaultValue,      // [OUT] size of constant string in chars, 0 for non-strings.
		uint* pmdSetter,             // [OUT] setter method of the property
		uint* pmdGetter,             // [OUT] getter method of the property
		uint* rmdOtherMethod,       // [OUT] other method of the property
		uint cMax,                   // [IN] size of rmdOtherMethod
		uint* pcOtherMethod);   // [OUT] total number of other method of this property

	HRESULT GetParamProps(               // S_OK or error.
		uint tk,                     // [IN]The Parameter.
		uint* pmd,                   // [OUT] Parent Method token.
		uint* pulSequence,           // [OUT] Parameter sequence.
			char* szName,                 // [OUT] Put name here.
		uint cchName,                // [OUT] Size of name buffer.
		uint* pchName,               // [OUT] Put actual size of name here.
		uint* pdwAttr,               // [OUT] Put flags here.
		uint* pdwCPlusTypeFlag,      // [OUT] Flag for value type. selected ELEMENT_TYPE_*.
		void** ppValue,             // [OUT] Constant value.
		uint* pcchValue);       // [OUT] size of constant string in chars, 0 for non-strings.

	HRESULT GetCustomAttributeByName(    // S_OK or error.
		uint tkObj,                  // [IN] Object with Custom Attribute.
		string szName,                 // [IN] Name of desired Custom Attribute.

		void** ppData,               // [OUT] Put pointer to data here.
		uint* pcbData);         // [OUT] Put size of data here.

	[PreserveSig]
	bool IsValidToken(         // True or False.
		uint tk);               // [IN] Given token.

	HRESULT GetNestedClassProps(         // S_OK or error.
		uint tdNestedClass,          // [IN] NestedClass token.
		uint* ptdEnclosingClass); // [OUT] EnclosingClass token.

	HRESULT GetNativeCallConvFromSig(    // S_OK or error.
		void* pvSig,                 // [IN] Pointer to signature.
		uint cbSig,                  // [IN] Count of signature bytes.
		uint* pCallConv);       // [OUT] Put calling conv here (see CorPinvokemap).

	HRESULT IsGlobal(                    // S_OK or error.
		uint pd,                     // [IN] Type, Field, or Method token.
		int* pbGlobal);        // [OUT] Put 1 if global, 0 otherwise.

	// This interface is sealed.  Do not change, add, or remove anything.  Instead, derive a new iterface.
}
