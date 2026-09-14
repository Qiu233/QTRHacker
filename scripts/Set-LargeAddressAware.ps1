#requires -Version 5.1
<#
.SYNOPSIS
Enables IMAGE_FILE_LARGE_ADDRESS_AWARE on an existing x86 executable.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Path
)

$ErrorActionPreference = 'Stop'
$resolved = (Resolve-Path -LiteralPath $Path).ProviderPath
$stream = [IO.File]::Open($resolved, [IO.FileMode]::Open, [IO.FileAccess]::ReadWrite, [IO.FileShare]::Read)
$reader = [IO.BinaryReader]::new($stream, [Text.Encoding]::UTF8, $true)
try {
    # Validate the DOS header, PE signature, COFF header and PE32 optional header
    # before touching the two-byte Characteristics field.
    if ($stream.Length -lt 64 -or $reader.ReadUInt16() -ne 0x5A4D) {
        throw "Not a Windows executable: $resolved"
    }
    $stream.Position = 0x3C
    $peOffset = $reader.ReadUInt32()
    if ($peOffset -lt 64 -or [long]$peOffset + 26 -gt $stream.Length) {
        throw "Invalid PE header: $resolved"
    }
    $stream.Position = $peOffset
    if ($reader.ReadUInt32() -ne 0x4550 -or $reader.ReadUInt16() -ne 0x014C) {
        throw "Expected an x86 executable: $resolved"
    }
    $stream.Position = $peOffset + 20
    $optionalHeaderSize = $reader.ReadUInt16()
    $characteristics = $reader.ReadUInt16()
    if ($optionalHeaderSize -lt 96 -or [long]$peOffset + 24 + $optionalHeaderSize -gt $stream.Length -or
        $reader.ReadUInt16() -ne 0x010B -or ($characteristics -band 0x0002) -eq 0 -or
        ($characteristics -band 0x2000) -ne 0) {
        throw "Expected a PE32 executable, not a DLL: $resolved"
    }
    if (($characteristics -band 0x0020) -eq 0) {
        $stream.Position = $peOffset + 22
        # Only the low byte changes. Preserve every other flag and file byte.
        $stream.WriteByte([byte](($characteristics -bor 0x0020) -band 0xFF))
        $stream.Flush()
        $stream.Position = $peOffset + 22
        if ($reader.ReadUInt16() -ne ($characteristics -bor 0x0020)) {
            throw "Could not enable LAA: $resolved"
        }
        Write-Host "Enabled LAA: $resolved"
    }
}
finally {
    $reader.Dispose()
    $stream.Dispose()
}
