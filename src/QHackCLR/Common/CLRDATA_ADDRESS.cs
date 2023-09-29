using System.Runtime.InteropServices;

namespace QHackCLR.Common;
[StructLayout(LayoutKind.Sequential, Size = 8)]
public readonly record struct CLRDATA_ADDRESS(ulong Value)
{
    public static implicit operator CLRDATA_ADDRESS(ulong value) => new(value);
    public static implicit operator ulong(CLRDATA_ADDRESS value) => value.Value;
    public static implicit operator nuint(CLRDATA_ADDRESS value) => (nuint)value.Value;

    public override string ToString()
    {
        return Value.ToString("X8");
    }
}
