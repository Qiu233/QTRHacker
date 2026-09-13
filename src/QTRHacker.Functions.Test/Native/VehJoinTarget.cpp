// Standalone x86 target. No production hooks, CLR callbacks, or game process.
#include "VehJoin.h"
#include "LiveCrashWatch.h"
#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <string>
#include <vector>

static void* g_code;
static HANDLE g_worker;
static bool g_inline;
static volatile LONG g_calls;
static DWORD g_beforeEsp, g_afterEsp, g_afterFlags;
static DWORD g_registers[7];
__declspec(align(16)) static BYTE g_originalFx[512], g_expectedFx[512], g_actualFx[512];
__declspec(align(16)) static BYTE g_xmm[128];

[[noreturn]] static void Fail(const char* message) {
    std::fprintf(stderr, "FAIL: %s (Win32 %lu)\n", message, GetLastError());
    ExitProcess(1);
}
static void Require(bool ok, const char* message) { if (!ok) Fail(message); }

// Both continuations are resident. The first models a callable payload; the
// second models returning into an original function after its relocated prologue.
__declspec(naked) static void ReturnContinuation() { __asm { ret } }
__declspec(naked) static void InlineContinuation() {
    __asm {
        mov esp, ebp
        pop ebp
        ret
    }
}

// Save the caller's real state, install distinctive live registers, then capture
// them immediately after the continuation. No managed transition hides damage.
__declspec(naked) static void InvokePayload() {
    __asm {
        pushfd
        pushad
        fxsave g_originalFx
        fninit
        fld1
        movdqa xmm0, xmmword ptr [g_xmm]
        movdqa xmm1, xmmword ptr [g_xmm + 16]
        movdqa xmm2, xmmword ptr [g_xmm + 32]
        movdqa xmm3, xmmword ptr [g_xmm + 48]
        movdqa xmm4, xmmword ptr [g_xmm + 64]
        movdqa xmm5, xmmword ptr [g_xmm + 80]
        movdqa xmm6, xmmword ptr [g_xmm + 96]
        movdqa xmm7, xmmword ptr [g_xmm + 112]
        fxsave g_expectedFx
        mov g_beforeEsp, esp
        mov eax, 11223344h
        mov ebx, 22334455h
        mov ecx, 33445566h
        mov edx, 44556677h
        mov esi, 55667788h
        mov edi, 66778899h
        mov ebp, 778899AAh
        push 247h
        popfd
        call dword ptr [g_code]
        mov [g_registers], eax
        mov [g_registers + 4], ebx
        mov [g_registers + 8], ecx
        mov [g_registers + 12], edx
        mov [g_registers + 16], esi
        mov [g_registers + 20], edi
        mov [g_registers + 24], ebp
        mov g_afterEsp, esp
        pushfd
        pop g_afterFlags
        fxsave g_actualFx
        fxrstor g_originalFx
        popad
        popfd
        ret
    }
}
static DWORD WINAPI Worker(void*) { InvokePayload(); return 42; }

static void ProbeUnrelatedExceptions() {
    int caught = 0;
    __try { RaiseException(0xE0421234, 0, 0, nullptr); }
    __except (GetExceptionCode() == 0xE0421234 ? EXCEPTION_EXECUTE_HANDLER : EXCEPTION_CONTINUE_SEARCH) { ++caught; }
    __try { __debugbreak(); }
    __except (GetExceptionCode() == EXCEPTION_BREAKPOINT ? EXCEPTION_EXECUTE_HANDLER : EXCEPTION_CONTINUE_SEARCH) { ++caught; }
    Require(caught == 2, "VEH swallowed an unrelated exception");
}
static void ProbeWrongThread() {
    bool caught = false;
    __try { reinterpret_cast<void(__cdecl*)()>(g_code)(); }
    __except (GetExceptionCode() == EXCEPTION_BREAKPOINT ? EXCEPTION_EXECUTE_HANDLER : EXCEPTION_CONTINUE_SEARCH) { caught = true; }
    Require(caught && g_join.state == Armed, "VEH accepted the right address on the wrong thread");
    InterlockedExchange(&g_calls, 0);
}

