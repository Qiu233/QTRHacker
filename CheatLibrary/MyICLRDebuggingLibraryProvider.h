#pragma once

#include <CorDebug.h>


class CMyICLRDebuggingLibraryProvider :
	public ICLRDebuggingLibraryProvider

{
private:
	long m_ref;
	ICLRRuntimeInfo *RuntimeInfo;

public:
	CMyICLRDebuggingLibraryProvider(ICLRRuntimeInfo *rti)
	{
		m_ref = 1;
		RuntimeInfo = rti;

	}
	~CMyICLRDebuggingLibraryProvider()
	{
	}
	STDMETHOD(QueryInterface)(__in REFIID InterfaceId, __out PVOID* Interface);
	STDMETHOD_(ULONG, AddRef)();
	STDMETHOD_(ULONG, Release)();

	virtual HRESULT STDMETHODCALLTYPE ProvideLibrary(
		const WCHAR *pwszFileName,
		DWORD dwTimestamp,
		DWORD dwSizeOfImage,
		HMODULE *phModule);


};

