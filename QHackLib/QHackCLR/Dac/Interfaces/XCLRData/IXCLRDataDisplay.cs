using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("A3C1704A-4559-4a67-8D28-E8F4FE3B3F62")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataDisplay
	{
		HRESULT ErrorPrintF<T>(char* fmt, T vargs) where T : unmanaged;
		HRESULT NativeImageDimensions(nuint _base, nuint size, uint sectionAlign);
		HRESULT Section(char* name, nuint rva, nuint size);
		HRESULT GetDumpOptions([Out] out CLRNativeImageDumpOptions pOptions);
		HRESULT StartDocument();
		HRESULT EndDocument();
		HRESULT StartCategory(char* name);
		HRESULT EndCategory();
		HRESULT StartElement(char* name);
		HRESULT EndElement();
		HRESULT StartVStructure(char* name);
		HRESULT StartVStructureWithOffset(char* name, uint fieldOffset, uint fieldSize);
		HRESULT EndVStructure();
		HRESULT StartTextElement(char* name);
		HRESULT EndTextElement();
		HRESULT WriteXmlText<T>(char* fmt, T vargs) where T : unmanaged;
		HRESULT WriteXmlTextBlock<T>(char* fmt, T vargs) where T : unmanaged;
		HRESULT WriteEmptyElement(char* element);
		HRESULT WriteElementPointer(char* element, nuint ptr);
		HRESULT WriteElementPointerAnnotated(char* element, nuint ptr, char* annotation);
		HRESULT WriteElementAddress(char* element, nuint _base, nuint size);
		HRESULT WriteElementAddressNamed(char* element, char* name, nuint _base, nuint size);
		HRESULT WriteElementAddressNamedW(char* element, char* name, nuint _base, nuint size);
		HRESULT WriteElementString(char* element, char* data);
		HRESULT WriteElementStringW(char* element, char* data);
		HRESULT WriteElementInt(char* element, int value);
		HRESULT WriteElementUInt(char* element, uint value);
		HRESULT WriteElementEnumerated(char* element, uint value, char* mnemonic);
		HRESULT WriteElementIntWithSuppress(char* element, int value, int suppressIfEqual);
		HRESULT WriteElementFlag(char* element, int flag);
		HRESULT StartArray(char* name, char* fmt);
		HRESULT EndArray(char* countPrefix);
		HRESULT StartList(char* fmt);
		HRESULT EndList();
		HRESULT StartArrayWithOffset(char* name, uint fieldOffset, uint fieldSize, char* fmt);
		#if COMMENTED
		HResult WriteFieldIntWithSuppress(char* element, uint offset, int value, int suppressIfEqual);
		#endif
		HRESULT WriteFieldString(char* element, uint fieldOffset, uint fieldSize, char* data);
		HRESULT WriteFieldStringW(char* element, uint fieldOffset, uint fieldSize, char* data);
		HRESULT WriteFieldPointer(char* element, uint fieldOffset, uint fieldSize, nuint ptr);
		HRESULT WriteFieldPointerWithSize(char* element, uint fieldOffset, uint fieldSize, nuint ptr, nuint size);
		HRESULT WriteFieldInt(char* element, uint fieldOffset, uint fieldSize, int value);
		HRESULT WriteFieldUInt(char* element, uint fieldOffset, uint fieldSize, uint value);
		HRESULT WriteFieldEnumerated(char* element, uint fieldOffset, uint fieldSize, uint value, char* mnemonic);
		HRESULT WriteFieldEmpty(char* element, uint fieldOffset, uint fieldSize);
		HRESULT WriteFieldFlag(char* element, uint fieldOffset, uint fieldSize, int flag);
		HRESULT WriteFieldPointerAnnotated(char* element, uint fieldOffset, uint fieldSize, nuint ptr, char* annotation);
		HRESULT WriteFieldAddress(char* element, uint fieldOffset, uint fieldSize, nuint _base, nuint size);
		#if COMMENTED
		HResult WriteFieldFlag(char* element, uint offset, int flag);
		HResult WriteFieldInt(char* element, uint offset, int value);
		HResult WriteFieldUInt(char* element, uint offset, uint value);
		HResult WriteFieldEnumerated(char* element, uint offset, uint value, char* mnemonic);
		#endif
		HRESULT StartStructure(char* name, nuint ptr, nuint size);
		HRESULT StartStructureWithNegSpace(char* name, nuint ptr, nuint startPtr, nuint totalSize);
		HRESULT StartStructureWithOffset(char* name, uint fieldOffset, uint fieldSize, nuint ptr, nuint size);
		HRESULT EndStructure();
		#if COMMENTED
		HResult WriteElementPointerWithSize(char* element, nuint ptr, nuint size);
		HResult WriteElementInt(char* element, int value);
		HResult WriteFixupDescription(nuint ptr, uint tagged, nuint handle, char* name);
		HResult StartStructure(char* name, nuint ptr, nuint size);
		HResult EndStructure();
		#endif
	}
}