static void Prepare(bool inlineMode, bool held, bool high, bool legacy) {
    Require(!g_worker && !g_code, "previous invocation was not retired");
    g_inline = inlineMode;
    g_join = {};
    g_join.gate = held ? 0 : 1;
    g_calls = 0;
    g_code = VirtualAlloc(nullptr, 4096, MEM_RESERVE | MEM_COMMIT | (high ? MEM_TOP_DOWN : 0), PAGE_READWRITE);
    Require(g_code != nullptr, "VirtualAlloc");
    Require(!high || reinterpret_cast<DWORD>(g_code) >= 0x80000000u, "high address allocation did not exceed 2 GB");
    std::vector<BYTE> code;
    auto bytes = [&](std::initializer_list<BYTE> values) { code.insert(code.end(), values); };
    auto address = [&](const volatile void* p) {
        DWORD value = reinterpret_cast<DWORD>(p);
        auto first = reinterpret_cast<const BYTE*>(&value);
        code.insert(code.end(), first, first + 4);
    };
    bytes({0x9C, 0xFF, 0x05}); address(&g_calls); bytes({0x9D}); // pushfd; inc [calls]; popfd
    if (legacy) {
        // Deterministically stretch the old flag-before-ret window.
        bytes({0xC7, 0x05}); address(&g_join.state); bytes({2, 0, 0, 0});
        bytes({0x83, 0x3D}); address(&g_join.gate); bytes({0, 0x74, 0xF7, 0xC3});
    } else {
        if (inlineMode) bytes({0x55, 0x8B, 0xEC, 0x8D, 0x64, 0x24, 0xF0}); // push ebp; mov ebp,esp; lea esp,[esp-16]
        g_join.breakpoint = reinterpret_cast<DWORD>(g_code) + static_cast<DWORD>(code.size());
        bytes({0xCC, 0x0F, 0x0B}); // int3; ud2 (fallthrough is always a failure)
    }
    std::memcpy(g_code, code.data(), code.size());
    DWORD oldProtect;
    Require(VirtualProtect(g_code, 4096, PAGE_EXECUTE_READ, &oldProtect) != FALSE, "VirtualProtect");
    Require(FlushInstructionCache(GetCurrentProcess(), g_code, code.size()) != FALSE, "FlushInstructionCache");
    g_join.resume = reinterpret_cast<DWORD>(inlineMode ? InlineContinuation : ReturnContinuation);
    g_worker = CreateThread(nullptr, 0, Worker, nullptr, CREATE_SUSPENDED, &g_join.owner);
    Require(g_worker != nullptr, "CreateThread");
    InterlockedExchange(&g_join.state, Armed);
    ProbeUnrelatedExceptions(); // Test filters while a request is armed, too.
    if (!legacy && !inlineMode) ProbeWrongThread();
    Require(ResumeThread(g_worker) != static_cast<DWORD>(-1), "ResumeThread");
}

static void Finish(bool locallyOwned) {
    Require(WaitForSingleObject(g_worker, 10000) == WAIT_OBJECT_0, "worker did not return");
    DWORD exitCode;
    Require(GetExitCodeThread(g_worker, &exitCode) && exitCode == 42, "worker result");
    Require(g_join.state == LeftBlock && g_join.returning == 1 && g_calls == 1, "completion/once state");
    const DWORD expected[] = {0x11223344, 0x22334455, 0x33445566, 0x44556677, 0x55667788, 0x66778899, 0x778899AA};
    Require(std::memcmp(g_registers, expected, sizeof(expected)) == 0, "general purpose registers changed");
    Require(g_beforeEsp == g_afterEsp, "stack was not balanced");
    Require(g_join.savedEsp == g_beforeEsp - (g_inline ? 24 : 4), "unexpected stack at breakpoint");
    Require((g_afterFlags & 0xCD5) == (0x247 & 0xCD5), "arithmetic/direction flags changed");
    Require(std::memcmp(g_expectedFx, g_actualFx, 5) == 0, "x87 control/status/tag changed");
    Require(std::memcmp(g_expectedFx + 24, g_actualFx + 24, 4) == 0, "MXCSR changed");
    Require(std::memcmp(g_expectedFx + 32, g_actualFx + 32, 10) == 0, "x87 ST(0) changed");
    Require(std::memcmp(g_expectedFx + 160, g_actualFx + 160, 128) == 0, "XMM0-XMM7 changed");
    Require(CloseHandle(g_worker) != FALSE, "CloseHandle(worker)");
    g_worker = nullptr;
    if (locallyOwned) Require(VirtualFree(g_code, 0, MEM_RELEASE) != FALSE, "local VirtualFree");
    g_code = nullptr;
    InterlockedExchange(&g_join.state, Idle);
}

