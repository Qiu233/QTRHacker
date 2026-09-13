#include "VehJoin.h"
#include <cstdio>

static PVOID g_cookie;
static volatile LONG g_mailbox[2]; // request, completed (host observes actual Tick return)
static LONG WINAPI CrashReport(EXCEPTION_POINTERS* ep) {
    std::fprintf(stderr, "NATIVE_CRASH code=%08lX EIP=%08lX exception=%p thread=%lu\n",
        ep->ExceptionRecord->ExceptionCode, ep->ContextRecord->Eip,
        ep->ExceptionRecord->ExceptionAddress, GetCurrentThreadId());
    std::fflush(stderr);
    TerminateProcess(GetCurrentProcess(), ep->ExceptionRecord->ExceptionCode);
    return EXCEPTION_EXECUTE_HANDLER;
}

extern "C" __declspec(dllexport) JoinRecord* __cdecl Initialize() {
    SetErrorMode(SEM_FAILCRITICALERRORS | SEM_NOGPFAULTERRORBOX);
    SetUnhandledExceptionFilter(CrashReport);
    g_join.owner = GetCurrentThreadId();
    g_join.gate = 1;
    g_cookie = AddVectoredExceptionHandler(1, JoinVEH);
    return g_cookie ? &g_join : nullptr;
}
extern "C" __declspec(dllexport) volatile LONG* __cdecl Mailbox() { return g_mailbox; }
extern "C" __declspec(dllexport) BOOL __cdecl Shutdown() {
    return RemoveVectoredExceptionHandler(g_cookie) != 0;
}
