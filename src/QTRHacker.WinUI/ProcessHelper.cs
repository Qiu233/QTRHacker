using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace QTRHacker;

internal static class ProcessHelper
{
    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool OpenProcessToken(nint ProcessHandle, uint DesiredAccess, out nint TokenHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(nint hObject);

    private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
    private const int TOKEN_ASSIGN_PRIMARY = 0x1;
    private const int TOKEN_DUPLICATE = 0x2;
    private const int TOKEN_IMPERSONATE = 0x4;
    private const int TOKEN_QUERY = 0x8;
    private const int TOKEN_QUERY_SOURCE = 0x10;
    private const int TOKEN_ADJUST_GROUPS = 0x40;
    private const int TOKEN_ADJUST_PRIVILEGES = 0x20;
    private const int TOKEN_ADJUST_SESSIONID = 0x100;
    private const int TOKEN_ADJUST_DEFAULT = 0x80;
    private const int TOKEN_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_SESSIONID | TOKEN_ADJUST_DEFAULT;

    public static bool IsProcessOwnerAdmin(this Process proc)
    {
        nint ph = 0;
        try
        {
            OpenProcessToken(proc.Handle, TOKEN_ALL_ACCESS, out ph);
        }
        catch
        {
            return true;
        }
        WindowsIdentity iden = new(ph);
        bool result = false;
        foreach (IdentityReference role in iden.Groups!)
        {
            if (role is not SecurityIdentifier sid)
                continue;

            if (sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) || sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
            {
                result = true;
                break;
            }
        }
        CloseHandle(ph);
        return result;
    }
}