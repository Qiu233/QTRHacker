#pragma once
#include <Windows.h>
#include <DbgHelp.h>
#include <cstdio>
#include <string>
#pragma comment(lib, "Dbghelp.lib")

// External, observational crash collector. It never changes game code or a
// thread's registers. The attach breakpoint is consumed; all other exceptions
// are delivered to the game normally after capturing diagnostics.
static int WatchLiveCrash(DWORD pid, const wchar_t* directory, DWORD controllerPid) {
    HANDLE controller = OpenProcess(SYNCHRONIZE, FALSE, controllerPid);
    HANDLE process = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, pid);
    if (!controller || !process) return 2;
    if (!DebugActiveProcess(pid)) { std::fprintf(stderr, "DebugActiveProcess: %lu\n", GetLastError()); return 3; }
    if (!DebugSetProcessKillOnExit(FALSE)) { DebugActiveProcessStop(pid); return 4; }
    std::wstring stop = std::wstring(directory) + L"\\watch.stop";
    bool initialBreakpoint = true, exited = false;
    int dumps = 0;
    while (!exited) {
        DEBUG_EVENT event{};
        if (WaitForDebugEvent(&event, 100)) {
            DWORD disposition = DBG_CONTINUE;
            switch (event.dwDebugEventCode) {
            case CREATE_PROCESS_DEBUG_EVENT:
                if (event.u.CreateProcessInfo.hFile) CloseHandle(event.u.CreateProcessInfo.hFile);
                break;
            case LOAD_DLL_DEBUG_EVENT:
                if (event.u.LoadDll.hFile) CloseHandle(event.u.LoadDll.hFile);
                break;
            case EXCEPTION_DEBUG_EVENT: {
                auto& record = event.u.Exception.ExceptionRecord;
                disposition = DBG_EXCEPTION_NOT_HANDLED;
                if (initialBreakpoint && record.ExceptionCode == EXCEPTION_BREAKPOINT) {
                    initialBreakpoint = false;
                    disposition = DBG_CONTINUE;
                    std::printf("READY %lu\n", pid); std::fflush(stdout);
                } else if (record.ExceptionCode == EXCEPTION_ACCESS_VIOLATION || !event.u.Exception.dwFirstChance) {
                    HANDLE thread = OpenThread(THREAD_GET_CONTEXT | THREAD_QUERY_INFORMATION, FALSE, event.dwThreadId);
                    CONTEXT context{}; context.ContextFlags = CONTEXT_ALL;
                    BOOL gotContext = thread && GetThreadContext(thread, &context);
                    DWORD fault = record.NumberParameters >= 2 ? static_cast<DWORD>(record.ExceptionInformation[1]) : 0;
                    MEMORY_BASIC_INFORMATION region{};
                    VirtualQueryEx(process, reinterpret_cast<void*>(fault), &region, sizeof(region));
                    std::printf("EXCEPTION code=%08lX first=%lu tid=%lu address=%p EIP=%08lX ESP=%08lX EBP=%08lX ECX=%08lX EDX=%08lX operation=%lu fault=%08lX state=%08lX protect=%08lX\n",
                        record.ExceptionCode, event.u.Exception.dwFirstChance, event.dwThreadId, record.ExceptionAddress,
                        context.Eip, context.Esp, context.Ebp, context.Ecx, context.Edx,
                        record.NumberParameters ? static_cast<DWORD>(record.ExceptionInformation[0]) : 0, fault, region.State, region.Protect);
                    std::fflush(stdout);
                    if (gotContext && dumps < 2) {
                        std::wstring path = std::wstring(directory) + L"\\exception-" + std::to_wstring(++dumps) + L".dmp";
                        HANDLE file = CreateFileW(path.c_str(), GENERIC_WRITE, 0, nullptr, CREATE_NEW, FILE_ATTRIBUTE_NORMAL, nullptr);
                        EXCEPTION_POINTERS pointers{&record, &context};
                        MINIDUMP_EXCEPTION_INFORMATION info{event.dwThreadId, &pointers, FALSE};
                        auto flags = static_cast<MINIDUMP_TYPE>(MiniDumpWithThreadInfo | MiniDumpWithFullMemoryInfo |
                            MiniDumpWithUnloadedModules | MiniDumpWithIndirectlyReferencedMemory | MiniDumpWithCodeSegs);
                        BOOL dumped = file != INVALID_HANDLE_VALUE && MiniDumpWriteDump(process, pid, file, flags, &info, nullptr, nullptr);
                        std::printf("DUMP success=%d error=%lu\n", dumped, dumped ? 0 : GetLastError()); std::fflush(stdout);
                        if (file != INVALID_HANDLE_VALUE) CloseHandle(file);
                    }
                    if (thread) CloseHandle(thread);
                }
                break;
            }
            case EXIT_PROCESS_DEBUG_EVENT:
                std::printf("EXIT code=%08lX\n", event.u.ExitProcess.dwExitCode); std::fflush(stdout);
                exited = true;
                break;
            }
            if (!ContinueDebugEvent(event.dwProcessId, event.dwThreadId, disposition)) break;
        }
        if (WaitForSingleObject(controller, 0) == WAIT_OBJECT_0 || GetFileAttributesW(stop.c_str()) != INVALID_FILE_ATTRIBUTES) break;
    }
    if (!exited) DebugActiveProcessStop(pid);
    CloseHandle(process); CloseHandle(controller);
    return 0;
}
