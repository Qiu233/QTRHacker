using System.Runtime.InteropServices;

namespace QHackCLR.Common;


[StructLayout(LayoutKind.Sequential, Size = 4)]
public readonly record struct HRESULT(uint Value)
{
    public static readonly HRESULT S_OK = new(0);
    public static readonly HRESULT E_FAIL = new(0x80004005);
    public static readonly HRESULT E_NOTIMPL = new(0x80004001);
    public static readonly HRESULT E_INVALIDARG = new(0x80070057);
    public static readonly HRESULT E_NOINTERFACE = new(0x80004002);

    public bool Failed => (int)Value < 0;
}
