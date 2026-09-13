#pragma once
#include <Windows.h>
#include <intrin.h>
#include <cstddef>

static_assert(sizeof(void*) == 4, "This prototype models the x86 injector.");
enum : LONG { Idle, Armed, LeftBlock };
struct JoinRecord {
    volatile LONG state, gate, returning;
    DWORD owner, breakpoint, resume, savedEip, savedEsp;
};
static_assert(sizeof(JoinRecord) == 32 && offsetof(JoinRecord, breakpoint) == 16 &&
    offsetof(JoinRecord, resume) == 20, "Update the controller's JoinRecord offsets together.");
// Resident single-invocation metadata. The host must join before reusing it.
static JoinRecord g_join{};

static LONG CALLBACK JoinVEH(EXCEPTION_POINTERS* ep) {
    if (ep->ExceptionRecord->ExceptionCode != EXCEPTION_BREAKPOINT ||
        InterlockedCompareExchange(&g_join.state, Idle, Idle) != Armed ||
        reinterpret_cast<DWORD>(ep->ExceptionRecord->ExceptionAddress) != g_join.breakpoint ||
        GetCurrentThreadId() != g_join.owner)
        return EXCEPTION_CONTINUE_SEARCH;

    g_join.savedEip = ep->ContextRecord->Eip;
    g_join.savedEsp = ep->ContextRecord->Esp;
    ep->ContextRecord->Eip = g_join.resume;
    InterlockedExchange(&g_join.state, LeftBlock);

    // TEST ONLY: the native fixture holds this interval to free/poison B before
    // VEH returns. All normal and CLR A/B runs keep gate = 1 (no waiting).
    while (InterlockedCompareExchange(&g_join.gate, 0, 0) == 0) YieldProcessor();
    InterlockedExchange(&g_join.returning, 1);
    return EXCEPTION_CONTINUE_EXECUTION;
}
