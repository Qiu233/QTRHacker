using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace QHackCLR.Dac.Interfaces
{
	/// <summary>
	/// This interface is rewritten by hands.<br/>
	/// Carefully look at the parameters before any use.<br/>
	/// Check and make any change if necessary.
	/// </summary>
	[ComImport, Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IMetadataImport
	{
		void CloseEnum(
			nuint hEnum);
		HRESULT CountEnum(
			nuint hEnum,
			out uint pulCount);
		HRESULT ResetEnum(
			nuint hEnum,
			uint ulPos);
		HRESULT EnumTypeDefs(
			ref nuint phEnum,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] rTypeDefs,
			uint cMax,
			out uint pcTypeDefs);

		HRESULT EnumInterfaceImpls(
			ref nuint phEnum,
			int td,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rImpls,
			uint cMax,
			out uint pcImpls);

		HRESULT EnumTypeRefs(
			ref nuint phEnum,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] rTypeRefs,
			uint cMax,
			out uint pcTypeRefs);

		HRESULT FindTypeDefByName(
			[MarshalAs(UnmanagedType.LPWStr)] string szTypeDef,
			int tkEnclosingClass,
			out int ptd);

		HRESULT GetScopeProps(
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			uint cchName,
			out uint pchName,
			out Guid pmvid);

		HRESULT GetModuleFromScope(
			out int pmd);

		HRESULT GetTypeDefProps(
			int td,
			char* szTypeDef,
			uint cchTypeDef,
			out uint pchTypeDef,
			out uint pdwTypeDefFlags,
			out int ptkExtends);

		HRESULT GetInterfaceImplProps(
			int iiImpl,
			out int pClass,
			out int ptkIface);

		HRESULT GetTypeRefProps(
			int tr,
			out int ptkResolutionScope,
			char* szName,
			uint cchName,
			out uint pchName);

		HRESULT ResolveTypeRef(
			int tr,
			in Guid riid,
			out IntPtr ppIScope,
			out int ptd);

		HRESULT EnumMembers(
			ref nuint phEnum,
			int cl,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rMembers,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumMembersWithName(
			ref nuint phEnum,
			int cl,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] rMembers,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumMethods(
			ref nuint phEnum,
			int cl,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rMethods,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumMethodsWithName(
			ref nuint phEnum,
			int cl,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] rMethods,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumFields(
			ref nuint phEnum,
			int cl,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rFields,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumFieldsWithName(
			ref nuint phEnum,
			int cl,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] rFields,
			uint cMax,
			out uint pcTokens);


		HRESULT EnumParams(
			ref nuint phEnum,
			int mb,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rParams,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumMemberRefs(
			ref nuint phEnum,
			int tkParent,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rMemberRefs,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumMethodImpls(
			ref nuint phEnum,
			int td,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] rMethodBody,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] rMethodDecl,
			uint cMax,
			out uint pcTokens);

		HRESULT EnumPermissionSets(
			ref nuint phEnum,
			int tk,
			uint dwActions,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] rPermission,
			uint cMax,
			out uint pcTokens);

		HRESULT FindMember(
			int td,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			IntPtr pvSigBlob,
			uint cbSigBlob,
			out int pmb);

		HRESULT FindMethod(
			int td,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			IntPtr pvSigBlob,
			uint cbSigBlob,
			out int pmb);

		HRESULT FindField(
			int td,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			IntPtr pvSigBlob,
			uint cbSigBlob,
			out int pmb);

		HRESULT FindMemberRef(
			int td,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			IntPtr pvSigBlob,
			uint cbSigBlob,
			out int pmr);

		HRESULT GetMethodProps(
			int mb,
			out int pClass,
			char* szMethod,
			uint cchMethod,
			out uint pchMethod,
			out MethodAttributes pdwAttr,
			out IntPtr ppvSigBlob,
			out uint pcbSigBlob,
			out uint pulCodeRVA,
			out uint pdwImplFlags);

		HRESULT GetMemberRefProps(
			int mr,
			out int ptk,
			char* szMember,
			uint cchMember,
			out uint pchMember,
			out IntPtr ppvSigBlob,
			out uint pbSig);

		HRESULT EnumProperties(
			ref nuint phEnum,
			int td,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rProperties,
			uint cMax,
			out uint pcProperties);

		HRESULT EnumEvents(
			ref nuint phEnum,
			int td,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rEvents,
			uint cMax,
			out uint pcEvents);

		HRESULT GetEventProps(
			int ev,
			out int pClass,
			char* szEvent,
			uint cchEvent,
			out uint pchEvent,
			out uint pdwEventFlags,
			out int ptkEventType,
			out int pmdAddOn,
			out int pmdRemoveOn,
			out int pmdFire,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 11)] int[] rmdOtherMethod,
			uint cMax,
			out uint pcOtherMethod);

		HRESULT EnumMethodSemantics(
			ref nuint phEnum,
			int mb,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] rEventProp,
			uint cMax,
			out uint pcEventProp);

		HRESULT GetMethodSemantics(
			int mb,
			int tkEventProp,
			out uint pdwSemanticsFlags);

		HRESULT GetClassLayout(
			int td,
			out uint pdwPackSize,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] COR_FIELD_OFFSET[] rFieldOffset,
			uint cMax,
			out uint pcFieldOffset,
			out uint pulClassSize);

		HRESULT GetFieldMarshal(
			int tk,
			out IntPtr ppvNativeType,
			out uint pcbNativeType);

		HRESULT GetRVA(
			int tk,
			out uint pulCodeRVA,
			out uint pdwImplFlags);

		HRESULT GetPermissionSetProps(
			int pm,
			out uint pdwAction,
			out IntPtr ppvPermission,
			out uint pcbPermission);

		HRESULT GetSigFromToken(
			int mdSig,
			out IntPtr ppvSig,
			out uint pcbSig);

		HRESULT GetModuleRefProps(
			int mur,
			char* szName,
			uint cchName,
			out uint pchName);

		HRESULT EnumModuleRefs(
			ref nuint phEnum,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] rModuleRefs,
			uint cmax,
			out uint pcModuleRefs);

		HRESULT GetTypeSpecFromToken(
			int typespec,
			out IntPtr ppvSig,
			out uint pcbSig);

		HRESULT GetNameFromToken(
			int tk,
			char* pszUtf8NamePtr);

		HRESULT EnumUnresolvedMethods(
			ref nuint phEnum,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] rMethods,
			uint cMax,
			out uint pcTokens);

		HRESULT GetUserString(
			int stk,
			char* szString,
			uint cchString,
			out uint pchString);

		HRESULT GetPinvokeMap(
			int tk,
			out uint pdwMappingFlags,
			char* szImportName,
			uint cchImportName,
			out uint pchImportName,
			out int pmrImportDLL);

		HRESULT EnumSignatures(
			ref nuint phEnum,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] rSignatures,
			uint cmax,
			out uint pcSignatures);

		HRESULT EnumTypeSpecs(
			ref nuint phEnum,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] rTypeSpecs,
			uint cmax,
			out uint pcTypeSpecs);

		HRESULT EnumUserStrings(
			ref nuint phEnum,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] rStrings,
			uint cmax,
			out uint pcStrings);

		HRESULT GetParamForMethodIndex(
			int md,
			uint ulParamSeq,
			out int ppd);

		HRESULT EnumCustomAttributes(
			ref nuint phEnum,
			int tk,
			int tkType,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] rCustomAttributes,
			uint cMax,
			out uint pcCustomAttributes);

		HRESULT GetCustomAttributeProps(
			int cv,
			out int ptkObj,
			out int ptkType,
			out IntPtr ppBlob,
			out uint pcbSize);

		HRESULT FindTypeRef(
			int tkResolutionScope,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			out int ptr);

		HRESULT GetMemberProps(
			int mb,
			out int pClass,
			char* szMember,
			uint cchMember,
			out uint pchMember,
			out uint pdwAttr,
			out IntPtr ppvSigBlob,
			out uint pcbSigBlob,
			out uint pulCodeRVA,
			out uint pdwImplFlags,
			out uint pdwCPlusTypeFlag,
			out IntPtr ppValue,
			out uint pcchValue);

		HRESULT GetFieldProps(
			int mb,
			out int pClass,
			char* szField,
			uint cchField,
			out uint pchField,
			out FieldAttributes pdwAttr,
			out IntPtr ppvSigBlob,
			out uint pcbSigBlob,
			out uint pdwCPlusTypeFlag,
			out IntPtr ppValue,
			out uint pcchValue);

		HRESULT GetPropertyProps(
			int prop,
			out int pClass,
			char* szProperty,
			uint cchProperty,
			out uint pchProperty,
			out uint pdwPropFlags,
			out IntPtr ppvSig,
			out uint pbSig,
			out uint pdwCPlusTypeFlag,
			out IntPtr ppDefaultValue,
			out uint pcchDefaultValue,
			out int pmdSetter,
			out int pmdGetter,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 14)] int[] rmdOtherMethod,
			uint cMax,
			out uint pcOtherMethod);

		HRESULT GetParamProps(
			int tk,
			out int pmd,
			out uint pulSequence,
			char* szName,
			uint cchName,
			out uint pchName,
			out ParameterAttributes pdwAttr,
			out uint pdwCPlusTypeFlag,
			out IntPtr ppValue,
			out uint pcchValue);

		HRESULT GetCustomAttributeByName(
			int tkObj,
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			out IntPtr ppData,
			out uint pcbData);

		bool IsValidToken(
			int tk);

		HRESULT GetNestedClassProps(
			int tdNestedClass,
			out int ptdEnclosingClass);

		HRESULT GetNativeCallConvFromSig(
			IntPtr pvSig,
			uint cbSig,
			out uint pcallConv);

		HRESULT IsGlobal(
			int pd,
			out int pbGlobal);
	}
}