// Suppress crash dialogs for the intentionally failing legacy child only.
static LONG WINAPI Unhandled(EXCEPTION_POINTERS* ep) {
    TerminateProcess(GetCurrentProcess(), ep->ExceptionRecord->ExceptionCode);
    return EXCEPTION_EXECUTE_HANDLER;
}

int wmain(int argc, wchar_t** argv) {
    if (argc == 5 && std::wcscmp(argv[1], L"--watch") == 0)
        return WatchLiveCrash(std::wcstoul(argv[2], nullptr, 10), argv[3], std::wcstoul(argv[4], nullptr, 10));
    SetErrorMode(SEM_FAILCRITICALERRORS | SEM_NOGPFAULTERRORBOX);
    SetUnhandledExceptionFilter(Unhandled);
    for (size_t i = 0; i < sizeof(g_xmm); ++i) g_xmm[i] = static_cast<BYTE>(i * 37 + 11);
    PVOID cookie = AddVectoredExceptionHandler(1, JoinVEH);
    Require(cookie != nullptr, "AddVectoredExceptionHandler");
    ProbeUnrelatedExceptions();

    if (argc == 4 && std::wcscmp(argv[1], L"--orphan") == 0) {
        HANDLE controller = OpenProcess(SYNCHRONIZE, FALSE, std::wcstoul(argv[2], nullptr, 10));
        Require(controller != nullptr, "OpenProcess(controller)");
        std::printf("READY %lu\n", GetCurrentProcessId()); std::fflush(stdout);
        Require(WaitForSingleObject(controller, 10000) == WAIT_OBJECT_0, "controller did not exit");
        CloseHandle(controller);
        // All executions begin AFTER the controller dies. Nothing in VEH waits
        // for that controller; the local host joins before retiring its metadata.
        for (int i = 0; i < 64; ++i) {
            Prepare((i & 1) != 0, false, (i & 2) != 0, false);
            Finish(true);
        }
        FILE* report = nullptr;
        Require(_wfopen_s(&report, argv[3], L"w") == 0 && report, "open orphan report");
        std::fputs("PASS: 64 invocations after controller termination\n", report);
        Require(std::fclose(report) == 0, "close orphan report");
    } else {
        Require(argc == 1, "unexpected target arguments");
        std::printf("READY %lu\n", GetCurrentProcessId()); std::fflush(stdout);
        char command[128];
        while (std::fgets(command, sizeof(command), stdin)) {
            if (std::strncmp(command, "run ", 4) == 0) {
                Prepare(std::strstr(command, "inline") != nullptr, std::strstr(command, "held") != nullptr,
                    std::strstr(command, "high") != nullptr, std::strstr(command, "legacy") != nullptr);
                std::printf("RUN %08lX %08lX %08lX %08lX\n", reinterpret_cast<DWORD>(g_code),
                    reinterpret_cast<DWORD>(&g_join.state), reinterpret_cast<DWORD>(&g_join.gate), reinterpret_cast<DWORD>(&g_join.returning));
            } else if (std::strncmp(command, "finish", 6) == 0) {
                Finish(false); // Controller owns/frees B in these cases.
                std::printf("PASS EIP_delta=%ld\n", static_cast<LONG>(g_join.savedEip - g_join.breakpoint));
            } else if (std::strncmp(command, "quit", 4) == 0) {
                Require(!g_worker, "quit with an active worker");
                break;
            } else Fail("unknown command");
            std::fflush(stdout);
        }
    }
    Require(!g_worker, "EOF with an active worker");
    Require(RemoveVectoredExceptionHandler(cookie) != 0, "RemoveVectoredExceptionHandler");
    return 0;
}
